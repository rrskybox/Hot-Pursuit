using System;
using System.Deployment.Application;
using System.Drawing;
using System.Windows.Forms;

namespace Hot_Pursuit
{
    public partial class FormHotPursuit : Form
    {
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

            PursueButton.BackColor = Color.LightGreen;
            AbortButton.BackColor = Color.LightGreen;
            CloseButton.BackColor = Color.LightGreen;
        }

        public bool AbortRequested { get; set; } = false;

        private void PursueButton_Click(object sender, EventArgs e)
        {
            PursueButton.BackColor = Color.Salmon;
            SearchScout ss = new SearchScout();
            ss.EphStart = DateTime.UtcNow;
            if (MinutesButton.Checked)
                ss.EphStep = TimeSpan.FromMinutes((double)UpdateBox.Value);
            else
                ss.EphStep = TimeSpan.FromMinutes(1);
            ss.EphEnd = ss.EphStart + TimeSpan.FromMinutes((100 * ss.EphStep.TotalMinutes));
            ss.LoadTargetData(MinutesButton.Checked, (int)UpdateBox.Value);
            TargetBox.Text = ss.TgtName;
            //Fire off first tracking instruction
            SpeedVector nextUpdateSV = ss.GetNextRateUpdate(ss.EphStart);
            ss.SlewToTarget(nextUpdateSV);
            ss.SetTargetTracking(nextUpdateSV);
            RateBox.Text = nextUpdateSV.Rate.ToString("0.00");
            PABox.Text = nextUpdateSV.PA.ToString("0.00");
            DateTime nextUpdate = nextUpdateSV.Time;
            if (MinutesButton.Checked)
                nextUpdate += TimeSpan.FromMinutes((int)UpdateBox.Value);
            else
                nextUpdate += TimeSpan.FromSeconds((int)UpdateBox.Value);
            NextUpdateBox.Text = (nextUpdate - DateTime.UtcNow).TotalSeconds.ToString("0");
            //Set up for next tracking instruction
            while (!AbortRequested)
            {
                //the next target ephemeris has been loaded into the ss object, but assume not
                nextUpdateSV = ss.GetNextRateUpdate(DateTime.UtcNow);
                if (nextUpdateSV != null)
                {
                    nextUpdate = nextUpdateSV.Time;
                    while (DateTime.UtcNow < nextUpdate)
                    {
                        OneSecondPulse();
                        NextUpdateBox.Text = (nextUpdate - DateTime.UtcNow).TotalSeconds.ToString("0");
                        if (AbortRequested)
                            break;
                    }
                    ss.SetTargetTracking(nextUpdateSV);
                    RateBox.Text = nextUpdateSV.Rate.ToString("0.00");
                    PABox.Text = nextUpdateSV.PA.ToString("0.00");
                }
                else //no new update -- go get another
                {
                    ss = new SearchScout();
                    ss.EphStart = DateTime.UtcNow;
                    ss.EphStep = TimeSpan.FromMinutes((double)UpdateBox.Value);
                    ss.EphEnd = ss.EphStart + TimeSpan.FromMinutes((100 * ss.EphStep.TotalMinutes));
                    ss.LoadTargetData(MinutesButton.Checked, (int)UpdateBox.Value);
                }
            }
            AbortRequested = false;
            PursueButton.BackColor = Color.LightGreen;
        }

        private void OneSecondPulse()
        {
            PursueButton.BackColor = Color.LightSalmon;
            Show();
            Application.DoEvents();
            System.Threading.Thread.Sleep(500);
            PursueButton.BackColor = Color.Salmon;
            Show();
            Application.DoEvents();
            System.Threading.Thread.Sleep(500);
            Show();
            Application.DoEvents();

        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AbortButton_Click(object sender, EventArgs e)
        {
            AbortRequested = true;
        }
    }
}
