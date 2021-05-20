
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
            this.TargetBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.NextUpdateBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.RateBox = new System.Windows.Forms.TextBox();
            this.SecondsButton = new System.Windows.Forms.RadioButton();
            this.MinutesButton = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PABox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.OnTopBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.UpdateBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PursueButton
            // 
            this.PursueButton.Location = new System.Drawing.Point(27, 113);
            this.PursueButton.Name = "PursueButton";
            this.PursueButton.Size = new System.Drawing.Size(75, 23);
            this.PursueButton.TabIndex = 0;
            this.PursueButton.Text = "Pursue";
            this.PursueButton.UseVisualStyleBackColor = true;
            this.PursueButton.Click += new System.EventHandler(this.PursueButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(283, 113);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // AbortButton
            // 
            this.AbortButton.Location = new System.Drawing.Point(155, 113);
            this.AbortButton.Name = "AbortButton";
            this.AbortButton.Size = new System.Drawing.Size(75, 23);
            this.AbortButton.TabIndex = 2;
            this.AbortButton.Text = "Abort";
            this.AbortButton.UseVisualStyleBackColor = true;
            this.AbortButton.Click += new System.EventHandler(this.AbortButton_Click);
            // 
            // UpdateBox
            // 
            this.UpdateBox.Location = new System.Drawing.Point(17, 19);
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
            // TargetBox
            // 
            this.TargetBox.Location = new System.Drawing.Point(97, 13);
            this.TargetBox.Name = "TargetBox";
            this.TargetBox.Size = new System.Drawing.Size(77, 20);
            this.TargetBox.TabIndex = 5;
            this.TargetBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(14, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Target";
            // 
            // NextUpdateBox
            // 
            this.NextUpdateBox.Location = new System.Drawing.Point(316, 77);
            this.NextUpdateBox.Name = "NextUpdateBox";
            this.NextUpdateBox.Size = new System.Drawing.Size(54, 20);
            this.NextUpdateBox.TabIndex = 7;
            this.NextUpdateBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(210, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Next Update (Secs)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(14, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Rate (\"/min)";
            // 
            // RateBox
            // 
            this.RateBox.Location = new System.Drawing.Point(97, 37);
            this.RateBox.Name = "RateBox";
            this.RateBox.Size = new System.Drawing.Size(78, 20);
            this.RateBox.TabIndex = 10;
            this.RateBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SecondsButton
            // 
            this.SecondsButton.AutoSize = true;
            this.SecondsButton.Location = new System.Drawing.Point(97, 8);
            this.SecondsButton.Name = "SecondsButton";
            this.SecondsButton.Size = new System.Drawing.Size(67, 17);
            this.SecondsButton.TabIndex = 11;
            this.SecondsButton.Text = "Seconds";
            this.SecondsButton.UseVisualStyleBackColor = true;
            // 
            // MinutesButton
            // 
            this.MinutesButton.AutoSize = true;
            this.MinutesButton.Checked = true;
            this.MinutesButton.Location = new System.Drawing.Point(97, 29);
            this.MinutesButton.Name = "MinutesButton";
            this.MinutesButton.Size = new System.Drawing.Size(62, 17);
            this.MinutesButton.TabIndex = 12;
            this.MinutesButton.TabStop = true;
            this.MinutesButton.Text = "Minutes";
            this.MinutesButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.MinutesButton);
            this.groupBox1.Controls.Add(this.SecondsButton);
            this.groupBox1.Controls.Add(this.UpdateBox);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(199, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(171, 54);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Update Interval";
            // 
            // PABox
            // 
            this.PABox.Location = new System.Drawing.Point(97, 61);
            this.PABox.Name = "PABox";
            this.PABox.Size = new System.Drawing.Size(78, 20);
            this.PABox.TabIndex = 15;
            this.PABox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(14, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "PA  (deg)";
            // 
            // OnTopBox
            // 
            this.OnTopBox.AutoSize = true;
            this.OnTopBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.OnTopBox.ForeColor = System.Drawing.Color.White;
            this.OnTopBox.Location = new System.Drawing.Point(27, 89);
            this.OnTopBox.Name = "OnTopBox";
            this.OnTopBox.Size = new System.Drawing.Size(62, 17);
            this.OnTopBox.TabIndex = 16;
            this.OnTopBox.Text = "On Top";
            this.OnTopBox.UseVisualStyleBackColor = true;
            this.OnTopBox.CheckedChanged += new System.EventHandler(this.OnTopBox_CheckedChanged);
            // 
            // FormHotPursuit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkCyan;
            this.ClientSize = new System.Drawing.Size(382, 147);
            this.Controls.Add(this.OnTopBox);
            this.Controls.Add(this.PABox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.RateBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NextUpdateBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TargetBox);
            this.Controls.Add(this.AbortButton);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.PursueButton);
            this.Controls.Add(this.groupBox1);
            this.ForeColor = System.Drawing.Color.Teal;
            this.MaximizeBox = false;
            this.Name = "FormHotPursuit";
            this.Text = "Hot Pursuit";
            ((System.ComponentModel.ISupportInitialize)(this.UpdateBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button PursueButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button AbortButton;
        private System.Windows.Forms.NumericUpDown UpdateBox;
        private System.Windows.Forms.TextBox TargetBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NextUpdateBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox RateBox;
        private System.Windows.Forms.RadioButton SecondsButton;
        private System.Windows.Forms.RadioButton MinutesButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox PABox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox OnTopBox;
    }
}

