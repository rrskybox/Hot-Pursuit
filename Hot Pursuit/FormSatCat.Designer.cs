
namespace Hot_Pursuit
{
    partial class FormSatCat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SatTree = new System.Windows.Forms.TreeView();
            this.SatCatCloseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SatTree
            // 
            this.SatTree.Location = new System.Drawing.Point(2, 2);
            this.SatTree.Name = "SatTree";
            this.SatTree.Size = new System.Drawing.Size(310, 186);
            this.SatTree.TabIndex = 41;
            this.SatTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.SatTree_AfterSelect);
            // 
            // SatCatCloseButton
            // 
            this.SatCatCloseButton.Location = new System.Drawing.Point(237, 194);
            this.SatCatCloseButton.Name = "SatCatCloseButton";
            this.SatCatCloseButton.Size = new System.Drawing.Size(75, 23);
            this.SatCatCloseButton.TabIndex = 42;
            this.SatCatCloseButton.Text = "Choose";
            this.SatCatCloseButton.UseVisualStyleBackColor = true;
            this.SatCatCloseButton.Click += new System.EventHandler(this.SatCatCloseButton_Click);
            // 
            // FormSatCat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 222);
            this.Controls.Add(this.SatCatCloseButton);
            this.Controls.Add(this.SatTree);
            this.Name = "FormSatCat";
            this.Text = "Celestrak Satellite Catalog";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView SatTree;
        private System.Windows.Forms.Button SatCatCloseButton;
    }
}