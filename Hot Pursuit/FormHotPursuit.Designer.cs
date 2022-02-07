
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
            this.ScoutButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.AbortButton = new System.Windows.Forms.Button();
            this.UpdateBox = new System.Windows.Forms.NumericUpDown();
            this.TargetBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.NextUpdateBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SecondsButton = new System.Windows.Forms.RadioButton();
            this.MinutesButton = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.OnTopBox = new System.Windows.Forms.CheckBox();
            this.SequencerGroupBox = new System.Windows.Forms.GroupBox();
            this.RepsBox = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.FiltersListBox = new System.Windows.Forms.ComboBox();
            this.ImageAbort = new System.Windows.Forms.Button();
            this.FullReductionCheckBox = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ExposureBox = new System.Windows.Forms.NumericUpDown();
            this.ImageButton = new System.Windows.Forms.Button();
            this.HPStatusBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.CorrectionBox = new System.Windows.Forms.TextBox();
            this.DecRateBox = new System.Windows.Forms.TextBox();
            this.RARateBox = new System.Windows.Forms.TextBox();
            this.LookUpCheckBox = new System.Windows.Forms.CheckBox();
            this.HorizonsButton = new System.Windows.Forms.Button();
            this.MPESButton = new System.Windows.Forms.Button();
            this.RangeBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.CLSBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.UpdateBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SequencerGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RepsBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExposureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ScoutButton
            // 
            this.ScoutButton.Location = new System.Drawing.Point(6, 120);
            this.ScoutButton.Name = "ScoutButton";
            this.ScoutButton.Size = new System.Drawing.Size(60, 23);
            this.ScoutButton.TabIndex = 0;
            this.ScoutButton.Text = "Scout";
            this.ScoutButton.UseVisualStyleBackColor = true;
            this.ScoutButton.Click += new System.EventHandler(this.ScoutButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(295, 120);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // AbortButton
            // 
            this.AbortButton.Location = new System.Drawing.Point(214, 120);
            this.AbortButton.Name = "AbortButton";
            this.AbortButton.Size = new System.Drawing.Size(75, 23);
            this.AbortButton.TabIndex = 2;
            this.AbortButton.Text = "Stop";
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
            this.TargetBox.Location = new System.Drawing.Point(112, 6);
            this.TargetBox.Name = "TargetBox";
            this.TargetBox.Size = new System.Drawing.Size(77, 20);
            this.TargetBox.TabIndex = 5;
            this.TargetBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TargetBox.Click += new System.EventHandler(this.TargetBox_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(18, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Target";
            // 
            // NextUpdateBox
            // 
            this.NextUpdateBox.Location = new System.Drawing.Point(317, 64);
            this.NextUpdateBox.Name = "NextUpdateBox";
            this.NextUpdateBox.Size = new System.Drawing.Size(54, 20);
            this.NextUpdateBox.TabIndex = 7;
            this.NextUpdateBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(213, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Next Update (Secs)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(18, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "RA Rate (\"/min)";
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
            this.groupBox1.Size = new System.Drawing.Size(171, 50);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Update Interval";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(18, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Dec Rate (\"/min)";
            // 
            // OnTopBox
            // 
            this.OnTopBox.AutoSize = true;
            this.OnTopBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.OnTopBox.ForeColor = System.Drawing.Color.White;
            this.OnTopBox.Location = new System.Drawing.Point(309, 91);
            this.OnTopBox.Name = "OnTopBox";
            this.OnTopBox.Size = new System.Drawing.Size(62, 17);
            this.OnTopBox.TabIndex = 16;
            this.OnTopBox.Text = "On Top";
            this.OnTopBox.UseVisualStyleBackColor = true;
            this.OnTopBox.CheckedChanged += new System.EventHandler(this.OnTopBox_CheckedChanged);
            // 
            // SequencerGroupBox
            // 
            this.SequencerGroupBox.BackColor = System.Drawing.Color.LightSeaGreen;
            this.SequencerGroupBox.Controls.Add(this.RepsBox);
            this.SequencerGroupBox.Controls.Add(this.label9);
            this.SequencerGroupBox.Controls.Add(this.FiltersListBox);
            this.SequencerGroupBox.Controls.Add(this.ImageAbort);
            this.SequencerGroupBox.Controls.Add(this.FullReductionCheckBox);
            this.SequencerGroupBox.Controls.Add(this.label6);
            this.SequencerGroupBox.Controls.Add(this.label5);
            this.SequencerGroupBox.Controls.Add(this.ExposureBox);
            this.SequencerGroupBox.Controls.Add(this.ImageButton);
            this.SequencerGroupBox.ForeColor = System.Drawing.Color.MintCream;
            this.SequencerGroupBox.Location = new System.Drawing.Point(380, 9);
            this.SequencerGroupBox.Name = "SequencerGroupBox";
            this.SequencerGroupBox.Size = new System.Drawing.Size(182, 134);
            this.SequencerGroupBox.TabIndex = 17;
            this.SequencerGroupBox.TabStop = false;
            this.SequencerGroupBox.Text = "QAD Imaging";
            // 
            // RepsBox
            // 
            this.RepsBox.Location = new System.Drawing.Point(42, 39);
            this.RepsBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.RepsBox.Name = "RepsBox";
            this.RepsBox.Size = new System.Drawing.Size(37, 20);
            this.RepsBox.TabIndex = 25;
            this.RepsBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(6, 43);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 13);
            this.label9.TabIndex = 24;
            this.label9.Text = "Reps";
            // 
            // FiltersListBox
            // 
            this.FiltersListBox.FormattingEnabled = true;
            this.FiltersListBox.Location = new System.Drawing.Point(120, 39);
            this.FiltersListBox.Name = "FiltersListBox";
            this.FiltersListBox.Size = new System.Drawing.Size(55, 21);
            this.FiltersListBox.TabIndex = 21;
            this.FiltersListBox.SelectedIndexChanged += new System.EventHandler(this.FiltersListBox_SelectedIndexChanged);
            // 
            // ImageAbort
            // 
            this.ImageAbort.ForeColor = System.Drawing.Color.Teal;
            this.ImageAbort.Location = new System.Drawing.Point(101, 102);
            this.ImageAbort.Name = "ImageAbort";
            this.ImageAbort.Size = new System.Drawing.Size(75, 23);
            this.ImageAbort.TabIndex = 18;
            this.ImageAbort.Text = "Abort";
            this.ImageAbort.UseVisualStyleBackColor = true;
            this.ImageAbort.Click += new System.EventHandler(this.ImageAbort_Click);
            // 
            // FullReductionCheckBox
            // 
            this.FullReductionCheckBox.AutoSize = true;
            this.FullReductionCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.FullReductionCheckBox.ForeColor = System.Drawing.Color.White;
            this.FullReductionCheckBox.Location = new System.Drawing.Point(100, 71);
            this.FullReductionCheckBox.Name = "FullReductionCheckBox";
            this.FullReductionCheckBox.Size = new System.Drawing.Size(75, 17);
            this.FullReductionCheckBox.TabIndex = 18;
            this.FullReductionCheckBox.Text = "Reduction";
            this.FullReductionCheckBox.UseVisualStyleBackColor = true;
            this.FullReductionCheckBox.CheckedChanged += new System.EventHandler(this.FullReductionCheckBox_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(85, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Filter";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(6, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Exposure (Secs)";
            // 
            // ExposureBox
            // 
            this.ExposureBox.Location = new System.Drawing.Point(120, 15);
            this.ExposureBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.ExposureBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ExposureBox.Name = "ExposureBox";
            this.ExposureBox.Size = new System.Drawing.Size(55, 20);
            this.ExposureBox.TabIndex = 13;
            this.ExposureBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ExposureBox.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            // 
            // ImageButton
            // 
            this.ImageButton.ForeColor = System.Drawing.Color.Teal;
            this.ImageButton.Location = new System.Drawing.Point(5, 102);
            this.ImageButton.Name = "ImageButton";
            this.ImageButton.Size = new System.Drawing.Size(75, 23);
            this.ImageButton.TabIndex = 18;
            this.ImageButton.Text = "Image";
            this.ImageButton.UseVisualStyleBackColor = true;
            this.ImageButton.Click += new System.EventHandler(this.ImageButton_Click);
            // 
            // HPStatusBox
            // 
            this.HPStatusBox.AllowDrop = true;
            this.HPStatusBox.Location = new System.Drawing.Point(6, 148);
            this.HPStatusBox.Multiline = true;
            this.HPStatusBox.Name = "HPStatusBox";
            this.HPStatusBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.HPStatusBox.Size = new System.Drawing.Size(556, 37);
            this.HPStatusBox.TabIndex = 18;
            this.HPStatusBox.Text = "Status Box";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(18, 97);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Correction";
            // 
            // CorrectionBox
            // 
            this.CorrectionBox.Location = new System.Drawing.Point(89, 95);
            this.CorrectionBox.Name = "CorrectionBox";
            this.CorrectionBox.Size = new System.Drawing.Size(100, 20);
            this.CorrectionBox.TabIndex = 20;
            this.CorrectionBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // DecRateBox
            // 
            this.DecRateBox.Location = new System.Drawing.Point(112, 70);
            this.DecRateBox.Name = "DecRateBox";
            this.DecRateBox.Size = new System.Drawing.Size(77, 20);
            this.DecRateBox.TabIndex = 23;
            this.DecRateBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RARateBox
            // 
            this.RARateBox.Location = new System.Drawing.Point(111, 46);
            this.RARateBox.Name = "RARateBox";
            this.RARateBox.Size = new System.Drawing.Size(77, 20);
            this.RARateBox.TabIndex = 24;
            this.RARateBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LookUpCheckBox
            // 
            this.LookUpCheckBox.AutoSize = true;
            this.LookUpCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LookUpCheckBox.ForeColor = System.Drawing.Color.White;
            this.LookUpCheckBox.Location = new System.Drawing.Point(34, 29);
            this.LookUpCheckBox.Name = "LookUpCheckBox";
            this.LookUpCheckBox.Size = new System.Drawing.Size(67, 17);
            this.LookUpCheckBox.TabIndex = 25;
            this.LookUpCheckBox.Text = "Look Up";
            this.LookUpCheckBox.UseVisualStyleBackColor = true;
            // 
            // HorizonsButton
            // 
            this.HorizonsButton.Location = new System.Drawing.Point(72, 120);
            this.HorizonsButton.Name = "HorizonsButton";
            this.HorizonsButton.Size = new System.Drawing.Size(60, 23);
            this.HorizonsButton.TabIndex = 26;
            this.HorizonsButton.Text = "Horizons";
            this.HorizonsButton.UseVisualStyleBackColor = true;
            this.HorizonsButton.Click += new System.EventHandler(this.HorizonsButton_Click);
            // 
            // MPESButton
            // 
            this.MPESButton.Location = new System.Drawing.Point(138, 120);
            this.MPESButton.Name = "MPESButton";
            this.MPESButton.Size = new System.Drawing.Size(60, 23);
            this.MPESButton.TabIndex = 27;
            this.MPESButton.Text = "MPC";
            this.MPESButton.UseVisualStyleBackColor = true;
            this.MPESButton.Click += new System.EventHandler(this.MPESButton_Click);
            // 
            // RangeBox
            // 
            this.RangeBox.Location = new System.Drawing.Point(234, 88);
            this.RangeBox.Name = "RangeBox";
            this.RangeBox.Size = new System.Drawing.Size(55, 20);
            this.RangeBox.TabIndex = 28;
            this.RangeBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(196, 91);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 13);
            this.label8.TabIndex = 29;
            this.label8.Text = "Range";
            // 
            // CLSBox
            // 
            this.CLSBox.AutoSize = true;
            this.CLSBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CLSBox.ForeColor = System.Drawing.Color.White;
            this.CLSBox.Location = new System.Drawing.Point(121, 28);
            this.CLSBox.Name = "CLSBox";
            this.CLSBox.Size = new System.Drawing.Size(46, 17);
            this.CLSBox.TabIndex = 30;
            this.CLSBox.Text = "CLS";
            this.CLSBox.UseVisualStyleBackColor = true;
            this.CLSBox.CheckedChanged += new System.EventHandler(this.CLSBox_CheckedChanged);
            // 
            // FormHotPursuit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkCyan;
            this.ClientSize = new System.Drawing.Size(569, 191);
            this.Controls.Add(this.CLSBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.RangeBox);
            this.Controls.Add(this.MPESButton);
            this.Controls.Add(this.HorizonsButton);
            this.Controls.Add(this.LookUpCheckBox);
            this.Controls.Add(this.RARateBox);
            this.Controls.Add(this.DecRateBox);
            this.Controls.Add(this.CorrectionBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.HPStatusBox);
            this.Controls.Add(this.SequencerGroupBox);
            this.Controls.Add(this.OnTopBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NextUpdateBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TargetBox);
            this.Controls.Add(this.AbortButton);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ScoutButton);
            this.Controls.Add(this.groupBox1);
            this.ForeColor = System.Drawing.Color.Teal;
            this.MaximizeBox = false;
            this.Name = "FormHotPursuit";
            this.Text = "Hot Pursuit";
            ((System.ComponentModel.ISupportInitialize)(this.UpdateBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.SequencerGroupBox.ResumeLayout(false);
            this.SequencerGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RepsBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExposureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ScoutButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button AbortButton;
        private System.Windows.Forms.NumericUpDown UpdateBox;
        private System.Windows.Forms.TextBox TargetBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NextUpdateBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton SecondsButton;
        private System.Windows.Forms.RadioButton MinutesButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox OnTopBox;
        private System.Windows.Forms.GroupBox SequencerGroupBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown ExposureBox;
        private System.Windows.Forms.Button ImageButton;
        private System.Windows.Forms.Button ImageAbort;
        private System.Windows.Forms.CheckBox FullReductionCheckBox;
        private System.Windows.Forms.ComboBox FiltersListBox;
        private System.Windows.Forms.TextBox HPStatusBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox CorrectionBox;
        private System.Windows.Forms.TextBox DecRateBox;
        private System.Windows.Forms.TextBox RARateBox;
        private System.Windows.Forms.CheckBox LookUpCheckBox;
        private System.Windows.Forms.Button HorizonsButton;
        private System.Windows.Forms.Button MPESButton;
        private System.Windows.Forms.TextBox RangeBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown RepsBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox CLSBox;
    }
}

