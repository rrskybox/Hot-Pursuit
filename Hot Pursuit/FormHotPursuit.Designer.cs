
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
            this.CloseButton = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.RefreshIntervalBox = new System.Windows.Forms.NumericUpDown();
            this.TargetBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.NextRefreshBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.OnTopBox = new System.Windows.Forms.CheckBox();
            this.SequencerGroupBox = new System.Windows.Forms.GroupBox();
            this.RecenterBox = new System.Windows.Forms.CheckBox();
            this.LiveStackBox = new System.Windows.Forms.CheckBox();
            this.RepsBox = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.FiltersListBox = new System.Windows.Forms.ComboBox();
            this.FullReductionCheckBox = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ExposureBox = new System.Windows.Forms.NumericUpDown();
            this.ImageButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.SlewSettlingTimeDelayBox = new System.Windows.Forms.NumericUpDown();
            this.HPStatusBox = new System.Windows.Forms.TextBox();
            this.DecRateBox = new System.Windows.Forms.TextBox();
            this.RARateBox = new System.Windows.Forms.TextBox();
            this.RangeBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.CLSBox = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.SecondsButton = new System.Windows.Forms.RadioButton();
            this.MinutesButton = new System.Windows.Forms.RadioButton();
            this.ScoutRadioButton = new System.Windows.Forms.RadioButton();
            this.HorizonsRadioButton = new System.Windows.Forms.RadioButton();
            this.MPCRadioButton = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.RefreshIntervalBox)).BeginInit();
            this.SequencerGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RepsBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExposureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SlewSettlingTimeDelayBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CloseButton
            // 
            this.CloseButton.ForeColor = System.Drawing.Color.Black;
            this.CloseButton.Location = new System.Drawing.Point(129, 112);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(50, 23);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.ForeColor = System.Drawing.Color.Black;
            this.StopButton.Location = new System.Drawing.Point(71, 112);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(52, 23);
            this.StopButton.TabIndex = 2;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // RefreshIntervalBox
            // 
            this.RefreshIntervalBox.Location = new System.Drawing.Point(53, 20);
            this.RefreshIntervalBox.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.RefreshIntervalBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.RefreshIntervalBox.Name = "RefreshIntervalBox";
            this.RefreshIntervalBox.Size = new System.Drawing.Size(41, 20);
            this.RefreshIntervalBox.TabIndex = 3;
            this.RefreshIntervalBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.RefreshIntervalBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // TargetBox
            // 
            this.TargetBox.Location = new System.Drawing.Point(56, 10);
            this.TargetBox.Name = "TargetBox";
            this.TargetBox.Size = new System.Drawing.Size(143, 20);
            this.TargetBox.TabIndex = 5;
            this.TargetBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TargetBox.DoubleClick += new System.EventHandler(this.TargetBox_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Target";
            // 
            // NextRefreshBox
            // 
            this.NextRefreshBox.Location = new System.Drawing.Point(117, 48);
            this.NextRefreshBox.Name = "NextRefreshBox";
            this.NextRefreshBox.Size = new System.Drawing.Size(63, 20);
            this.NextRefreshBox.TabIndex = 7;
            this.NextRefreshBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(7, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Refresh ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(12, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "RA Rate (\"/min)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(12, 62);
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
            this.OnTopBox.Location = new System.Drawing.Point(117, 80);
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
            this.SequencerGroupBox.Controls.Add(this.label7);
            this.SequencerGroupBox.Controls.Add(this.RecenterBox);
            this.SequencerGroupBox.Controls.Add(this.LiveStackBox);
            this.SequencerGroupBox.Controls.Add(this.SlewSettlingTimeDelayBox);
            this.SequencerGroupBox.Controls.Add(this.RepsBox);
            this.SequencerGroupBox.Controls.Add(this.label9);
            this.SequencerGroupBox.Controls.Add(this.FiltersListBox);
            this.SequencerGroupBox.Controls.Add(this.FullReductionCheckBox);
            this.SequencerGroupBox.Controls.Add(this.label6);
            this.SequencerGroupBox.Controls.Add(this.label5);
            this.SequencerGroupBox.Controls.Add(this.ExposureBox);
            this.SequencerGroupBox.Controls.Add(this.ImageButton);
            this.SequencerGroupBox.ForeColor = System.Drawing.Color.MintCream;
            this.SequencerGroupBox.Location = new System.Drawing.Point(400, 7);
            this.SequencerGroupBox.Name = "SequencerGroupBox";
            this.SequencerGroupBox.Size = new System.Drawing.Size(182, 145);
            this.SequencerGroupBox.TabIndex = 17;
            this.SequencerGroupBox.TabStop = false;
            this.SequencerGroupBox.Text = "QAD Imaging";
            // 
            // RecenterBox
            // 
            this.RecenterBox.AutoSize = true;
            this.RecenterBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RecenterBox.Checked = true;
            this.RecenterBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RecenterBox.ForeColor = System.Drawing.Color.White;
            this.RecenterBox.Location = new System.Drawing.Point(106, 72);
            this.RecenterBox.Name = "RecenterBox";
            this.RecenterBox.Size = new System.Drawing.Size(70, 17);
            this.RecenterBox.TabIndex = 27;
            this.RecenterBox.Text = "Recenter";
            this.RecenterBox.UseVisualStyleBackColor = true;
            // 
            // LiveStackBox
            // 
            this.LiveStackBox.AutoSize = true;
            this.LiveStackBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LiveStackBox.ForeColor = System.Drawing.Color.White;
            this.LiveStackBox.Location = new System.Drawing.Point(6, 89);
            this.LiveStackBox.Name = "LiveStackBox";
            this.LiveStackBox.Size = new System.Drawing.Size(77, 17);
            this.LiveStackBox.TabIndex = 26;
            this.LiveStackBox.Text = "Live Stack";
            this.LiveStackBox.UseVisualStyleBackColor = true;
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
            // FullReductionCheckBox
            // 
            this.FullReductionCheckBox.AutoSize = true;
            this.FullReductionCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.FullReductionCheckBox.ForeColor = System.Drawing.Color.White;
            this.FullReductionCheckBox.Location = new System.Drawing.Point(6, 66);
            this.FullReductionCheckBox.Name = "FullReductionCheckBox";
            this.FullReductionCheckBox.Size = new System.Drawing.Size(64, 17);
            this.FullReductionCheckBox.TabIndex = 18;
            this.FullReductionCheckBox.Text = "Reduce";
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
            3600,
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
            this.ImageButton.ForeColor = System.Drawing.Color.Black;
            this.ImageButton.Location = new System.Drawing.Point(35, 112);
            this.ImageButton.Name = "ImageButton";
            this.ImageButton.Size = new System.Drawing.Size(55, 23);
            this.ImageButton.TabIndex = 18;
            this.ImageButton.Text = "Image";
            this.ImageButton.UseVisualStyleBackColor = true;
            this.ImageButton.Click += new System.EventHandler(this.ImageButton_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(110, 90);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 13);
            this.label7.TabIndex = 29;
            this.label7.Text = "Settle (sec)";
            // 
            // SlewSettlingTimeDelayBox
            // 
            this.SlewSettlingTimeDelayBox.Location = new System.Drawing.Point(120, 108);
            this.SlewSettlingTimeDelayBox.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.SlewSettlingTimeDelayBox.Name = "SlewSettlingTimeDelayBox";
            this.SlewSettlingTimeDelayBox.Size = new System.Drawing.Size(41, 20);
            this.SlewSettlingTimeDelayBox.TabIndex = 28;
            this.SlewSettlingTimeDelayBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SlewSettlingTimeDelayBox.ValueChanged += new System.EventHandler(this.StartDelayBox_ValueChanged);
            // 
            // HPStatusBox
            // 
            this.HPStatusBox.AllowDrop = true;
            this.HPStatusBox.Location = new System.Drawing.Point(6, 158);
            this.HPStatusBox.Multiline = true;
            this.HPStatusBox.Name = "HPStatusBox";
            this.HPStatusBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.HPStatusBox.Size = new System.Drawing.Size(576, 45);
            this.HPStatusBox.TabIndex = 18;
            this.HPStatusBox.Text = "Log";
            // 
            // DecRateBox
            // 
            this.DecRateBox.Location = new System.Drawing.Point(122, 59);
            this.DecRateBox.Name = "DecRateBox";
            this.DecRateBox.Size = new System.Drawing.Size(77, 20);
            this.DecRateBox.TabIndex = 23;
            this.DecRateBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RARateBox
            // 
            this.RARateBox.Location = new System.Drawing.Point(122, 34);
            this.RARateBox.Name = "RARateBox";
            this.RARateBox.Size = new System.Drawing.Size(77, 20);
            this.RARateBox.TabIndex = 24;
            this.RARateBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RangeBox
            // 
            this.RangeBox.Location = new System.Drawing.Point(122, 84);
            this.RangeBox.Name = "RangeBox";
            this.RangeBox.Size = new System.Drawing.Size(77, 20);
            this.RangeBox.TabIndex = 28;
            this.RangeBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(12, 87);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 29;
            this.label8.Text = "Range (AU)";
            // 
            // CLSBox
            // 
            this.CLSBox.AutoSize = true;
            this.CLSBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CLSBox.ForeColor = System.Drawing.Color.White;
            this.CLSBox.Location = new System.Drawing.Point(8, 80);
            this.CLSBox.Name = "CLSBox";
            this.CLSBox.Size = new System.Drawing.Size(46, 17);
            this.CLSBox.TabIndex = 30;
            this.CLSBox.Text = "CLS";
            this.CLSBox.UseVisualStyleBackColor = true;
            this.CLSBox.CheckedChanged += new System.EventHandler(this.CLSBox_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(6, 52);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(105, 13);
            this.label10.TabIndex = 32;
            this.label10.Text = "Next Refresh  (Secs)";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.StartButton);
            this.groupBox1.Controls.Add(this.SecondsButton);
            this.groupBox1.Controls.Add(this.MinutesButton);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.RefreshIntervalBox);
            this.groupBox1.Controls.Add(this.CLSBox);
            this.groupBox1.Controls.Add(this.OnTopBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.StopButton);
            this.groupBox1.Controls.Add(this.CloseButton);
            this.groupBox1.Controls.Add(this.NextRefreshBox);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(205, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(188, 145);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tracking";
            // 
            // StartButton
            // 
            this.StartButton.ForeColor = System.Drawing.Color.Black;
            this.StartButton.Location = new System.Drawing.Point(13, 112);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(52, 23);
            this.StartButton.TabIndex = 35;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // SecondsButton
            // 
            this.SecondsButton.AutoSize = true;
            this.SecondsButton.Location = new System.Drawing.Point(140, 22);
            this.SecondsButton.Name = "SecondsButton";
            this.SecondsButton.Size = new System.Drawing.Size(42, 17);
            this.SecondsButton.TabIndex = 34;
            this.SecondsButton.Text = "sec";
            this.SecondsButton.UseVisualStyleBackColor = true;
            // 
            // MinutesButton
            // 
            this.MinutesButton.AutoSize = true;
            this.MinutesButton.Checked = true;
            this.MinutesButton.Location = new System.Drawing.Point(100, 22);
            this.MinutesButton.Name = "MinutesButton";
            this.MinutesButton.Size = new System.Drawing.Size(41, 17);
            this.MinutesButton.TabIndex = 33;
            this.MinutesButton.TabStop = true;
            this.MinutesButton.Text = "min";
            this.MinutesButton.UseVisualStyleBackColor = true;
            // 
            // ScoutRadioButton
            // 
            this.ScoutRadioButton.AutoSize = true;
            this.ScoutRadioButton.Checked = true;
            this.ScoutRadioButton.ForeColor = System.Drawing.Color.White;
            this.ScoutRadioButton.Location = new System.Drawing.Point(17, 125);
            this.ScoutRadioButton.Name = "ScoutRadioButton";
            this.ScoutRadioButton.Size = new System.Drawing.Size(53, 17);
            this.ScoutRadioButton.TabIndex = 34;
            this.ScoutRadioButton.TabStop = true;
            this.ScoutRadioButton.Text = "Scout";
            this.ScoutRadioButton.UseVisualStyleBackColor = true;
            // 
            // HorizonsRadioButton
            // 
            this.HorizonsRadioButton.AutoSize = true;
            this.HorizonsRadioButton.ForeColor = System.Drawing.Color.White;
            this.HorizonsRadioButton.Location = new System.Drawing.Point(79, 125);
            this.HorizonsRadioButton.Name = "HorizonsRadioButton";
            this.HorizonsRadioButton.Size = new System.Drawing.Size(66, 17);
            this.HorizonsRadioButton.TabIndex = 35;
            this.HorizonsRadioButton.Text = "Horizons";
            this.HorizonsRadioButton.UseVisualStyleBackColor = true;
            // 
            // MPCRadioButton
            // 
            this.MPCRadioButton.AutoSize = true;
            this.MPCRadioButton.ForeColor = System.Drawing.Color.White;
            this.MPCRadioButton.Location = new System.Drawing.Point(151, 125);
            this.MPCRadioButton.Name = "MPCRadioButton";
            this.MPCRadioButton.Size = new System.Drawing.Size(48, 17);
            this.MPCRadioButton.TabIndex = 36;
            this.MPCRadioButton.Text = "MPC";
            this.MPCRadioButton.UseVisualStyleBackColor = true;
            // 
            // FormHotPursuit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkCyan;
            this.ClientSize = new System.Drawing.Size(588, 215);
            this.Controls.Add(this.MPCRadioButton);
            this.Controls.Add(this.HorizonsRadioButton);
            this.Controls.Add(this.ScoutRadioButton);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.RangeBox);
            this.Controls.Add(this.RARateBox);
            this.Controls.Add(this.DecRateBox);
            this.Controls.Add(this.HPStatusBox);
            this.Controls.Add(this.SequencerGroupBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TargetBox);
            this.Controls.Add(this.groupBox1);
            this.ForeColor = System.Drawing.Color.Teal;
            this.MaximizeBox = false;
            this.Name = "FormHotPursuit";
            this.Text = "  ";
            ((System.ComponentModel.ISupportInitialize)(this.RefreshIntervalBox)).EndInit();
            this.SequencerGroupBox.ResumeLayout(false);
            this.SequencerGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RepsBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExposureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SlewSettlingTimeDelayBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.NumericUpDown RefreshIntervalBox;
        private System.Windows.Forms.TextBox TargetBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NextRefreshBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox OnTopBox;
        private System.Windows.Forms.GroupBox SequencerGroupBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown ExposureBox;
        private System.Windows.Forms.Button ImageButton;
        private System.Windows.Forms.CheckBox FullReductionCheckBox;
        private System.Windows.Forms.ComboBox FiltersListBox;
        private System.Windows.Forms.TextBox HPStatusBox;
        private System.Windows.Forms.TextBox DecRateBox;
        private System.Windows.Forms.TextBox RARateBox;
        private System.Windows.Forms.TextBox RangeBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown RepsBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox CLSBox;
        private System.Windows.Forms.CheckBox LiveStackBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox RecenterBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton SecondsButton;
        private System.Windows.Forms.RadioButton MinutesButton;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.RadioButton ScoutRadioButton;
        private System.Windows.Forms.RadioButton HorizonsRadioButton;
        private System.Windows.Forms.RadioButton MPCRadioButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown SlewSettlingTimeDelayBox;
    }
}

