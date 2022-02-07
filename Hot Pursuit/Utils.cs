using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AstroMath;
using TheSky64Lib;
using System.Windows.Forms;

namespace Hot_Pursuit
{
    public static class Utils
    {
        public const double Astronomical_Unit = 149597870.700;  //km
        public const double Earth_Radius = 6371.0; //km

        public static double PARateToRA(double paD, double rate)
        {
            //Calculates dRA/dt for PA in Degrees and rate in arcsec/min (but really doesn't matter
            double paR = AstroMath.Transform.DegreesToRadians(paD);
            double raRate = rate * Math.Sin(paR);
            return raRate;
        }

        public static double PARateToDec(double paD, double rate)
        {
            //Calculates dDec/dt for PA in Degrees and rate in arcsec/min (but really doesn't matter
            double paR = AstroMath.Transform.DegreesToRadians(paD);
            double decRate = rate * Math.Cos(paR);
            return AstroMath.Transform.RadiansToDegrees(decRate);
        }
        public static string GetTargetName()
        {
            sky6ObjectInformation tsxoi = new sky6ObjectInformation();
            tsxoi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_NAME1);
            string tgtName = tsxoi.ObjInfoPropOut;
            return tgtName;
        }
        public static bool CLSToTarget(string tgtName, SpeedVector sv, bool IsPrecision = false)
        {
            //first, couple dome to telescope, if there is one
            sky6Dome tsxd = new sky6Dome();
            try
            {
                tsxd.Connect();
                tsxd.IsCoupled = 1;
            }
            catch (Exception ex)
            {
                //do nothing
            }

            int clsStatus = 123;
            sky6RASCOMTele tsxmt = new sky6RASCOMTele();
            ClosedLoopSlew tsx_cl = new ClosedLoopSlew();
            sky6StarChart tsxsc = new sky6StarChart();
            //Clear any image reduction, otherwise full reduction might cause a problem
            ccdsoftCamera tsxcam = new ccdsoftCamera()
            {
                ImageReduction = ccdsoftImageReduction.cdNone,
                Asynchronous = 1 //make sure nothing else happens while setting this up
            };
            //Abort any ongoing imaging
            tsxcam.Abort();

                bool returnStatus = true;
           double tgtRAH = Transform.DegreesToHours(sv.RA_Degrees);
            double tgtDecD = sv.Dec_Degrees;
            tsxsc.Find(tgtRAH.ToString() + ", " + tgtDecD.ToString());
            tsxmt.Connect();
            //tsxmt.Asynchronous = 0;
            try
            {
                tsxmt.SlewToRaDec(tgtRAH, tgtDecD, tgtName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Slew Failure: " + ex.Message);
                returnStatus=false;
            }
            if (IsPrecision && returnStatus)
            {
                //***  precision slew
                try
                {
                    clsStatus = tsx_cl.exec();
                }
                catch (Exception ex)
                {
                    returnStatus = false;
                }
            }
            try
            {
                tsxsc.Find(tgtName);
            }
            catch (Exception ex)
            {
                returnStatus = true;
            }
            return returnStatus;
        }

        public static bool SlewToTarget(string tgtName, SpeedVector sv)
        {

            sky6RASCOMTele tsxmt = new sky6RASCOMTele();
            double tgtRAH = Transform.DegreesToHours(sv.RA_Degrees);
            double tgtDecD = sv.Dec_Degrees;
            tsxmt.Connect();
            try
            {
                tsxmt.SlewToRaDec(tgtRAH, tgtDecD, tgtName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Slew Failed: " + ex.Message);
                return false;
            }
            return true;
        }

        public static bool SetTargetTracking(SpeedVector sv, double topo_Adjustment_RA, double topo_Adjustment_Dec)
        {
            const int ionTrackingOn = 1;
            const int ionTrackingOff = 0;
            const int ignoreRates = 1;
            const int useRates = 0;

            double tgtRateRA = sv.Rate_RA_CosDec_ArcsecPerMinute;
            double tgtRateDec = sv.Rate_Dec_ArcsecPerMinute;
            double adjtgtRateRA = tgtRateRA * topo_Adjustment_RA;
            double adjtgtRateDec = tgtRateDec * topo_Adjustment_Dec;

            sky6RASCOMTele tsxmt = new sky6RASCOMTele();
            tsxmt.Connect();
            //double dRA1 = tsxmt.dRaTrackingRate;
            //double dDec1 = tsxmt.dDecTrackingRate;
            try
            {
                //TSX expects tracking rates in arcsec/sec: convert it from arcsec/min
                tsxmt.SetTracking(ionTrackingOn, useRates, adjtgtRateRA / 60.0, adjtgtRateDec / 60.0);
            }
            catch
            {
                return false;
            }
            //double dRA2 = tsxmt.dRaTrackingRate;
            //double dDec2 = tsxmt.dDecTrackingRate;
            return true;
        }

        public static bool SetStandardTracking()
        {
            const int ionTrackingOn = 1;
            const int ionTrackingOff = 0;
            const int ignoreRates = 1;
            const int useRates = 1;  //Don't use rates

            sky6RASCOMTele tsxmt = new sky6RASCOMTele();
            tsxmt.Connect();
            try
            {
                tsxmt.SetTracking(ionTrackingOn, useRates, 0, 0);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static string HourString(double ha, bool shorten)
        {
            //convert double hours to hh:mm:ss
            int iSign = Math.Sign(ha);
            int iHrs = (int)((double)iSign * ha);
            int iMin = (int)((((double)iSign * ha) - (double)iHrs) * 60.0);
            double dSec = (double)((((double)iSign * ha) - ((double)iHrs + ((double)iMin / 60.0))) * 60.0 * 60.0);
            if (shorten && iHrs == 0)
                return (iSign * iMin).ToString("00") + "m" + dSec.ToString("00") + "s";
            else
                return (iSign * iHrs).ToString("00") + "h" + iMin.ToString("00") + "m" + dSec.ToString("00") + "s";

        }

        public static string DegreeString(double d, bool shorten)
        {
            //convert double degrees to dd:mm:ss
            int iSign = Math.Sign(d);
            int iHrs = (int)((double)iSign * d);
            int iMin = (int)((((double)iSign * d) - (double)iHrs) * 60.0);
            double dSec = (double)((((double)iSign * d) - ((double)iHrs + ((double)iMin / 60.0))) * 60.0 * 60.0);
            if (shorten && iHrs == 0)
                return (iSign * iMin).ToString("00") + "m" + dSec.ToString("00") + "s";
            else
                return (iSign * iHrs).ToString("00") + "d" + iMin.ToString("00") + "m" + dSec.ToString("00") + "s";
        }
    }
}
