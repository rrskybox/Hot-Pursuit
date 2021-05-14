using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Resources;

namespace Hot_Pursuit
{
    class Observatory
    {
        public Location BestObservatory { get; set; }

        public Observatory(double topolat, double topolng)
        {
            //Type s = typeof(string);
            // Stream resourceStream = Assembly.GetAssembly(s.GetManifestResourceStream("Observatories.txt");
            char[] space = new char[] { ' ' };
            //ResourceManager rm = new ResourceManager(typeof(Observatory));
            //Stream resourceStream = rm.GetStream("Observatories");
            //StreamReader textStream = new StreamReader(resourceStream);
            Assembly dgassembly = Assembly.GetExecutingAssembly();
            Stream dgstream = dgassembly.GetManifestResourceStream("Hot_Pursuit.Observatories.txt");
            StreamReader textStream = new StreamReader(dgstream);
            List<Location> obsList = new List<Location>();
            string allLines = textStream.ReadToEnd();  //read in the whole enchelata
            string[] allLinesList = allLines.Split('\n');
            for (int i = 1; i < allLinesList.Length; i++)
            {
                string recordLine = allLinesList[i];
                string[] records = recordLine.Split(space, StringSplitOptions.RemoveEmptyEntries);
                obsList.Add(new Location { Code = records[1], Lng = Convert.ToDouble(records[2]), Lat = Convert.ToDouble(records[3]) });
            }
            //change topolat from +/- to 360
            topolng = 360 - topolng;
            //Find the closest observatory to input lst and lng by simple RMS
            foreach (Location ob in obsList)
            {
                double dLat = Math.Abs(ob.Lat - topolat);
                double dLng = Math.Abs(ob.Lng - topolng);
                if (dLat < 1 && dLng < 1)
                    BestObservatory = ob;
            }
        }
    }

    public class Location
    {
        public string Code { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

}

