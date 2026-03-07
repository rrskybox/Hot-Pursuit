
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHotPursuit));
            this.CloseButton = new System.Windows.Forms.Button();
            this.RefreshIntervalBox = new System.Windows.Forms.NumericUpDown();
            this.TargetBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
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
            this.HPStatusBox = new System.Windows.Forms.TextBox();
            this.DecRateBox = new System.Windows.Forms.TextBox();
            this.RARateBox = new System.Windows.Forms.TextBox();
            this.RangeBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.CLSBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TrailButton = new System.Windows.Forms.Button();
            this.StartStopButton = new System.Windows.Forms.Button();
            this.SecondsButton = new System.Windows.Forms.RadioButton();
            this.MinutesButton = new System.Windows.Forms.RadioButton();
            this.PlotGroupBox = new System.Windows.Forms.GroupBox();
            this.PlotDaysBox = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.GenerateSDBButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.SourceBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.RefreshIntervalBox)).BeginInit();
            this.SequencerGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RepsBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExposureBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.PlotGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PlotDaysBox)).BeginInit();
            this.SuspendLayout();
            // 
            // CloseButton
            // 
            this.CloseButton.ForeColor = System.Drawing.Color.Black;
            this.CloseButton.Location = new System.Drawing.Point(258, 160);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(6);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(100, 44);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // RefreshIntervalBox
            // 
            this.RefreshIntervalBox.Location = new System.Drawing.Point(106, 38);
            this.RefreshIntervalBox.Margin = new System.Windows.Forms.Padding(6);
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
            this.RefreshIntervalBox.Size = new System.Drawing.Size(82, 31);
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
            this.TargetBox.Location = new System.Drawing.Point(112, 19);
            this.TargetBox.Margin = new System.Windows.Forms.Padding(6);
            this.TargetBox.Name = "TargetBox";
            this.TargetBox.Size = new System.Drawing.Size(282, 31);
            this.TargetBox.TabIndex = 5;
            this.TargetBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TargetBox.DoubleClick += new System.EventHandler(this.TargetBox_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(24, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "Target";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(14, 46);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 25);
            this.label2.TabIndex = 8;
            this.label2.Text = "Refresh ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(24, 71);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(159, 25);
            this.label3.TabIndex = 9;
            this.label3.Text = "RA Rate (\"/min)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(24, 119);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(168, 25);
            this.label4.TabIndex = 14;
            this.label4.Text = "Dec Rate (\"/min)";
            // 
            // OnTopBox
            // 
            this.OnTopBox.AutoSize = true;
            this.OnTopBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.OnTopBox.ForeColor = System.Drawing.Color.White;
            this.OnTopBox.Location = new System.Drawing.Point(234, 98);
            this.OnTopBox.Margin = new System.Windows.Forms.Padding(6);
            this.OnTopBox.Name = "OnTopBox";
            this.OnTopBox.Size = new System.Drawing.Size(115, 29);
            this.OnTopBox.TabIndex = 16;
            this.OnTopBox.Text = "On Top";
            this.OnTopBox.UseVisualStyleBackColor = true;
            this.OnTopBox.CheckedChanged += new System.EventHandler(this.OnTopBox_CheckedChanged);
            // 
            // SequencerGroupBox
            // 
            this.SequencerGroupBox.BackColor = System.Drawing.Color.DarkCyan;
            this.SequencerGroupBox.Controls.Add(this.RecenterBox);
            this.SequencerGroupBox.Controls.Add(this.LiveStackBox);
            this.SequencerGroupBox.Controls.Add(this.RepsBox);
            this.SequencerGroupBox.Controls.Add(this.label9);
            this.SequencerGroupBox.Controls.Add(this.FiltersListBox);
            this.SequencerGroupBox.Controls.Add(this.FullReductionCheckBox);
            this.SequencerGroupBox.Controls.Add(this.label6);
            this.SequencerGroupBox.Controls.Add(this.label5);
            this.SequencerGroupBox.Controls.Add(this.ExposureBox);
            this.SequencerGroupBox.Controls.Add(this.ImageButton);
            this.SequencerGroupBox.ForeColor = System.Drawing.Color.MintCream;
            this.SequencerGroupBox.Location = new System.Drawing.Point(1142, 13);
            this.SequencerGroupBox.Margin = new System.Windows.Forms.Padding(6);
            this.SequencerGroupBox.Name = "SequencerGroupBox";
            this.SequencerGroupBox.Padding = new System.Windows.Forms.Padding(6);
            this.SequencerGroupBox.Size = new System.Drawing.Size(364, 229);
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
            this.RecenterBox.Location = new System.Drawing.Point(212, 127);
            this.RecenterBox.Margin = new System.Windows.Forms.Padding(6);
            this.RecenterBox.Name = "RecenterBox";
            this.RecenterBox.Size = new System.Drawing.Size(131, 29);
            this.RecenterBox.TabIndex = 27;
            this.RecenterBox.Text = "Recenter";
            this.RecenterBox.UseVisualStyleBackColor = true;
            // 
            // LiveStackBox
            // 
            this.LiveStackBox.AutoSize = true;
            this.LiveStackBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LiveStackBox.ForeColor = System.Drawing.Color.White;
            this.LiveStackBox.Location = new System.Drawing.Point(12, 177);
            this.LiveStackBox.Margin = new System.Windows.Forms.Padding(6);
            this.LiveStackBox.Name = "LiveStackBox";
            this.LiveStackBox.Size = new System.Drawing.Size(144, 29);
            this.LiveStackBox.TabIndex = 26;
            this.LiveStackBox.Text = "Live Stack";
            this.LiveStackBox.UseVisualStyleBackColor = true;
            // 
            // RepsBox
            // 
            this.RepsBox.Location = new System.Drawing.Point(84, 75);
            this.RepsBox.Margin = new System.Windows.Forms.Padding(6);
            this.RepsBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.RepsBox.Name = "RepsBox";
            this.RepsBox.Size = new System.Drawing.Size(74, 31);
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
            this.label9.Location = new System.Drawing.Point(12, 83);
            this.label9.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 25);
            this.label9.TabIndex = 24;
            this.label9.Text = "Reps";
            // 
            // FiltersListBox
            // 
            this.FiltersListBox.FormattingEnabled = true;
            this.FiltersListBox.Location = new System.Drawing.Point(240, 75);
            this.FiltersListBox.Margin = new System.Windows.Forms.Padding(6);
            this.FiltersListBox.Name = "FiltersListBox";
            this.FiltersListBox.Size = new System.Drawing.Size(106, 33);
            this.FiltersListBox.TabIndex = 21;
            this.FiltersListBox.SelectedIndexChanged += new System.EventHandler(this.FiltersListBox_SelectedIndexChanged);
            // 
            // FullReductionCheckBox
            // 
            this.FullReductionCheckBox.AutoSize = true;
            this.FullReductionCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.FullReductionCheckBox.ForeColor = System.Drawing.Color.White;
            this.FullReductionCheckBox.Location = new System.Drawing.Point(12, 127);
            this.FullReductionCheckBox.Margin = new System.Windows.Forms.Padding(6);
            this.FullReductionCheckBox.Name = "FullReductionCheckBox";
            this.FullReductionCheckBox.Size = new System.Drawing.Size(142, 29);
            this.FullReductionCheckBox.TabIndex = 18;
            this.FullReductionCheckBox.Text = "Reduce    ";
            this.FullReductionCheckBox.UseVisualStyleBackColor = true;
            this.FullReductionCheckBox.CheckedChanged += new System.EventHandler(this.FullReductionCheckBox_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(170, 83);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 25);
            this.label6.TabIndex = 20;
            this.label6.Text = "Filter";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(12, 33);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(171, 25);
            this.label5.TabIndex = 18;
            this.label5.Text = "Exposure (Secs)";
            // 
            // ExposureBox
            // 
            this.ExposureBox.Location = new System.Drawing.Point(240, 29);
            this.ExposureBox.Margin = new System.Windows.Forms.Padding(6);
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
            this.ExposureBox.Size = new System.Drawing.Size(110, 31);
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
            this.ImageButton.Location = new System.Drawing.Point(236, 167);
            this.ImageButton.Margin = new System.Windows.Forms.Padding(6);
            this.ImageButton.Name = "ImageButton";
            this.ImageButton.Size = new System.Drawing.Size(110, 44);
            this.ImageButton.TabIndex = 18;
            this.ImageButton.Text = "Image";
            this.ImageButton.UseVisualStyleBackColor = true;
            this.ImageButton.Click += new System.EventHandler(this.ImageButton_Click);
            // 
            // HPStatusBox
            // 
            this.HPStatusBox.AllowDrop = true;
            this.HPStatusBox.Location = new System.Drawing.Point(12, 254);
            this.HPStatusBox.Margin = new System.Windows.Forms.Padding(6);
            this.HPStatusBox.Multiline = true;
            this.HPStatusBox.Name = "HPStatusBox";
            this.HPStatusBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.HPStatusBox.Size = new System.Drawing.Size(1490, 85);
            this.HPStatusBox.TabIndex = 18;
            // 
            // DecRateBox
            // 
            this.DecRateBox.Location = new System.Drawing.Point(244, 113);
            this.DecRateBox.Margin = new System.Windows.Forms.Padding(6);
            this.DecRateBox.Name = "DecRateBox";
            this.DecRateBox.Size = new System.Drawing.Size(150, 31);
            this.DecRateBox.TabIndex = 23;
            this.DecRateBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RARateBox
            // 
            this.RARateBox.Location = new System.Drawing.Point(244, 65);
            this.RARateBox.Margin = new System.Windows.Forms.Padding(6);
            this.RARateBox.Name = "RARateBox";
            this.RARateBox.Size = new System.Drawing.Size(150, 31);
            this.RARateBox.TabIndex = 24;
            this.RARateBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RangeBox
            // 
            this.RangeBox.Location = new System.Drawing.Point(244, 162);
            this.RangeBox.Margin = new System.Windows.Forms.Padding(6);
            this.RangeBox.Name = "RangeBox";
            this.RangeBox.Size = new System.Drawing.Size(150, 31);
            this.RangeBox.TabIndex = 28;
            this.RangeBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(24, 167);
            this.label8.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(124, 25);
            this.label8.TabIndex = 29;
            this.label8.Text = "Range (AU)";
            // 
            // CLSBox
            // 
            this.CLSBox.AutoSize = true;
            this.CLSBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CLSBox.ForeColor = System.Drawing.Color.White;
            this.CLSBox.Location = new System.Drawing.Point(16, 98);
            this.CLSBox.Margin = new System.Windows.Forms.Padding(6);
            this.CLSBox.Name = "CLSBox";
            this.CLSBox.Size = new System.Drawing.Size(85, 29);
            this.CLSBox.TabIndex = 30;
            this.CLSBox.Text = "CLS";
            this.CLSBox.UseVisualStyleBackColor = true;
            this.CLSBox.CheckedChanged += new System.EventHandler(this.CLSBox_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.DarkCyan;
            this.groupBox1.Controls.Add(this.TrailButton);
            this.groupBox1.Controls.Add(this.StartStopButton);
            this.groupBox1.Controls.Add(this.SecondsButton);
            this.groupBox1.Controls.Add(this.MinutesButton);
            this.groupBox1.Controls.Add(this.RefreshIntervalBox);
            this.groupBox1.Controls.Add(this.CLSBox);
            this.groupBox1.Controls.Add(this.OnTopBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.CloseButton);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(752, 13);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.Size = new System.Drawing.Size(376, 229);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Track";
            // 
            // TrailButton
            // 
            this.TrailButton.ForeColor = System.Drawing.Color.Black;
            this.TrailButton.Location = new System.Drawing.Point(142, 160);
            this.TrailButton.Margin = new System.Windows.Forms.Padding(6);
            this.TrailButton.Name = "TrailButton";
            this.TrailButton.Size = new System.Drawing.Size(104, 44);
            this.TrailButton.TabIndex = 36;
            this.TrailButton.Text = "Trail";
            this.TrailButton.UseVisualStyleBackColor = true;
            this.TrailButton.Click += new System.EventHandler(this.TrailButton_Click);
            // 
            // StartStopButton
            // 
            this.StartStopButton.ForeColor = System.Drawing.Color.Black;
            this.StartStopButton.Location = new System.Drawing.Point(26, 160);
            this.StartStopButton.Margin = new System.Windows.Forms.Padding(6);
            this.StartStopButton.Name = "StartStopButton";
            this.StartStopButton.Size = new System.Drawing.Size(104, 44);
            this.StartStopButton.TabIndex = 35;
            this.StartStopButton.Text = "Track";
            this.StartStopButton.UseVisualStyleBackColor = true;
            this.StartStopButton.Click += new System.EventHandler(this.StartStopButton_Click);
            // 
            // SecondsButton
            // 
            this.SecondsButton.AutoSize = true;
            this.SecondsButton.Location = new System.Drawing.Point(280, 42);
            this.SecondsButton.Margin = new System.Windows.Forms.Padding(6);
            this.SecondsButton.Name = "SecondsButton";
            this.SecondsButton.Size = new System.Drawing.Size(77, 29);
            this.SecondsButton.TabIndex = 34;
            this.SecondsButton.Text = "sec";
            this.SecondsButton.UseVisualStyleBackColor = true;
            // 
            // MinutesButton
            // 
            this.MinutesButton.AutoSize = true;
            this.MinutesButton.Checked = true;
            this.MinutesButton.Location = new System.Drawing.Point(200, 42);
            this.MinutesButton.Margin = new System.Windows.Forms.Padding(6);
            this.MinutesButton.Name = "MinutesButton";
            this.MinutesButton.Size = new System.Drawing.Size(77, 29);
            this.MinutesButton.TabIndex = 33;
            this.MinutesButton.TabStop = true;
            this.MinutesButton.Text = "min";
            this.MinutesButton.UseVisualStyleBackColor = true;
            // 
            // PlotGroupBox
            // 
            this.PlotGroupBox.BackColor = System.Drawing.Color.DarkCyan;
            this.PlotGroupBox.Controls.Add(this.PlotDaysBox);
            this.PlotGroupBox.Controls.Add(this.label10);
            this.PlotGroupBox.Controls.Add(this.GenerateSDBButton);
            this.PlotGroupBox.ForeColor = System.Drawing.Color.MintCream;
            this.PlotGroupBox.Location = new System.Drawing.Point(416, 13);
            this.PlotGroupBox.Margin = new System.Windows.Forms.Padding(6);
            this.PlotGroupBox.Name = "PlotGroupBox";
            this.PlotGroupBox.Padding = new System.Windows.Forms.Padding(6);
            this.PlotGroupBox.Size = new System.Drawing.Size(308, 227);
            this.PlotGroupBox.TabIndex = 37;
            this.PlotGroupBox.TabStop = false;
            this.PlotGroupBox.Text = "Plot";
            // 
            // PlotDaysBox
            // 
            this.PlotDaysBox.Location = new System.Drawing.Point(98, 58);
            this.PlotDaysBox.Margin = new System.Windows.Forms.Padding(6);
            this.PlotDaysBox.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.PlotDaysBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PlotDaysBox.Name = "PlotDaysBox";
            this.PlotDaysBox.Size = new System.Drawing.Size(148, 31);
            this.PlotDaysBox.TabIndex = 37;
            this.PlotDaysBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.PlotDaysBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(24, 62);
            this.label10.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 25);
            this.label10.TabIndex = 38;
            this.label10.Text = "Days";
            // 
            // GenerateSDBButton
            // 
            this.GenerateSDBButton.ForeColor = System.Drawing.Color.Black;
            this.GenerateSDBButton.Location = new System.Drawing.Point(58, 160);
            this.GenerateSDBButton.Margin = new System.Windows.Forms.Padding(6);
            this.GenerateSDBButton.Name = "GenerateSDBButton";
            this.GenerateSDBButton.Size = new System.Drawing.Size(188, 44);
            this.GenerateSDBButton.TabIndex = 36;
            this.GenerateSDBButton.Text = "Generate SDB";
            this.GenerateSDBButton.UseVisualStyleBackColor = true;
            this.GenerateSDBButton.Click += new System.EventHandler(this.GenerateSDBButton_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(24, 215);
            this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 25);
            this.label7.TabIndex = 38;
            this.label7.Text = "Source";
            // 
            // SourceBox
            // 
            this.SourceBox.FormattingEnabled = true;
            this.SourceBox.Items.AddRange(new object[] {
            "Scout",
            "Horizons",
            "MPC",
            "Raw"});
            this.SourceBox.Location = new System.Drawing.Point(172, 209);
            this.SourceBox.MaxDropDownItems = 4;
            this.SourceBox.Name = "SourceBox";
            this.SourceBox.Size = new System.Drawing.Size(222, 33);
            this.SourceBox.TabIndex = 39;
            this.SourceBox.Text = "Source Name";
            this.SourceBox.SelectedIndexChanged += new System.EventHandler(this.SourceBox_SelectedIndexChanged);
            // 
            // FormHotPursuit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1527, 355);
            this.Controls.Add(this.SourceBox);
            this.Controls.Add(this.label7);
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
            this.Controls.Add(this.PlotGroupBox);
            this.ForeColor = System.Drawing.Color.Teal;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.Name = "FormHotPursuit";
            this.Text = "  ";
            ((System.ComponentModel.ISupportInitialize)(this.RefreshIntervalBox)).EndInit();
            this.SequencerGroupBox.ResumeLayout(false);
            this.SequencerGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RepsBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExposureBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.PlotGroupBox.ResumeLayout(false);
            this.PlotGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PlotDaysBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.NumericUpDown RefreshIntervalBox;
        private System.Windows.Forms.TextBox TargetBox;
        private System.Windows.Forms.Label label1;
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
        private System.Windows.Forms.CheckBox RecenterBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton SecondsButton;
        private System.Windows.Forms.RadioButton MinutesButton;
        private System.Windows.Forms.Button StartStopButton;
        private System.Windows.Forms.Button TrailButton;
        private System.Windows.Forms.GroupBox PlotGroupBox;
        private System.Windows.Forms.Button GenerateSDBButton;
        private System.Windows.Forms.NumericUpDown PlotDaysBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox SourceBox;
    }
}

