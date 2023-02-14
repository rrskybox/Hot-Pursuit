using TheSky64Lib;

namespace Hot_Pursuit
{
    public static class SafetyCheck
    {
        //Class to deal with slewing or tracking over limits, if any
        public static bool IsMountAboveHorizon()
        {
            //Check to see if altitude is below horizon
            (double az, double alt) = Utils.GetCurrentAzAltPosition();
            if (alt <= 0)
                return false;
            else
                return true;
        }

        public static bool IsTargetAboveHorizon(double raDeg, double decDeg)
        {
            sky6Utils tsxu = new sky6Utils();
            double tgtRAH = AstroMath.Transform.DegreesToHours(raDeg);
            double tgtDecD = decDeg;
            tsxu.ConvertRADecToAzAlt(tgtRAH, tgtDecD);
            double tgtAzmD = tsxu.dOut0;
            double tgtAltD = tsxu.dOut1;
            if (tgtAltD > 0)
                return true;
            else
                return false;

        }
    }
}
