using System;
using System.IO;
using System.Windows.Forms;

namespace Hot_Pursuit
{
    public partial class FormSatCat : Form
    {

        private int payCatNode;
        private int rbCatNode;
        private int debCatNode;
        private int unkCatNode;

        public string TargetID { get; set; }

        public FormSatCat(bool useSatCatalog)
        {
            InitializeComponent();
            SatTree.Nodes.Clear();
            if (useSatCatalog)
                SelectSatTarget();
            else
                SelectTLETarget();
        }

        private void SelectSatTarget()
        { 
            payCatNode = AddMainNode("Payload");
            rbCatNode = AddMainNode("Booster");
            debCatNode = AddMainNode("Debris");
            unkCatNode = AddMainNode("Unknown");

            SatCat scList = new SatCat();

            foreach (SatCat.SatEntry sc in scList.SatelliteCatalog)
            {
                switch (sc.ObjectType)
                {
                    case SatCat.SatCatEntryType.Payload:
                        {
                            AddLeafNode(payCatNode, sc.ObjectName, sc.ObjectInternationalID, sc.ObjectNoradID );
                            break;
                        }
                    case SatCat.SatCatEntryType.Booster:
                        {
                            AddLeafNode(rbCatNode, sc.ObjectName, sc.ObjectInternationalID, sc.ObjectNoradID);
                            break;
                        }
                    case SatCat.SatCatEntryType.Debris:
                        {
                            AddLeafNode(debCatNode, sc.ObjectName, sc.ObjectInternationalID, sc.ObjectNoradID);
                            break;
                        }
                     case SatCat.SatCatEntryType.Unknown:
                        {
                            AddLeafNode(unkCatNode, sc.ObjectName, sc.ObjectInternationalID, sc.ObjectNoradID);
                            break;
                        }
                }
            }
            Show(); System.Windows.Forms.Application.DoEvents();
            return;
        }

        private void SelectTLETarget()
        {

            string nameLine = null;
            string firstLine = null;
            string secondLine = null;

            //Reads custom .txt file of TLE entries for satellite entry with tgtName as first line
            //
            //REad in list of TLE entries
            //Get User Documents Folder
            string satTLEPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Properties.Settings.Default.TLECatalogPath;
            if (!File.Exists(satTLEPath))
                return;
            StreamReader satTLEFile = File.OpenText(satTLEPath);
            //Read in the remaining lines and stuff into staName List
            while (satTLEFile.Peek() != -1)
            {
                //Read sets of three lines, look for tgtName in first line, break out with result
                nameLine = satTLEFile.ReadLine();
                firstLine = satTLEFile.ReadLine();
                secondLine = satTLEFile.ReadLine();
                AddMainNode(nameLine);
            }
            Show(); System.Windows.Forms.Application.DoEvents();
            return;
        }

        private int AddMainNode(string section)
        {
            TreeNode cNode = SatTree.Nodes.Add(section, section);
            int indx = SatTree.Nodes.IndexOf(cNode);
            return indx;
        }

        private void AddLeafNode(int mainIdx, string objName, string objIntID, string objNoradID)
        {
            int oIdx = SatTree.Nodes[mainIdx].Nodes.IndexOfKey(objName);
            if (oIdx == -1)
            {
                TreeNode cNode = SatTree.Nodes[mainIdx].Nodes.Add(objName, objName);
                oIdx = SatTree.Nodes[mainIdx].Nodes.IndexOf(cNode);
            }
            SatTree.Nodes[mainIdx].Nodes[oIdx].Nodes.Add(objNoradID, objIntID);
            return;
        }

        private void SatCatCloseButton_Click(object sender, EventArgs e)
        {
            TreeNode tn = SatTree.SelectedNode;
            TargetID = tn.Name;
            getSatCatID_CallBack(TargetID);
            this.Close();
        }

        public delegate void callback_data(string tgtID);
        public event callback_data getSatCatID_CallBack;

        private void SatTree_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}
