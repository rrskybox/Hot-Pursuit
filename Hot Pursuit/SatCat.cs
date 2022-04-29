using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Linq;
using System.Web;

namespace Hot_Pursuit
{
    public partial class SatCat
    {
        const string celesTrakSatCatURL = "https://www.celestrak.com/pub/satcat.csv";
        const string celesTrakSatTgtURL = "https://celestrak.com/NORAD/elements/gp.php?";
        const string catDirectory = "\\Hot Pursuit\\TLE";
        const string catFileName = "\\SatCat.csv";

        public List<SatEntry> SatelliteCatalog = new List<SatEntry>();

        public enum SatCatEntryType
        {
            Payload,
            Debris,
            Booster,
            Unknown
        }
        public struct SatEntry
        {
            public string ObjectName { get; set; }
            public SatCatEntryType ObjectType { get; set; }
            public string ObjectInternationalID { get; set; }
            public string ObjectNoradID { get; set; }
        }

        public static bool RefreshSatelliteCatalog()
        {
            WebClient client = new WebClient();
            string urlSearch, satCatCSV;
            try
            {
                urlSearch = celesTrakSatCatURL;
                satCatCSV = client.DownloadString(urlSearch);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Catalog Download Error: " + ex.Message);
                return false;
            };
            //Copy csv data to file
            //Get User Documents Folder
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (!Directory.Exists(userFolder + catDirectory))
                Directory.CreateDirectory(userFolder + catDirectory);
            //Write url query contents to cat file
            string satCatPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + catDirectory + catFileName;
            File.WriteAllText(satCatPath, satCatCSV);
            return true;
        }

        public SatCat()
        {
            //Convert current sat catalog to XML structure
            //Get User Documents Folder
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (!Directory.Exists(userFolder + catDirectory))
                Directory.CreateDirectory(userFolder + catDirectory);
            //Write url query contents to cat file
            string satCatPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + catDirectory + catFileName;
            if (!File.Exists(satCatPath))
                return;
            StreamReader satCatFile = File.OpenText(satCatPath);
            //Read and discard the first line
            if (satCatFile.Peek() != -1) satCatFile.ReadLine();
            //Read in the remaining lines and stuff into staName List
            while (satCatFile.Peek() != -1)
            {
                string line = satCatFile.ReadLine();
                string[] lineEntries = line.Split(',');
                SatCatEntryType se = new SatCatEntryType();
                switch (lineEntries[3])
                {
                    case "PAY":
                        {
                            se = SatCatEntryType.Payload;
                            break;
                        }
                    case "R/B":
                        {
                            se = SatCatEntryType.Booster;
                            break;
                        }
                    case "DEB":
                        {
                            se = SatCatEntryType.Debris;
                            break;
                        }
                    case "UNK":
                        {
                            se = SatCatEntryType.Unknown;
                            break;
                        }
                }
                SatEntry satEnt = new SatEntry()
                {
                    ObjectName = lineEntries[0],
                    ObjectInternationalID = lineEntries[1],
                    ObjectNoradID = lineEntries[2],
                    ObjectType = se
                };
                SatelliteCatalog.Add(satEnt);
            }
            //Sort Catalog by Object Name alphabetically
            //SatelliteCatalog.Sort((x, y) => x.ObjectName.CompareTo(y.ObjectName));
            SatelliteCatalog = (from ce in SatelliteCatalog orderby ce.ObjectName select ce).ToList<SatEntry>();
        }

        public static string ReadCelesTrakTLE(string catID)
        {
            //Queries CelesTrak for satellite entry of catID
            //Example: https://celestrak.com/NORAD/elements/gp.php?CATNR=25544&FORMAT=TLE
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["CATNR"] = catID;
            queryString["FORMAT"] = "TLE";
            string q = queryString.ToString();
            //fix bug where queryString inserts %2f instead of %2F for the "/" char
            q.Replace("%2f", "%2F");

            WebClient client = new WebClient();
            string urlSearch, satCatTLE;
            try
            {
                urlSearch = celesTrakSatTgtURL + queryString;
                satCatTLE = client.DownloadString(urlSearch);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Catalog Download Error: " + ex.Message);
                return null;
            };
            return satCatTLE;
        }

        public static string ReadCustomTLE(string tgtName)
        {

            string nameLine = null;
            string firstLine = null;
            string secondLine = null;
            string catID = null;

            //Reads custom .txt file of TLE entries for satellite entry with tgtName as first line
            //
            //REad in list of TLE entries
            //Get User Documents Folder
            string satTLEPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Properties.Settings.Default.TLECatalogPath ;
            StreamReader satTLEFile = File.OpenText(satTLEPath);
            //Read in the remaining lines and stuff into staName List
            while (satTLEFile.Peek() != -1)
            {
                //Read sets of three lines, look for tgtName in first line, break out with result
                nameLine = satTLEFile.ReadLine();
                firstLine = satTLEFile.ReadLine();
                secondLine = satTLEFile.ReadLine();
                catID = firstLine.Substring(2, 5);
                if (tgtName == catID)
                    break;
            }
            if (tgtName == catID)
                return (nameLine + "\n" + firstLine + "\n" + secondLine);            //return concatenated string
            else
                return null;

        }


 

    }
}

