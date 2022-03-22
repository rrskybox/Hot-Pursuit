using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TheSky64Lib;

//Observatory codes can be found at https://www.projectpluto.com/obsc.htm
//Copy text data to Observatories.txt

namespace Hot_Pursuit
{
    public class Observatory
    {
        public Location BestObservatory { get; set; }

        public Observatory()
        {
            char[] space = new char[] { ' ' };

            BestObservatory = new Location();

            //Load site location
            GetSiteLocation();

            Assembly dgassembly = Assembly.GetExecutingAssembly();
            Stream dgstream = dgassembly.GetManifestResourceStream("Hot_Pursuit.Observatories.txt");
            StreamReader textStream = new StreamReader(dgstream);
            List<Location> obsList = new List<Location>();
            string allLines = textStream.ReadToEnd();  //read in the whole enchelata
            string[] allLinesList = allLines.Split('\n');
            for (int i = 1; i < allLinesList.Length; i++)
            {
                Location parsedline = ParseObservatory(allLinesList[i]);
                if (parsedline != null)
                    obsList.Add(parsedline);
            }
            //convert MPC longitude from +/- to 360 (format of MPC site)
            double topolng360 = 360 - BestObservatory.MySiteLong;
            double leastRMS = 360;
            //Find the closest observatory to input lst and lng by simple RMS
            foreach (Location ob in obsList)
            {
                double oLat = ob.MPC_ObsLat;
                double oLong = ob.MPC_ObsLong;
                double dLat = Math.Abs(ob.MPC_ObsLat - BestObservatory.MySiteLat);
                double dLng = Math.Abs(ob.MPC_ObsLong - topolng360);
                double siteRMS = Math.Sqrt((Math.Pow(dLat, 2) + Math.Pow(dLng, 2)) / 2);
                if (siteRMS < leastRMS)
                {
                    leastRMS = siteRMS;
                    ob.MySiteLat = BestObservatory.MySiteLat;
                    ob.MySiteLong = topolng360;
                    ob.VarianceRA = ob.MPC_ObsLat - BestObservatory.MySiteLat;
                    ob.VarianceDec = ob.MPC_ObsLong - topolng360;
                    BestObservatory = ob;
                }
            }
        }


        private Location ParseObservatory(string ent)
        {
            Location parsed = new Location();
            if (ent.Length < 4)
                return null;
            else
            {
                parsed.MPC_Code = ent.Substring(0, 3);
                parsed.MPC_ObsLong = Convert.ToDouble(ent.Substring(5, 10));
                parsed.MPC_ObsLat = Convert.ToDouble(ent.Substring(16, 10));
                string alt = ent.Substring(29, 8);
                string rhocos = ent.Substring(38, 7);
                string rhosinphi = ent.Substring(48, 11);
                string region = ent.Substring(62, 14);
                parsed.Description = ent.Substring(76);
                return parsed;
            }
        }

        private void GetSiteLocation()
        {
            //Import scout text query and parse out tracking parameters
            sky6StarChart tsxsc = new sky6StarChart();
            tsxsc.DocumentProperty(Sk6DocumentProperty.sk6DocProp_Latitude);
            BestObservatory.MySiteLat = tsxsc.DocPropOut;
            tsxsc.DocumentProperty(Sk6DocumentProperty.sk6DocProp_Longitude);
            BestObservatory.MySiteLong = tsxsc.DocPropOut;
            tsxsc.DocumentProperty(Sk6DocumentProperty.sk6DocProp_ElevationInMeters);
            BestObservatory.MySiteElev = tsxsc.DocPropOut / 1000;
            return;
        }
    }

    public class Location
    {
        public string MPC_Code { get; set; }
        public double MySiteLat { get; set; }
        public double MySiteLong { get; set; }
        public double MySiteElev { get; set; }
        public double MPC_ObsLat { get; set; }
        public double MPC_ObsLong { get; set; }
        public double VarianceRA { get; set; }
        public double VarianceDec { get; set; }
        public string Description { get; set; }

    }

}

