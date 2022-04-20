using System;
using System.Collections.Generic;

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

            double diffRA = (endSV.RA_Degrees - startSV.RA_Degrees)/ updatePeriods;
            double diffDec = (endSV.Dec_Degrees - startSV.Dec_Degrees)/updatePeriods;
            double diffRate = (endSV.Rate_ArcsecPerMinute - startSV.Rate_ArcsecPerMinute) / updatePeriods;
            double diffPA = (endSV.PA_Degrees - startSV.PA_Degrees) / updatePeriods;
            double diffRARate = (endSV.Rate_RA_ArcsecPerMinute - startSV.Rate_RA_ArcsecPerMinute) / updatePeriods;
            double diffDecRate = (endSV.Rate_Dec_ArcsecPerMinute - startSV.Rate_Dec_ArcsecPerMinute) / updatePeriods;

            DateTime updateTime = startSV.Time_UTC + TimeSpan.FromSeconds(updateSeconds);
            double updateRA = startSV.RA_Degrees;
            double updateDec = startSV.Dec_Degrees;
            double updateRate = startSV.Rate_ArcsecPerMinute;
            double updatePA = startSV.PA_Degrees;
            double updateRACosDecRate = startSV.Rate_RA_CosDec_ArcsecPerMinute;
            double updateRARate = startSV.Rate_RA_ArcsecPerMinute;
            double updateDecRate = startSV.Rate_Dec_ArcsecPerMinute;
            while (updateTime < endSV.Time_UTC)
            {
                WayPoints.Add(new SpeedVector
                {
                    Time_UTC = updateTime,
                    RA_Degrees = updateRA,
                    Dec_Degrees = updateDec,
                    Rate_ArcsecPerMinute = updateRate,
                    PA_Degrees = updatePA,
                    Rate_RA_CosDec_ArcsecPerMinute = updateRACosDecRate,
                    Rate_RA_ArcsecPerMinute = updateRARate,
                    Rate_Dec_ArcsecPerMinute = updateDecRate
                });
                updateTime += TimeSpan.FromSeconds(updateSeconds);
                updateRate += diffRate;
                updatePA += diffPA;
                updateRARate += diffRARate;
                updateDecRate += diffDecRate;
                updateRA += diffRA;
                updateDec += diffDec;
            }
        }
    }
}
