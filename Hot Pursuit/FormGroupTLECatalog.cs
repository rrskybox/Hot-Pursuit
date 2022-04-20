using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Xml.Linq;
using System.IO;

namespace Hot_Pursuit
{
    public partial class FormGroupTLECatalog : Form
    {
        public FormGroupTLECatalog()
        {
            InitializeComponent();
            ReadInCelesTrakGroup();

        }

        private void ReadInCelesTrakGroup()
        {
            //Queries CelesTrak for new group list of TLE's
            Assembly dgassembly = Assembly.GetExecutingAssembly();
            Stream dgstream = dgassembly.GetManifestResourceStream("Hot_Pursuit.CelesTrakGroup.xml");
            XElement cGroupList = XElement.Load(dgstream);
            CelesTrakGroupBox.Items.Clear();
            foreach (XElement xg in cGroupList.Elements("row"))
                CelesTrakGroupBox.Items.Add(xg.Element("GroupName").Value);
            Show(); Application.DoEvents();
            return;
        }

        private void CelesTrakGroupBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Over-write current CustomTLE.txt file with queried TLE file for group named in selection


        }
    }
}
