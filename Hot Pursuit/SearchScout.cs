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

namespace Hot_Pursuit
{
    class SearchScout
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

        public string? TgtName { get; set; }
        public DateTime EphStart { get; set; }
        public DateTime EphEnd { get; set; }
        public TimeSpan EphStep { get; set; }  //query results steps as time span
        public XDocument ScoutResultsX { get; set; }
        public List<SpeedVector> UpdateRateTable { get; set; }
        public Observatory MPC_Observatory { get; set; }
        public double RA_CorrectionH { get; set; } //Hours
        public double Dec_CorrectionD { get; set; } //Degrees
        public double Diff_RA_CorrectionH { get; set; } //Hours
        public double Diff_Dec_CorrectionD { get; set; } //Degrees

        public bool LoadTargetData(bool isMinutes, int updateInterval)
        {
            //Import TNS CSV text query and parse out tracking parameters
            //Get the closest observatory
            sky6StarChart tsxsc = new sky6StarChart();
            tsxsc.DocumentProperty(Sk6DocumentProperty.sk6DocProp_Latitude);
            double lat = tsxsc.DocPropOut;
            tsxsc.DocumentProperty(Sk6DocumentProperty.sk6DocProp_Longitude);
            double lng = tsxsc.DocPropOut;

            //Get closest observatory to user TSX location
            MPC_Observatory = new Observatory(lat, lng);

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
                // string urlSearch = url_NEO_search + MakeSearchQuery();
                urlSearch = URL_NEO_search + MakeSearchQuery(GetTargetName());
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
            foreach (XElement ephX in sEphXList)
            {
                IEnumerable<XElement> sPositionX = ephX.Element("data").Elements("data");
                List<XElement> sPositionList = sPositionX.ToList();
                BasicRateTable.Add(new SpeedVector
                {
                    Time = Convert.ToDateTime(ephX.Element("time").Value),
                    Rate = Convert.ToDouble(sPositionList[idx_rate].Value),
                    PA = (Convert.ToDouble(sPositionList[idx_pa].Value)) * Math.PI / 180.0,
                    RA = Convert.ToDouble(sPositionList[idx_ra].Value) * 24.0 / 360.0,
                    Dec = Convert.ToDouble(sPositionList[idx_dec].Value)
                });
            }

            if (isMinutes)
            {
                UpdateRateTable = BasicRateTable;
            }
            else
            {
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
            string neoResultText;
            string urlSearch;
            WebClient client = new WebClient();
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
            //Get geocentric ephemeris for this target at eph start
            try
            {
                urlSearch = URL_NEO_search + MakeCalibrateQuery(GetTargetName(), "500");
                neoResultText = client.DownloadString(urlSearch);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Download Error: " + ex.Message);
                return false;
            };
            XDocument geoResultsX = JsonConvert.DeserializeXNode(neoResultText, "Root");
            XElement sEphXGeo = geoResultsX.Element("Root").Element("eph");
            if (sEphXGeo == null)
                return false;
            IEnumerable<XElement> sPositionX = sEphXGeo.Element("data").Elements("data");
            List<XElement> sPositionList = sPositionX.ToList();
            SpeedVector geoResultsSV = new SpeedVector
            {
                Time = Convert.ToDateTime(sEphXGeo.Element("time").Value),
                Rate = Convert.ToDouble(sPositionList[idx_rate].Value),
                PA = (Convert.ToDouble(sPositionList[idx_pa].Value)) * Math.PI / 180.0,
                RA = Convert.ToDouble(sPositionList[idx_ra].Value) * 24.0 / 360.0,
                Dec = Convert.ToDouble(sPositionList[idx_dec].Value)
            };
            //Get topocentric ephemeris for the observatory near this site
            try
            {
                urlSearch = URL_NEO_search + MakeCalibrateQuery(GetTargetName(), MPC_Observatory.BestObservatory.Code);
                neoResultText = client.DownloadString(urlSearch);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Download Error: " + ex.Message);
                return false;
            };
            XDocument topoResultsX = JsonConvert.DeserializeXNode(neoResultText, "Root");
            XElement tEphXTopo = topoResultsX.Element("Root").Element("eph");
            IEnumerable<XElement> tPositionX = tEphXTopo.Element("data").Elements("data");
            List<XElement> tPositionList = tPositionX.ToList();
            SpeedVector topoResultsSV = new SpeedVector
            {
                Time = Convert.ToDateTime(tEphXTopo.Element("time").Value),
                Rate = Convert.ToDouble(tPositionList[idx_rate].Value),
                PA = (Convert.ToDouble(tPositionList[idx_pa].Value)) * Math.PI / 180.0,
                RA = Convert.ToDouble(tPositionList[idx_ra].Value) * 24.0 / 360.0,
                Dec = Convert.ToDouble(tPositionList[idx_dec].Value)
            };

            double tgtDistance = MPC_Observatory.GeocentricDistanceToTarget(MPC_Observatory.BestObservatory.ObsLat, geoResultsSV.Dec, topoResultsSV.Dec);
            Dec_CorrectionD = MPC_Observatory.GeodeticAngleToTarget(MPC_Observatory.BestObservatory.SiteLat, geoResultsSV.Dec, tgtDistance) -
                MPC_Observatory.GeodeticAngleToTarget(MPC_Observatory.BestObservatory.ObsLat, geoResultsSV.Dec, tgtDistance);
            RA_CorrectionH = (MPC_Observatory.GeodeticAngleToTarget(MPC_Observatory.BestObservatory.SiteLong, geoResultsSV.RA, tgtDistance) -
                MPC_Observatory.GeodeticAngleToTarget(MPC_Observatory.BestObservatory.ObsLong, geoResultsSV.RA, tgtDistance)) * 24.0 / 3600.0;
            Diff_Dec_CorrectionD = 1.0 / (1.0 + Math.Pow(Dec_CorrectionD, 2));
            Diff_RA_CorrectionH = 1.0 / (1.0 + Math.Pow(RA_CorrectionH, 2));
            return true;
        }

        public SpeedVector? GetNextRateUpdate(DateTime nextTime)
        {
            for (int i = 0; i < UpdateRateTable.Count; i++)
                if (UpdateRateTable[i].Time > nextTime)
                    return UpdateRateTable[i];
            return null;
        }

        public string GetTargetName()
        {
            sky6ObjectInformation tsxoi = new sky6ObjectInformation();
            tsxoi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_NAME1);
            TgtName = tsxoi.ObjInfoPropOut;
            return TgtName;
        }

        public bool SlewToTarget(SpeedVector sv)
        {
            sky6RASCOMTele tsxmt = new sky6RASCOMTele();
            tsxmt.Connect();
            try
            {
                tsxmt.SlewToRaDec(sv.RA - RA_CorrectionH, sv.Dec - Dec_CorrectionD, TgtName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Slew Failed: " + ex.Message);
                return false;
            }
            return true;
        }

        public bool SetTargetTracking(SpeedVector sv)
        {
            const int ionTrackingOn = 1;
            const int ionTrackingOff = 0;
            const int ignoreRates = 1;
            const int useRates = 0;

            double tgtRateRA = sv.Rate * Math.Cos(sv.PA);
            double tgtRateDec = sv.Rate * Math.Sin(sv.PA);
            double adjtgtRateRA = tgtRateRA * Diff_RA_CorrectionH;
            double adjtgtRateDec = tgtRateDec * Diff_Dec_CorrectionD;

            sky6RASCOMTele tsxmt = new sky6RASCOMTele();
            tsxmt.Connect();
            try
            {
                //TSX expects tracking rates in arcsec/sec
                tsxmt.SetTracking(ionTrackingOn, useRates, adjtgtRateRA/60.0, adjtgtRateDec/60.0);
            }
            catch
            {
                return false;
            }
            return true;
        }
        private string MakeSearchQuery(string tgtName)
        {
            //Returns a url string for querying the TNS website
            //tdes=C5M39P2&eph-start&n-orbits=1

            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["tdes"] = tgtName;
            queryString["n-orbits"] = "1";
            queryString["eph-start"] = EphStart.ToString("yyyy-MM-ddTHH:mm:ss");
            queryString["eph-stop"] = EphEnd.ToString("yyyy-MM-ddTHH:mm:ss");
            queryString["eph-step"] = EphStep.Minutes.ToString("0") + "m";
            queryString["obs-code"] = MPC_Observatory.BestObservatory.Code;
            //queryString["obs-lat"] = "47.0";
            //queryString["obs-long"] = "122.0";             
            return queryString.ToString(); // Returns "key1=value1&key2=value2", all URL-encoded
        }

        private string MakeCalibrateQuery(string tgtName, string mpc_observatory_code)
        {
            //Returns a url string for querying the TNS website
            //tdes=C5M39P2&eph-start&n-orbits=1

            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["tdes"] = tgtName;
            queryString["n-orbits"] = "1";
            queryString["eph-start"] = EphStart.ToString("yyyy-MM-ddTHH:mm:ss");
            //queryString["eph-stop"] = EphEnd.ToString("yyyy-MM-ddTHH:mm:ss");
            //queryString["eph-step"] = EphStep.Minutes.ToString("0") + "m";
            queryString["obs-code"] = mpc_observatory_code;
            return queryString.ToString(); // Returns "key1=value1&key2=value2", all URL-encoded
        }

    }
}

