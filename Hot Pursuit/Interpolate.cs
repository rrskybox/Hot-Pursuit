using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AstroMath;
using System.Threading.Tasks;

namespace Hot_Pursuit
{
    //this class is only used by SearchScout, not by SearchHorizons
    //  so, PA is not required for Horizons

    public class Interpolate
    {
        public List<SpeedVector> WayPoints { get; set; }

        public Interpolate(SpeedVector startSV, SpeedVector endSV, int updateSeconds)
        {
            //Linear interpolation of speed and direction changes
            WayPoints = new List<SpeedVector>();
            int updatePeriods = (int)((endSV.Time_UTC - startSV.Time_UTC).TotalSeconds) / updateSeconds;
            double diffRate = (endSV.Rate_ArcsecPerMinute - startSV.Rate_ArcsecPerMinute) / updatePeriods;
            double diffPA = (endSV.PA_Degrees - startSV.PA_Degrees) / updatePeriods;
            double diffRARate = (endSV.RA_Degrees - startSV.RA_Degrees) / updatePeriods;
            double diffDecRate = (endSV.Dec_Degrees - startSV.Dec_Degrees) / updatePeriods;

            DateTime updateTime = startSV.Time_UTC;
            double updateRate = startSV.Rate_ArcsecPerMinute;
            double updatePA = startSV.PA_Degrees;
            double updateRARate = startSV.Rate_RA_CosDec_ArcsecPerMinute;
            double updateDecRate = startSV.Rate_Dec_ArcsecPerMinute;
            double startRA = startSV.RA_Degrees;
            double startDec = startSV.Dec_Degrees;
            while (updateTime < endSV.Time_UTC)
            {
                WayPoints.Add(new SpeedVector { Time_UTC = updateTime, Rate_ArcsecPerMinute = updateRate, PA_Degrees = updatePA, RA_Degrees = startRA, Dec_Degrees = startDec, Rate_RA_CosDec_ArcsecPerMinute = updateRARate, Rate_Dec_ArcsecPerMinute = updateDecRate });
                updateTime += TimeSpan.FromSeconds(updateSeconds);
                updateRate += diffRate;
                updatePA += diffPA;
                updateRARate += diffRARate;
                updateDecRate += diffDecRate;
            }
        }
    }
    public class SpeedVector
    {
        //Structure to hold RA and Dec tracking speeds
        public DateTime Time_UTC { get; set; }
        public double RA_Degrees { get; set; }
        public double Dec_Degrees { get; set; }
        public double Rate_ArcsecPerMinute { get; set; }
        public double PA_Degrees { get; set; }
        public double Rate_RA_CosDec_ArcsecPerMinute { get; set; }
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
