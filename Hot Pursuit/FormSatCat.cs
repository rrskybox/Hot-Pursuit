using System;
using System.Windows.Forms;

namespace Hot_Pursuit
{
    public partial class FormSatCat : Form
    {

        private int payCatNode;
        private int rbCatNode;
        private int debCatNode;

        public string TargetID { get; set; }

        public FormSatCat()
        {
            InitializeComponent();
            SatTree.Nodes.Clear();

            payCatNode = AddMainNode("Payload");
            rbCatNode = AddMainNode("Booster");
            debCatNode = AddMainNode("Debris");

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
                }
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
