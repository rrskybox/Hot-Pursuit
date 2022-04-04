using AstroImage;
using AstroMath;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using TheSky64Lib;

namespace Hot_Pursuit
{
    public partial class FormHotPursuit : Form
    {
        public bool InPursuit = false;
        public bool IsImaging = false;

        public Ephemeris EphemTable;

        public string HPDirectoryPath;
        public string HPLogDirectoryPath;
        public string HPLogFilePath;
        public string HPImageFilePath;

        public List<FitsFile> ImageFrames;
        public FormImageStack formStack = null;

        public Thread StackThread = null;
        public Point StackFormLocation = new Point(0, 0);

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

            StartButton.BackColor = Color.LightGreen;
            StopButton.BackColor = Color.LightGreen;
            CloseButton.BackColor = Color.LightGreen;

            ImageButton.BackColor = Color.Yellow;

            HPDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Hot Pursuit";
            if (!Directory.Exists(HPDirectoryPath)) Directory.CreateDirectory(HPDirectoryPath);
            HPLogDirectoryPath = HPDirectoryPath + "\\" + "Logs";
            if (!Directory.Exists(HPLogDirectoryPath)) Directory.CreateDirectory(HPLogDirectoryPath);
            HPLogFilePath = HPLogDirectoryPath + "\\" + DateTime.Now.ToString("yyyy-MMM-dd") + ".txt";
            if (!File.Exists(HPLogFilePath)) File.CreateText(HPLogFilePath);
            return;
        }

        public bool AbortRequested { get; set; } = false;

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (InPursuit)
                return;
            //Retrieve current target name from TSX and set in ss
            string tName;
            if (TargetBox.Text == "")
                tName = Utils.GetTargetName();
            else
                tName = TargetBox.Text;
            TargetBox.Text = tName;
            Show();
            System.Windows.Forms.Application.DoEvents();
            if (tName == "")
                return;

            InPursuit = true;
            ClearFields();

            if (ScoutRadioButton.Checked)
            {
                EphemTable = new Ephemeris(Ephemeris.EphemSource.Scout, tName, MinutesButton.Checked, (int)RefreshIntervalBox.Value);
                WalkEphemerisTable(Ephemeris.EphemSource.Scout);
            }

            if (HorizonsRadioButton.Checked)
            {
                EphemTable = new Ephemeris(Ephemeris.EphemSource.MPES, tName, MinutesButton.Checked, (int)RefreshIntervalBox.Value);
                WalkEphemerisTable(Ephemeris.EphemSource.Horizons);
            }

            if (MPCRadioButton.Checked)
            {
                EphemTable = new Ephemeris(Ephemeris.EphemSource.MPES, tName, MinutesButton.Checked, (int)RefreshIntervalBox.Value);
                WalkEphemerisTable(Ephemeris.EphemSource.MPES);
            }

        }

        private void WalkEphemerisTable(Ephemeris.EphemSource dSource)
        {
            if (!EphemTable.HasData)
            {
                UpdateStatusLine("Problem with loading target data. The target may no longer be in the CNEOS Listing.");
                CleanupOnFault();
                return;
            }
            //Handle exceptions
            if (EphemTable.TgtName == null)
            {
                UpdateStatusLine("No target is found.  Check TheSkyX for target assignment.");
                CleanupOnFault();
                return;
            }
            UpdateStatusLine("Now targetting: " + EphemTable.TgtName);
            //fill in Filters list
            FiltersListBox.Items.AddRange(Filters.FilterNameSet());
            FiltersListBox.SelectedIndex = Properties.Settings.Default.FilterIndexZeroBased;
            this.Show();
            //Update ss with ephemeris data from scout -- handle problems if they occur
            UpdateStatusLine("Closest observatory: " + EphemTable.MPC_Observatory.BestObservatory.MPC_Code +
                            " (" + EphemTable.MPC_Observatory.BestObservatory.Description + ")" +
                            " Variance: " + Utils.HourString(EphemTable.MPC_Observatory.BestObservatory.VarianceRA, true) +
                            "(Lat) / " + Utils.DegreeString(EphemTable.MPC_Observatory.BestObservatory.VarianceDec, true) +
                            " (Lon)");
            //Fire off first tracking instruction
            SpeedVector nextUpdateSV = EphemTable.GetNextRateUpdate(EphemTable.EphStart);
            InitializeTargetTracking(nextUpdateSV);

            //Wait for 20 seconds for this to stablize
            System.Threading.Thread.Sleep(20);

            //**************************  site location status code
            UpdateStatusLine("Site corrected astrometry: " +
                                EphemTable.Site_Corrected_Range.ToString("0.00") + " AU:  " +
                                Utils.HourString(Transform.DegreesToHours(EphemTable.Site_Corrected_RA), false) + " / " +
                                Utils.DegreeString(EphemTable.Site_Corrected_Dec, false) + " (RA/Dec)");
            //**************************
            //Update status
            AssembleStatusUpdate(nextUpdateSV, true);
            //Set up for next tracking instruction
            while (!AbortRequested)
            {
                //the next target ephemeris has been loaded into the ss object, but assume not
                nextUpdateSV = EphemTable.GetNextRateUpdate(DateTime.UtcNow);
                if (nextUpdateSV != null)
                {
                    DateTime nextUpdate = nextUpdateSV.Time_UTC;
                    while (DateTime.UtcNow <= nextUpdate)
                    {
                        NextRecenterBox.Text = (nextUpdate - DateTime.UtcNow).TotalSeconds.ToString("0");
                        OneSecondPulse(StartButton);
                        CheckImaging();
                        if (AbortRequested)
                            break;
                    }
                    if (!Utils.SetTargetTracking(nextUpdateSV, EphemTable.Topo_RA_Correction_Factor, EphemTable.Topo_Dec_Correction_Factor))
                        TargetBox.BackColor = Color.LightSalmon;
                    else
                        TargetBox.BackColor = Color.LightGreen;
                    RARateBox.Text = nextUpdateSV.Rate_RA_ArcsecPerMinute.ToString("0.000");
                    DecRateBox.Text = nextUpdateSV.Rate_Dec_ArcsecPerMinute.ToString("0.000");
                    RangeBox.Text = nextUpdateSV.Range_AU.ToString("0.00");
                    //Update status
                    AssembleStatusUpdate(nextUpdateSV, false);
                    //
                }
                else //no new update -- go get another
                {
                    EphemTable = new Ephemeris(dSource, TargetBox.Text, MinutesButton.Checked, (int)RefreshIntervalBox.Value);
                    EphemTable.EphStart = DateTime.UtcNow;
                    EphemTable.EphStep = TimeSpan.FromMinutes((double)RefreshIntervalBox.Value);
                    EphemTable.EphEnd = EphemTable.EphStart + TimeSpan.FromMinutes((100 * EphemTable.EphStep.TotalMinutes));
                }
            }
            CleanupOnFault();
            return;
        }

        private void ImageButton_Click(object sender, EventArgs e)
        {
            //Sets up and runs a single shot using the exposure and filter set by the form
            ImageButton.BackColor = Color.LightSalmon;
            ImageFrames = new List<FitsFile>();  //Clear image stack
            TakeImage();
            return;
        }

        private bool TakeImage()
        {
            ccdsoftCamera tsxc = new ccdsoftCamera()
            {
                Frame = ccdsoftImageFrame.cdLight,
                Subframe = 0,
                Delay = 0,
                AutoSaveOn = 1,
                ExposureTime = (double)ExposureBox.Value,
                Asynchronous = 1, //asychronous is on
                AutoSavePrefix = TargetBox.Text.Replace('/', '_')
            };
            //try to set filter, if any
            if (FiltersListBox.Text != "")
                tsxc.FilterIndexZeroBased = (int)Filters.LookUpFilterIndex(FiltersListBox.Text);

            if (FullReductionCheckBox.Checked)
            {
                tsxc.ImageReduction = ccdsoftImageReduction.cdBiasDarkFlat;
                string binning = "1X1";
                Reduction calLib = new Reduction();
                calLib.SetReductionGroup(tsxc.FilterIndexZeroBased, tsxc.ExposureTime, (int)tsxc.TemperatureSetPoint, binning);
            }
            try
            {
                tsxc.Connect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Camera connect failure: " + ex.Message);
                LogEntry("Camera connect failure: " + ex.Message);
                IsImaging = true;
                tsxc = null;
                RepsBox.Value = 1;
                return false;
            }
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
                IsImaging = true;
                tsxc = null;
                return false;
            }
            IsImaging = true;
            tsxc = null;
            return true;
        }

        private void CleanupOnFault()
        {
            AbortRequested = false;
            InPursuit = false;
            StartButton.BackColor = Color.LightGreen;
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
            if (InPursuit)
            {
                if (tsxc.State == ccdsoftCameraState.cdStateNone && IsImaging)
                {
                    {
                        if (RepsBox.Value > 1)
                        {
                            RepsBox.Value--;
                            ImageButton.BackColor = Color.Salmon;
                            if (RecenterBox.Checked)
                            {
                                SpeedVector nextUpdateSV = EphemTable.GetNextRateUpdate(DateTime.UtcNow);
                                InPursuit = InitializeTargetTracking(nextUpdateSV);

                                //Wait for 20 seconds for this to stablize
                                System.Threading.Thread.Sleep(20);

                            }
                            TakeImage();
                        }
                        else
                        {
                            ImageButton.BackColor = Color.LightGreen;
                            tsxc.AutoSavePrefix = "";  //Clear target name prefix
                            IsImaging = false;
                        }
                        //display the most recent image stack
                        if (LiveStackBox.Checked)
                        {
                            FitsFile nextFF = new FitsFile(tsxc.LastImageFileName, true);
                            ImageFrames.Add(nextFF);  //load most recent fits file
                            if (StackThread != null)
                            {
                                StackFormLocation = formStack.Location;
                                StackThread.Abort();
                            }
                            else
                            {
                                StackFormLocation = new Point(200, 200);
                            }
                            ThreadStart displayStackForm = DisplayFrameStack;
                            StackThread = new Thread(displayStackForm);
                            StackThread.Start();
                        }
                    }
                }
            }
            else
            {
                tsxc.AutoSavePrefix = "";  //Clear target name prefix
                ImageButton.BackColor = Color.Yellow;
                if (IsImaging)
                    tsxc.Abort();
                IsImaging = false;
            }
        }

        private void DisplayFrameStack()
        {
            formStack = new FormImageStack(ImageFrames);
            formStack.Location = new Point(StackFormLocation.X, StackFormLocation.Y);
            formStack.ShowDialog();
            return;
        }

        private void ClearFields()
        {
            //Removes text from all fields
            RARateBox.Text = "";
            DecRateBox.Text = "";
            //CorrectionBox.Text = "";
            NextRecenterBox.Text = "";
            RangeBox.Text = "";
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (IsImaging)
                ImageAbort();
            Properties.Settings.Default.Save();
            Close();
        }

        private void StopButton_Click(object sender, EventArgs e)
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

        private void ImageAbort()
        {
            //Stop the imaging, clean up form
            ccdsoftCamera tsxc = new ccdsoftCamera();
            tsxc.Abort();
            ImageButton.BackColor = Color.LightGreen;
            RepsBox.Value = 1;
            return;
        }

        private void UpdateStatusLine(string status)
        {
            //print out status if not null
            if (status != null)
            {
                HPStatusBox.AppendText(status + "\r\n");
                LogEntry(status);
            }
            return;
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

        private void CLSBox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.UseCLS = CLSBox.Checked;
            Properties.Settings.Default.Save();
            return;
        }

        private void AssembleStatusUpdate(SpeedVector sv, Boolean fullStatus)
        {
            //Update status
            string returnStatus;
            (double dRAout, double dDecout) = Utils.GetTargetTracking();
            (double dRATSX, double dDecTSX) = Utils.GetObjectRates();
            returnStatus = "Ephemeris @" + sv.Time_UTC.ToString("HH:mm:ss") + " (UTC)"
                           + "    MPC Obs " + EphemTable.MPC_Observatory.BestObservatory.MPC_Code + ": "
                           + Utils.HourString(Transform.DegreesToHours(sv.RA_Degrees), false)
                           + " / " + Utils.DegreeString((sv.Dec_Degrees), false) + "(RA/Dec)";
            UpdateStatusLine(returnStatus);
            if (fullStatus)
            {
                returnStatus = "    Site corrected: " + Utils.HourString(Transform.DegreesToHours(sv.RA_Degrees - EphemTable.RA_CorrectionD), false)
                           + " / " + Utils.DegreeString((sv.Dec_Degrees - EphemTable.Dec_CorrectionD), false) + "(RA/Dec)";
                UpdateStatusLine(returnStatus);
            }
            returnStatus = "dRA/dt & dDec/dt (set) = "
                                + sv.Rate_RA_ArcsecPerMinute.ToString("0.000")
                                + "/"
                                + sv.Rate_Dec_ArcsecPerMinute.ToString("0.000")
                                + " (get) = "
                                + dRAout.ToString("0.000")
                                + "/"
                                + dDecout.ToString("0.000");
            UpdateStatusLine(returnStatus);
        }

        private bool InitializeTargetTracking(SpeedVector nextUpdateSV)
        {
            //CLS to where target should be currently, deal with CLS failure
            if (!Utils.CLSToTarget(EphemTable.TgtName, nextUpdateSV, CLSBox.Checked))
            {
                UpdateStatusLine("Tracking failed: Problem with Slew.");
                CleanupOnFault();
                return false;
            }
            (double r, double d) = Utils.GetCurrentTelePosition();
            //Prompt for imaging
            ImageButton.BackColor = Color.LightGreen;
            //Set custom tracking 
            if (!Utils.SetTargetTracking(nextUpdateSV, EphemTable.Topo_RA_Correction_Factor, EphemTable.Topo_Dec_Correction_Factor))
                TargetBox.BackColor = Color.LightSalmon;
            else
                TargetBox.BackColor = Color.LightGreen;
            RARateBox.Text = nextUpdateSV.Rate_RA_ArcsecPerMinute.ToString("0.000");
            DecRateBox.Text = nextUpdateSV.Rate_Dec_ArcsecPerMinute.ToString("0.000");
            RangeBox.Text = nextUpdateSV.Range_AU.ToString("0.00");
            DateTime nextUpdate = nextUpdateSV.Time_UTC;
            if (MinutesButton.Checked)
                nextUpdate += TimeSpan.FromMinutes((int)RefreshIntervalBox.Value);
            else
                nextUpdate += TimeSpan.FromSeconds((int)RefreshIntervalBox.Value);
            NextRecenterBox.Text = (nextUpdate - DateTime.UtcNow).TotalSeconds.ToString("0");
            return true;
        }

        private void LogEntry(string entryStuff)
        {
            string logtime = DateTime.Now.ToString("HH:mm:ss");
            File.AppendAllText(HPLogFilePath, (logtime + ": " + entryStuff + "\r\n"));
        }

    }
}

