/*
* SearchMPES Class
*
* Class for downloading and parsing database NASA Horizons query results
* 
* This class serves as method template for conversions from all 
*  catalog sources
* 
* Author:           Rick McAlister
* Date:             1/18/22
* Current Version:  1.0
* Developed in:     Visual Studio 2019
* Coded in:         C# 8.0
* App Envioronment: Windows 10 Pro, .Net 5.0, TSX 5.0 Build 13258
* 
* API Doc: none
* 
* Change Log:
* 
* 
*/
using System;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using AstroMath;
using TheSky64Lib;

namespace Hot_Pursuit
{
    public class SearchMPES
    {
        const string URL_MPES_Search = "https://cgi.minorplanetcenter.net/cgi-bin/mpeph2.cgi?";

        #region column headers
        const string mUTDate = "UTDate";
        const string mUTHrMin = "UTHrMin";
        const string mRA = "RA";
        const string mDec = "Dec";
        const string mDelta = "Delta";
        const string mr = "Range";
        const string mEl = "Elevation";
        const string mPh = "Phase";
        const string mV = "MagnitudeV";
        const string mdRACosD = "dRACosD";
        const string mdDec = "dDec";
        const string mAzi = "Azimuth";
        const string mAlt = "Altitude";
        const string mSunAlt = "SunAlt";
        const string mMoonPhase = "MoonPhase";
        const string mMoonDist = "MoonDist";
        const string mMoonAlt = "MoonAlt";
        const string mUncertainty3sig = "Uncertainty3sig";
        const string mUncertaintyPh = "UncertaintyPh";
        //ignore the rest

        private string[] headerNames =
        {
            mUTDate,
            mUTHrMin,
            mRA,
            mDec,
            mDelta,
            mr,
            mEl,
            mPh,
            mV,
            mdRACosD,
            mdDec,
            mSunAlt,
            mMoonPhase,
            mMoonDist,
            mMoonAlt
        };
        #endregion

        public string? TgtName { get; set; }
        public DateTime EphStart { get; set; }
        public DateTime EphEnd { get; set; }
        public TimeSpan EphStep { get; set; }  //query results steps as time span
        public XDocument ScoutResultsX { get; set; }
        public List<SpeedVector> UpdateRateTable { get; set; }
        public Observatory MPC_Observatory { get; set; }
        public double RA_CorrectionD { get; set; } //Hours
        public double Dec_CorrectionD { get; set; } //Degrees
        public double Diff_RA_CorrectionD { get; set; } //Hours
        public double Diff_Dec_CorrectionD { get; set; } //Degrees
        public double RangeAU { get; set; } //AU
        public double Range_CorrectionAU { get; set; } //AU


        public double Site_Corrected_Range { get; set; }
        public double Site_Corrected_RA { get; set; }
        public double Site_Corrected_Dec { get; set; }


        public bool LoadTargetData(bool isMinutes, int updateInterval)
        {
            //Get site location and closest MPC observatory
            MPC_Observatory = new Observatory();

            //Get Geocentric ephemeris
            if (!GeoToSiteCalibration())
                return false;
            //Find geocentric ephemeris at current time (single ephemeris)
            //Find observatory ephemeris at current time (100 count)
            //Calculate geocentric to geodetic transformation for RA/Dec and dRA/dDec
            if (ServerQueryToSpeedVectors(isMinutes, updateInterval))
                return true;
            else
                return false;
        }

        public bool ServerQueryToSpeedVectors(bool isMinutes, int updateInterval)
        {
            string hzResultText;
            string urlSearch;
            WebClient client = new WebClient();
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
            try
            {
                urlSearch = URL_MPES_Search + MakeSearchQuery();
                hzResultText = client.DownloadString(urlSearch);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Download Error: " + ex.Message);
                return false;
            };
            //Check result
            if (!hzResultText.Contains("Date"))
            {
                MessageBox.Show("Target not found");
                return false;
            }
            //Convert Text to XML  -- JSON format is rudimentary and no better than text
            List<SpeedVector> BasicRateTable = new List<SpeedVector>();
            UpdateRateTable = new List<SpeedVector>();
            char[] spc = { ' ' };
            string[] mpcLineItems = hzResultText.Split('\n');
            int soeIdx = FindLine(mpcLineItems, "Date") + 2;
            int eoeIdx = FindLine(mpcLineItems, " These calculations have been performed on the") - 2;
            XElement ephmList = new XElement("Ephemeris");
            for (int i = soeIdx; i < eoeIdx; i++)
            {
                XElement ephmRecord = new XElement("Data");
                string[] columns = mpcLineItems[i].Split(spc, StringSplitOptions.RemoveEmptyEntries);
                ephmRecord.Add(new XElement(mUTDate, columns[0] + "-" + columns[1] + "-" + columns[2]));
                string timestring = columns[3];
                string sTime = timestring[0].ToString() + timestring[1].ToString() + ":" + timestring[2].ToString() + timestring[3].ToString() + ":" + timestring[4].ToString() + timestring[5].ToString();
                ephmRecord.Add(new XElement(mUTHrMin, sTime));
                for (int r = 2; r < headerNames.Count(); r++)
                    ephmRecord.Add(new XElement(headerNames[r], columns[r + 2]));
                ephmList.Add(ephmRecord);
            }

            //Convert XML to speed vector array
            SpeedVector currentSpeedVector;
            double changeRaArcSec = 0;
            double changeDecArcSec = 0;
            foreach (XElement ephX in ephmList.Elements("Data"))
            {
                string sDate = ephX.Element(mUTDate).Value;
                string sTime = ephX.Element(mUTHrMin).Value;
                string sUTDateTime = sDate + " " + sTime;
                DateTime sUT = Convert.ToDateTime(sUTDateTime);
                double sRA_D = Convert.ToDouble(ephX.Element(mRA).Value);
                double sDec_D = Convert.ToDouble(ephX.Element(mDec).Value);
                double sElevation_KM = MPC_Observatory.BestObservatory.MySiteElev;
                //Compute PA
                double sdDecdt = Convert.ToDouble(ephX.Element(mdDec).Value);  //arcsec/min
                double sdRAdt = Convert.ToDouble(ephX.Element(mdRACosD).Value);  //arcsec/min
                double sPA_D = Math.Atan2(sdDecdt, sdRAdt);
                double sRate = Convert.ToDouble(ephX.Element(mr).Value);
                currentSpeedVector = new SpeedVector
                {
                    Time_UTC = sUT,
                    Rate_ArcsecPerMinute = sRate,
                    Rate_RA_CosDec_ArcsecPerMinute = sdRAdt,
                    Rate_Dec_ArcsecPerMinute = sdDecdt,
                    RA_Degrees = sRA_D,  //Scout delivers RA in degrees
                    Dec_Degrees = sDec_D,
                    PA_Degrees = sPA_D,
                    //Range_AU = sRange  //AU
                    Elevation_KM = sElevation_KM
                };
                BasicRateTable.Add(currentSpeedVector);

                if (isMinutes)
                {
                    UpdateRateTable = BasicRateTable;
                }
                else
                {
                    //must add interpolated ephemeras
                    for (int bIdx = 0; bIdx < BasicRateTable.Count - 1; bIdx++)
                    {
                        UpdateRateTable.Add(BasicRateTable[bIdx]);
                        Interpolate intp = new Interpolate(BasicRateTable[bIdx], BasicRateTable[bIdx + 1], updateInterval);
                        foreach (SpeedVector sv in intp.WayPoints)
                            UpdateRateTable.Add(sv);
                    }
                }
            }
            return true;
        }

        private int FindLine(string[] sTable, string startPhrase)
        {
            //finds the first entry in sTable that starts with startPhrase
            for (int i = 0; i < sTable.Count(); i++)
            {
                if (sTable[i].StartsWith(startPhrase))
                    return i;
            }
            return -1;
        }

        private bool GeoToSiteCalibration()
        {
            return true;
        }

        public string GetTargetName()
        {
            sky6ObjectInformation tsxoi = new sky6ObjectInformation();
            tsxoi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_NAME1);
            string tgtName = tsxoi.ObjInfoPropOut;
            return tgtName;
        }

        public SpeedVector? GetNextRateUpdate(DateTime nextTime)
        {
            for (int i = 0; i < UpdateRateTable.Count; i++)
                if (UpdateRateTable[i].Time_UTC > nextTime)
                    return UpdateRateTable[i];
            return null;
        }

        public bool SlewToTarget(SpeedVector sv)
        {

            sky6RASCOMTele tsxmt = new sky6RASCOMTele();
            double tgtRAH = Transform.DegreesToHours(sv.RA_Degrees - RA_CorrectionD);
            double tgtDecD = sv.Dec_Degrees - Dec_CorrectionD;
            tsxmt.Connect();
            try
            {
                tsxmt.SlewToRaDec(tgtRAH, tgtDecD, TgtName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Slew Failed: " + ex.Message);
                return false;
            }
            return true;
        }

        public bool CLSToTarget(SpeedVector sv)
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

            double tgtRAH = Transform.DegreesToHours(sv.RA_Degrees - RA_CorrectionD);
            double tgtDecD = sv.Dec_Degrees - Dec_CorrectionD;
            tsxsc.Find(tgtRAH.ToString() + ", " + tgtDecD.ToString());
            tsxmt.Connect();
            //tsxmt.Asynchronous = 0;
            try
            {
                tsxmt.SlewToRaDec(tgtRAH, tgtDecD, GetTargetName());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Slew Failure: " + ex.Message);
                //return false;
            }
            //********** CLS AVOIDANCE CODE FOR SIMULATOR DEBUGGING PURPOSES
            //tsxsc.Find(TgtName);
            //return true;
            //*********************
            bool returnStatus = true;
            try
            {
                clsStatus = tsx_cl.exec();
            }
            catch (Exception ex)
            {
                returnStatus = false;
            }
            try
            {
                tsxsc.Find(TgtName);
            }
            catch (Exception ex)
            {
                returnStatus = true;
            }
            return returnStatus;
        }

        public bool SetTargetTracking(SpeedVector sv)
        {
            const int ionTrackingOn = 1;
            const int ionTrackingOff = 0;
            const int ignoreRates = 1;
            const int useRates = 0;

            double tgtRateRA = sv.Rate_RA_CosDec_ArcsecPerMinute;
            double tgtRateDec = sv.Rate_Dec_ArcsecPerMinute;
            sky6RASCOMTele tsxmt = new sky6RASCOMTele();
            tsxmt.Connect();
            try
            {
                //TSX expects tracking rates in arcsec/sec: convert it from arcsec/min
                tsxmt.SetTracking(ionTrackingOn, useRates, tgtRateRA / 60.0, tgtRateDec / 60.0);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool SetStandardTracking()
        {
            const int ionTrackingOn = 1;
            const int ionTrackingOff = 0;
            const int ignoreRates = 1;
            const int useRates = 0;

            sky6RASCOMTele tsxmt = new sky6RASCOMTele();
            tsxmt.Connect();
            try
            {
                tsxmt.SetTracking(ionTrackingOn, ignoreRates, 0, 0);
            }
            catch
            {
                return false;
            }
            return true;
        }

        #region APIFields
        const String mEphemerisType = "ty"; // type:  ty=e
        const String mTarget = "TextArea"; //:  TextArea=111+ate
        const String mStartDate = "d"; //: d= yyyy-mm-dd(2022-1-31)
        const String mNumberOfRecords = "l"; //: l=x
        const String mInterval = "i"; //: i=xx
        const String mIntervalUnits = "u"; //: u=d
        const String mUTOffset = "uto"; // (hours) :  uto=0
        const String mObservatoryCode = "c"; //: c= xxx
        const String mSiteLongitude = "long"; //: long= x.xxx
        const String mSiteLatitude = "lat"; //: lat= x.xxx
        const String mSiteAltitude = "alt"; //:alt = x.xxx
        const String mDataType = "raty"; //:  raty= “d” for decimal, “x” for decimal degrees
        const String mRADecMotions = "s"; //: s= t for together, c for separate
        const String mDisplayMotion = "m";  //: m=”s” for sec, “m” for min, “h” for hour
        const String mMeasureAzimuth = "adir"; // = "S"  //Measure Azimuths: adir=S for West from South Meridian, N for East from North Meridian
        const String moed = "oed";  // don't know
        const String mElementsOutput = "e"; //ElementsOutput:  e=”-2” for none
        const string mForcePerturbed = "fp"; // //ForcePerturbedEphemritas: fp = “y” or “n”
        const String mresoc = "resoc";  // don't know
        const String mtit = "tit";  // don't know
        const String mbu = "bu";  // don't know
        const String mch = "ch";  // set to "c"
        const String mce = "ce"; //set to  f
        const String mjs = "js"; //set to f
        const string mSuppressSun = "igd"; // SuppressSun: igd = “y” or “n” (default) Suppress output if sun above horizon
        const string mSuppressHorizon = "igd";  // igd = “y” or “n” (default) Suppress output if target is below horizon

        //Ephemeris Specific Parameters


        #endregion

        public string MakeSearchQuery()
        {
            //Returns a url string for querying the TNS website

            //figure out site location
            string siteLong = (360 - MPC_Observatory.BestObservatory.MySiteLong).ToString("0.000");  //converted to the 0-360 form that MPC likes it
            string siteLat = MPC_Observatory.BestObservatory.MySiteLat.ToString("0.000");
            string siteElev = MPC_Observatory.BestObservatory.MySiteElev.ToString("0.000");
            string center = siteLong + ":" + siteLat + ":" + siteElev;
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString[mEphemerisType] = "e";
            queryString[mTarget] = TgtName; // ";" means that it is a small body search for name
            queryString[mStartDate] = EphStart.ToString("yyyy-MM-dd"); // "2021-01-12";
            queryString[mNumberOfRecords] = "1440";  // one day's worth at 1 min
            queryString[mInterval] = "1";
            queryString[mIntervalUnits] = "m";
            queryString[mUTOffset] = "0";
            queryString[mObservatoryCode] = "";
            queryString[mSiteLongitude] = MPC_Observatory.BestObservatory.MySiteLong.ToString("0.000");
            queryString[mSiteLatitude] = MPC_Observatory.BestObservatory.MySiteLat.ToString("0.000");
            queryString[mSiteAltitude] = MPC_Observatory.BestObservatory.MySiteElev.ToString("0");
            queryString[mDataType] = "x"; //Decimal data
            queryString[mRADecMotions] = "s";  //Separate sky coordinates
            queryString[mDisplayMotion] = "m";  //motion in arcsec/min
            queryString[mMeasureAzimuth] = "S";
            queryString[moed] = "";
            queryString[mElementsOutput] = "-2";
            //queryString[mForcePerturbed] = "n";
            queryString[mresoc] = ""; // = "?";  // don't know
            queryString[mtit] = ""; // = "?";  // don't know
            queryString[mbu] = ""; // = "?";  // don't know
            queryString[mch] = "c";  // set to "c"
            queryString[mce] = "f"; //set to  f
            queryString[mjs] = "f"; // = "js"; //set to f
            //queryString[mSuppressSun] = "n"; // = "igd"; // SuppressSun: igd = “y” or “n” (default) Suppress output if sun above horizon
            //queryString[mSuppressHorizon] = "n"; // = "igd";  // igd = “y” or “n” (default) Suppress output if target is below horizon

            return queryString.ToString(); // Returns "key1=value1&key2=value2", all URL-encoded
        }
    }
}

