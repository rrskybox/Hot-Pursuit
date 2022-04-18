using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Hot_Pursuit
{
    public class TLE
    {
        const double GM = 2.9755364e15;  //km3/d2
        //string[] CelestrakGroups = { "Amateur", "Beidou", "Galileo", "Geo", "GLO-ops", "GPS-ops", "Orbcomm", "Weather", "visual" };

        private List<TwoLineElement> satTLE;
        public List<string> TLECatalog = new List<string>();

        public TLE(string catPath)
        {
            satTLE = ParseFile(catPath);
            foreach (TwoLineElement tle in satTLE)
                TLECatalog.Add(tle.SatelliteName);
        }

        //public string GetTLEString(string tleName)
        //{
        //    //finds the record for satellite tleName and returns the original record in 3 strings
        //    string tn, t1, t2;
        //    var tleList = from TLE a Where (a => a.SatelliteName == tleName) select a;
        //    TwoLineElement tle = tleList.FirstOrDefault();
        //    return (tle.NameString + "\n" + tle.Line1String + "\n" + tle.Line2String);
        //}

        private List<TwoLineElement> ParseFile(string filePath)
        {
            //Reads in text file with list of TLE and returns results as list of TwoLineElements
            List<TwoLineElement> parsedTLE = new List<TwoLineElement>();
            StreamReader fTLE = File.OpenText(filePath);
            while (fTLE.Peek() != -1)
            {
                string nameLine = fTLE.ReadLine();
                string firstLine = fTLE.ReadLine();
                string secondLine = fTLE.ReadLine();

                parsedTLE.Add(ParseTLERecord(nameLine, firstLine, secondLine));
            }
            fTLE.Close();
            return parsedTLE;
        }

        private DateTime ConvertEpoch(string eString)
        {
            return DateTime.UtcNow;
        }

        private double ConvertMeanMotion2(string mm2)
        {
            return 0;
        }

        public TwoLineElement ParseTLERecord(string nameLine, string firstLine, string secondLine)
        {
            TwoLineElement tle = new TwoLineElement();
            //Save original TLE record
            tle.NameString = nameLine;
            tle.Line1String = firstLine;
            tle.Line2String = secondLine;

            tle.SatelliteName = nameLine;
            tle.SatelliteNumberA = firstLine.Substring(2, 5);
            tle.Classification = firstLine.Substring(7, 1);
            tle.LaunchYear = firstLine.Substring(9, 2);
            tle.LaunchNumber = firstLine.Substring(11, 2);
            tle.LaunchPiece = firstLine.Substring(12, 2);
            tle.EpochYear = firstLine.Substring(18, 2);
            tle.EpochDay = firstLine.Substring(20, 12);
            tle.MeanMotion_dt = firstLine.Substring(33, 10);
            tle.MeanMotion_d2t = firstLine.Substring(44, 8);
            tle.BStar = firstLine.Substring(53, 8);
            tle.EphemerisType = firstLine.Substring(62, 1);
            tle.ElementNumber = Convert.ToInt32(firstLine.Substring(64, 4));

            tle.SatelliteNumberB = Convert.ToInt32(secondLine.Substring(2, 5));
            tle.Inclination = Convert.ToDouble(secondLine.Substring(8, 8));
            tle.OmegaRA = Convert.ToDouble(secondLine.Substring(17, 8));
            tle.Eccentricity = Convert.ToDouble(secondLine.Substring(26, 7));
            tle.OmicronPerigree = Convert.ToDouble(secondLine.Substring(34, 8));
            tle.MeanAnomoly = Convert.ToDouble(secondLine.Substring(43, 8));
            tle.MeanMotion = Convert.ToDouble(secondLine.Substring(52, 11));
            tle.Revolution = Convert.ToDouble(secondLine.Substring(63, 5));
            //Launch as DateTime
            int lYear = Convert.ToInt32(tle.LaunchYear) + 2000;
            int lMonth = Convert.ToInt32(tle.LaunchNumber);
            int lDay = Convert.ToInt32(tle.LaunchPiece);
            try { tle.Launch = new DateTime(lYear, lMonth, lDay); }
            catch { };
            //Epoch as DateTime
            int eYear = Convert.ToInt32(tle.EpochYear) + 2000;
            double eDay = Convert.ToDouble(tle.EpochDay);
            tle.Epoch = new DateTime(eYear, 1, 1) + TimeSpan.FromDays(eDay);
            //SemiMajor Axis in km
            double T = 1.0 / tle.MeanMotion;
            double G = GM / (4 * Math.Pow(Math.PI, 2));
            double a3 = G * Math.Pow(T, 2);

            tle.SemiMajorAxis = Math.Pow(a3, (double)1.0 / 3.0);  //km

            return tle;
        }

        public struct TwoLineElement
        {
            public string NameString { get; set; }
            public string Line1String { get; set; }
            public string Line2String { get; set; }
            public string SatelliteName { get; set; }
            public string SatelliteNumberA { get; set; }
            public string Classification { get; set; }
            public string LaunchYear { get; set; }
            public string LaunchNumber { get; set; }
            public string LaunchPiece { get; set; }
            public string EpochYear { get; set; }
            public string EpochDay { get; set; }
            public string MeanMotion_dt { get; set; }
            public string MeanMotion_d2t { get; set; }
            public string BStar { get; set; }
            public string EphemerisType { get; set; }
            public int ElementNumber { get; set; }
            public int SatelliteNumberB { get; set; }
            public double Inclination { get; set; }
            public double OmegaRA { get; set; }
            public double Eccentricity { get; set; }
            public double OmicronPerigree { get; set; }
            public double MeanAnomoly { get; set; }
            public double MeanMotion { get; set; }
            public double Revolution { get; set; }
            public DateTime Launch { get; set; }
            public DateTime Epoch { get; set; }
            public double SemiMajorAxis { get; set; }
        }
    }
}

