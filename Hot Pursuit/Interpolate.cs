using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot_Pursuit
{
    class Interpolate
    {
        public List<SpeedVector> WayPoints { get; set; }

        public Interpolate(SpeedVector startSV, SpeedVector endSV, int updateSeconds)
        {
            //Linear interpolation of speed and direction changes
            WayPoints = new List<SpeedVector>();
            int updatePeriods = (int)((endSV.Time - startSV.Time).TotalSeconds) / updateSeconds;
            double diffRate = (endSV.Rate - startSV.Rate) / updatePeriods;
            double diffPA = (endSV.PA - startSV.PA) / updatePeriods;
            DateTime updateTime = startSV.Time;
            double updateRate = startSV.Rate;
            double updatePA = startSV.PA;
            double startRA = startSV.RA;
            double startDec = startSV.Dec;
            while (updateTime < endSV.Time)
            {
                WayPoints.Add(new SpeedVector { Time = updateTime, Rate = updateRate, PA = updatePA, RA = startRA, Dec = startDec });
                updateTime += TimeSpan.FromSeconds(updateSeconds);
                updateRate += diffRate;
                updatePA += diffPA;
            }
        }
    }
    public class SpeedVector
    {
        //Structure to hold RA and Dec tracking speeds
        public DateTime Time { get; set; }
        public double RA { get; set; }
        public double Dec { get; set; }
        public double Rate { get; set; }
        public double PA { get; set; }
        public SpeedVector() { }

        public double VelocityRA()
        {
            return Rate * Math.Cos(PA);
        }
        public double VelocityDec()
        {
            return Rate * Math.Sin(PA);
        }

    }
}
