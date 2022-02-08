using System;
using System.Deployment.Application;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using AstroMath;
using TheSky64Lib;

namespace Hot_Pursuit
{
    public partial class FormHotPursuit : Form
    {
        public bool InPursuit = false;
        public bool IsImaging = false;
        public SearchScout ScoutData;
        public SearchHorizons HorizonsData;
        public SearchMPES MPESData;

        public FormHotPursuit()
        {
            InitializeComponent();
            string version;
            try
            { version = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(); }
            catch
            {
                //probably in debug mode
                version = "  **in Debug**";
            }
            this.Text = "Hot Pursuit V" + version;
            FullReductionCheckBox.Checked = Properties.Settings.Default.FullReduction;
            OnTopBox.Checked = Properties.Settings.Default.IsOnTop;
            CLSBox.Checked = Properties.Settings.Default.UseCLS;

            ScoutButton.BackColor = Color.LightGreen;
            HorizonsButton.BackColor = Color.LightGreen;
            MPESButton.BackColor = Color.LightGreen;
            AbortButton.BackColor = Color.LightGreen;
            CloseButton.BackColor = Color.LightGreen;

            ImageButton.BackColor = Color.Yellow;
            ImageAbort.Visible = false;

            //fill in filters box
            return;
        }

        public bool AbortRequested { get; set; } = false;

        private void ScoutButton_Click(object sender, EventArgs e)
        {
            if (InPursuit)
                return;
            InPursuit = true;
            ScoutButton.BackColor = Color.Salmon;
            ClearFields();
            ScoutData = new SearchScout();
            //Retrieve current target name from TSX and set in ss
            if (LookUpCheckBox.Checked)
                ScoutData.TgtName = TargetBox.Text;
            else
                ScoutData.TgtName = Utils.GetTargetName();
            TargetBox.Text = ScoutData.TgtName;
            //Handle exceptions
            if (ScoutData.TgtName == null)
            {
                UpdateStatusLine("No target is found.  Check TheSkyX for target assignment.");
                CleanupOnFault();
                return;
            }
            UpdateStatusLine("Now targetting: " + ScoutData.TgtName);
            //fill in Filters list
            FiltersListBox.Items.AddRange(Filters.FilterNameSet());
            FiltersListBox.SelectedIndex = Properties.Settings.Default.FilterIndexZeroBased;
            this.Show();
            //Set start time for ephemeris to current UTC, then set update step period from form
            ScoutData.EphStart = DateTime.UtcNow;
            if (MinutesButton.Checked)
                ScoutData.EphStep = TimeSpan.FromMinutes((double)UpdateBox.Value);
            else
                ScoutData.EphStep = TimeSpan.FromMinutes(1);
            ScoutData.EphEnd = ScoutData.EphStart + TimeSpan.FromMinutes((100 * ScoutData.EphStep.TotalMinutes));
            //Update ss with ephemeris data from scout -- handle problems if they occur
            if (!ScoutData.LoadTargetData(MinutesButton.Checked, (int)UpdateBox.Value))
            {
                UpdateStatusLine("Problem with loading target data. The target may no longer be in the CNEOS Listing.");
                CleanupOnFault();
                return;
            }
            UpdateStatusLine("Closest observatory: " + ScoutData.MPC_Observatory.BestObservatory.MPC_Code +
                            " (" + ScoutData.MPC_Observatory.BestObservatory.Description + ")" +
                            " Variance: " + Utils.HourString(ScoutData.MPC_Observatory.BestObservatory.VarianceRA, true) +
                            "(Lat) / " + Utils.DegreeString(ScoutData.MPC_Observatory.BestObservatory.VarianceDec, true) +
                            " (Lon)");
            //Fire off first tracking instruction
            SpeedVector nextUpdateSV = ScoutData.GetNextRateUpdate(ScoutData.EphStart);
            //CLS to where target should be currently, deal with CLS failure
            if (!Utils.CLSToTarget(ScoutData.TgtName, nextUpdateSV, CLSBox.Checked))
            {
                UpdateStatusLine("Tracking failed: Problem with CLS.");
                CleanupOnFault();
                return;
            }
            (double r, double d) = GetCurrentTelePosition();
            //Prompt for imaging
            ImageButton.BackColor = Color.LightGreen;
            //Set custom tracking 
            if (!Utils.SetTargetTracking(nextUpdateSV, ScoutData.Topo_RA_Correction_Factor, ScoutData.Topo_Dec_Correction_Factor))
                TargetBox.BackColor = Color.LightSalmon;
            else
                TargetBox.BackColor = Color.LightGreen;
            RARateBox.Text = nextUpdateSV.Rate_RA_CosDec_ArcsecPerMinute.ToString("0.000");
            DecRateBox.Text = nextUpdateSV.Rate_Dec_ArcsecPerMinute.ToString("0.000");
            CorrectionBox.Text = Utils.HourString(ScoutData.RA_CorrectionD, true) + "/" + Utils.DegreeString(ScoutData.Dec_CorrectionD, true);
            RangeBox.Text = nextUpdateSV.Range_AU.ToString("0.00");
            DateTime nextUpdate = nextUpdateSV.Time_UTC;
            if (MinutesButton.Checked)
                nextUpdate += TimeSpan.FromMinutes((int)UpdateBox.Value);
            else
                nextUpdate += TimeSpan.FromSeconds((int)UpdateBox.Value);
            NextUpdateBox.Text = (nextUpdate - DateTime.UtcNow).TotalSeconds.ToString("0");
            //**************************  site location status code
            UpdateStatusLine("This site astometry: " +
                                ScoutData.Site_Corrected_Range.ToString("0.00") + " AU:  " +
                                Utils.HourString(Transform.DegreesToHours(ScoutData.Site_Corrected_RA), false) + " / " +
                                Utils.DegreeString(ScoutData.Site_Corrected_Dec, false) + " (RA/Dec)");
            //**************************
            //Update status
            UpdateStatusLine("Ephemeris @" + nextUpdateSV.Time_UTC.ToString("HH:mm:ss") + " (UTC)\r\n" +
                            "    MPC Obs " + ScoutData.MPC_Observatory.BestObservatory.MPC_Code + ": " + Utils.HourString(Transform.DegreesToHours(nextUpdateSV.RA_Degrees), false) +
                            " / " + Utils.DegreeString((nextUpdateSV.Dec_Degrees), false) + "(RA/Dec) " +
                            "    Site corrected: " + Utils.HourString(Transform.DegreesToHours(nextUpdateSV.RA_Degrees - ScoutData.RA_CorrectionD), false) +
                            " / " + Utils.DegreeString((nextUpdateSV.Dec_Degrees - ScoutData.Dec_CorrectionD), false) + "(RA/Dec)");

            //Set up for next tracking instruction
            while (!AbortRequested)
            {
                //the next target ephemeris has been loaded into the ss object, but assume not
                nextUpdateSV = ScoutData.GetNextRateUpdate(DateTime.UtcNow);
                if (nextUpdateSV != null)
                {
                    nextUpdate = nextUpdateSV.Time_UTC;
                    while (DateTime.UtcNow < nextUpdate)
                    {
                        OneSecondPulse(ScoutButton);
                        NextUpdateBox.Text = (nextUpdate - DateTime.UtcNow).TotalSeconds.ToString("0");
                        CheckImaging();
                        if (AbortRequested)
                            break;
                    }
                    if (!Utils.SetTargetTracking(nextUpdateSV, ScoutData.Topo_RA_Correction_Factor, ScoutData.Topo_Dec_Correction_Factor))
                        TargetBox.BackColor = Color.LightSalmon;
                    else
                        TargetBox.BackColor = Color.LightGreen;
                    RARateBox.Text = nextUpdateSV.Rate_RA_CosDec_ArcsecPerMinute.ToString("0.000");
                    DecRateBox.Text = nextUpdateSV.Rate_Dec_ArcsecPerMinute.ToString("0.000");
                    RangeBox.Text = nextUpdateSV.Range_AU.ToString("0.00");
                    //Update status
                    UpdateStatusLine("Ephemeris @" + nextUpdateSV.Time_UTC.ToString("HH:mm:ss") + " (UTC)\r\n" +
                                    "    MPC Obs " + ScoutData.MPC_Observatory.BestObservatory.MPC_Code + ": " + Utils.HourString(Transform.DegreesToHours(nextUpdateSV.RA_Degrees), false) +
                                    " / " + Utils.DegreeString((nextUpdateSV.Dec_Degrees), false) + "(RA/Dec) " +
                                    "    Site corrected: " + Utils.HourString(Transform.DegreesToHours(nextUpdateSV.RA_Degrees - ScoutData.RA_CorrectionD), false) +
                                    " / " + Utils.DegreeString((nextUpdateSV.Dec_Degrees - ScoutData.Dec_CorrectionD), false) + "(RA/Dec)");
                    //
                }
                else //no new update -- go get another
                {
                    ScoutData = new SearchScout();
                    ScoutData.EphStart = DateTime.UtcNow;
                    ScoutData.EphStep = TimeSpan.FromMinutes((double)UpdateBox.Value);
                    ScoutData.EphEnd = ScoutData.EphStart + TimeSpan.FromMinutes((100 * ScoutData.EphStep.TotalMinutes));
                    ScoutData.LoadTargetData(MinutesButton.Checked, (int)UpdateBox.Value);
                }
            }
            CleanupOnFault();
            return;
        }

        private void HorizonsButton_Click(object sender, EventArgs e)
        {
            if (InPursuit)
                return;
            InPursuit = true;

            HorizonsButton.BackColor = Color.Salmon;
            ClearFields();
            HorizonsData = new SearchHorizons();
            if (LookUpCheckBox.Checked)
                HorizonsData.TgtName = TargetBox.Text;
            else
                HorizonsData.TgtName = Utils.GetTargetName();
            TargetBox.Text = HorizonsData.TgtName;
            //Handle exceptions
            if (HorizonsData.TgtName == null)
            {
                UpdateStatusLine("No target is found.  Check TheSkyX for target assignment.");
                CleanupOnFault();
                return;
            }
            UpdateStatusLine("Now targetting: " + HorizonsData.TgtName);
            //fill in Filters list
            FiltersListBox.Items.AddRange(Filters.FilterNameSet());
            FiltersListBox.SelectedIndex = Properties.Settings.Default.FilterIndexZeroBased;
            this.Show();
            //Set start time for ephemeris to current UTC, then set update step period from form
            HorizonsData.EphStart = DateTime.UtcNow;
            HorizonsData.EphStep = TimeSpan.FromMinutes(1); //Shortest that Horizons can do is 1 minute intervals
            HorizonsData.EphEnd = HorizonsData.EphStart + TimeSpan.FromDays(1);  //Shortest time that Horizons can do is one day
            if (!HorizonsData.LoadTargetData(MinutesButton.Checked, (int)UpdateBox.Value))
            {
                UpdateStatusLine("Problem with loading target data. The target may no longer be in the Horizons Listing.");
                CleanupOnFault();
                return;
            }
            UpdateStatusLine("Closest observatory: " + HorizonsData.MPC_Observatory.BestObservatory.MPC_Code +
                            " (" + HorizonsData.MPC_Observatory.BestObservatory.Description + ")" +
                            " Variance: " + Utils.HourString(HorizonsData.MPC_Observatory.BestObservatory.VarianceRA, true) +
                            "(Lat) / " + Utils.DegreeString(HorizonsData.MPC_Observatory.BestObservatory.VarianceDec, true) +
                            " (Lon)");
            //Fire off first tracking instruction
            SpeedVector nextUpdateSV = HorizonsData.GetNextRateUpdate(DateTime.UtcNow);
            //CLS to where target should be currently, deal with CLS failure
            if (!Utils.CLSToTarget(HorizonsData.TgtName, nextUpdateSV, CLSBox.Checked))
            {
                UpdateStatusLine("Tracking failed: Problem with CLS.");
                CleanupOnFault();
                return;
            }
            //Prompt for imaging
            ImageButton.BackColor = Color.LightGreen;

            //Set custom tracking 
            if (!Utils.SetTargetTracking(nextUpdateSV, 1, 1))
                TargetBox.BackColor = Color.LightSalmon;
            else
                TargetBox.BackColor = Color.LightGreen;
            RARateBox.Text = nextUpdateSV.Rate_RA_CosDec_ArcsecPerMinute.ToString("0.000");
            DecRateBox.Text = nextUpdateSV.Rate_Dec_ArcsecPerMinute.ToString("0.000");
            RangeBox.Text = nextUpdateSV.Range_AU.ToString("0.00");
            //CorrectionBox.Text = Utils.HourString(HorizonsData.RA_CorrectionD, true) + "/" + Utils.DegreeString(HorizonsData.Dec_CorrectionD, true);
            CorrectionBox.Text = "N/A";  //Not used -- Horizons ephemeras are topocentric to user's site
            DateTime nextUpdate = nextUpdateSV.Time_UTC;
            if (MinutesButton.Checked)
                nextUpdate += TimeSpan.FromMinutes((int)UpdateBox.Value);
            else
                nextUpdate += TimeSpan.FromSeconds((int)UpdateBox.Value);
            NextUpdateBox.Text = (nextUpdate - DateTime.UtcNow).TotalSeconds.ToString("0");
            //**************************  site location status code
            UpdateStatusLine("This site astometry: " +
                                HorizonsData.Site_Corrected_Range.ToString("0.00") + " AU:  " +
                                Utils.HourString(Transform.DegreesToHours(HorizonsData.Site_Corrected_RA), false) + " / " +
                                Utils.DegreeString(HorizonsData.Site_Corrected_Dec, false) + " (RA/Dec)");
            //**************************
            //Update status
            UpdateStatusLine("Ephemeris @" + nextUpdateSV.Time_UTC.ToString("HH:mm:ss") + " (UTC)\r\n" +
                            "    MPC Obs " + HorizonsData.MPC_Observatory.BestObservatory.MPC_Code + ": " + Utils.HourString(Transform.DegreesToHours(nextUpdateSV.RA_Degrees), false) +
                            " / " + Utils.DegreeString((nextUpdateSV.Dec_Degrees), false) + "(RA/Dec) " +
                            "    Site corrected: " + Utils.HourString(Transform.DegreesToHours(nextUpdateSV.RA_Degrees - HorizonsData.RA_CorrectionD), false) +
                            " / " + Utils.DegreeString((nextUpdateSV.Dec_Degrees - HorizonsData.Dec_CorrectionD), false) + "(RA/Dec)");

            //Set up for next tracking instruction
            while (!AbortRequested)
            {
                //the next target ephemeris has been loaded into the ss object, but assume not
                nextUpdateSV = HorizonsData.GetNextRateUpdate(DateTime.UtcNow);
                if (nextUpdateSV != null)
                {
                    nextUpdate = nextUpdateSV.Time_UTC;
                    while (DateTime.UtcNow < nextUpdate)
                    {
                        OneSecondPulse(HorizonsButton);
                        NextUpdateBox.Text = (nextUpdate - DateTime.UtcNow).TotalSeconds.ToString("0");
                        CheckImaging();
                        if (AbortRequested)
                            break;
                    }
                    if (!Utils.SetTargetTracking(nextUpdateSV, 1, 1))
                        TargetBox.BackColor = Color.LightSalmon;
                    else
                        TargetBox.BackColor = Color.LightGreen;
                    RARateBox.Text = nextUpdateSV.Rate_RA_CosDec_ArcsecPerMinute.ToString("0.000");
                    DecRateBox.Text = nextUpdateSV.Rate_Dec_ArcsecPerMinute.ToString("0.000");
                    //Update status
                    UpdateStatusLine("Ephemeris @" + nextUpdateSV.Time_UTC.ToString("HH:mm:ss") + " (UTC)\r\n" +
                                    "    MPC Obs " + HorizonsData.MPC_Observatory.BestObservatory.MPC_Code + ": " + Utils.HourString(Transform.DegreesToHours(nextUpdateSV.RA_Degrees), false) +
                                    " / " + Utils.DegreeString((nextUpdateSV.Dec_Degrees), false) + "(RA/Dec) " +
                                    "    Site corrected: " + Utils.HourString(Transform.DegreesToHours(nextUpdateSV.RA_Degrees - HorizonsData.RA_CorrectionD), false) +
                                    " / " + Utils.DegreeString((nextUpdateSV.Dec_Degrees - HorizonsData.Dec_CorrectionD), false) + "(RA/Dec)");
                    //
                }
                else //no new update -- go get another
                {
                    HorizonsData = new SearchHorizons
                    {
                        EphStart = DateTime.UtcNow,
                        EphEnd = DateTime.UtcNow + TimeSpan.FromDays(1),
                        EphStep = TimeSpan.FromMinutes((double)UpdateBox.Value)
                    };
                    HorizonsData.LoadTargetData(MinutesButton.Checked, (int)UpdateBox.Value);
                }
            }
            CleanupOnFault();
            return;
        }

        private void MPESButton_Click(object sender, EventArgs e)
        {
            if (InPursuit)
                return;
            InPursuit = true;

            MPESButton.BackColor = Color.Salmon;
            ClearFields();
            MPESData = new SearchMPES();
            if (LookUpCheckBox.Checked)
                MPESData.TgtName = TargetBox.Text;
            else
                MPESData.TgtName = MPESData.GetTargetName();
            TargetBox.Text = MPESData.TgtName;
            //Handle exceptions
            if (MPESData.TgtName == null)
            {
                UpdateStatusLine("No target is found.  Check TheSkyX for target assignment.");
                CleanupOnFault();
                return;
            }
            UpdateStatusLine("Now targetting: " + MPESData.TgtName);
            //fill in Filters list
            FiltersListBox.Items.AddRange(Filters.FilterNameSet());
            FiltersListBox.SelectedIndex = Properties.Settings.Default.FilterIndexZeroBased;
            this.Show();
            //Set start time for ephemeris to current UTC, then set update step period from form
            MPESData.EphStart = DateTime.UtcNow;
            MPESData.EphStep = TimeSpan.FromMinutes(1); //Shortest that MPES can do is 1 minute intervals
            MPESData.EphEnd = MPESData.EphStart + TimeSpan.FromDays(1);  //Shortest time that MPES can do is one day
            if (!MPESData.LoadTargetData(MinutesButton.Checked, (int)UpdateBox.Value))
            {
                UpdateStatusLine("Problem with loading target data. The target may no longer be in the MPES Listing.");
                CleanupOnFault();
                return;
            }
            UpdateStatusLine("Closest observatory: " + MPESData.MPC_Observatory.BestObservatory.MPC_Code +
                            " (" + MPESData.MPC_Observatory.BestObservatory.Description + ")" +
                            " Variance: " + Utils.HourString(MPESData.MPC_Observatory.BestObservatory.VarianceRA, true) +
                            "(Lat) / " + Utils.DegreeString(MPESData.MPC_Observatory.BestObservatory.VarianceDec, true) +
                            " (Lon)");
            //Fire off first tracking instruction
            SpeedVector nextUpdateSV = MPESData.GetNextRateUpdate(DateTime.UtcNow);
            //CLS to where target should be currently, deal with CLS failure
            if (!Utils.CLSToTarget(MPESData.TgtName, nextUpdateSV, CLSBox.Checked))
            {
                UpdateStatusLine("Tracking failed: Problem with CLS.");
                CleanupOnFault();
                return;
            }
            //Prompt for imaging
            ImageButton.BackColor = Color.LightGreen;

            //Set custom tracking 
            if (!Utils.SetTargetTracking(nextUpdateSV, 1, 1))
                TargetBox.BackColor = Color.LightSalmon;
            else
                TargetBox.BackColor = Color.LightGreen;
            RARateBox.Text = nextUpdateSV.Rate_RA_CosDec_ArcsecPerMinute.ToString("0.000");
            DecRateBox.Text = nextUpdateSV.Rate_Dec_ArcsecPerMinute.ToString("0.000");
            RangeBox.Text = nextUpdateSV.Range_AU.ToString("0.00");
            //CorrectionBox.Text = Utils.HourString(MPESData.RA_CorrectionD, true) + "/" + Utils.DegreeString(MPESData.Dec_CorrectionD, true);
            CorrectionBox.Text = "N/A";  //Not used -- MPES ephemeras are topocentric to user's site
            DateTime nextUpdate = nextUpdateSV.Time_UTC;
            if (MinutesButton.Checked)
                nextUpdate += TimeSpan.FromMinutes((int)UpdateBox.Value);
            else
                nextUpdate += TimeSpan.FromSeconds((int)UpdateBox.Value);
            NextUpdateBox.Text = (nextUpdate - DateTime.UtcNow).TotalSeconds.ToString("0");
            //**************************  site location status code
            UpdateStatusLine("This site astometry: " +
                                MPESData.Site_Corrected_Range.ToString("0.00") + " AU:  " +
                                Utils.HourString(Transform.DegreesToHours(MPESData.Site_Corrected_RA), false) + " / " +
                                Utils.DegreeString(MPESData.Site_Corrected_Dec, false) + " (RA/Dec)");
            //**************************
            //Update status
            UpdateStatusLine("Ephemeris @" + nextUpdateSV.Time_UTC.ToString("HH:mm:ss") + " (UTC)\r\n" +
                            "    MPC Obs " + MPESData.MPC_Observatory.BestObservatory.MPC_Code + ": " + Utils.HourString(Transform.DegreesToHours(nextUpdateSV.RA_Degrees), false) +
                            " / " + Utils.DegreeString((nextUpdateSV.Dec_Degrees), false) + "(RA/Dec) " +
                            "    Site corrected: " + Utils.HourString(Transform.DegreesToHours(nextUpdateSV.RA_Degrees - MPESData.RA_CorrectionD), false) +
                            " / " + Utils.DegreeString((nextUpdateSV.Dec_Degrees - MPESData.Dec_CorrectionD), false) + "(RA/Dec)");

            //Set up for next tracking instruction
            while (!AbortRequested)
            {
                //the next target ephemeris has been loaded into the ss object, but assume not
                nextUpdateSV = MPESData.GetNextRateUpdate(DateTime.UtcNow);
                if (nextUpdateSV != null)
                {
                    nextUpdate = nextUpdateSV.Time_UTC;
                    while (DateTime.UtcNow < nextUpdate)
                    {
                        OneSecondPulse(MPESButton);
                        NextUpdateBox.Text = (nextUpdate - DateTime.UtcNow).TotalSeconds.ToString("0");
                        CheckImaging();
                        if (AbortRequested)
                            break;
                    }
                    if (!Utils.SetTargetTracking(nextUpdateSV, 1, 1))
                        TargetBox.BackColor = Color.LightSalmon;
                    else
                        TargetBox.BackColor = Color.LightGreen;
                    RARateBox.Text = nextUpdateSV.Rate_RA_CosDec_ArcsecPerMinute.ToString("0.000");
                    DecRateBox.Text = nextUpdateSV.Rate_Dec_ArcsecPerMinute.ToString("0.000");
                    RangeBox.Text = nextUpdateSV.Range_AU.ToString("0.00");
                    //Update status
                    UpdateStatusLine("Ephemeris @" + nextUpdateSV.Time_UTC.ToString("HH:mm:ss") + " (UTC)\r\n" +
                                    "    MPC Obs " + MPESData.MPC_Observatory.BestObservatory.MPC_Code + ": " + Utils.HourString(Transform.DegreesToHours(nextUpdateSV.RA_Degrees), false) +
                                    " / " + Utils.DegreeString((nextUpdateSV.Dec_Degrees), false) + "(RA/Dec) " +
                                    "    Site corrected: " + Utils.HourString(Transform.DegreesToHours(nextUpdateSV.RA_Degrees - MPESData.RA_CorrectionD), false) +
                                    " / " + Utils.DegreeString((nextUpdateSV.Dec_Degrees - MPESData.Dec_CorrectionD), false) + "(RA/Dec)");
                    //
                }
                else //no new update -- go get another
                {
                    MPESData = new SearchMPES()
                    {
                        EphStart = DateTime.UtcNow,
                        EphEnd = DateTime.UtcNow + TimeSpan.FromDays(1),
                        EphStep = TimeSpan.FromMinutes((double)UpdateBox.Value)
                    };
                    MPESData.LoadTargetData(MinutesButton.Checked, (int)UpdateBox.Value);
                }
            }
            CleanupOnFault();
            return;
        }

        private void ImageButton_Click(object sender, EventArgs e)
        {
            //Sets up and runs a single shot using the exposure and filter set by the form
            ImageButton.BackColor = Color.LightSalmon;
            ImageAbort.Visible = true;
            ImageAbort.BackColor = Color.LightGreen;
            ImageAbort.Enabled = true;

            TakeImage();
            return;
        }

        private void TakeImage()
        {
            ccdsoftCamera tsxc = new ccdsoftCamera()
            {
                Subframe = 0,
                Delay = 0,
                AutoSaveOn = 1,
                FilterIndexZeroBased = (int)Filters.LookUpFilterIndex(FiltersListBox.Text),
                ExposureTime = (double)ExposureBox.Value,
                Asynchronous = 1 //asychronous is on
            };
            if (FullReductionCheckBox.Checked)
            {
                tsxc.ImageReduction = ccdsoftImageReduction.cdBiasDarkFlat;
                string binning = "1X1";
                Reduction calLib = new Reduction();
                calLib.SetReductionGroup(tsxc.FilterIndexZeroBased, tsxc.ExposureTime, (int)tsxc.TemperatureSetPoint, binning);
            }
            ImageAbort.BackColor = Color.LightGreen;
            tsxc.Connect();
            try
            {
                tsxc.TakeImage();
            }
            catch (Exception ex)
            {
                ImageButton.BackColor = Color.Yellow;
                IsImaging = false;
                UpdateStatusLine("Imaging Error: " + ex.Message);
                RepsBox.Value = 1;
            }
            IsImaging = true;
            tsxc = null;
            return;
        }

        private void CleanupOnFault()
        {
            AbortRequested = false;
            InPursuit = false;
            ScoutButton.BackColor = Color.LightGreen;
            MPESButton.BackColor = Color.LightGreen;
            HorizonsButton.BackColor = Color.LightGreen;
            TargetBox.BackColor = Color.White;
            CheckImaging();
            return;
        }

        private void OneSecondPulse(Button cmd)
        {
            cmd.BackColor = Color.LightSalmon;
            Show();
            System.Windows.Forms.Application.DoEvents();
            System.Threading.Thread.Sleep(500);
            cmd.BackColor = Color.Salmon;
            Show();
            System.Windows.Forms.Application.DoEvents();
            System.Threading.Thread.Sleep(500);
            Show();
            System.Windows.Forms.Application.DoEvents();
            return;
        }

        private void CheckImaging()
        {
            //Checks to see if imaging is going on, if not, turn off indicators that it is
            ccdsoftCamera tsxc = new ccdsoftCamera();
            if (tsxc.State == ccdsoftCameraState.cdStateNone && IsImaging)
            {
                if (InPursuit)
                {
                    if (RepsBox.Value > 1)
                    {
                        RepsBox.Value--;
                        ImageButton.BackColor = Color.Salmon;
                        TakeImage();
                    }
                    else
                        ImageButton.BackColor = Color.LightGreen;
                }
                else
                {
                    ImageButton.BackColor = Color.Yellow;
                    IsImaging = false;
                    ImageAbort.Visible = false;
                }
            }
        }


        private void ClearFields()
        {
            //Removes text from all fields
            RARateBox.Text = "";
            DecRateBox.Text = "";
            CorrectionBox.Text = "";
            NextUpdateBox.Text = "";
            RangeBox.Text = "";
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            Close();
        }

        private void AbortButton_Click(object sender, EventArgs e)
        {
            if (InPursuit)
                AbortRequested = true;
            return;
        }

        private void OnTopBox_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = OnTopBox.Checked;
            Properties.Settings.Default.IsOnTop = OnTopBox.Checked;
            Properties.Settings.Default.Save();
            return;
        }


        private void ImageAbort_Click(object sender, EventArgs e)
        {
            //Stop the imaging, clean up form
            ccdsoftCamera tsxc = new ccdsoftCamera();
            tsxc.Abort();
            ImageButton.BackColor = Color.LightGreen;
            ImageAbort.Visible = false;
            RepsBox.Value = 1;
            return;
        }

        private void UpdateStatusLine(string status)
        {
            //print out status if not null
            if (status != null)
                HPStatusBox.AppendText("\r\n" + status);
            return;
        }

        public static (double, double) GetCurrentTelePosition()
        {
            sky6RASCOMTele tsxm = new sky6RASCOMTele();
            tsxm.GetRaDec();
            sky6Utils tsxu = new sky6Utils();
            tsxu.PrecessNowTo2000(tsxm.dRa, tsxm.dDec);
            return (tsxu.dOut0, tsxu.dOut1);
        }

        private void FullReductionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.FullReduction = FullReductionCheckBox.Checked;
            Properties.Settings.Default.Save();
            return;
        }

        private void FiltersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.FilterIndexZeroBased = FiltersListBox.SelectedIndex;
            Properties.Settings.Default.Save();
            return;
        }

        private void TargetBox_Click(object sender, EventArgs e)
        {
            LookUpCheckBox.Checked = true;
            return;
        }

        private void CLSBox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.UseCLS = CLSBox.Checked;
            Properties.Settings.Default.Save();
            return;

        }
    }
}

