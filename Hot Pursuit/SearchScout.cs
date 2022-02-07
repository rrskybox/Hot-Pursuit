/*
* SearchScout Class
*
* Class for downloading and parsing database NASA NEO Scout query results
* 
* This class serves as method template for conversions from all 
*  catalog sources
* 
* Author:           Rick McAlister
* Date:             5/12/21
* Current Version:  1.0
* Developed in:     Visual Studio 2019
* Coded in:         C# 8.0
* App Envioronment: Windows 10 Pro, .Net 4.8, TSX 5.0 Build 12978
* 
* API Doc: https://ssd-api.jpl.nasa.gov/doc/scout.html
* 
* Change Log:
* 
* Added this class to make V1.1 Release (5/12/21)
* 
*/
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml.Linq;
using TheSky64Lib;
using AstroMath;

namespace Hot_Pursuit
{
    public class SearchScout
    {
        const string URL_NEO_search = "https://ssd-api.jpl.nasa.gov/scout.api?";

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
        const int idx_el = 11;
        const int idx_reG = 12;
        const int idx_reO = 13;
        const int idx_rs = 13;

        public string? TgtName { get; set; }
        public DateTime EphStart { get; set; }
        public DateTime EphEnd { get; set; }
        public TimeSpan EphStep { get; set; }  //query results steps as time span
        public XDocument ScoutResultsX { get; set; }
        public List<SpeedVector> UpdateRateTable { get; set; }
        public Observatory MPC_Observatory { get; set; }
        public double RA_CorrectionD { get; set; } //Hours
        public double Dec_CorrectionD { get; set; } //Degrees
        public double Topo_RA_Correction_Factor { get; set; } //Hours
        public double Topo_Dec_Correction_Factor { get; set; } //Degrees
        public double RangeAU { get; set; } //AU
        public double Range_CorrectionAU { get; set; } //AU
        public double Site_Corrected_Range { get; set; }
        public double Site_Corrected_RA { get; set; }
        public double Site_Corrected_Dec { get; set; }

        public bool LoadTargetData(bool isMinutes, int updateInterval)
        {
            //Get site locatino and closest observatory
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

        private bool ServerQueryToSpeedVectors(bool isMinutes, int updateInterval)
        {
            string neoResultText;
            string urlSearch;
            WebClient client = new WebClient();
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
            try
            {
                urlSearch = URL_NEO_search + MakeSearchQuery(TgtName, MPC_Observatory.BestObservatory.MPC_Code);
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
            SpeedVector currentSpeedVector;
            foreach (XElement ephX in sEphXList)
            {
                IEnumerable<XElement> sPositionX = ephX.Element("data").Elements("data");
                List<XElement> sPositionList = sPositionX.ToList();
                currentSpeedVector = new SpeedVector
                {
                    Time_UTC = Convert.ToDateTime(ephX.Element("time").Value),
                    Rate_ArcsecPerMinute = Convert.ToDouble(sPositionList[idx_rate].Value),  //Arcsec/min
                    PA_Degrees = (Convert.ToDouble(sPositionList[idx_pa].Value)),
                    RA_Degrees = Convert.ToDouble(sPositionList[idx_ra].Value),  //Scout delivers RA in degrees
                    Dec_Degrees = Convert.ToDouble(sPositionList[idx_dec].Value),
                    Elevation_KM = Convert.ToDouble(sPositionList[idx_el].Value),
                    Range_AU = Convert.ToDouble(sPositionList[idx_reO].Value)  //AU
                };
                currentSpeedVector.Rate_RA_CosDec_ArcsecPerMinute = Utils.PARateToRA(currentSpeedVector.PA_Degrees, currentSpeedVector.Rate_ArcsecPerMinute);
                currentSpeedVector.Rate_Dec_ArcsecPerMinute = Utils.PARateToDec(currentSpeedVector.PA_Degrees, currentSpeedVector.Rate_ArcsecPerMinute);
                BasicRateTable.Add(currentSpeedVector);
            }
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
            return true;
        }

        private bool GeoToSiteCalibration()
        {
            //MPC_Observatory must be set before calibration
            string mpcResultText;
            string urlSearch;
            WebClient client = new WebClient();
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
            //Get geocentric ephemeris for this target at eph start
            try
            {
                urlSearch = URL_NEO_search + MakeSearchQuery(TgtName);
                mpcResultText = client.DownloadString(urlSearch);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Geocentric Ephemeris Download Error: " + ex.Message);
                return false;
            };
            XDocument geoResultsX = JsonConvert.DeserializeXNode(mpcResultText, "Root");
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
                Range_AU = Convert.ToDouble(gPositionList[idx_reG].Value)
            };
            //Get topocentric ephemeris for the observatory nearest this site
            try
            {
                urlSearch = URL_NEO_search + MakeSearchQuery(TgtName, MPC_Observatory.BestObservatory.MPC_Code);
                mpcResultText = client.DownloadString(urlSearch);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Topocentric Ephemeris Download Error: " + ex.Message);
                return false;
            };
            XDocument mpcResultsX = JsonConvert.DeserializeXNode(mpcResultText, "Root");
            XElement mpcEphXTopo = mpcResultsX.Element("Root").Element("eph");
            IEnumerable<XElement> mPositionX = mpcEphXTopo.Element("data").Elements("data");
            List<XElement> mPositionList = mPositionX.ToList();
            SpeedVector mpcResultsSV = new SpeedVector
            {
                Time_UTC = Convert.ToDateTime(mpcEphXTopo.Element("time").Value),
                Rate_ArcsecPerMinute = Convert.ToDouble(mPositionList[idx_rate].Value),
                PA_Degrees = (Convert.ToDouble(mPositionList[idx_pa].Value)),
                RA_Degrees = Convert.ToDouble(mPositionList[idx_ra].Value),
                Dec_Degrees = Convert.ToDouble(mPositionList[idx_dec].Value),
                Range_AU = Convert.ToDouble(mPositionList[idx_reO].Value)
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

            Site_Corrected_Range = transNEO_RD.Range / Utils.Astronomical_Unit;
            Site_Corrected_Dec = Transform.RadiansToDegrees(transNEO_RD.Dec);
            Site_Corrected_RA = Transform.RadiansToDegrees(transNEO_RD.RA);

            Dec_CorrectionD = mpcResultsSV.Dec_Degrees - Site_Corrected_Dec;  //degrees Dec
            RA_CorrectionD = mpcResultsSV.RA_Degrees - Site_Corrected_RA;  //degrees RA
            Topo_Dec_Correction_Factor = 1.0 / (1.0 + Math.Pow(Dec_CorrectionD, 2));  //degrees per arcdegree
            Topo_RA_Correction_Factor = 1.0 / (1.0 + Math.Pow(RA_CorrectionD, 2));  //degrees per arcdegree
            RangeAU = mpcResultsSV.Range_AU; //AU
            Range_CorrectionAU = RangeAU - Site_Corrected_Range;  //AU
            return true;
        }

        public SpeedVector? GetNextRateUpdate(DateTime nextTime)
        {
            for (int i = 0; i < UpdateRateTable.Count; i++)
                if (UpdateRateTable[i].Time_UTC > nextTime)
                    return UpdateRateTable[i];
            return null;
        }

        private string MakeSearchQuery(string tgtName, string mpc_observatory_code = "500")
        {
            //Returns a url query string for Scout website
            // "key1=value1&key2=value2", all URL-encoded   
            // MPC observation code is optional.  If not filled in, then geocentric is used.

            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["tdes"] = tgtName;
            queryString["n-orbits"] = "1";
            queryString["eph-start"] = EphStart.ToString("yyyy-MM-ddTHH:mm:ss");
            queryString["eph-stop"] = EphEnd.ToString("yyyy-MM-ddTHH:mm:ss");
            queryString["eph-step"] = EphStep.Minutes.ToString("0") + "m";
            queryString["obs-code"] = mpc_observatory_code;
            queryString["ranges"] = "true";
            return queryString.ToString();
        }

    }
}

