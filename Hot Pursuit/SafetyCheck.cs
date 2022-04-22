using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot_Pursuit
{
    public static class SafetyCheck
    {
        //Class to deal with slewing or tracking over limits, if any
        public static bool IsAboveHorizon()
        {
            //Check to see if altitude is below horizon
            (double az, double alt) = Utils.GetCurrentAzAltPosition();
            if (alt <= 0)
                return false;
            else
                return true;
        }
    }
}
