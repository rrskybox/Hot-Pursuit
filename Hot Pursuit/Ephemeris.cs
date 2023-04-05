using AstroMath;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Hot_Pursuit
{
    public class Ephemeris
    {
        public enum EphemSource
        {
            Scout,
            MPES,
            Horizons,
            HorizonsSat,
            HorizonsTLE
        }

        public bool HasData { get; set; } = false;
        public string? TgtName { get; set; }
        public DateTime EphStart { get; set; }
        public DateTime EphEnd { get; set; }
        public TimeSpan EphStep { get; set; }  //query results steps as time span
        public XDocument ScoutResultsX { get; set; }
        public List<SpeedVector> UpdateRateTable { get; set; }
        public Observatory MPC_Observatory { get; set; }
        public double RA_CorrectionD { get; set; } //Hours
        public double Dec_CorrectionD { get; set; } //Degrees
        public double RangeAU { get; set; } //AU
        public double Range_CorrectionAU { get; set; } //AU
        public double Site_Corrected_Range { get; set; }
        public double Site_Corrected_RA { get; set; }
        public double Site_Corrected_Dec { get; set; }
        public double Topo_RA_Correction_Factor { get; set; } = 1;
        public double Topo_Dec_Correction_Factor { get; set; } = 1;

        public Ephemeris(EphemSource eps, string targetName, bool isMinutes, int updateRate)
        {
            TgtName = targetName;
            EphStart = DateTime.UtcNow;
            if (isMinutes)
            {
                EphStep = TimeSpan.FromMinutes(updateRate);
                EphEnd = EphStart + TimeSpan.FromMinutes((100 * updateRate));
            }
            else
            {
                EphStep = TimeSpan.FromSeconds(updateRate);
                EphEnd = EphStart + TimeSpan.FromSeconds(EphStep.TotalSeconds * 600);  //10 minutes for seconds data
            }
            switch (eps)
            {
                case EphemSource.Scout:
                    {
                        HasData = DownloadScoutData(isMinutes, updateRate);
                        break;
                    }
                case EphemSource.Horizons:
                    {
                        HasData = DownloadHorizonsData(isMinutes, updateRate, EphemSource.Horizons);
                        break;
                    }
                case EphemSource.MPES:
                    {
                        HasData = DownloadMPESData(isMinutes, updateRate);
                        break;
                    }
                case EphemSource.HorizonsSat:
                    {
                        HasData = DownloadHorizonsData(isMinutes, updateRate, EphemSource.HorizonsSat);
                        break;
                    }
                case EphemSource.HorizonsTLE:
                    {
                        HasData = DownloadHorizonsData(isMinutes, updateRate, EphemSource.HorizonsTLE);
                        break;
                    }
            }
            return;
        }

        public Ephemeris(EphemSource eps, string targetName, bool isMinutes, int updateRate, int minutes)
        {
            TgtName = targetName;
            EphStart = DateTime.UtcNow;
            if (isMinutes)
            {
                EphStep = TimeSpan.FromMinutes(updateRate);
                EphEnd = EphStart + TimeSpan.FromMinutes(minutes);
            }
            else
            {
                EphStep = TimeSpan.FromSeconds(updateRate);
                EphEnd = EphStart + TimeSpan.FromSeconds(EphStep.TotalSeconds * 600);  //10 minutes for seconds data
            }
            switch (eps)
            {
                case EphemSource.Scout:
                    {
                        HasData = DownloadScoutData(isMinutes, updateRate);
                        break;
                    }
                case EphemSource.Horizons:
                    {
                        HasData = DownloadHorizonsData(isMinutes, updateRate, EphemSource.Horizons);
                        break;
                    }
                case EphemSource.MPES:
                    {
                        HasData = DownloadMPESData(isMinutes, updateRate);
                        break;
                    }
                case EphemSource.HorizonsSat:
                    {
                        HasData = DownloadHorizonsData(isMinutes, updateRate, EphemSource.HorizonsSat);
                        break;
                    }
                case EphemSource.HorizonsTLE:
                    {
                        HasData = DownloadHorizonsData(isMinutes, updateRate, EphemSource.HorizonsTLE);
                        break;
                    }
            }
            return;
        }

        #region EphemXFields

        const string xUTDate = "UTDate";
        const string xUTHrMin = "UTHrMin";
        const string xRA = "RA";
        const string xDec = "Dec";
        const string xDelta = "Delta";
        const string xRng = "Range";
        const string xEl = "Elevation";
        const string xPh = "Phase";
        const string xV = "MagnitudeV";
        const string xdRACosD = "dRACosD";
        const string xdDec = "dDec";
        const string xPA = "PA";
        const string xdRate = "Rate";

        //const string xAzi = "Azimuth";
        //const string xAlt = "Altitude";
        //const string xSunAlt = "SunAlt";
        //const string xMoonPhase = "MoonPhase";
        //const string xMoonDist = "MoonDist";
        //const string xMoonAlt = "MoonAlt";
        //const string xUncertainty3sig = "Uncertainty3sig";
        //const string xUncertaintyPh = "UncertaintyPh";

        #endregion

        private bool EphemerisListToSpeedVector(XElement ephmList, bool isMinutes, int updateInterval)
        {
            //DateTime beginningTime = DateTime.UtcNow;
            //Convert XML to speed vector array
            List<SpeedVector> BasicRateTable = new List<SpeedVector>();
            UpdateRateTable = new List<SpeedVector>();
            foreach (XElement ephX in ephmList.Elements("Data"))
            {
                string sDate = ephX.Element(xUTDate).Value;
                string sTime = ephX.Element(xUTHrMin).Value;
                string sUTDateTime = sDate + " " + sTime;
                DateTime sUT = Convert.ToDateTime(sUTDateTime);
                double sRA_D = Convert.ToDouble(ephX.Element(xRA).Value);
                double sDec_D = Convert.ToDouble(ephX.Element(xDec).Value);
                double sElevation_KM = MPC_Observatory.BestObservatory.MySiteElev;
                //Compute PA
                double sdDecdt = Convert.ToDouble(ephX.Element(xdDec).Value);  //arcsec/min
                double sdRACosDecdt = Convert.ToDouble(ephX.Element(xdRACosD).Value);  //arcsec/min
                double sdRAdt = sdRACosDecdt / Math.Cos(AstroMath.Transform.DegreesToRadians(sDec_D));
                double sPA_D = Math.Atan2(sdDecdt, sdRACosDecdt);
                double sRange = Convert.ToDouble(ephX.Element(xRng).Value);

                BasicRateTable.Add(new SpeedVector
                {
                    Time_UTC = sUT,
                    RA_Degrees = sRA_D,  //Scout delivers RA in degrees
                    Dec_Degrees = sDec_D,
                    Rate_RA_CosDec_ArcsecPerMinute = sdRACosDecdt,
                    Rate_Dec_ArcsecPerMinute = sdDecdt,
                    Rate_RA_ArcsecPerMinute = sdRAdt,
                    PA_Degrees = sPA_D,
                    Range_AU = sRange,  //AU
                    Elevation_KM = sElevation_KM
                });
            }
            if (isMinutes)
            {
                UpdateRateTable = BasicRateTable;
            }
            else
            {
                //must add interpolated ephemeras
                //Horizons delivers a full day worth of data at 1 minute intervals
                // only do the first 10 minutes
                DateTime maxTime = BasicRateTable[0].Time_UTC + TimeSpan.FromMinutes(10);
                for (int bIdx = 0; (bIdx < BasicRateTable.Count - 1 && BasicRateTable[bIdx].Time_UTC < maxTime); bIdx++)
                {
                    UpdateRateTable.Add(BasicRateTable[bIdx]);
                    Interpolate intp = new Interpolate(BasicRateTable[bIdx], BasicRateTable[bIdx + 1], updateInterval);
                    foreach (SpeedVector sv in intp.WayPoints)
                        UpdateRateTable.Add(sv);
                }

            }
            UpdateRateTable.Sort((x, y) => x.Time_UTC.CompareTo(y.Time_UTC));
            return true;
        }

        public List<SDBDesigner.TargetData> SpeedVectorToTargetData()
        {
            //Returns TargetData formated list of speedvector data
            //  Convert RA degrees to RA hours
            List<SDBDesigner.TargetData> tdList = new List<SDBDesigner.TargetData>();
            foreach (SpeedVector sv in UpdateRateTable)
                tdList.Add(new SDBDesigner.TargetData
                {
                    TargetName = sv.Time_UTC.ToString(),
                    TargetRA = Utility.DegreesToHours(sv.RA_Degrees),
                    TargetDec = sv.Dec_Degrees,
                    TargetMag = 0
                });
            return tdList;
        }

        public SpeedVector? GetNearestRateUpdate(DateTime nearTime)
        {
            TimeSpan closeApproach = TimeSpan.MaxValue;
            int closest = 0;
            for (int i = 0; i < UpdateRateTable.Count; i++)
            {
                TimeSpan interval = (UpdateRateTable[i].Time_UTC - nearTime).Duration();
                if (interval < closeApproach)
                {
                    closest = i;
                    closeApproach = interval;
                }
            }
            if (closest != UpdateRateTable.Count - 1)
                return UpdateRateTable[closest];
            else
                return null;
        }


        #region scout

        const string URL_NEO_search = "https://ssd-api.jpl.nasa.gov/scout.api?";
        const int SAMPLEORBITS = 1000;
        const string GEOCENTRIC_OBSERVATORY_CODE = "500";

        const int idx_ra = 0;
        const int idx_dec = 1;
        const int idx_rate = 2;
        const int idx_pa = 3;
        const int idx_vmag = 4;
        const int idx_elong = 5;
        const int idx_moon = 6;
        const int idx_neo = 7;
        const int idx_pha = 8;
        const int idx_geo = 9;
        const int idx_imp = 10;

        const int idx_reG_Geo = 11;
        const int idx_reO_Geo = 12;
        const int idx_rs_Geo = 13;

        const int idx_el_Topo = 11;
        const int idx_reG_Topo = 12;
        const int idx_reO_Topo = 13;
        const int idx_rs_Topo = 14;

        private bool DownloadScoutData(bool isMinutes, int updateInterval)
        {

            //Get site locatino and closest observatory
            MPC_Observatory = new Observatory();
            //Get Geocentric ephemeris
            if (!GeoToScoutSiteCalibration())
                return false;
            //Topo_Dec_Correction_Factor = 1;
            //Topo_RA_Correction_Factor = 1;
            //Find geocentric ephemeris at current time (single ephemeris)
            //Find observatory ephemeris at current time (100 count)
            //Calculate geocentric to geodetic transformation for RA/Dec and dRA/dDec
            if (ScoutQueryToSpeedVectors(isMinutes, updateInterval))
                return true;
            else
                return false;
        }

        private bool ScoutQueryToSpeedVectors(bool isMinutes, int updateInterval)
        {
            string neoResultText;
            string urlSearch;
            WebClient client = new WebClient();
            try
            {
                urlSearch = URL_NEO_search + MakeScoutQuery(MPC_Observatory.BestObservatory.MPC_Code);
                neoResultText = client.DownloadString(urlSearch);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Download Error: " + ex.Message + "\n Possibly the NEO target is not available on Scout for ephemeris.");
                return false;
            };
            List<SpeedVector> BasicRateTable = new List<SpeedVector>();
            UpdateRateTable = new List<SpeedVector>();
            ScoutResultsX = JsonConvert.DeserializeXNode(neoResultText, "Root");
            IEnumerable<XElement> sEphXList = ScoutResultsX.Element("Root").Elements("eph");
            //Translate Json results to "standard" XML that the speedVector converter can use
            XElement ephmList = new XElement("Ephemeris");
            foreach (XElement ephX in sEphXList)
            {
                XElement ephmRecord = new XElement("Data");
                DateTime datetimeX = Convert.ToDateTime(ephX.Element("time").Value);
                string dateString = datetimeX.ToString("yyyy-MM-dd");
                string timeString = datetimeX.ToString("HH:mm:ss");
                XElement sPositionX = ephX.Element("median");

                ephmRecord.Add(new XElement(xUTDate, dateString));
                ephmRecord.Add(new XElement(xUTHrMin, timeString));
                ephmRecord.Add(new XElement(xRA, sPositionX.Element("ra").Value));//Scout delivers RA in degrees
                ephmRecord.Add(new XElement(xDec, sPositionX.Element("dec").Value)); ;
                ephmRecord.Add(new XElement(xPA, sPositionX.Element("pa").Value));
                ephmRecord.Add(new XElement(xdRate, sPositionX.Element("rate").Value));
                ephmRecord.Add(new XElement(xdRACosD, sPositionX.Element("dra").Value));
                ephmRecord.Add(new XElement(xdDec, sPositionX.Element("ddec").Value));
                ephmRecord.Add(new XElement(xRng, sPositionX.Element("rs").Value));  //AU
                ephmRecord.Add(new XElement(xV, sPositionX.Element("vmag").Value));  //Magnitude
                ephmList.Add(ephmRecord);
            }
            return EphemerisListToSpeedVector(ephmList, isMinutes, updateInterval);
        }

        private bool GeoToScoutSiteCalibration()
        {
            //MPC_Observatory must be set before calibration
            string scoutResultText;
            string urlSearch;
            WebClient client = new WebClient();

            //Get geocentric ephemeris for this target at eph start
            try
            {
                urlSearch = URL_NEO_search + MakeScoutQuery(GEOCENTRIC_OBSERVATORY_CODE);
                scoutResultText = client.DownloadString(urlSearch);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Geocentric Ephemeris Download Error: " + ex.Message);
                return false;
            };
            XDocument geoResultsX = JsonConvert.DeserializeXNode(scoutResultText, "Root");
            XElement sEphXGeo = geoResultsX.Element("Root").Element("eph");
            if (sEphXGeo == null)
                return false;
            IEnumerable<XElement> gPositionX = sEphXGeo.Element("data").Elements("data");
            List<XElement> gPositionList = gPositionX.ToList();
            SpeedVector geoResultsSV = new SpeedVector
            {
                Time_UTC = Convert.ToDateTime(sEphXGeo.Element("time").Value),
                Rate_ArcsecPerMinute = Convert.ToDouble(gPositionList[idx_rate].Value),
                PA_Degrees = (Convert.ToDouble(gPositionList[idx_pa].Value)),
                RA_Degrees = Convert.ToDouble(gPositionList[idx_ra].Value),
                Dec_Degrees = Convert.ToDouble(gPositionList[idx_dec].Value),
                Range_AU = Convert.ToDouble(gPositionList[idx_reG_Geo].Value)
            };

            //Get topocentric ephemeris for the observatory nearest this site
            try
            {
                urlSearch = URL_NEO_search + MakeScoutQuery(MPC_Observatory.BestObservatory.MPC_Code);
                scoutResultText = client.DownloadString(urlSearch);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Topocentric Ephemeris Download Error: " + ex.Message);
                return false;
            };
            XDocument mpcResultsX = JsonConvert.DeserializeXNode(scoutResultText, "Root");
            XElement mpcEphXTopo = mpcResultsX.Element("Root").Element("eph");
            IEnumerable<XElement> mPositionX = mpcEphXTopo.Element("data").Elements("data");
            List<XElement> mPositionList = mPositionX.ToList();
            SpeedVector scoutResultsSV = new SpeedVector
            {
                Time_UTC = Convert.ToDateTime(mpcEphXTopo.Element("time").Value),
                Rate_ArcsecPerMinute = Convert.ToDouble(mPositionList[idx_rate].Value),
                PA_Degrees = (Convert.ToDouble(mPositionList[idx_pa].Value)),
                RA_Degrees = Convert.ToDouble(mPositionList[idx_ra].Value),
                Dec_Degrees = Convert.ToDouble(mPositionList[idx_dec].Value),
                Elevation_KM = Convert.ToDouble(mPositionList[idx_el_Topo].Value),
                Range_AU = Convert.ToDouble(mPositionList[idx_reO_Topo].Value)
            };

            //New try using spherical coordinates and ranges to compute site sky location
            //Convert the geocentric Range/RA/Dec point to spherical radius/phi/theta point
            AstroMath.Spherical.PointRangeRADec geoNEO_RD = new Spherical.PointRangeRADec()
            {
                Range = geoResultsSV.Range_AU * Utils.Astronomical_Unit,  // convert to km,
                Dec = Transform.DegreesToRadians(geoResultsSV.Dec_Degrees), //radians
                RA = Transform.DegreesToRadians(geoResultsSV.RA_Degrees)  //radians
            };
            //Convert to spherical point
            AstroMath.Spherical.PointSph geoNEO_Sph = AstroMath.Spherical.RADecToSpherical(geoNEO_RD);
            //Convert site location to spherical coordinates
            AstroMath.Spherical.PointRangeRADec MySite_RD = new Spherical.PointRangeRADec()
            {
                Range = Utils.Earth_Radius,  //in km
                Dec = Transform.DegreesToRadians(MPC_Observatory.BestObservatory.MySiteLat),
                RA = Transform.DegreesToRadians(-MPC_Observatory.BestObservatory.MySiteLong)
            };
            //Convert site location to spherical
            AstroMath.Spherical.PointSph MySite_Sph = AstroMath.Spherical.RADecToSpherical(MySite_RD);
            //Translate neo location to siteLocation coordinates
            AstroMath.Spherical.PointSph transNEO_Sph = Spherical.TranslateSpherical(geoNEO_Sph, MySite_Sph);
            //Convert new neo coordinates to RADec
            AstroMath.Spherical.PointRangeRADec transNEO_RD = Spherical.SphericalToRADec(transNEO_Sph);

            //Compute corrections for rates, range
            Site_Corrected_Range = transNEO_RD.Range / Utils.Astronomical_Unit;
            Site_Corrected_Dec = Transform.RadiansToDegrees(transNEO_RD.Dec);
            Site_Corrected_RA = Transform.RadiansToDegrees(transNEO_RD.RA);

            Dec_CorrectionD = scoutResultsSV.Dec_Degrees - Site_Corrected_Dec;  //offset in minutes Dec
            RA_CorrectionD = scoutResultsSV.RA_Degrees - Site_Corrected_RA;  //offset in minutes RA
            RangeAU = scoutResultsSV.Range_AU; //AU
            Range_CorrectionAU = RangeAU - Site_Corrected_Range;  //AU
            return true;
        }

        private string MakeScoutQuery(string mpc_observatory_code)
        {
            //Returns a url query string for Scout website
            // "key1=value1&key2=value2", all URL-encoded   
            // MPC observation code is optional.  If not filled in, then geocentric is used.

            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["tdes"] = TgtName;
            queryString["orbits"] = "0";
            queryString["n-orbits"] = SAMPLEORBITS.ToString();
            queryString["eph-start"] = EphStart.ToString("yyyy-MM-ddTHH:mm:ss");
            if (EphStep.Minutes < 1)
            {
                queryString["eph-step"] = "1" + "m";
                //Compute new eph-stop based on limiting data point to 100
                DateTime EphEnd = EphStart + TimeSpan.FromMinutes(99);
            }
            else
            {
                queryString["eph-step"] = EphStep.Minutes.ToString("0") + "m";
                //Compute new eph-stop based on limiting data point to 100
                EphEnd = EphStart + TimeSpan.FromMinutes(EphStep.Minutes * 99);
            }
            queryString["eph-stop"] = EphEnd.ToString("yyyy-MM-ddTHH:mm:ss");
            queryString["obs-code"] = mpc_observatory_code;
            queryString["ranges"] = "true";
            return queryString.ToString();
        }

        #endregion


        #region horizons

        const string URL_Horizons_Search = "https://ssd.jpl.nasa.gov/api/horizons.api?";
        //const string URL_Horizons_Search = "https://cgi.minorplanetcenter.net/cgi-bin";

        #region horizon results header conversion

        const string hzCommand = "Command";
        const string hzTLE = "TLE";
        const string hzUTDate = xUTDate;
        const string hzUTHrMin = xUTHrMin;
        const string hzB1 = "B1";
        const string hzB2 = "B2";
        const string hzRA = xRA;
        const string hzDec = xDec;
        const string hzRA_a = "RA_a";
        const string hzDec_a = "Dec_a";
        const string hzdRACosD = xdRACosD;
        const string hzdDec = xdDec;
        const string hzAzim = "Azim";
        const string hzElev = "Elev";
        const string hzdAzCosE = "dAzCosE";
        const string hzdELV = xEl;
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
        const string hzVis = "Vis";
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
        const string hzr = xRng;
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
        const string hzMNIll = "MN_Ill";
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
        const string hzDec_ICRF_a_app = "RA_ICRF_a_app";
        const string hzI_dRAcosD = "I_dRAcosD";
        const string hzIdDEC = "IdDEC";
        const string hzSky_motion = "Sky_motion";
        const string hzSky_mot_PA = "Sky_mot_PA";
        const string hzRelVel_ANG = "RelVel_ANG";
        const string hzLun_Sky_Brt = "Lun_Sky_Brt";
        const string hzsky_SNR = "sky_SNR";
        const string hzB3 = "B3";

        private string[] horizonsHeaderNames =
        {
            hzUTDate+"_"+hzUTHrMin,
            hzB1,
            hzB2,
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
            hzVis,
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
            hzMNIll,
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
            hzDec_ICRF_a_app,
            hzI_dRAcosD,
            hzIdDEC,
            hzSky_motion,
            hzSky_mot_PA,
            hzRelVel_ANG,
            hzLun_Sky_Brt,
            hzsky_SNR,
            hzB3
        };
        #endregion

        public bool DownloadHorizonsData(bool isMinutes, int updateInterval, Ephemeris.EphemSource tleSource)
        {
            //Get site location (and closest MPC observatory -- although unneeded)
            MPC_Observatory = new Observatory();

            //Get Topocentric ephemeris
            if (HorizonsQueryToSpeedVectors(isMinutes, updateInterval, tleSource))
                return true;
            else
                return false;
        }

        private bool HorizonsQueryToSpeedVectors(bool isMinutes, int updateInterval, Ephemeris.EphemSource tleSource)
        {
            string hzResultText;
            string urlSearch = null;
            WebClient client = new WebClient();
            try
            {
                switch (tleSource)
                {
                    case EphemSource.HorizonsSat:
                        {
                            urlSearch = URL_Horizons_Search + MakeHorizonsTLEQuery(SatCat.ReadCelesTrakTLE(TgtName));
                            break;
                        }
                    case EphemSource.HorizonsTLE:
                        {
                            urlSearch = URL_Horizons_Search + MakeHorizonsTLEQuery(SatCat.ReadCustomTLE(TgtName));
                            break;
                        }
                    case EphemSource.Horizons:
                        {
                            urlSearch = URL_Horizons_Search + MakeHorizonsQuery();
                            break;
                        }
                }
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
            //Convert Text to raw XML list
            char[] splitChar = { ',' };
            string[] hzLineItems = hzResultText.Split('\n');
            int soeIdx = Array.IndexOf(hzLineItems, "$$SOE");
            int eoeIdx = Array.IndexOf(hzLineItems, "$$EOE");
            string[] headers = hzLineItems[soeIdx - 2].Split(splitChar);
            XElement ephmList = new XElement("Ephemeris");
            for (int i = soeIdx + 1; i < eoeIdx; i++)
            {
                XElement ephmRecord = new XElement("Data");
                //string cleanLine = hzLineItems[i].Remove(17, 4);  //Clear out some garbage that Horizons leaves in the line for some reason -- minutes? seconds?
                string[] columns = hzLineItems[i].Split(splitChar);
                //Horizons combines date and time.  Separate and add XML records
                DateTime utcDateTime = Convert.ToDateTime(columns[0]);
                ephmRecord.Add(new XElement(xUTDate, utcDateTime.ToString("yyyy-MM-dd")));
                ephmRecord.Add(new XElement(xUTHrMin, utcDateTime.ToString("HH:mm")));
                //HOrizons dRA/DDec is in arcsec/hr, correct to arcsec/min
                columns[7] = (Convert.ToDouble(columns[7].ToString()) / 60.0).ToString();
                columns[8] = (Convert.ToDouble(columns[8].ToString()) / 60.0).ToString();
                for (int r = 1; r < headers.Count(); r++)
                    ephmRecord.Add(new XElement(horizonsHeaderNames[r], columns[r]));
                ephmList.Add(ephmRecord);
            }
            //Convert raw XML list to common speed vector format and return
            return EphemerisListToSpeedVector(ephmList, isMinutes, updateInterval);
        }

        private string ScrubSmallBodyNameHorizons(string longName)
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
                    if (splits[0].Length == 4) //Comet 2021 xx
                        scrub = splits[0] + " " + splits[1] + ";";  //Comet format (e.g. 2021 A7) so return the first two fields and small body search designator (";")
                    else
                        scrub = splits[1] + ";"; //Asteroid format ( e.g. 7 Isis) so return just the name and small body search designator (";")
                else
                    scrub = splits[0] + " " + splits[1] + ";";  //Comet format (e.g. 2021 A7) so return the first two fields and small body search designator (";")
            }
            return scrub;
        }

        #region Horizons Query Strings

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

        private string MakeHorizonsQuery()
        {
            //Returns a url string for querying the TNS website

            //figure out site location
            string scrubbedTargetName = ScrubSmallBodyNameHorizons(TgtName);
            //string siteLong = (360 - MPC_Observatory.BestObservatory.MySiteLong).ToString("0.000");  //converted to the 0-360 form that MPC likes it
            string siteLong = MPC_Observatory.BestObservatory.MySiteLong.ToString("0.000");  //in degrees E
            string siteLat = MPC_Observatory.BestObservatory.MySiteLat.ToString("0.000");
            string siteElev = MPC_Observatory.BestObservatory.MySiteElev.ToString("0.000");
            //string siteCoords = '\'' + ((int)Math.Round(Convert.ToDouble(siteLong))).ToString() + "," +
            //                           ((int)Math.Round(Convert.ToDouble(siteLat))).ToString() + "," +
            //                           ((int)Math.Round(Convert.ToDouble(siteElev))).ToString() + '\'';
            string siteCoords = '\'' + siteLong + "," + siteLat + "," + siteElev + '\'';
            string startTime = "\'" + EphStart.ToString("yyyy-MM-dd HH:mm") + "\'";
            string endTime = "\'" + EphEnd.ToString("yyyy-MM-dd HH:mm") + "\'";
            string siteName = MPC_Observatory.BestObservatory.MPC_Code;
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString[hFormat] = hFormatTypeText;
            queryString[hCommand] = "\'NAME=" + scrubbedTargetName + "\'"; // ";" means that it is a small body search for name
            queryString[hMakeEphemeris] = hYes;
            queryString[hEphemerisType] = "OBSERVER";
            queryString[hCenter] = "coord@399";  //if using site coordinates
            //queryString[hCenter] = siteName; //Closest observatory
            queryString[hCoordinateType] = "GEODETIC";
            queryString[hSiteCoordinate] = siteCoords;  //e-long(degrees),lat(degrees),elevation(km)
            queryString[hStartTime] = startTime; // "2021-01-12";
            queryString[hStopTime] = endTime; // "2021-01-13";
            queryString[hStepSize] = (10).ToString("0") + "m";
            queryString[hAngleFormat] = hAngleFormatDegrees;
            queryString[hTimeDigits] = "Seconds";
            queryString[hRangeUnits] = "AU";
            //queryString[hQuantities ] = "'46'";
            queryString[hOutUnits] = hUnitTypeKMS;
            queryString[hExtraPrecisionFormat] = hYes;
            queryString[hCSVFormat] = hYes;
            queryString[hObjectData] = hNo;

            string q = queryString.ToString();
            //fix bug where queryString inserts %2f instead of %2F for the "/" char
            q = q.Replace("%2f", "%2F");
            return q; // Returns "key1=value1&key2=value2", all URL-encoded
        }

        private string MakeHorizonsTLEQuery(string tleString)
        {
            //Returns a url string for querying the TNS website

            //figure out site location
            //string siteLong = (360 - MPC_Observatory.BestObservatory.MySiteLong).ToString("0.000");  //converted to the 0-360 form that MPC likes it
            string siteLong = MPC_Observatory.BestObservatory.MySiteLong.ToString("0.000");  //in degrees E
            string siteLat = MPC_Observatory.BestObservatory.MySiteLat.ToString("0.000");
            string siteElev = MPC_Observatory.BestObservatory.MySiteElev.ToString("0.000");
            //string siteCoords = '\'' + ((int)Math.Round(Convert.ToDouble(siteLong))).ToString() + "," +
            //                           ((int)Math.Round(Convert.ToDouble(siteLat))).ToString() + "," +
            //                           ((int)Math.Round(Convert.ToDouble(siteElev))).ToString() + '\'';
            string siteCoords = '\'' + siteLong + "," + siteLat + "," + siteElev + '\'';
            string startTime = "\'" + EphStart.ToString("yyyy-MM-dd HH:mm") + "\'";
            string endTime = "\'" + EphEnd.ToString("yyyy-MM-dd HH:mm") + "\'";
            string siteName = MPC_Observatory.BestObservatory.MPC_Code;
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString[hzCommand] = "TLE";
            queryString[hzTLE] = tleString;
            queryString[hFormat] = hFormatTypeText;
            queryString[hMakeEphemeris] = hYes;
            queryString[hEphemerisType] = hObserverType;
            queryString[hCenter] = "coord@399"; //if using coordinates
            //queryString[hCenter] = siteName;
            queryString[hCoordinateType] = "GEODETIC";
            queryString[hSiteCoordinate] = siteCoords;  //e-long(degrees),lat(degrees),elevation(km)
            queryString[hStartTime] = startTime; // "2021-01-12";
            queryString[hStopTime] = endTime; // "2021-01-13";
            queryString[hStepSize] = "1m"; // shortest time that horizons can do
            queryString[hAngleFormat] = hAngleFormatDegrees;
            queryString[hTimeDigits] = "Seconds";
            queryString[hRangeUnits] = "AU";
            //queryString[hQuantities ] = "'46'";
            queryString[hOutUnits] = hUnitTypeKMS;
            queryString[hExtraPrecisionFormat] = hYes;
            queryString[hCSVFormat] = hYes;
            queryString[hObjectData] = hNo;

            string q = queryString.ToString();
            //fix bug where queryString inserts %2f instead of %2F for the "/" char
            q = q.Replace("%2f", "%2F");
            return q; // Returns "key1=value1&key2=value2", all URL-encoded
        }

        #endregion


        #region mpes

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
        const int colUTDateM = 5;
        const int colUTDateD = 8;
        const int colUTHrMin = 11;
        const int colRA = 18;
        const int colDec = 27;
        const int colDelta = 37;
        const int colr = 47;
        const int colEl = 55;
        const int colPh = 62;
        const int colV = 68;
        const int coldRACosD = 74;
        const int coldDec = 83;
        const int colAzi = 92;
        const int colAlt = 98;
        const int colSunAlt = 104;
        const int colMoonPhase = 109;
        const int colMoonDist = 118;
        const int colMoonAlt = 122;

        private string[] MPESHeaderNames =
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

        public bool DownloadMPESData(bool isMinutes, int updateInterval)
        {
            //Get site location (and closest MPC observatory -- although unneeded)
            MPC_Observatory = new Observatory();

            //Find topocentric ephemeris at current time (100 count)
            if (MPESQueryToSpeedVectors(isMinutes, updateInterval))
                return true;
            else
                return false;
        }

        public bool MPESQueryToSpeedVectors(bool isMinutes, int updateInterval)
        {
            string mpesResultText;
            string urlSearch;
            WebClient client = new WebClient();
            try
            {
                urlSearch = URL_MPES_Search + MakeMPESQuery((int)(EphEnd - EphStart).TotalMinutes);
                mpesResultText = client.DownloadString(urlSearch);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Download Error: " + ex.Message);
                return false;
            };
            //Check result
            if (!mpesResultText.Contains("Date"))
            {
                MessageBox.Show("Target not found");
                return false;
            }
            //Convert Text to XML  -- JSON format is rudimentary and no better than text
            char[] spc = { ' ' };
            string[] mpcLineItems = mpesResultText.Split('\n');
            int soeIdx = FindLine(mpcLineItems, "Date") + 2;
            int eoeIdx = FindLine(mpcLineItems, " These calculations have been performed on the") - 2;
            XElement ephmList = new XElement("Ephemeris");
            for (int i = soeIdx; i < eoeIdx; i++)
            {
                XElement ephmRecord = new XElement("Data");
                string mpcDataLine = mpcLineItems[i];
                ephmRecord.Add(new XElement(mUTDate, mpcDataLine.Substring(colUTDateY, 4) + "-" + mpcDataLine.Substring(colUTDateM, 2) + "-" + mpcDataLine.Substring(colUTDateD, 2)));
                string timestring = mpcDataLine.Substring(colUTHrMin, 6);
                string sTime = timestring[0].ToString() + timestring[1].ToString() + ":" + timestring[2].ToString() + timestring[3].ToString() + ":" + timestring[4].ToString() + timestring[5].ToString();
                ephmRecord.Add(new XElement(xUTHrMin, sTime));
                ephmRecord.Add(new XElement(xRA, mpcDataLine.Substring(colRA, 8)));
                ephmRecord.Add(new XElement(xDec, mpcDataLine.Substring(colDec, 9)));
                ephmRecord.Add(new XElement(xDelta, mpcDataLine.Substring(colDelta, 9)));
                ephmRecord.Add(new XElement(xRng, mpcDataLine.Substring(colr, 7)));
                ephmRecord.Add(new XElement(xEl, mpcDataLine.Substring(colEl, 6)));
                ephmRecord.Add(new XElement(xPh, mpcDataLine.Substring(colPh, 5)));
                ephmRecord.Add(new XElement(xV, mpcDataLine.Substring(colV, 5)));
                ephmRecord.Add(new XElement(xdRACosD, mpcDataLine.Substring(coldRACosD, 9)));
                ephmRecord.Add(new XElement(xdDec, mpcDataLine.Substring(coldDec, 9)));
                ephmList.Add(ephmRecord);
            }
            return EphemerisListToSpeedVector(ephmList, isMinutes, updateInterval);
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

        public static string ScrubSmallBodyNameMPES(string longName)
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

        public string MakeMPESQuery(int rangeInMinutes)
        {
            //Returns a url string for querying the TNS website

            string scrubbedTgtName = ScrubSmallBodyNameMPES(TgtName);
            //figure out site location
            //string siteLong = (360 - MPC_Observatory.BestObservatory.MySiteLong).ToString("0.000");  //converted to the 0-360 form that MPC likes it
            string siteLong = MPC_Observatory.BestObservatory.MySiteLong.ToString("0.000");  //in degrees E converted to the 0-360 form that MPC likes it
            string siteLat = MPC_Observatory.BestObservatory.MySiteLat.ToString("0.000");
            string siteElev = MPC_Observatory.BestObservatory.MySiteElev.ToString("0.000");
            string center = siteLong + ":" + siteLat + ":" + siteElev;
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString[mEphemerisType] = "e";
            queryString[mTarget] = scrubbedTgtName;
            //queryString[mStartDate] = EphStart.ToString("yyyy-MM-dd"); // "2021-01-12";
            queryString[mStartDate] = ""; // Current ephemeris
            //queryString[mUTOffset] = EphStart.Hour.ToString("0");  //puts us in the correct hour of the day
            queryString[mUTOffset] = "";  //No offset for current ephemeris

            //Determine range of query
            //  if greater than 1440 minutes, then convert to hours
            //  if greater than 1440 hours then convert to days
            int rangeInHours = rangeInMinutes / 60;
            int rangeInDays = rangeInMinutes / 60 / 24;
            if (rangeInMinutes < 1440)  //Range from 1 - 1440 minutes (1 day)
            {
                queryString[mInterval] = "1";
                queryString[mIntervalUnits] = "m";
                queryString[mNumberOfRecords] = "1440";  // just do 1440 minutes
            }
            else if (rangeInHours < 1440)  //Range from 1 - 59 days
            {
                int minuteInterval = rangeInMinutes / 1440;
                queryString[mInterval] = minuteInterval.ToString(); ;
                queryString[mIntervalUnits] = "m";
                queryString[mNumberOfRecords] = "1440";
            }
            else if (rangeInDays < 1440)  //Range from 60 - 1440 days
            {
                int hourInterval = rangeInMinutes / 60 / 1440;
                queryString[mInterval] = hourInterval.ToString(); ;
                queryString[mIntervalUnits] = "h";
                queryString[mNumberOfRecords] = "1440";
            }
            else //Range from 1440 days up
            {
                int dayInterval = rangeInMinutes / 60 / 24 / 1440;
                queryString[mInterval] = dayInterval.ToString(); ;
                queryString[mIntervalUnits] = "d";
                queryString[mNumberOfRecords] = "1440";
            }

            queryString[mObservatoryCode] = "";
            queryString[mSiteLongitude] = MPC_Observatory.BestObservatory.MySiteLong.ToString("0.000");
            queryString[mSiteLatitude] = MPC_Observatory.BestObservatory.MySiteLat.ToString("0.000");
            queryString[mSiteAltitude] = MPC_Observatory.BestObservatory.MySiteElev.ToString("0");
            queryString[mDataType] = "x"; //Decimal data
            queryString[mRADecMotions] = "s";  // t for total, c for coordinate motion, s for sky motion 
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

        #endregion


    }

    public class SpeedVector
    {
        //Structure to hold RA and Dec tracking speeds
        public DateTime Time_UTC { get; set; }
        public double RA_Degrees { get; set; }
        public double Dec_Degrees { get; set; }
        public double Rate_ArcsecPerMinute { get; set; }
        public double PA_Degrees { get; set; }
        public double Rate_RA_CosDec_ArcsecPerMinute { get; set; }  //Fixed image rate
        public double Rate_RA_ArcsecPerMinute { get; set; }  //Tracking image rate
        public double Rate_Dec_ArcsecPerMinute { get; set; }
        public double Elevation_KM { get; set; }  //Meters?
        public double Range_AU { get; set; }  //AU 

        public SpeedVector() { }

        public double VelocityRA()
        {
            return Rate_ArcsecPerMinute * Math.Sin(Transform.DegreesToRadians(PA_Degrees));
        }
        public double VelocityDec()
        {
            return Rate_ArcsecPerMinute * Math.Cos(Transform.DegreesToRadians(PA_Degrees));
        }
    }


}



