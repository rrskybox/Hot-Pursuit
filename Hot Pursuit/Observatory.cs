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
            char[] space = new char[] { ' ' };

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
                obsList.Add(new Location { Code = records[1], ObsLong = Convert.ToDouble(records[2]), ObsLat = Convert.ToDouble(records[3]) });
            }
            //change topolat from +/- to 360
            topolng = 360 - topolng;
            double leastRMS = 360;
            //Find the closest observatory to input lst and lng by simple RMS
            foreach (Location ob in obsList)
            {
                double oLat = ob.ObsLat;
                double oLong = ob.ObsLong;
                double dLat = Math.Abs(ob.ObsLat - topolat);
                double dLng = Math.Abs(ob.ObsLong - topolng);
                double siteRMS = Math.Sqrt((Math.Pow(dLat, 2) + Math.Pow(dLng, 2)) / 2);
                if (siteRMS < leastRMS)
                {
                    leastRMS = siteRMS;
                    ob.SiteLat = topolat;
                    ob.SiteLong = topolng;
                    BestObservatory = ob;
                }
            }
        }

        //public double DeltaAngToTarget(double aAngDeg, double bAngDeg, double geoDist)
        //{
        //    //Compute the angular differential between the dec angle at two different 
        //    //  latitudes to a point at the same distance
        //    double aLatAngDeg = GeodeticAngleToTarget(aAngDeg, geoDist);
        //    double bLatAngDeg = GeodeticAngleToTarget(bAngDeg, geoDist);
        //    return aLatAngDeg - bLatAngDeg;
        //}

        public double GeocentricDistanceToTarget(double latMdeg, double decGdeg, double decMdeg)
        {
            //Draw two lines, one from the center of a circle and one from the rim of the circle
            // which meet outside the circle at a point that is some distance from the center of the circle.
            // the point on the circle is defined with respect to the angular arc from this point on the circle to the
            // intersection of the center line and the circle.
            //Given the angle of the arc and the angle at the line intersection, this procedure returns the distance (miles) to the point
            // for a circle the siae of the earth.

            //Earth radius (avg)
            const double RE = 3958.8; //radiusEarthInMiles
            //Convert degrees to radians
            double latMrad = latMdeg * Math.PI / 180.0;
            double decGrad = decGdeg * Math.PI / 180.0;
            double decMrad = decMdeg * Math.PI / 180.0;
            //Calculate angles -- difference in declination and difference in latitude
            double deltaDecMGrad = decMrad - decGrad;
            double deltaLatMGrad = latMrad - decGrad;
            //Calculate legs of interior triangle
            double distBCmiles = RE * Math.Cos(deltaLatMGrad);
            double distMBmiles = RE * Math.Sin(deltaLatMGrad);
            //Calculate tangent of difference in dec (angle at N)
            double tanN = Math.Tan(deltaDecMGrad);
            double distNBmiles = distMBmiles / tanN;
            double distNCmiles = distNBmiles + distBCmiles;
            //

            return distNCmiles;
        }

        public double GeodeticAngleToTarget(double latSdeg, double decGdeg, double geoDist)
        {
            //Draw two lines, one from the center of a circle and one from the rim of the circle
            // which meet outside the circle at a point that is some distance from the center of the circle.
            // the point on the circle is defined with respect to the angular arc from this point on the circle to the
            // intersection of the center line and the circle.
            //Given the angle of the arc and the distance to the point, this procedure returns the arc angle
            // for a circle the siae of the earth.

            //Earth radius (average)
            const double Re = 3958.8; //Miles
            //Convert degrees to radians
            double latSrad = latSdeg * Math.PI / 180.0;
            double decGrad = decGdeg * Math.PI / 180.0;
            //Calculate angles -- difference in declination and difference in latitude
            //double deltaDecMGrad = decSrad - decGrad;
            double angDCSrad = latSrad - decGrad;
            //Calculate legs
            double distDC = Re * Math.Cos(angDCSrad);
            double distSD = Re * Math.Sin(angDCSrad);
            double distND = geoDist - distDC;
            //Calculate angle
            double angSNBrad = Math.Atan(distSD/distND);
            double angSNBdeg = angSNBrad * (180 / Math.PI);
            return angSNBdeg;
        }
    }

    public class Location
    {
        public string Code { get; set; }
        public double SiteLat { get; set; }
        public double SiteLong { get; set; }
        public double ObsLat { get; set; }
        public double ObsLong { get; set; }
        public double VarianceRA { get; set; }
        public double VarianceDec { get; set; }

    }

}

