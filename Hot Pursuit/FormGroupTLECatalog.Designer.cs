
namespace Hot_Pursuit
{
    partial class FormGroupTLECatalog
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
            this.CelesTrakGroupBox = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // CelesTrakGroupBox
            // 
            this.CelesTrakGroupBox.CheckOnClick = true;
            this.CelesTrakGroupBox.FormattingEnabled = true;
            this.CelesTrakGroupBox.Location = new System.Drawing.Point(10, 8);
            this.CelesTrakGroupBox.Name = "CelesTrakGroupBox";
            this.CelesTrakGroupBox.Size = new System.Drawing.Size(343, 814);
            this.CelesTrakGroupBox.Sorted = true;
            this.CelesTrakGroupBox.TabIndex = 0;
            this.CelesTrakGroupBox.SelectedIndexChanged += new System.EventHandler(this.CelesTrakGroupBox_SelectedIndexChanged);
            // 
            // FormGroupTLECatalog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 833);
            this.Controls.Add(this.CelesTrakGroupBox);
            this.Name = "FormGroupTLECatalog";
            this.Text = "CelesTrak Group";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox CelesTrakGroupBox;
    }
}