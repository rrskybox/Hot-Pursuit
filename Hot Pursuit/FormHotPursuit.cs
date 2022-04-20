﻿using AstroImage;
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
        public enum ControlColor
        {
            DarkRed,
            Red,
            Yellow,
            Green
        }

        public bool InPursuit = false;
        public bool IsImaging = false;
        public string QuerySite = "Scout";

        public Ephemeris EphemTable;

        public string HPDirectoryPath;
        public string HPLogDirectoryPath;
        public string HPLogFilePath;
        public string HPImageFilePath;

        public List<FitsFile> ImageFrames;
        public FormImageStack formStack = null;

        public DateTime refreshTime;

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
            //Configure app to ignore SSL problems -- optional if servers are not working
            //System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

            FullReductionCheckBox.Checked = Properties.Settings.Default.FullReduction;
            OnTopBox.Checked = Properties.Settings.Default.IsOnTop;
            CLSBox.Checked = Properties.Settings.Default.UseCLS;
            SlewSettlingTimeDelayBox.Value = (decimal)Properties.Settings.Default.StartDelay;
            if (ScoutRadioButton.Checked) QuerySite = "Scout";
            if (HorizonsRadioButton.Checked) QuerySite = "Horizons";
            if (MPCRadioButton.Checked) QuerySite = "MPC";
            if (SatRadioButton.Checked) QuerySite = "Sat";
            if (TLERadioButton.Checked) QuerySite = "3TLE";

            SetBackColor(StartButton, ControlColor.Green);
            SetBackColor(StopButton, ControlColor.Green);
            SetBackColor(CloseButton, ControlColor.Green);
            SetBackColor(ImageButton, ControlColor.Yellow);

            HPDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Hot Pursuit";
            if (!Directory.Exists(HPDirectoryPath)) Directory.CreateDirectory(HPDirectoryPath);
            HPLogDirectoryPath = HPDirectoryPath + "\\" + "Logs";
            if (!Directory.Exists(HPLogDirectoryPath)) Directory.CreateDirectory(HPLogDirectoryPath);
            HPLogFilePath = HPLogDirectoryPath + "\\" + DateTime.Now.ToString("yyyy-MMM-dd") + ".txt";
            if (!File.Exists(HPLogFilePath)) File.CreateText(HPLogFilePath);
            return;
        }

        public bool AbortRequested { get; set; } = false;

        public void SetBackColor(Button button, ControlColor clr)
        {
            switch (clr)
            {
                case ControlColor.DarkRed:
                    {
                        button.BackColor = Color.Salmon;
                        break;
                    }
                case ControlColor.Red:
                    {
                        button.BackColor = Color.LightSalmon;
                        break;
                    }
                case ControlColor.Yellow:
                    {
                        button.BackColor = Color.LightYellow;
                        break;
                    }
                case ControlColor.Green:
                    {
                        button.BackColor = Color.LightGreen;
                        break;
                    }
            }
        }

        public void SetBackColor(TextBox button, ControlColor clr)
        {
            switch (clr)
            {
                case ControlColor.DarkRed:
                    {
                        button.BackColor = Color.Salmon;
                        break;
                    }
                case ControlColor.Red:
                    {
                        button.BackColor = Color.LightSalmon;
                        break;
                    }
                case ControlColor.Yellow:
                    {
                        button.BackColor = Color.LightYellow;
                        break;
                    }
                case ControlColor.Green:
                    {
                        button.BackColor = Color.LightGreen;
                        break;
                    }
            }
        }

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
            UpdateStatusLine("Querying " + QuerySite + " for " + tName);
            if (QuerySite == "Scout")
                UpdateStatusLine("Scout takes time; Be patient...");
            Show();
            System.Windows.Forms.Application.DoEvents();
            if (tName == "")
                return;

            InPursuit = true;
            ClearFields();

            if (ScoutRadioButton.Checked)
            {
                EphemTable = new Ephemeris(Ephemeris.EphemSource.Scout, tName, MinutesButton.Checked, (int)RefreshIntervalBox.Value);
                while (WalkEphemerisTable(Ephemeris.EphemSource.Scout))
                {
                    UpdateStatusLine("Acquiring new ephemeris table");
                    EphemTable = new Ephemeris(Ephemeris.EphemSource.Scout, tName, MinutesButton.Checked, (int)RefreshIntervalBox.Value);
                }
            }
            if (HorizonsRadioButton.Checked)
            {
                EphemTable = new Ephemeris(Ephemeris.EphemSource.Horizons, tName, MinutesButton.Checked, (int)RefreshIntervalBox.Value);
                while (WalkEphemerisTable(Ephemeris.EphemSource.Horizons))
                {
                    UpdateStatusLine("Acquiring new ephemeris table");
                    EphemTable = new Ephemeris(Ephemeris.EphemSource.Horizons, tName, MinutesButton.Checked, (int)RefreshIntervalBox.Value);
                }
            }
            if (MPCRadioButton.Checked)
            {
                EphemTable = new Ephemeris(Ephemeris.EphemSource.MPES, tName, MinutesButton.Checked, (int)RefreshIntervalBox.Value);
                {
                    UpdateStatusLine("Acquiring new ephemeris table");
                    while (WalkEphemerisTable(Ephemeris.EphemSource.MPES))
                        EphemTable = new Ephemeris(Ephemeris.EphemSource.MPES, tName, MinutesButton.Checked, (int)RefreshIntervalBox.Value);
                }
            }
            if (SatRadioButton.Checked)
            {
                EphemTable = new Ephemeris(Ephemeris.EphemSource.HorizonsSat, tName, MinutesButton.Checked, (int)RefreshIntervalBox.Value);
                while (WalkEphemerisTable(Ephemeris.EphemSource.HorizonsSat))
                {
                    UpdateStatusLine("Acquiring new ephemeris table");
                    EphemTable = new Ephemeris(Ephemeris.EphemSource.HorizonsSat, tName, MinutesButton.Checked, (int)RefreshIntervalBox.Value);
                }
            }
            if (TLERadioButton.Checked)
            {
                EphemTable = new Ephemeris(Ephemeris.EphemSource.HorizonsTLE, tName, MinutesButton.Checked, (int)RefreshIntervalBox.Value);
                {
                    UpdateStatusLine("Acquiring new ephemeris table");
                    while (WalkEphemerisTable(Ephemeris.EphemSource.HorizonsTLE))
                        EphemTable = new Ephemeris(Ephemeris.EphemSource.HorizonsTLE, tName, MinutesButton.Checked, (int)RefreshIntervalBox.Value);
                }
            }

        }

        private bool WalkEphemerisTable(Ephemeris.EphemSource dSource)
        {
            if (!EphemTable.HasData)
            {
                UpdateStatusLine("Problem with loading target data. The target may no longer be carried in its catalog.");
                CleanupOnFault();
                return false;
            }
            //Handle exceptions
            if (EphemTable.TgtName == null)
            {
                UpdateStatusLine("No target is found.  Check TheSkyX for target assignment.");
                CleanupOnFault();
                return false;
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
            SpeedVector nextUpdateSV = EphemTable.GetNearestRateUpdate(DateTime.UtcNow);
            if (!InitializeTargetTracking(nextUpdateSV))
            {
                CleanupOnFault();
                return false;
            }
            //Set refresh countdown timer;
            refreshTime = nextUpdateSV.Time_UTC + EphemTable.EphStep;
            //**************************  site location status code
            UpdateStatusLine("Site corrected astrometry: " +
                                EphemTable.Site_Corrected_Range.ToString("0.00") + " AU:  " +
                                Utils.HourString(Transform.DegreesToHours(EphemTable.Site_Corrected_RA), false) + " / " +
                                Utils.DegreeString(EphemTable.Site_Corrected_Dec, false) + " (RA/Dec)");
            //**************************
            //Update status
            AssembleStatusUpdate(nextUpdateSV, true);
            //Loop while no abort has been set
            while (!AbortRequested) //refresh loop
            {
                //Check to see if we are still within the refresh interval
                while (DateTime.UtcNow < refreshTime)
                {
                    //If still within the refresh interval,
                    //Update the refresh display
                    NextRefreshBox.Text = (DateTime.UtcNow - refreshTime).Duration().TotalSeconds.ToString("0");
                    //Perform a one second wait and pulse the Start Command button
                    OneSecondPulse(StartButton);
                    //Check to see if imaging is happening and, if so, handle it
                    CheckImaging();
                    //Quick check to see if an abort was set while sitting out for a sec and checking imaging
                    if (AbortRequested)
                        break;
                }
                //Quick check to see if an abort was set while looping on refresh period, if so exit
                if (AbortRequested)
                    break;
                //Refrest period has expired, get next ephemeris entry, if any
                nextUpdateSV = EphemTable.GetNearestRateUpdate(DateTime.UtcNow);
                //If closest ephemeris entry is not null, 
                //  then update the tracking with no delay and display new numbers
                //  if the update fails then exit
                if (nextUpdateSV != null)
                {
                    if (Utils.SetTargetTracking(nextUpdateSV))
                    {
                        //Set refresh countdown timer;
                        refreshTime = nextUpdateSV.Time_UTC + EphemTable.EphStep;
                        RARateBox.Text = (nextUpdateSV.Rate_RA_ArcsecPerMinute * EphemTable.Topo_RA_Correction_Factor).ToString("0.000");
                        DecRateBox.Text = (nextUpdateSV.Rate_Dec_ArcsecPerMinute * EphemTable.Topo_Dec_Correction_Factor).ToString("0.000");
                        RangeBox.Text = nextUpdateSV.Range_AU.ToString("0.00");
                        //Update status
                        AssembleStatusUpdate(nextUpdateSV, false);

                    }
                    else  //Set tracking failure, so exit refresh loop
                        break;
                }
                else  //no ephemeris entry -- get new table
                {
                    return true;
                }
            }
            CleanupOnFault();
            return false;
        }

        private bool InitializeTargetTracking(SpeedVector currentSpeedVector)
        {
            //CLS to where target should be currently, deal with CLS failure
            UpdateStatusLine("Slewing to target @ RA/Dec: " +
                                Utils.HourString(AstroMath.Transform.DegreesToHours(currentSpeedVector.RA_Degrees),true) + "/" +
                                Utils.DegreeString(currentSpeedVector.Dec_Degrees, true));
                if (!Utils.CLSToTarget(EphemTable.TgtName, currentSpeedVector, CLSBox.Checked))
            {
                UpdateStatusLine("Tracking failed: Problem with Slew.");
                ReportSpeeds(currentSpeedVector);
                return false;
            }
            //Prompt for imaging
            SetBackColor(ImageButton, ControlColor.Green);
            //Set custom tracking 
            if (Utils.SetTargetTracking(currentSpeedVector))
                SetBackColor(TargetBox, ControlColor.Red);
            else
            {
                UpdateStatusLine("Set non-sidereal tracking failed.");
                ReportSpeeds(currentSpeedVector);
                SetBackColor(TargetBox, ControlColor.Green);
                return false;
            }
            //Wait out delay, if any
            if ((int)SlewSettlingTimeDelayBox.Value > 0)
            {
                SetBackColor(TargetBox, ControlColor.Yellow);
                Show();
                System.Windows.Forms.Application.DoEvents();
                System.Threading.Thread.Sleep((int)SlewSettlingTimeDelayBox.Value * 1000);
                SetBackColor(TargetBox, ControlColor.Red);
                Show();
                System.Windows.Forms.Application.DoEvents();
            }
            ReportSpeeds(currentSpeedVector);
            return true;
        }

        private void ReportSpeeds(SpeedVector sv)
        {
            RARateBox.Text = sv.Rate_RA_ArcsecPerMinute.ToString("0.000");
            DecRateBox.Text = sv.Rate_Dec_ArcsecPerMinute.ToString("0.000");
            RangeBox.Text = sv.Range_AU.ToString("0.00");
        }

        private void ImageButton_Click(object sender, EventArgs e)
        {
            //Sets up and runs a single shot using the exposure and filter set by the form
            SetBackColor(ImageButton, ControlColor.Red);
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
                SetBackColor(ImageButton, ControlColor.Yellow);
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

        private void CheckImaging()
        {
            //Checks to see if imaging is going on, if not, turn off indicators that it is
            ccdsoftCamera tsxc = new ccdsoftCamera();
            if (InPursuit)
            {
                //Target Tracking is running
                if (tsxc.State == ccdsoftCameraState.cdStateNone && IsImaging)
                {
                    //Save the current image path
                    string lastImagePath = tsxc.LastImageFileName;
                    if (RepsBox.Value > 1)
                    {
                        //More Reps to do
                        RepsBox.Value--;
                        bool IsReady = false;
                        if (RecenterBox.Checked)
                        {
                            SpeedVector currentSV = EphemTable.GetNearestRateUpdate(DateTime.UtcNow);
                            IsReady = InitializeTargetTracking(currentSV);
                        }
                        SetBackColor(ImageButton, ControlColor.Red);
                        if (IsReady)
                            TakeImage();
                        else //Something went bad in the target tracking
                            UpdateStatusLine("Failed to recenter target. Terminating image sequence.");
                    }
                    else //All reps are done
                    {
                        SetBackColor(ImageButton, ControlColor.Green);
                        tsxc.AutoSavePrefix = "";  //Clear target name prefix
                        IsImaging = false;
                    }
                    //display the most recent image stack
                    if (LiveStackBox.Checked)
                    {
                        FitsFile nextFF = new FitsFile(lastImagePath, true);
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
            else //Target Tracking is off
            {
                //Check if imaging is underway, if so, shut it down
                if (IsImaging)
                {
                    tsxc.Abort();
                    tsxc.AutoSavePrefix = "";  //Clear target name prefix
                    SetBackColor(ImageButton, ControlColor.Yellow);
                    IsImaging = false;
                }
            }
        }

        private void CleanupOnFault()
        {
            AbortRequested = false;
            InPursuit = false;
            SetBackColor(StartButton, ControlColor.Green);
            SetBackColor(TargetBox, ControlColor.Green);
            CheckImaging();
            Utils.SetStandardTracking();
            return;
        }

        private void OneSecondPulse(Button cmd)
        {
            SetBackColor(cmd, ControlColor.DarkRed);
            Show();
            System.Windows.Forms.Application.DoEvents();
            System.Threading.Thread.Sleep(500);
            SetBackColor(cmd, ControlColor.Red);
            Show();
            System.Windows.Forms.Application.DoEvents();
            System.Threading.Thread.Sleep(500);
            Show();
            System.Windows.Forms.Application.DoEvents();
            return;
        }

        private void AssembleStatusUpdate(SpeedVector sv, Boolean fullStatus)
        {
            //Update status
            string returnStatus;
            (double dRAout, double dDecout) = Utils.GetTargetTracking();
            //(double dRATSX, double dDecTSX) = Utils.GetObjectRates();
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
                                + (sv.Rate_RA_ArcsecPerMinute * EphemTable.Topo_RA_Correction_Factor).ToString("0.000")
                                + "/"
                                + (sv.Rate_Dec_ArcsecPerMinute * EphemTable.Topo_Dec_Correction_Factor).ToString("0.000")
                                + " (get) = "
                                + dRAout.ToString("0.000")
                                + "/"
                                + dDecout.ToString("0.000");
            UpdateStatusLine(returnStatus);
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
            NextRefreshBox.Text = "";
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

        private void StartDelayBox_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartDelay = (int)SlewSettlingTimeDelayBox.Value;
            Properties.Settings.Default.Save();
            return;
        }

        private void TargetBox_DoubleClick(object sender, EventArgs e)
        {
            if (!InPursuit)
                TargetBox.Text = "";
            return;
        }

        private void LogEntry(string entryStuff)
        {
            string logtime = DateTime.Now.ToString("HH:mm:ss");
            File.AppendAllText(HPLogFilePath, (logtime + ": " + entryStuff + "\r\n"));
        }

        private void ScoutRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            QuerySite = "Scout";
        }

        private void HorizonsRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            QuerySite = "Horizons";
        }

        private void MPCRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            QuerySite = "MPC";
        }

        private void SatRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            QuerySite = "Sat";
            SatRadioButton.ForeColor = Color.Pink;
            DialogResult newCat = MessageBox.Show("Do you want an updated satellite catalog?", "Satellite Catalog Check", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (newCat == DialogResult.Yes)
                SatCat.RefreshSatelliteCatalog();
            //Change the refresh rate to seconds
            SecondsButton.Checked = true;
            //Uncheck CLS box -- too slow
            CLSBox.Checked = false;
            //Check SatCatBox
            Show(); System.Windows.Forms.Application.DoEvents();
            FormSatCat obj = new FormSatCat(true);
            obj.getSatCatID_CallBack += getData;
            SatRadioButton.ForeColor = Color.White;
        }

        private void TLERadioButton_CheckedChanged(object sender, EventArgs e)
        {
            QuerySite = "3TLE";
            TLERadioButton.ForeColor = Color.Pink;
            //Change the refresh rate to seconds
            SecondsButton.Checked = true;
            //Uncheck CLS box -- too slow
            CLSBox.Checked = false;
            Show(); System.Windows.Forms.Application.DoEvents();
            FormSatCat obj = new FormSatCat(false);
            obj.getSatCatID_CallBack += getData;
            TLERadioButton.ForeColor = Color.White;
        }

        private void getData(string tgtID)
        {
            TargetBox.Text = tgtID;
        }

    }
}

