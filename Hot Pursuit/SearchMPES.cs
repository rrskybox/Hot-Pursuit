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

        //Column Positions
        const int colUTDateY = 0;
        const int colUTDateM =5;
        const int colUTDateD = 8;
        const int colUTHrMin = 11;
        const int colRA = 18;
        const int colDec = 29;
        const int colDelta = 39;
        const int colr = 48;
        const int colEl = 55;
        const int colPh =62;
        const int colV = 68;
        const int coldRACosD = 74;
        const int coldDec = 82;
        const int colAzi = 91;
        const int colAlt = 98;
        const int colSunAlt = 104;
        const int colMoonPhase = 109;
        const int colMoonDist = 118;
        const int colMoonAlt = 122;


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
                string mpcDataLine = mpcLineItems[i];
                ephmRecord.Add(new XElement(mUTDate, mpcDataLine.Substring(colUTDateY,4) + "-" + mpcDataLine.Substring(colUTDateM,2)+ "-" + mpcDataLine.Substring(colUTDateD,2)));
                string timestring = mpcDataLine.Substring(colUTHrMin,6);
                string sTime = timestring[0].ToString() + timestring[1].ToString() + ":" + timestring[2].ToString() + timestring[3].ToString() + ":" + timestring[4].ToString() + timestring[5].ToString();
                ephmRecord.Add(new XElement(mUTHrMin, sTime));
                ephmRecord.Add(new XElement(mRA, mpcDataLine.Substring(colRA,8)));
                ephmRecord.Add(new XElement(mDec, mpcDataLine.Substring(colDec,9)));
                ephmRecord.Add(new XElement(mDelta, mpcDataLine.Substring(colDelta,9)));
                ephmRecord.Add(new XElement(mr, mpcDataLine.Substring(colr,7)));
                ephmRecord.Add(new XElement(mEl, mpcDataLine.Substring(colEl,6)));
                ephmRecord.Add(new XElement(mPh, mpcDataLine.Substring(colPh,5)));
                ephmRecord.Add(new XElement(mV, mpcDataLine.Substring(colV,5)));
                ephmRecord.Add(new XElement(mdRACosD, mpcDataLine.Substring(coldRACosD,8)));
                ephmRecord.Add(new XElement(mdDec, mpcDataLine.Substring(coldDec,8)));
                ephmRecord.Add(new XElement(mAzi, mpcDataLine.Substring(colAzi,6)));
                ephmRecord.Add(new XElement(mAlt, mpcDataLine.Substring(colAlt,5)));
                ephmRecord.Add(new XElement(mSunAlt, mpcDataLine.Substring(colSunAlt,5)));
                ephmRecord.Add(new XElement(mMoonPhase, mpcDataLine.Substring(colMoonPhase,6)));
                ephmRecord.Add(new XElement(mMoonDist, mpcDataLine.Substring(colMoonDist,5)));
                ephmRecord.Add(new XElement(mMoonAlt, mpcDataLine.Substring(colMoonAlt,4)));

                ephmList.Add(ephmRecord);
            }

            //Convert XML to speed vector array
            SpeedVector currentSpeedVector;
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
                double sRange = Convert.ToDouble(ephX.Element(mr).Value);

                currentSpeedVector = new SpeedVector
                {
                    Time_UTC = sUT,
                    Rate_RA_CosDec_ArcsecPerMinute = sdRAdt,
                    Rate_Dec_ArcsecPerMinute = sdDecdt,
                    RA_Degrees = sRA_D,  //Scout delivers RA in degrees
                    Dec_Degrees = sDec_D,
                    PA_Degrees = sPA_D,
                    Range_AU = sRange,  //AU
                    Elevation_KM = sElevation_KM
                };
                BasicRateTable.Add(currentSpeedVector);
            }
            if (isMinutes)
            {
                UpdateRateTable = BasicRateTable;
            }
            else
            {
                //must add interpolated ephemeras
                //Horizons delivers a full day worth of data at 1 minute intervals
                // throwaway all but the first hour (60 readings) so we don't run out of memory interpolating
                for (int bIdx = 0; bIdx < BasicRateTable.Count - 1; bIdx++)
                {
                    if (BasicRateTable[bIdx].Time_UTC > DateTime.UtcNow && BasicRateTable[bIdx].Time_UTC < DateTime.UtcNow + TimeSpan.FromHours(1))
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

        public static string ScrubSmallBodyName(string longName)
        {
            //Decoder for small body name input.  MPES does not do well parsing numbers and names sometimes
            //This program reduces a standard comet, asteroid or other name to something
            //  that MPES can search for.
            //Comets will start with P/ or C/.  Horizons cant search that so it must be scrubbed.
            string scrub;
            //  Anything trailing parenthesis (e.g. (PANSTARRS) must be removed.
            if (longName.Contains("("))
            {
                scrub = longName.Remove(longName.IndexOf('(')).TrimEnd(new char[] { ' ' });
            }
            else
            {
                scrub = longName;
            }
            return scrub;
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
        const String mRADecMotions = "s"; //: s= t for total, c for coordinate motion, s for sky motion
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

            string scrubbedTgtName = ScrubSmallBodyName(TgtName);
            //figure out site location
            string siteLong = (360 - MPC_Observatory.BestObservatory.MySiteLong).ToString("0.000");  //converted to the 0-360 form that MPC likes it
            string siteLat = MPC_Observatory.BestObservatory.MySiteLat.ToString("0.000");
            string siteElev = MPC_Observatory.BestObservatory.MySiteElev.ToString("0.000");
            string center = siteLong + ":" + siteLat + ":" + siteElev;
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString[mEphemerisType] = "e";
            queryString[mTarget] = scrubbedTgtName;
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
            queryString[mRADecMotions] = "c";  // t for total, c for coordinate motion, s for sky motion 
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
            string q = queryString.ToString();
            // May be problem with "/" = %2F rather than %2f
            q = q.Replace("%2f", "%2F");
            return q; // Returns "key1=value1&key2=value2", all URL-encoded
        }
    }
}

