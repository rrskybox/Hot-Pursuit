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
            ss.EphStart = DateTime.Now;
            ss.EphStep = TimeSpan.FromMinutes((double)UpdateBox.Value);
            ss.EphEnd = ss.EphStart + TimeSpan.FromMinutes((100 * ss.EphStep.TotalMinutes));
            bool ready = ss.LoadTargetData();
            if (!ready)
                return;
            TargetBox.Text = ss.TgtName;
            ss.SlewToTarget();
            while (!AbortRequested)
            {
                //the next target ephemeris has been loaded into the ss object, but assume not
                if (ss.GetNextPositionUpdate())
                {
                    DateTime nextUpdate = ss.NextUpdateAt;
                    RateBox.Text = ss.TgtRate.ToString("0.0");
                    int updateSecs = (int)UpdateBox.Value;
                    NextUpdateBox.Text = nextUpdate.ToShortTimeString();
                    if (nextUpdate != null)
                    {
                        ss.LoadTargetData();
                        ss.SetTargetTracking();
                        while (DateTime.Now < nextUpdate)
                        {
                            OneSecondPulse();
                            if (AbortRequested)
                                break;
                        }
                        nextUpdate = DateTime.Now + TimeSpan.FromSeconds(updateSecs);
                        NextUpdateBox.Text = nextUpdate.ToShortTimeString();
                    }
                }
                else //no new update -- go get another
                {
                    ss = new SearchScout();
                    ss.EphStart = DateTime.Now;
                    ss.EphStep = TimeSpan.FromMinutes((double)UpdateBox.Value);
                    ss.EphEnd = ss.EphStart + TimeSpan.FromMinutes((100 * ss.EphStep.TotalMinutes));
                    if (!ss.LoadTargetData())
                        break;
                    ss.SlewToTarget();
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
