using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot_Pursuit
{
    public static class Utils
    {
        public const double Astronomical_Unit = 149597870.700;  //km
        public const double Earth_Radius = 6371.0; //km


        public static string HourString(double ha, bool shorten)
        {
            //convert double hours to hh:mm:ss
            int iSign = Math.Sign(ha);
            int iHrs = (int)((double)iSign * ha);
            int iMin = (int)((((double)iSign * ha) - (double)iHrs) * 60.0);
            double dSec = (double)((((double)iSign * ha) - ((double)iHrs + ((double)iMin / 60.0))) * 60.0 * 60.0);
            if (shorten && iHrs == 0)
                return (iSign * iMin).ToString("00") + "m" + dSec.ToString("00") + "s";
            else
                return (iSign * iHrs).ToString("00") + "h" + iMin.ToString("00") + "m" + dSec.ToString("00") + "s";

        }

        public static string DegreeString(double d, bool shorten)
        {
            //convert double degrees to dd:mm:ss
            int iSign = Math.Sign(d);
            int iHrs = (int)((double)iSign * d);
            int iMin = (int)((((double)iSign * d) - (double)iHrs) * 60.0);
            double dSec = (double)((((double)iSign * d) - ((double)iHrs + ((double)iMin / 60.0))) * 60.0 * 60.0);
            if (shorten && iHrs == 0)
                return (iSign * iMin).ToString("00") + "m" + dSec.ToString("00") + "s";
            else
                return (iSign * iHrs).ToString("00") + "d" + iMin.ToString("00") + "m" + dSec.ToString("00") + "s";
        }
    }
}
