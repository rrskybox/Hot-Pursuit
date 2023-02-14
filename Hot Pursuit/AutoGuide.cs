using System;
using System.Collections.Generic;
using System.Linq;
using TheSky64Lib;

namespace Hot_Pursuit
{
    public static partial class AutoGuide
    {
        public const int edgeKeepOut = 100;
        public const int minNeighborDistance = 100;

        //MaxPixel-based method for getting the ADU of a guide star
        public static bool TrailBrightestSource(double exposure)
        {
            //Take a subframe image on the main camera, assuming it has been already set
            ccdsoftCamera tsxc = new ccdsoftCamera()
            {
                Frame = ccdsoftImageFrame.cdLight,
                BinX = 2,
                BinY = 2,
                Delay = 0,
                ExposureTime = exposure,
                ImageReduction = ccdsoftImageReduction.cdAutoDark,
                AutoSaveOn = 1
            };
            tsxc.Abort();
            //Full frame
            int tstat = tsxc.TakeImage();
            double maxPixel = tsxc.MaximumPixel;

            //acquire the current trackbox size (need it later)
            int TrackBoxSize = tsxc.TrackBoxX;

            SexTractor tsex = new SexTractor();

            try
            {
                int sStat = tsex.SourceExtractGuider();
            }
            catch (Exception ex)
            {
                // Just close up, TSX will spawn error window
                tsex.Close();
                return false;
            }

            int Xsize = tsex.WidthInPixels;
            int Ysize = tsex.HeightInPixels;

            // Collect astrometric light source data from the image linking into single index arrays: 
            //  magnitude, fmhm, ellipsicity, x and y positionc
            //

            List<double> MagArr = tsex.GetSourceExtractionArray(SexTractor.SourceExtractionType.sexMagnitude).ToList();
            int starCount = MagArr.Count();

            if (starCount == 0)
            {
                tsex.Close();
                return false;
            }
            List<double> FWHMArr = tsex.GetSourceExtractionArray(SexTractor.SourceExtractionType.sexFWHM).ToList();
            List<double> XPosArr = tsex.GetSourceExtractionArray(SexTractor.SourceExtractionType.sexX).ToList();
            List<double> YPosArr = tsex.GetSourceExtractionArray(SexTractor.SourceExtractionType.sexY).ToList();
            List<double> ElpArr = tsex.GetSourceExtractionArray(SexTractor.SourceExtractionType.sexEllipticity).ToList();
            List<double> ClsArr = tsex.GetSourceExtractionArray(SexTractor.SourceExtractionType.sexClass).ToList();

            // Get some useful statistics
            // Max and min magnitude
            // Max and min FWHM
            // Max and min ellipticity
            // max and min class
            // Average FWHM

            double maxMag = MagArr.Max();
            double minMag = MagArr.Min();
            double maxFWHM = FWHMArr.Max();
            double minFWHM = FWHMArr.Min();
            double maxElp = ElpArr.Max();
            double minElp = ElpArr.Min();
            double maxCls = ClsArr.Max();
            double minCls = ClsArr.Min();

            double avgFWHM = FWHMArr.Average();
            double avgMag = MagArr.Average();

            // Create a set of "best" values
            double optMag = minMag;       // Magnitudes increase with negative values
            double optFWHM = avgFWHM;     // Looking for the closest to maximum FWHM
            double optElp = minElp;     // Want the minimum amount of elongation
            double optCls = maxCls;      // 1 = star,0 = galaxy
                                         // Create a set of ranges
            double rangeMag = maxMag - minMag;
            double rangeFWHM = maxFWHM - minFWHM;
            double rangeElp = maxElp - minElp;
            double rangeCls = maxCls - minCls;
            // Create interrum variables for weights
            double normMag;
            double normFWHM;
            double normElp;
            double normCls;
            // Count keepers for statistics
            int SourceCount = 0;

            // Create a selection array to store normilized and summed difference values
            double[] NormArr = new double[starCount];

            // Convert all points to normalized differences, checking for zero ranges (e.g.single or identical data points)
            for (int i = 0; i < starCount; i++)
            {
                if (rangeMag != 0) { normMag = 1 - Math.Abs(optMag - MagArr[i]) / rangeMag; }
                else { normMag = 0; }
                if (rangeFWHM != 0) { normFWHM = 1 - Math.Abs(optFWHM - FWHMArr[i]) / rangeFWHM; }
                else { normFWHM = 0; }
                if (rangeElp != 0) { normElp = 1 - Math.Abs(optElp - ElpArr[i]) / rangeElp; }
                else { normElp = 0; }
                if (rangeCls != 0) { normCls = 1 - Math.Abs(optCls - ClsArr[i]) / rangeCls; }
                else { normCls = 0; }

                // Sum the normalized points, weight and store value
                NormArr[i] = (normMag) + (normFWHM) + (normElp) + (normCls);
                SourceCount += 1;

                // Remove neighbors and edge liers
                if (IsOnEdge((int)XPosArr[i], (int)YPosArr[i], Xsize, Ysize, edgeKeepOut )) { NormArr[i] = -1; }
                else
                {
                    for (int j = i + 1; j < starCount - 1; j++)
                    {
                        //if (IsNeighbor((int)XPosArr[i], (int)YPosArr[i], (int)XPosArr[j], (int)YPosArr[j], TrackBoxSize)) { NormArr[i] = -2; }
                        if (IsNeighbor((int)XPosArr[i], (int)YPosArr[i], (int)XPosArr[j], (int)YPosArr[j], minNeighborDistance )) { NormArr[i] = -2; }
                    }
                }
            }

            // Now find the best remaining entry

            int bestOne = 0;
            for (int i = 0; i < starCount; i++)
            {
                if (NormArr[i] > NormArr[bestOne])
                {
                    bestOne = i;
                }
            }

            tsxc.GuideStarX = XPosArr[bestOne] * tsxc.BinX;
            tsxc.GuideStarY = YPosArr[bestOne] * tsxc.BinY;
            //tsxc.SubframeLeft = (int)(XPosArr[bestOne] - (TrackBoxSize / 2)) * tsxc.BinX;
            //tsxc.SubframeRight = (int)(XPosArr[bestOne] + (TrackBoxSize / 2)) * tsxc.BinX;
            //tsxc.SubframeTop = (int)(YPosArr[bestOne] - (TrackBoxSize / 2)) * tsxc.BinY;
            //tsxc.SubframeBottom = (int)(YPosArr[bestOne] + (TrackBoxSize / 2)) * tsxc.BinY;

            if (NormArr[bestOne] != -1)
            {
                tsex.Close();
                tsxc.Asynchronous = 1;
                tsxc.Autoguide();
                return true;
            }
            else
            {
                tsex.Close();
                return false;
            }
        }

        //*** Determines if the given x,y position is off the border by at least the xsize and y size
        private static bool IsOnEdge(int Xpos, int Ypos, int Xsize, int Ysize, int border)
        {
            if ((Xpos - border > 0) &&
                (Xpos + border < Xsize) &&
                (Ypos - border > 0) &&
                (Ypos + border) < Ysize)
            { return false; }
            else
            { return true; }
        }

        //*** Determines if two x,y positions are within a given distance of each other
        private static bool IsNeighbor(int Xpos1, int Ypos1, int Xpos2, int Ypos2, int subsize)
        {
            int limit = subsize / 2;
            if ((Math.Abs(Xpos1 - Xpos2) >= limit) || (Math.Abs(Ypos1 - Ypos2) >= limit))
            { return false; }
            else
            { return true; };
        }

        public static void TrailAbort()
        {
            ccdsoftCamera tsxc = new ccdsoftCamera();
            tsxc.Abort();
            return;
        }
    }
    #region SexTractor

    public class SexTractor
    {
        // Added enumeration of inventory index because TSX doesn't
        public enum SourceExtractionType
        {
            sexX,
            sexY,
            sexMagnitude,
            sexClass,
            sexFWHM,
            sexMajorAxis,
            sexMinorAxis,
            sexTheta,
            sexEllipticity
        }

        public ccdsoftImage timg = null;

        public SexTractor()
        {
            //ccdsoftCamera tsxa = new ccdsoftCamera();
            timg = new ccdsoftImage();
            return;
        }

        public void Close()
        {
            return;
        }

        public int SourceExtractGuider()
        {
            int aStat = timg.AttachToActive();
            int iStat = timg.ShowInventory();
            return iStat;
        }

        //*** Converts an array of generic "objects" to an array of doubles
        private double[] ConvertDoubleArray(object[] oIn)
        {
            double[] dOut = new double[oIn.Length];
            for (int i = 0; i < oIn.Length; i++)
            {
                dOut[i] = Convert.ToDouble(oIn[i]);
            }
            return dOut;
        }

        //*** Converts an array of generic "objects" to a list of doubles
        private List<double> ConvertDoubleList(object[] oIn)
        {
            List<double> dOut = new List<double>();
            for (int i = 0; i < oIn.Length; i++)
            {
                dOut.Add(Convert.ToDouble(oIn[i]));
            }
            return dOut;
        }

        public double[] GetSourceExtractionArray(SourceExtractionType dataIndex)
        {
            {
                object[] iA = timg.InventoryArray((int)dataIndex);
                double[] sexArray = ConvertDoubleArray(iA);
                return sexArray;
            }

        }

        public int WidthInPixels => timg.WidthInPixels;

        public int HeightInPixels => timg.HeightInPixels;

    }

    #endregion

}
