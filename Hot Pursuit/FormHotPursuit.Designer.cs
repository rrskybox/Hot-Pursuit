
namespace Hot_Pursuit
{
    partial class FormHotPursuit
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
            this.PursueButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.AbortButton = new System.Windows.Forms.Button();
            this.UpdateBox = new System.Windows.Forms.NumericUpDown();
            this.UpdateLabel = new System.Windows.Forms.Label();
            this.TargetBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.NextUpdateBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.RateBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.UpdateBox)).BeginInit();
            this.SuspendLayout();
            // 
            // PursueButton
            // 
            this.PursueButton.Location = new System.Drawing.Point(12, 92);
            this.PursueButton.Name = "PursueButton";
            this.PursueButton.Size = new System.Drawing.Size(75, 23);
            this.PursueButton.TabIndex = 0;
            this.PursueButton.Text = "Pursue";
            this.PursueButton.UseVisualStyleBackColor = true;
            this.PursueButton.Click += new System.EventHandler(this.PursueButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(275, 92);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // AbortButton
            // 
            this.AbortButton.Location = new System.Drawing.Point(151, 92);
            this.AbortButton.Name = "AbortButton";
            this.AbortButton.Size = new System.Drawing.Size(75, 23);
            this.AbortButton.TabIndex = 2;
            this.AbortButton.Text = "Abort";
            this.AbortButton.UseVisualStyleBackColor = true;
            this.AbortButton.Click += new System.EventHandler(this.AbortButton_Click);
            // 
            // UpdateBox
            // 
            this.UpdateBox.Location = new System.Drawing.Point(138, 47);
            this.UpdateBox.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.UpdateBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpdateBox.Name = "UpdateBox";
            this.UpdateBox.Size = new System.Drawing.Size(55, 20);
            this.UpdateBox.TabIndex = 3;
            this.UpdateBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.UpdateBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // UpdateLabel
            // 
            this.UpdateLabel.AutoSize = true;
            this.UpdateLabel.Location = new System.Drawing.Point(12, 49);
            this.UpdateLabel.Name = "UpdateLabel";
            this.UpdateLabel.Size = new System.Drawing.Size(120, 13);
            this.UpdateLabel.TabIndex = 4;
            this.UpdateLabel.Text = "Update Period (minutes)";
            // 
            // TargetBox
            // 
            this.TargetBox.Location = new System.Drawing.Point(138, 12);
            this.TargetBox.Name = "TargetBox";
            this.TargetBox.Size = new System.Drawing.Size(55, 20);
            this.TargetBox.TabIndex = 5;
            this.TargetBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Target";
            // 
            // NextUpdateBox
            // 
            this.NextUpdateBox.Location = new System.Drawing.Point(272, 47);
            this.NextUpdateBox.Name = "NextUpdateBox";
            this.NextUpdateBox.Size = new System.Drawing.Size(78, 20);
            this.NextUpdateBox.TabIndex = 7;
            this.NextUpdateBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(203, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Next Update";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(203, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Rate (\"/min)";
            // 
            // RateBox
            // 
            this.RateBox.Location = new System.Drawing.Point(272, 12);
            this.RateBox.Name = "RateBox";
            this.RateBox.Size = new System.Drawing.Size(78, 20);
            this.RateBox.TabIndex = 10;
            this.RateBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FormHotPursuit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 127);
            this.Controls.Add(this.RateBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NextUpdateBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TargetBox);
            this.Controls.Add(this.UpdateLabel);
            this.Controls.Add(this.UpdateBox);
            this.Controls.Add(this.AbortButton);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.PursueButton);
            this.MaximizeBox = false;
            this.Name = "FormHotPursuit";
            this.Text = "FormHotPursuit";
            ((System.ComponentModel.ISupportInitialize)(this.UpdateBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button PursueButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button AbortButton;
        private System.Windows.Forms.NumericUpDown UpdateBox;
        private System.Windows.Forms.Label UpdateLabel;
        private System.Windows.Forms.TextBox TargetBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NextUpdateBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox RateBox;
    }
}

