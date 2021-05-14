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

        public string TgtName { get; set; }
        public double TgtRA { get; set; }
        public double TgtDec { get; set; }
        public double TgtRate { get; set; }
        public double TgtPA { get; set; }
        public double TgtRateRA { get; set; }
        public double TgtRateDec { get; set; }
        public DateTime EphStart { get; set; }
        public DateTime EphEnd { get; set; }
        public TimeSpan EphStep { get; set; }  //query results steps as time span
        public XDocument ScoutResultsX { get; set; }
        public DateTime NextUpdateAt { get; set; }
        public string? MPC_Observatory { get; set; }

        public bool LoadTargetData()
        {
            //Import TNS CSV text query and parse out tracking parameters
            //Get the closest observatory
            sky6StarChart tsxsc = new sky6StarChart();
            tsxsc.DocumentProperty(Sk6DocumentProperty.sk6DocProp_Latitude);
            double lat = tsxsc.DocPropOut;
            tsxsc.DocumentProperty(Sk6DocumentProperty.sk6DocProp_Longitude);
            double lng = tsxsc.DocPropOut;
            Observatory obs = new Observatory(lat, lng);
            MPC_Observatory = obs.BestObservatory.Code ;
            ServerQueryToResultsXML();
            return GetNextPositionUpdate();
        }

        /// <summary>
        /// Method to import TNS server database and convert to internal XML db
        /// </summary>
        /// <returns></returns>
        private bool ServerQueryToResultsXML()
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
                MessageBox.Show("Download Error: " + ex.Message);
                return false;
            };
            //ScoutJSON scoutBase = JsonSerializer.Deserialize<ScoutJSON>(neoResultText);
            ScoutResultsX = JsonConvert.DeserializeXNode(neoResultText, "Root");
            return true;
        }

        public bool GetNextPositionUpdate()
        {
            //Looks for next time for update

            IEnumerable<XElement> sEphXList = ScoutResultsX.Element("Root").Elements("eph");
            foreach (XElement ephX in sEphXList)
            {
                DateTime ephTime = Convert.ToDateTime(ephX.Element("time").Value);
                if (ephTime >= DateTime.Now)
                {
                    IEnumerable<XElement> sPositionX = ephX.Element("data").Elements("data");
                    List<XElement> sPositionList = sPositionX.ToList();
                    TgtRA = (Convert.ToDouble(sPositionList[idx_ra].Value)) * 24.0 / 360.0;  //degrees to hours
                    TgtDec = Convert.ToDouble(sPositionList[idx_dec].Value);
                    TgtRate = Convert.ToDouble(sPositionList[idx_rate].Value);
                    TgtPA = (Convert.ToDouble(sPositionList[idx_pa].Value)) * Math.PI / 180.0;
                    TgtRateRA = TgtRate * Math.Cos(TgtPA);
                    TgtRateDec = TgtRate * Math.Sin(TgtPA);
                    NextUpdateAt = ephTime;
                    return true;
                }
            }
            return false;
        }

        public string GetTargetName()
        {
            sky6ObjectInformation tsxoi = new sky6ObjectInformation();
            tsxoi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_NAME1);
            TgtName = tsxoi.ObjInfoPropOut;
            return TgtName;
        }

        public void SlewToTarget()
        {
            sky6RASCOMTele tsxmt = new sky6RASCOMTele();
            tsxmt.Connect();
            tsxmt.SlewToRaDec(TgtRA, TgtDec, TgtName);
            return;
        }

        public void SetTargetTracking()
        {
            const int ionTrackingOn = 1;
            const int ionTrackingOff = 0;
            const int ignoreRates = 1;
            const int useRates = 0;

            sky6RASCOMTele tsxmt = new sky6RASCOMTele();
            tsxmt.Connect();
            try { tsxmt.SetTracking(ionTrackingOn, useRates, TgtRateRA, TgtRateDec); }
            catch { }
            return;
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
            queryString["obs-code"] = MPC_Observatory;
            return queryString.ToString(); // Returns "key1=value1&key2=value2", all URL-encoded
        }
    }
}

