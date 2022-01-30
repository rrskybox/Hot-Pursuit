using System;
using System.Deployment.Application;
using System.Drawing;
using System.Windows.Forms;
using AstroMath;
using TheSky64Lib;

namespace Hot_Pursuit
{
    public partial class FormHotPursuit : Form
    {
        public bool InPursuit = false;
        public SearchScout ScoutData;
        public SearchHorizons HorizonsData;

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

            PursueButton.BackColor = Color.LightGreen;
            HorizonsButton.BackColor = Color.LightGreen;
            AbortButton.BackColor = Color.LightGreen;
            CloseButton.BackColor = Color.LightGreen;

            ImageButton.BackColor = Color.Yellow;
            ImageAbort.Visible = false;

            //fill in filters box
            return;
        }

        public bool AbortRequested { get; set; } = false;

        private void PursueButton_Click(object sender, EventArgs e)
        {
            if (InPursuit)
                return;
            InPursuit = true;
            PursueButton.BackColor = Color.Salmon;
            ScoutData = new SearchScout();
            //Retrieve current target name from TSX and set in ss
            if (LookUpCheckBox.Checked)
                ScoutData.TgtName = TargetBox.Text;
            else
                ScoutData.TgtName = ScoutData.GetTargetName();
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
            if (!ScoutData.CLSToTarget(nextUpdateSV))
            {
                UpdateStatusLine("Tracking failed: Problem with CLS.");
                CleanupOnFault();
                return;
            }
            (double r, double d) = GetCurrentTelePosition();
            //Prompt for imaging
            ImageButton.BackColor = Color.LightGreen;
            //Set custom tracking 
            if (!ScoutData.SetTargetTracking(nextUpdateSV))
                TargetBox.BackColor = Color.LightSalmon;
            else
                TargetBox.BackColor = Color.LightGreen;
            RARateBox.Text = nextUpdateSV.Rate_RA_CosDec_ArcsecPerMinute.ToString("0.000");
            DecRateBox.Text = nextUpdateSV.Rate_Dec_ArcsecPerMinute.ToString("0.000");
            CorrectionBox.Text = Utils.HourString(ScoutData.RA_CorrectionD, true) + "/" + Utils.DegreeString(ScoutData.Dec_CorrectionD, true);
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
                        OneSecondPulse(PursueButton);
                        NextUpdateBox.Text = (nextUpdate - DateTime.UtcNow).TotalSeconds.ToString("0");
                        if (AbortRequested)
                            break;
                    }
                    ScoutData.SetTargetTracking(nextUpdateSV);
                    if (!ScoutData.SetTargetTracking(nextUpdateSV))
                        TargetBox.BackColor = Color.LightSalmon;
                    else
                        TargetBox.BackColor = Color.LightGreen;
                    RARateBox.Text = nextUpdateSV.Rate_RA_CosDec_ArcsecPerMinute.ToString("0.000");
                    DecRateBox.Text = nextUpdateSV.Rate_Dec_ArcsecPerMinute.ToString("0.000");
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
            HorizonsData = new SearchHorizons();
            if (LookUpCheckBox.Checked)
                HorizonsData.TgtName = TargetBox.Text;
            else
                HorizonsData.TgtName = HorizonsData.GetTargetName();
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
            FiltersListBox.SelectedIndex = 0;
            this.Show();
            //Set start time for ephemeris to current UTC, then set update step period from form
            HorizonsData.EphStart = DateTime.UtcNow;
            HorizonsData.EphStep = TimeSpan.FromMinutes(1); //Shortest that Horizons can do is 1 minute intervals
            HorizonsData.EphEnd = HorizonsData.EphStart + TimeSpan.FromDays(1);  //Shortest time that Horizons can do is one day
            if (!HorizonsData.LoadTargetData())
            {
                UpdateStatusLine("Problem with loading target data. The target may no longer be in the CNEOS Listing.");
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
            if (!HorizonsData.CLSToTarget(nextUpdateSV))
            {
                UpdateStatusLine("Tracking failed: Problem with CLS.");
                CleanupOnFault();
                return;
            }
            //Prompt for imaging
            ImageButton.BackColor = Color.LightGreen;

            //Set custom tracking 
            if (!HorizonsData.SetTargetTracking(nextUpdateSV))
                TargetBox.BackColor = Color.LightSalmon;
            else
                TargetBox.BackColor = Color.LightGreen;
            RARateBox.Text = nextUpdateSV.Rate_RA_CosDec_ArcsecPerMinute.ToString("0.000");
            DecRateBox.Text = nextUpdateSV.Rate_Dec_ArcsecPerMinute.ToString("0.000");
            CorrectionBox.Text = Utils.HourString(HorizonsData.RA_CorrectionD, true) + "/" + Utils.DegreeString(HorizonsData.Dec_CorrectionD, true);
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
                        if (AbortRequested)
                            break;
                    }
                    HorizonsData.SetTargetTracking(nextUpdateSV);
                    if (!HorizonsData.SetTargetTracking(nextUpdateSV))
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
                    HorizonsData.LoadTargetData();
                }
            }
            CleanupOnFault();
            return;
        }

        private void CleanupOnFault()
        {
            AbortRequested = false;
            InPursuit = false;
            PursueButton.BackColor = Color.LightGreen;
            HorizonsButton.BackColor = Color.LightGreen;
            TargetBox.BackColor = Color.White;
            ImageButton.BackColor = Color.Yellow;
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

        private void CloseButton_Click(object sender, EventArgs e)
        {
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
            return;
        }

        private void ImageButton_Click(object sender, EventArgs e)
        {
            //Sets up and runs a single shot using the exposure and filter set by the form
            ImageButton.BackColor = Color.LightSalmon;
            ImageAbort.Visible = true;
            ImageAbort.BackColor = Color.LightGreen;
            ImageAbort.Enabled = true;

            ccdsoftCamera tsxc = new ccdsoftCamera()
            {
                Subframe = 0,
                Delay = 0,
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
                UpdateStatusLine("Imaging Error: " + ex.Message);
            }
            return;
        }

        private void ImageAbort_Click(object sender, EventArgs e)
        {
            //Stop the imaging, clean up form
            ccdsoftCamera tsxc = new ccdsoftCamera();
            tsxc.Abort();
            ImageButton.BackColor = Color.LightGreen;
            ImageAbort.Visible = false;
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
            return;
        }

        private void FiltersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.FilterIndexZeroBased = FiltersListBox.SelectedIndex;
            return;
        }

    }
}

