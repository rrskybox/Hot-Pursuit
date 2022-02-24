/*
* SearchHorizons Class
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
* API Doc: https://ssd-api.jpl.nasa.gov/doc/horizons.html
* 
* Change Log:
* 
* Added this class to make V1.1 Release (1/18/21)
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
    public class SearchHorizons
    {
        const string URL_Horizons_Search = "https://ssd.jpl.nasa.gov/api/horizons.api?";
        //const string URL_Horizons_Search = "https://cgi.minorplanetcenter.net/cgi-bin";

        #region column headers
        const string hzUTDate = "UTDate";
        const string hzUTHrMin = "UTHrMin";
        const string hzRA = "RA";
        const string hzDec = "Dec";
        const string hzRA_a = "RA_a";
        const string hzDec_a = "Dec_a";
        const string hzdRACosD = "dRACosD";
        const string hzdDec = "dDec";
        const string hzAzim = "Azim";
        const string hzElev = "Elev";
        const string hzdAzCosE = "dAzCosE";
        const string hzdELV = "dELV";
        const string hzX = "X";
        const string hzY = "Y";
        const string hzStaPang = "StaPang";
        const string hzL_Ap_Sid_Time = "L_Ap_Sid_Time";
        const string hza_mass = "a_mass";
        const string hzmag_ex = "mag_ex";
        const string hzAPmag = "APmag";
        const string hzS_brt = "S_brt";
        const string hzIllu = "Illu";
        const string hzDef_illu = "Def_illu";
        const string hzang_sep = "ang_sep";
        const string hzAng_diam = "Ang_diam";
        const string hzObsSub_LON = "ObsSub_LON";
        const string hzObsSub_LAT = "ObsSub_LAT";
        const string hzSunSub_LON = "SunSub_LON";
        const string hzSunSub_LAT = "SunSub_LAT";
        const string hzSN_ang = "SN_ang";
        const string hzSN_dist = "SN_dist";
        const string hzNP_ang = "NP_ang";
        const string hzNP_dist = "NPT_dist";
        const string hzhEcl_Lon = "hEcl_Lon";
        const string hzhEcl_Lat = "hEcl_Lat";
        const string hzr = "r";
        const string hzrdot = "rdot";
        const string hzdelta = "delta";
        const string hzdeldot = "deldot";
        const string hzOne_way_down = "One_way_down";
        const string hzVmagSn = "VmagSn";
        const string hzVmagOb = "VmagOb";
        const string hzS_O_T = "S_O_T";
        const string hzSlashR = "SlashR";
        const string hzS_T_O = "S_T_O";
        const string hzT_O_M = "T_O_M";
        const string hzO_P_T = "O_P_T";
        const string hzPsAng = "PsAng";
        const string hzPsAMV = "PsAMV";
        const string hzPlAng = "PlAng";
        const string hzCnst = "Cnst";
        const string hzTDB_UT = "TDB_UT";
        const string hzObsEcLon = "ObsEcLon";
        const string hzObsEcLat = "ObsEcLat";
        const string hzN_Pole_RA = "N_Pole_RA";
        const string hzN_Pole_DC = "N_Pole_DC";
        const string hzGlxLon = "GlxLon";
        const string hzGlxLat = "GlxLat";
        const string hzL_Ap_SOL_Time = "L_Ap_SOL_Time";
        const string hzThree99_ins_LT = "Three99_ins_LT";
        const string hzRA_3sigma = "RA_3sigma";
        const string hzDEC_3sigma = "DEC_3sigma";
        const string hzSMAA_3sig = "SMAA_3sig";
        const string hzSMIA_3sig = "SMIA_3sig";
        const string hzTheta = "Theta";
        const string hzArea_3sig = "Area_3sig";
        const string hzPOS_3sigma = "POS_3sigma";
        const string hzRNG_3sigma = "RNG_3sigma";
        const string hzRNGRT_3sig = "RNGRT_3sig";
        const string hzDOP_S_3sig = "DOP_S_3sig";
        const string hzDOP_X_3sig = "DOP_X_3sig";
        const string hzRT_delay_3sig = "RT_delay_3sig";
        const string hzTru_Anom = "Tru_Anom";
        const string hzL_Ap_Hour_Ang = "L_Ap_Hour_Ang";
        const string hzphi = "phi";
        const string hzPAB_LON = "PAB_LON";
        const string hzPAB_LAT = "PAB_LAT";
        const string hzApp_Lon_Sun = "App_Lon_Sun";
        const string hzRA_ICRF_a_app = "RA_ICRF_a_app";
        const string hzI_dRAcosD = "I_dRAcosD";
        const string hzIdDEC = "IdDEC";
        const string hzSky_motion = "Sky_motion";
        const string hzSky_mot_PA = "Sky_mot_PA";
        const string hzRelVel_ANG = "RelVel_ANG";
        const string hzLun_Sky_Brt = "Lun_Sky_Brt";
        const string hzsky_SNR = "sky_SNR";

        private string[] headerNames =
        {
            hzUTDate,
            hzUTHrMin,
            hzRA,
            hzDec,
            hzRA_a,
            hzDec_a,
            hzdRACosD,
            hzdDec,
            hzAzim,
            hzElev,
            hzdAzCosE,
            hzdELV,
            hzX,
            hzY,
            hzStaPang,
            hzL_Ap_Sid_Time,
            hza_mass,
            hzmag_ex,
            hzAPmag,
            hzS_brt,
            hzIllu,
            hzDef_illu,
            hzang_sep,
            hzAng_diam,
            hzObsSub_LON,
            hzObsSub_LAT,
            hzSunSub_LON,
            hzSunSub_LAT,
            hzSN_ang,
            hzSN_dist,
            hzNP_ang,
            hzNP_dist,
            hzhEcl_Lon,
            hzhEcl_Lat,
            hzr,
            hzrdot,
            hzdelta,
            hzdeldot,
            hzOne_way_down,
            hzVmagSn,
            hzVmagOb,
            hzS_O_T,
            hzSlashR,
            hzS_T_O,
            hzT_O_M,
            hzO_P_T,
            hzPsAng,
            hzPsAMV,
            hzPlAng,
            hzCnst,
            hzTDB_UT,
            hzObsEcLon,
            hzObsEcLat,
            hzN_Pole_RA,
            hzN_Pole_DC,
            hzGlxLon,
            hzGlxLat,
            hzL_Ap_SOL_Time,
            hzThree99_ins_LT,
            hzRA_3sigma,
            hzDEC_3sigma,
            hzSMAA_3sig,
            hzSMIA_3sig,
            hzTheta,
            hzArea_3sig,
            hzPOS_3sigma,
            hzRNG_3sigma,
            hzRNGRT_3sig,
            hzDOP_S_3sig,
            hzDOP_X_3sig,
            hzRT_delay_3sig,
            hzTru_Anom,
            hzL_Ap_Hour_Ang,
            hzphi,
            hzPAB_LON,
            hzPAB_LAT,
            hzApp_Lon_Sun,
            hzRA_ICRF_a_app,
            hzI_dRAcosD,
            hzIdDEC,
            hzSky_motion,
            hzSky_mot_PA,
            hzRelVel_ANG,
            hzLun_Sky_Brt,
            hzsky_SNR
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

            //Don't need geocentric ephemeris for Horizons -- using topocentric
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
                urlSearch = URL_Horizons_Search + MakeSearchQuery();
                hzResultText = client.DownloadString(urlSearch);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Download Error: " + ex.Message);
                return false;
            };
            //Check result
            if (!hzResultText.Contains("$$SOE"))
            {
                MessageBox.Show("Target not found");
                return false;
            }
            //Convert Text to XML  -- JSON format is rudimentary and no better than text
            List<SpeedVector> BasicRateTable = new List<SpeedVector>();
            UpdateRateTable = new List<SpeedVector>();
            char[] spc = { ' ' };
            string[] hzLineItems = hzResultText.Split('\n');
            int soeIdx = Array.IndexOf(hzLineItems, "$$SOE");
            int eoeIdx = Array.IndexOf(hzLineItems, "$$EOE");
            string[] headers = hzLineItems[soeIdx - 2].Split(spc, StringSplitOptions.RemoveEmptyEntries);
            XElement ephmList = new XElement("Ephemeris");
            for (int i = soeIdx + 1; i < eoeIdx; i++)
            {
                XElement ephmRecord = new XElement("Data");
                string cleanLine = hzLineItems[i].Remove(22, 2);  //Clear out some garbage that Horizons leaves in the line for some reason -- minutes? seconds?
                string[] columns = cleanLine.Split(spc, StringSplitOptions.RemoveEmptyEntries);
                for (int r = 0; r < headers.Count(); r++)
                    ephmRecord.Add(new XElement(headerNames[r], columns[r]));
                ephmList.Add(ephmRecord);
            }

            //Convert XML to speed vector array
            SpeedVector currentSpeedVector;
            foreach (XElement ephX in ephmList.Elements("Data"))
            {
                string sDate = ephX.Element(hzUTDate).Value;
                string sTime = ephX.Element(hzUTHrMin).Value;
                string sUTDateTime = sDate + " " + sTime;
                sUTDateTime = sUTDateTime.Replace('.', ' ');
                DateTime sUT = Convert.ToDateTime(sUTDateTime);
                double sRA_D = Convert.ToDouble(ephX.Element(hzRA).Value);
                double sDec_D = Convert.ToDouble(ephX.Element(hzDec).Value);
                double sElevation_KM = MPC_Observatory.BestObservatory.MySiteElev;
                //Compute PA
                double sdDecdt = Convert.ToDouble(ephX.Element(hzdDec).Value) / 60;  //convert to arcsec/min
                double sdRAdt = Convert.ToDouble(ephX.Element(hzdRACosD).Value) / 60;  //convert to arcsec/min
                double sPA_D = Math.Atan2(sdDecdt, sdRAdt);
                double sRange = Convert.ToDouble(ephX.Element(hzr).Value);
               
                currentSpeedVector = new SpeedVector
                {
                    Time_UTC = sUT,
                    //Rate_ArcsecPerMinute = sRate,
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

        public SpeedVector? GetNextRateUpdate(DateTime nextTime)
        {
            for (int i = 0; i < UpdateRateTable.Count; i++)
                if (UpdateRateTable[i].Time_UTC > nextTime)
                    return UpdateRateTable[i];
            return null;
        }

        public static string ScrubSmallBodyName(string longName)
        {
            //Decoder for small body name input.  Horizon's does not do well parsing numbers and names
            //This program reduces a standard comet, asteroid or other name to something
            //  that Horizons can search for.
            //Comets will start with P/ or C/.  Horizons cant search that so it must be scrubbed.
            //  Anything trailing (e.g. (PANSTARRS) must be removed as well.
            //if (longName.StartsWith("P/") || longName.StartsWith("C/"))
            string scrub;
            if (longName.Contains("/"))
            {
                //We got a comet, probably from TSX SDB
                //Several formats are possible:  P/asdfa, nnnP/asdfe, C/yyyy aD
                if (char.IsDigit(longName[0]))
                    scrub = longName;
                else
                {
                    string[] shortStrings = (longName.Remove(0, 2)).Split(' ');
                    scrub = shortStrings[0] + " " + shortStrings[1] + ";";  //Small Body ";"
                }
            }
            else
            {
                //Asteroid or something not an asteroid
                //this could be in the format of a single name (e.g. JWST),
                //  a comet designation (e.g. 2021 A7),
                //  or a asteroid designation (e.g. 7 Isis)
                //So, if it is a single name, we pass it through with no small body search designator (";")
                string[] splits = longName.Split(' ');
                if (splits.Count() < 2)
                    scrub = longName;  //single name format, e.g. "JWST", and so leave off the small body search designator
                else if (splits[1].All(Char.IsLetter)) //Comet -- 2021 A7 or Asteroid 7 Isis
                    scrub = splits[1] + ";"; //Asteroid format ( e.g. 7 Isis) so return just the name and small body search designator (";")
                else
                    scrub = splits[0] + " " + splits[1] + ";";  //Comet format (e.g. 2021 A7) so return the first two fields and small body search designator (";")
            }
            return scrub;
        }

        #region HorizonsFields
        const string hFormatTypeText = "text";
        const string hFormatTypeJSON = "json";
        const string hYes = "YES";
        const string hNo = "NO";
        const string hObserverType = "OBSERVER";
        const string hVectorsType = "VECTORS";
        const string hElementsType = "hELEMENTS";
        const string hSPKType = "SPK";
        const string hApproachType = "APPROACH";
        const string hUnitTypeKMS = "KM-S";
        const string hUnitTypeAUD = "AU-D";
        const string hUnitTypeKMD = "KM-D";
        const string hAngleFormatDegrees = "DEG";

        //Common Parameters
        const string hFormat = "format"; //format json    json, text specify output format: json for JSON or text for plain-text
        const string hCommand = "COMMAND"; //COMMAND     none see details below   target search, selection, or enter user-input object mode   link
        const string hDes = "DES"; //small body key word
        const string hName = "NAME"; //NAME for small body search
        const string hObjectData = "OBJ_DATA"; //OBJ_DATA    YES NO, YES     toggles return of object summary data
        const string hMakeEphemeris = "MAKE_EPHEM"; //MAKE_EPHEM  YES NO, YES     toggles generation of ephemeris, if possible
        const string hEphemerisType = "EPHEM_TYPE"; //EPHEM_TYPE  OBSERVER OBSERVER, VECTORS, ELEMENTS, SPK, APPROACH  selects type of ephemeris to generate(see details below)
        const string hEmailAddress = "EMAIL_ADDR"; //EMAIL_ADDR none    any valid email address     optional; used only in the event of highly unlikely problems needing follow-up

        //Ephemeris Specific Parameters

        const string hCenter = "CENTER";
        const string hReferencePlane = "REF_PLANE";
        const string hCoordinateType = "COORD_TYPE";
        const string hSiteCoordinate = "SITE_COORD";
        const string hStartTime = "START_TIME";
        const string hStopTime = "STOP_TIME";
        const string hStepSize = "STEP_SIZE";
        const string hTList = "TLIST";
        const string hTListTyp = "TLIST_TYPE";
        const string hQuantities = "QUANTITIES";
        const string hReferenceSystem = "REF_SYSTEM";
        const string hOutUnits = "OUT_UNITS";
        const string hVectorTable = "VEC_TABLE";
        const string hVectorCorrection = "VEC_CORR";
        const string hCalibrationFormat = "CAL_FORMAT";
        const string hAngleFormat = "ANG_FORMAT";
        const string hApparent = "APPARENT";
        const string hTimeDigits = "TIME_DIGITS";
        const string hTimeZone = "TIME_ZONE";
        const string hRangeUnits = "RANGE_UNITS";
        const string hSuppressRangeRate = "SUPPRESS_RANGE_RATE";
        const string hElevationCut = "ELEV_CUT";
        const string hSkipDayLight = "SKIP_DAYLT";
        const string hSolarElongation = "SOLAR_ELONG";
        const string hAirMass = "AIRMASS";
        const string hLHACutoff = "LHA_CUTOFF";
        const string hAngularRateCutoff = "ANG_RATE_CUTOFF";
        const string hExtraPrecisionFormat = "EXTRA_PREC";
        const string hCSVFormat = "CSV_FORMAT";
        const string hVectorLabels = "VEC_LABELS";
        const string hVectorDeltaT = "VEC_DELTA_T";
        const string hElmLabels = "ELM_LABELS";
        const string hTPType = "TP_TYPE";
        const string hRTSOnly = "R_T_S_ONLY";


        #endregion

        public string MakeSearchQuery()
        {
            //Returns a url string for querying the TNS website

            //figure out site location
            string scrubbedTargetName = ScrubSmallBodyName(TgtName);
            string siteLong = (360 - MPC_Observatory.BestObservatory.MySiteLong).ToString("0.000");  //converted to the 0-360 form that MPC likes it
            string siteLat = MPC_Observatory.BestObservatory.MySiteLat.ToString("0.000");
            string siteElev = MPC_Observatory.BestObservatory.MySiteElev.ToString("0.000");
            string center = siteLong + ":" + siteLat + ":" + siteElev;
            string startTime = EphStart.ToString("yyyy-MMM-dd");
            string endTime = EphEnd.ToString("yyyy-MMM-dd");
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString[hFormat] = hFormatTypeText;
            queryString[hCommand] = "\'NAME=" + scrubbedTargetName + "\'"; // ";" means that it is a small body search for name
            queryString[hMakeEphemeris] = hYes;
            queryString[hEphemerisType] = hObserverType;
            queryString[hCenter] = "399";  //Earth
            queryString[hSiteCoordinate] = center;  //e-long(degrees):lat(degrees):elevation(km)
            queryString[hStartTime] = startTime; // "2021-01-12";
            queryString[hStopTime] = endTime; // "2021-01-13";
            queryString[hStepSize] = "1m"; // shortest time that horizons can do
            queryString[hAngleFormat] = hAngleFormatDegrees;
            queryString[hTimeDigits] = "Seconds";
            //queryString[hQuantities ] = "'1,9,20,23,24,29'";
            queryString[hOutUnits] = hUnitTypeKMS;
            queryString[hExtraPrecisionFormat] = hYes;
            string q = queryString.ToString();
            //fix bug where queryString inserts %2f instead of %2F for the "/" char
            q = q.Replace("%2f", "%2F");
            return q; // Returns "key1=value1&key2=value2", all URL-encoded
        }
    }
}

