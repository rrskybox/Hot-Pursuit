﻿//#define simulator

using AstroMath;
using System;
using System.Windows.Forms;
using TheSky64Lib;

namespace Hot_Pursuit
{
    public static class Utils
    {
        public const double Astronomical_Unit = 149597870.700;  //km
        public const double Earth_Radius = 6371.0; //km

        public const int ionTrackingOn = 1;
        public const int ionTrackingOff = 0;
        public const int ignoreRates = 1;
        public const int useRates = 1;  //Don't use rates

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
            //Performs 
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
            sky6Utils tsxu = new sky6Utils();
            //Check to see if target is above horizon
            double tgtRAH = Transform.DegreesToHours(sv.RA_Degrees);
            double tgtDecD = sv.Dec_Degrees;
            tsxu.ConvertRADecToAzAlt(tgtRAH, tgtDecD);
            double tgtAzmD = tsxu.dOut0;
            double tgtAltD = tsxu.dOut1;
            if (tgtAltD <= 0)
            {
                MessageBox.Show("Slew failure: Target is below the horizon");
                return false;
            }
            if (IsPrecision)
            {
                //Clear any image reduction, otherwise full reduction might cause a problem
                ccdsoftCamera tsxcam = new ccdsoftCamera()
                {
                    ImageReduction = ccdsoftImageReduction.cdNone,
                    Asynchronous = 1 //make sure nothing else happens while setting this up
                };
                //Abort any ongoing imaging
                tsxcam.Abort();

            }
            bool returnStatus = true;
            tsxmt.Connect();
            tsxu.Precess2000ToNow(tgtRAH, tgtDecD);
            double jnRAH = tsxu.dOut0;
            double jnDecD = tsxu.dOut1;
            //tsxmt.Asynchronous = 0;
            try
            {
                tsxmt.SlewToRaDec(jnRAH, jnDecD, tgtName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Slew Failure: " + ex.Message);
                returnStatus = false;
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
            sky6Utils tsxu = new sky6Utils();
            tsxu.Precess2000ToNow(tgtRAH, tgtDecD);
            double jnRAH = tsxu.dOut0;
            double jnDecD = tsxu.dOut1;
            try
            {
                tsxmt.SlewToRaDec(jnRAH, jnDecD, tgtName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Slew Failed: " + ex.Message);
                return false;
            }
            return true;
        }

        public static bool SetTargetTracking(SpeedVector sv)
        {

            double tgtRateRA = sv.Rate_RA_ArcsecPerMinute;
            double tgtRateDec = sv.Rate_Dec_ArcsecPerMinute;

            sky6RASCOMTele tsxmt = new sky6RASCOMTele();
            tsxmt.Connect();
            if (SafetyCheck.IsAboveHorizon())
            {
                try
                {
                    //TSX expects tracking rates in arcsec/sec: convert it from arcsec/min
                    tsxmt.SetTracking(ionTrackingOn, useRates, tgtRateRA / 60.0, tgtRateDec / 60.0);
                }
                catch
                {
#if simulator
                return true;
#else
                    return false;
#endif
                }
            }
            else
            {
                Utils.StopTracking();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Retrieve tracking rate in arc min
        /// </summary>
        /// <returns>double dRA, double dDec</returns>
        public static (double, double) GetTargetTracking()
        {
            //Retrieve current tracking rates in arcmin
            sky6RASCOMTele tsxmt = new sky6RASCOMTele();
            tsxmt.Connect();
            double dRA = tsxmt.dRaTrackingRate * 60.0;
            double dDec = tsxmt.dDecTrackingRate * 60.0;
            return (dRA, dDec);
        }

        public static bool SetStandardTracking()
        {

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
                return (iSign * iMin).ToString("00") + "m" + dSec.ToString("00.0") + "s";
            else
                return (iSign * iHrs).ToString("00") + "h" + iMin.ToString("00") + "m" + dSec.ToString("00.0") + "s";

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

        public static (double, double) GetObjectRates()
        {
            //Get dRA/dDec rates for current target
            sky6ObjectInformation tsxo = new sky6ObjectInformation();
            tsxo.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_RA_RATE_ASPERSEC);
            double dRA = tsxo.ObjInfoPropOut;
            tsxo.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_DEC_RATE_ASPERSEC);
            double dDec = tsxo.ObjInfoPropOut;
            return (dRA, dDec);
        }

        public static (double, double) GetCurrentRADecPosition()
        {
            sky6RASCOMTele tsxm = new sky6RASCOMTele();
            tsxm.GetRaDec();
            sky6Utils tsxu = new sky6Utils();
            tsxu.PrecessNowTo2000(tsxm.dRa, tsxm.dDec);
            return (tsxu.dOut0, tsxu.dOut1);
        }

        public static (double, double) GetCurrentAzAltPosition()
        {
            sky6RASCOMTele tsxm = new sky6RASCOMTele();
            sky6Utils tsxu = new sky6Utils();
            tsxm.GetAzAlt();
            return (tsxu.dOut0, tsxu.dOut1);
        }

        public static void StopTracking()
        {
            sky6RASCOMTele tsxm = new sky6RASCOMTele();
            tsxm.SetTracking(ionTrackingOff, ignoreRates, 0, 0);
            return;
        }

    }
}
