using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AstroMath;
using System.Threading.Tasks;

namespace Hot_Pursuit
{
 

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

            DateTime updateTime = startSV.Time_UTC+TimeSpan.FromSeconds(updateSeconds);
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
}
