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
            bool ready = ss.GetTarget();
            if (!ready) return;
            TargetBox.Text = ss.TgtName;
            RateBox.Text = ss.TgtRate.ToString("0.0");
            ss.SetTargetTSX();
            ss.TrackTargetTSX();
            int updateSecs = (int)UpdateBox.Value * 60;
            DateTime nextUpdate = DateTime.Now + TimeSpan.FromSeconds(updateSecs);
            NextUpdateBox.Text = nextUpdate.ToShortTimeString();
            while (!AbortRequested)
            {
                ss.GetTarget();
                ss.TrackTargetTSX();
                //for (int sec = 0; sec < updateSecs; sec++)
                while (DateTime.Now < nextUpdate)
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
                    if (AbortRequested)
                        break;
                }
                nextUpdate = DateTime.Now + TimeSpan.FromSeconds(updateSecs);
                NextUpdateBox.Text = nextUpdate.ToShortTimeString();
            }
            AbortRequested = false;
            PursueButton.BackColor = Color.LightGreen;
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
