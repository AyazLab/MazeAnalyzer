namespace MazeAnalyzer
{
    partial class Heatmap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Heatmap));
            this.textBoxRes = new System.Windows.Forms.TextBox();
            this.buttonMaxColor = new System.Windows.Forms.Button();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.buttonMinColor = new System.Windows.Forms.Button();
            this.buttonBgColor = new System.Windows.Forms.Button();
            this.buttonMidColor = new System.Windows.Forms.Button();
            this.checkBoxShowBg = new System.Windows.Forms.CheckBox();
            this.checkBoxShowMidColor = new System.Windows.Forms.CheckBox();
            this.trackBarMidpoint = new System.Windows.Forms.TrackBar();
            this.comboBoxInterpolation = new System.Windows.Forms.ComboBox();
            this.labelRes = new System.Windows.Forms.Label();
            this.labelShowInterpolation = new System.Windows.Forms.Label();
            this.buttonSavePng = new System.Windows.Forms.Button();
            this.trackBarOpacity = new System.Windows.Forms.TrackBar();
            this.labelOpacity = new System.Windows.Forms.Label();
            this.textBoxOffset = new System.Windows.Forms.TextBox();
            this.labelOffset = new System.Windows.Forms.Label();
            this.comboBoxColorPreset = new System.Windows.Forms.ComboBox();
            this.labelColorPreset = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.labelDrawSettings = new System.Windows.Forms.Label();
            this.labelQualitySettings = new System.Windows.Forms.Label();
            this.labelColorSettings = new System.Windows.Forms.Label();
            this.checkBoxShowAnaRegns = new System.Windows.Forms.CheckBox();
            this.checkBoxShowMaze = new System.Windows.Forms.CheckBox();
            this.radioButtonTime = new System.Windows.Forms.RadioButton();
            this.radioButtonEntr = new System.Windows.Forms.RadioButton();
            this.radioButtonPres = new System.Windows.Forms.RadioButton();
            this.buttonSaveCsv = new System.Windows.Forms.Button();
            this.panelSettings = new System.Windows.Forms.Panel();
            this.comboBoxShowPaths = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelBgColor = new System.Windows.Forms.Label();
            this.labelMin = new System.Windows.Forms.Label();
            this.labelMax = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_heatmap = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.labelSharpness = new System.Windows.Forms.Label();
            this.labelSharpnessVal = new System.Windows.Forms.Label();
            this.buttonAutoRes = new System.Windows.Forms.Button();
            this.buttonOffset = new System.Windows.Forms.Button();
            this.labelOpacityPercent = new System.Windows.Forms.Label();
            this.trackBarSharpness = new System.Windows.Forms.TrackBar();
            this.labelScroll = new System.Windows.Forms.Label();
            this.pictureBoxHeatmap = new System.Windows.Forms.PictureBox();
            this.panelHeatmap = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMidpoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOpacity)).BeginInit();
            this.panelSettings.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSharpness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHeatmap)).BeginInit();
            this.panelHeatmap.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxRes
            // 
            this.textBoxRes.Location = new System.Drawing.Point(213, 371);
            this.textBoxRes.Name = "textBoxRes";
            this.textBoxRes.Size = new System.Drawing.Size(47, 22);
            this.textBoxRes.TabIndex = 1;
            this.textBoxRes.TextChanged += new System.EventHandler(this.textBoxRes_TextChanged);
            this.textBoxRes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxRes_KeyDown);
            // 
            // buttonMaxColor
            // 
            this.buttonMaxColor.Location = new System.Drawing.Point(119, 81);
            this.buttonMaxColor.Name = "buttonMaxColor";
            this.buttonMaxColor.Size = new System.Drawing.Size(29, 23);
            this.buttonMaxColor.TabIndex = 2;
            this.buttonMaxColor.UseVisualStyleBackColor = true;
            this.buttonMaxColor.Click += new System.EventHandler(this.buttonMaxColor_Click);
            // 
            // buttonMinColor
            // 
            this.buttonMinColor.Location = new System.Drawing.Point(119, 259);
            this.buttonMinColor.Name = "buttonMinColor";
            this.buttonMinColor.Size = new System.Drawing.Size(29, 23);
            this.buttonMinColor.TabIndex = 3;
            this.buttonMinColor.UseVisualStyleBackColor = true;
            this.buttonMinColor.Click += new System.EventHandler(this.buttonMinColor_Click);
            // 
            // buttonBgColor
            // 
            this.buttonBgColor.Location = new System.Drawing.Point(165, 293);
            this.buttonBgColor.Name = "buttonBgColor";
            this.buttonBgColor.Size = new System.Drawing.Size(31, 23);
            this.buttonBgColor.TabIndex = 4;
            this.buttonBgColor.UseVisualStyleBackColor = true;
            this.buttonBgColor.Click += new System.EventHandler(this.buttonBgColor_Click);
            // 
            // buttonMidColor
            // 
            this.buttonMidColor.Location = new System.Drawing.Point(119, 170);
            this.buttonMidColor.Name = "buttonMidColor";
            this.buttonMidColor.Size = new System.Drawing.Size(29, 23);
            this.buttonMidColor.TabIndex = 10;
            this.buttonMidColor.UseVisualStyleBackColor = true;
            this.buttonMidColor.Click += new System.EventHandler(this.buttonMidColor_Click);
            // 
            // checkBoxShowBg
            // 
            this.checkBoxShowBg.AutoSize = true;
            this.checkBoxShowBg.Location = new System.Drawing.Point(14, 296);
            this.checkBoxShowBg.Name = "checkBoxShowBg";
            this.checkBoxShowBg.Size = new System.Drawing.Size(18, 17);
            this.checkBoxShowBg.TabIndex = 17;
            this.checkBoxShowBg.UseVisualStyleBackColor = true;
            this.checkBoxShowBg.Click += new System.EventHandler(this.checkBoxShowBgTransparent_Click);
            // 
            // checkBoxShowMidColor
            // 
            this.checkBoxShowMidColor.AutoSize = true;
            this.checkBoxShowMidColor.Location = new System.Drawing.Point(63, 172);
            this.checkBoxShowMidColor.Name = "checkBoxShowMidColor";
            this.checkBoxShowMidColor.Size = new System.Drawing.Size(52, 21);
            this.checkBoxShowMidColor.TabIndex = 18;
            this.checkBoxShowMidColor.Text = "Mid";
            this.checkBoxShowMidColor.UseVisualStyleBackColor = true;
            this.checkBoxShowMidColor.Click += new System.EventHandler(this.checkBoxShowMidColor_Click);
            // 
            // trackBarMidpoint
            // 
            this.trackBarMidpoint.Location = new System.Drawing.Point(15, 81);
            this.trackBarMidpoint.Name = "trackBarMidpoint";
            this.trackBarMidpoint.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarMidpoint.Size = new System.Drawing.Size(56, 202);
            this.trackBarMidpoint.TabIndex = 19;
            this.trackBarMidpoint.Value = 5;
            this.trackBarMidpoint.Scroll += new System.EventHandler(this.trackBarMidpoint_Scroll);
            // 
            // comboBoxInterpolation
            // 
            this.comboBoxInterpolation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxInterpolation.FormattingEnabled = true;
            this.comboBoxInterpolation.Items.AddRange(new object[] {
            "None",
            "Bilinear",
            "HighQualityBilinear",
            "Bicubic",
            "HighQualityBicubic"});
            this.comboBoxInterpolation.Location = new System.Drawing.Point(102, 430);
            this.comboBoxInterpolation.Name = "comboBoxInterpolation";
            this.comboBoxInterpolation.Size = new System.Drawing.Size(169, 24);
            this.comboBoxInterpolation.TabIndex = 21;
            this.comboBoxInterpolation.SelectedIndexChanged += new System.EventHandler(this.comboBoxInterpolation_SelectedIndexChanged);
            // 
            // labelRes
            // 
            this.labelRes.AutoSize = true;
            this.labelRes.Location = new System.Drawing.Point(10, 371);
            this.labelRes.Name = "labelRes";
            this.labelRes.Size = new System.Drawing.Size(198, 17);
            this.labelRes.TabIndex = 22;
            this.labelRes.Text = "Resolution [Maze Units / Pixel]";
            // 
            // labelShowInterpolation
            // 
            this.labelShowInterpolation.AutoSize = true;
            this.labelShowInterpolation.Location = new System.Drawing.Point(10, 433);
            this.labelShowInterpolation.Name = "labelShowInterpolation";
            this.labelShowInterpolation.Size = new System.Drawing.Size(86, 17);
            this.labelShowInterpolation.TabIndex = 24;
            this.labelShowInterpolation.Text = "Interpolation";
            // 
            // buttonSavePng
            // 
            this.buttonSavePng.Location = new System.Drawing.Point(107, 706);
            this.buttonSavePng.Name = "buttonSavePng";
            this.buttonSavePng.Size = new System.Drawing.Size(93, 46);
            this.buttonSavePng.TabIndex = 26;
            this.buttonSavePng.Text = "Export Image";
            this.buttonSavePng.UseVisualStyleBackColor = true;
            this.buttonSavePng.Click += new System.EventHandler(this.buttonSavePng_Click);
            // 
            // trackBarOpacity
            // 
            this.trackBarOpacity.Location = new System.Drawing.Point(92, 509);
            this.trackBarOpacity.Maximum = 100;
            this.trackBarOpacity.Name = "trackBarOpacity";
            this.trackBarOpacity.Size = new System.Drawing.Size(121, 56);
            this.trackBarOpacity.TabIndex = 28;
            this.trackBarOpacity.TickFrequency = 10;
            this.trackBarOpacity.Scroll += new System.EventHandler(this.trackBarOpacity_Scroll);
            // 
            // labelOpacity
            // 
            this.labelOpacity.AutoSize = true;
            this.labelOpacity.Location = new System.Drawing.Point(9, 509);
            this.labelOpacity.Name = "labelOpacity";
            this.labelOpacity.Size = new System.Drawing.Size(56, 17);
            this.labelOpacity.TabIndex = 29;
            this.labelOpacity.Text = "Opacity";
            // 
            // textBoxOffset
            // 
            this.textBoxOffset.Location = new System.Drawing.Point(96, 400);
            this.textBoxOffset.Name = "textBoxOffset";
            this.textBoxOffset.Size = new System.Drawing.Size(100, 22);
            this.textBoxOffset.TabIndex = 34;
            this.textBoxOffset.TextChanged += new System.EventHandler(this.textBoxOffset_TextChanged);
            this.textBoxOffset.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxOffset_KeyDown);
            // 
            // labelOffset
            // 
            this.labelOffset.AutoSize = true;
            this.labelOffset.BackColor = System.Drawing.SystemColors.Control;
            this.labelOffset.Location = new System.Drawing.Point(10, 400);
            this.labelOffset.Name = "labelOffset";
            this.labelOffset.Size = new System.Drawing.Size(81, 17);
            this.labelOffset.TabIndex = 36;
            this.labelOffset.Text = "Offset (x, z)";
            this.labelOffset.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBoxColorPreset
            // 
            this.comboBoxColorPreset.FormattingEnabled = true;
            this.comboBoxColorPreset.Items.AddRange(new object[] {
            "Custom",
            "Hot",
            "Cool",
            "Gray",
            "Summer",
            "Autumn",
            "Winter",
            "Spring",
            "Jet",
            "Red-White-Blue",
            "Red-Yellow-Green",
            "Orange-White-Purple",
            "White-Black-Red"});
            this.comboBoxColorPreset.Location = new System.Drawing.Point(96, 40);
            this.comboBoxColorPreset.Name = "comboBoxColorPreset";
            this.comboBoxColorPreset.Size = new System.Drawing.Size(175, 24);
            this.comboBoxColorPreset.TabIndex = 38;
            this.comboBoxColorPreset.SelectedIndexChanged += new System.EventHandler(this.comboBoxColorPreset_SelectedIndexChanged);
            // 
            // labelColorPreset
            // 
            this.labelColorPreset.AutoSize = true;
            this.labelColorPreset.Location = new System.Drawing.Point(20, 42);
            this.labelColorPreset.Name = "labelColorPreset";
            this.labelColorPreset.Size = new System.Drawing.Size(49, 17);
            this.labelColorPreset.TabIndex = 39;
            this.labelColorPreset.Text = "Preset";
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 803);
            this.splitter1.TabIndex = 40;
            this.splitter1.TabStop = false;
            // 
            // labelDrawSettings
            // 
            this.labelDrawSettings.AutoSize = true;
            this.labelDrawSettings.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.labelDrawSettings.Location = new System.Drawing.Point(13, 558);
            this.labelDrawSettings.Name = "labelDrawSettings";
            this.labelDrawSettings.Size = new System.Drawing.Size(95, 17);
            this.labelDrawSettings.TabIndex = 49;
            this.labelDrawSettings.Text = "Draw Settings";
            // 
            // labelQualitySettings
            // 
            this.labelQualitySettings.AutoSize = true;
            this.labelQualitySettings.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.labelQualitySettings.Location = new System.Drawing.Point(10, 342);
            this.labelQualitySettings.Name = "labelQualitySettings";
            this.labelQualitySettings.Size = new System.Drawing.Size(107, 17);
            this.labelQualitySettings.TabIndex = 48;
            this.labelQualitySettings.Text = "Quality Settings";
            // 
            // labelColorSettings
            // 
            this.labelColorSettings.AutoSize = true;
            this.labelColorSettings.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.labelColorSettings.Location = new System.Drawing.Point(12, 9);
            this.labelColorSettings.Name = "labelColorSettings";
            this.labelColorSettings.Size = new System.Drawing.Size(96, 17);
            this.labelColorSettings.TabIndex = 47;
            this.labelColorSettings.Text = "Color Settings";
            // 
            // checkBoxShowAnaRegns
            // 
            this.checkBoxShowAnaRegns.AutoSize = true;
            this.checkBoxShowAnaRegns.Location = new System.Drawing.Point(165, 633);
            this.checkBoxShowAnaRegns.Name = "checkBoxShowAnaRegns";
            this.checkBoxShowAnaRegns.Size = new System.Drawing.Size(141, 21);
            this.checkBoxShowAnaRegns.TabIndex = 45;
            this.checkBoxShowAnaRegns.Text = "Analyzer Regions";
            this.checkBoxShowAnaRegns.UseVisualStyleBackColor = true;
            this.checkBoxShowAnaRegns.Click += new System.EventHandler(this.checkBoxShowAnalyzerRegions_Click);
            // 
            // checkBoxShowMaze
            // 
            this.checkBoxShowMaze.AutoSize = true;
            this.checkBoxShowMaze.Location = new System.Drawing.Point(165, 606);
            this.checkBoxShowMaze.Name = "checkBoxShowMaze";
            this.checkBoxShowMaze.Size = new System.Drawing.Size(64, 21);
            this.checkBoxShowMaze.TabIndex = 44;
            this.checkBoxShowMaze.Text = "Maze";
            this.checkBoxShowMaze.UseVisualStyleBackColor = true;
            this.checkBoxShowMaze.Click += new System.EventHandler(this.checkBoxShowMaze_Click);
            // 
            // radioButtonTime
            // 
            this.radioButtonTime.AutoSize = true;
            this.radioButtonTime.Location = new System.Drawing.Point(20, 661);
            this.radioButtonTime.Name = "radioButtonTime";
            this.radioButtonTime.Size = new System.Drawing.Size(60, 21);
            this.radioButtonTime.TabIndex = 42;
            this.radioButtonTime.Text = "Time";
            this.radioButtonTime.UseVisualStyleBackColor = true;
            this.radioButtonTime.CheckedChanged += new System.EventHandler(this.radioButtonTime_CheckedChanged);
            // 
            // radioButtonEntr
            // 
            this.radioButtonEntr.AutoSize = true;
            this.radioButtonEntr.Location = new System.Drawing.Point(20, 634);
            this.radioButtonEntr.Name = "radioButtonEntr";
            this.radioButtonEntr.Size = new System.Drawing.Size(86, 21);
            this.radioButtonEntr.TabIndex = 41;
            this.radioButtonEntr.Text = "Entrance";
            this.radioButtonEntr.UseVisualStyleBackColor = true;
            this.radioButtonEntr.CheckedChanged += new System.EventHandler(this.radioButtonEntr_CheckedChanged);
            // 
            // radioButtonPres
            // 
            this.radioButtonPres.AutoSize = true;
            this.radioButtonPres.Checked = true;
            this.radioButtonPres.Location = new System.Drawing.Point(20, 606);
            this.radioButtonPres.Name = "radioButtonPres";
            this.radioButtonPres.Size = new System.Drawing.Size(89, 21);
            this.radioButtonPres.TabIndex = 0;
            this.radioButtonPres.TabStop = true;
            this.radioButtonPres.Text = "Presence";
            this.radioButtonPres.UseVisualStyleBackColor = true;
            this.radioButtonPres.CheckedChanged += new System.EventHandler(this.radioButtonPres_CheckedChanged);
            // 
            // buttonSaveCsv
            // 
            this.buttonSaveCsv.Location = new System.Drawing.Point(8, 706);
            this.buttonSaveCsv.Name = "buttonSaveCsv";
            this.buttonSaveCsv.Size = new System.Drawing.Size(93, 46);
            this.buttonSaveCsv.TabIndex = 40;
            this.buttonSaveCsv.Text = "Export CSV";
            this.buttonSaveCsv.UseVisualStyleBackColor = true;
            this.buttonSaveCsv.Click += new System.EventHandler(this.buttonSaveCsv_Click);
            // 
            // panelSettings
            // 
            this.panelSettings.BackColor = System.Drawing.Color.White;
            this.panelSettings.Controls.Add(this.comboBoxShowPaths);
            this.panelSettings.Controls.Add(this.label5);
            this.panelSettings.Controls.Add(this.label4);
            this.panelSettings.Controls.Add(this.labelBgColor);
            this.panelSettings.Controls.Add(this.labelMin);
            this.panelSettings.Controls.Add(this.labelMax);
            this.panelSettings.Controls.Add(this.statusStrip1);
            this.panelSettings.Controls.Add(this.buttonCopy);
            this.panelSettings.Controls.Add(this.labelSharpness);
            this.panelSettings.Controls.Add(this.labelSharpnessVal);
            this.panelSettings.Controls.Add(this.buttonAutoRes);
            this.panelSettings.Controls.Add(this.buttonOffset);
            this.panelSettings.Controls.Add(this.labelOpacityPercent);
            this.panelSettings.Controls.Add(this.checkBoxShowBg);
            this.panelSettings.Controls.Add(this.labelDrawSettings);
            this.panelSettings.Controls.Add(this.labelColorSettings);
            this.panelSettings.Controls.Add(this.labelQualitySettings);
            this.panelSettings.Controls.Add(this.labelShowInterpolation);
            this.panelSettings.Controls.Add(this.buttonSavePng);
            this.panelSettings.Controls.Add(this.checkBoxShowAnaRegns);
            this.panelSettings.Controls.Add(this.labelRes);
            this.panelSettings.Controls.Add(this.checkBoxShowMaze);
            this.panelSettings.Controls.Add(this.comboBoxInterpolation);
            this.panelSettings.Controls.Add(this.radioButtonTime);
            this.panelSettings.Controls.Add(this.trackBarOpacity);
            this.panelSettings.Controls.Add(this.radioButtonEntr);
            this.panelSettings.Controls.Add(this.radioButtonPres);
            this.panelSettings.Controls.Add(this.buttonSaveCsv);
            this.panelSettings.Controls.Add(this.labelOpacity);
            this.panelSettings.Controls.Add(this.buttonBgColor);
            this.panelSettings.Controls.Add(this.checkBoxShowMidColor);
            this.panelSettings.Controls.Add(this.labelColorPreset);
            this.panelSettings.Controls.Add(this.buttonMidColor);
            this.panelSettings.Controls.Add(this.textBoxRes);
            this.panelSettings.Controls.Add(this.textBoxOffset);
            this.panelSettings.Controls.Add(this.comboBoxColorPreset);
            this.panelSettings.Controls.Add(this.buttonMinColor);
            this.panelSettings.Controls.Add(this.buttonMaxColor);
            this.panelSettings.Controls.Add(this.labelOffset);
            this.panelSettings.Controls.Add(this.trackBarMidpoint);
            this.panelSettings.Controls.Add(this.trackBarSharpness);
            this.panelSettings.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelSettings.Location = new System.Drawing.Point(844, 0);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Size = new System.Drawing.Size(309, 803);
            this.panelSettings.TabIndex = 42;
            // 
            // comboBoxShowPaths
            // 
            this.comboBoxShowPaths.FormattingEnabled = true;
            this.comboBoxShowPaths.Items.AddRange(new object[] {
            "Selected Paths",
            "All Paths"});
            this.comboBoxShowPaths.Location = new System.Drawing.Point(165, 658);
            this.comboBoxShowPaths.Name = "comboBoxShowPaths";
            this.comboBoxShowPaths.Size = new System.Drawing.Size(121, 24);
            this.comboBoxShowPaths.TabIndex = 62;
            this.comboBoxShowPaths.SelectedIndexChanged += new System.EventHandler(this.comboBoxShowPaths_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(162, 585);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 17);
            this.label5.TabIndex = 61;
            this.label5.Text = "Draw";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 585);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 17);
            this.label4.TabIndex = 60;
            this.label4.Text = "Heatmap Type";
            // 
            // labelBgColor
            // 
            this.labelBgColor.AutoSize = true;
            this.labelBgColor.Location = new System.Drawing.Point(34, 296);
            this.labelBgColor.Name = "labelBgColor";
            this.labelBgColor.Size = new System.Drawing.Size(121, 17);
            this.labelBgColor.TabIndex = 59;
            this.labelBgColor.Text = "Background Color";
            // 
            // labelMin
            // 
            this.labelMin.AutoSize = true;
            this.labelMin.Location = new System.Drawing.Point(72, 262);
            this.labelMin.Name = "labelMin";
            this.labelMin.Size = new System.Drawing.Size(30, 17);
            this.labelMin.TabIndex = 58;
            this.labelMin.Text = "Min";
            // 
            // labelMax
            // 
            this.labelMax.AutoSize = true;
            this.labelMax.Location = new System.Drawing.Point(72, 84);
            this.labelMax.Name = "labelMax";
            this.labelMax.Size = new System.Drawing.Size(33, 17);
            this.labelMax.TabIndex = 57;
            this.labelMax.Text = "Max";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_heatmap});
            this.statusStrip1.Location = new System.Drawing.Point(0, 779);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
            this.statusStrip1.Size = new System.Drawing.Size(309, 24);
            this.statusStrip1.TabIndex = 56;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel_heatmap
            // 
            this.toolStripStatusLabel_heatmap.Name = "toolStripStatusLabel_heatmap";
            this.toolStripStatusLabel_heatmap.Size = new System.Drawing.Size(0, 18);
            // 
            // buttonCopy
            // 
            this.buttonCopy.Location = new System.Drawing.Point(206, 706);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(91, 46);
            this.buttonCopy.TabIndex = 55;
            this.buttonCopy.Text = "Copy To Clipboard";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // labelSharpness
            // 
            this.labelSharpness.AutoSize = true;
            this.labelSharpness.Location = new System.Drawing.Point(10, 470);
            this.labelSharpness.Name = "labelSharpness";
            this.labelSharpness.Size = new System.Drawing.Size(76, 17);
            this.labelSharpness.TabIndex = 54;
            this.labelSharpness.Text = "Sharpness";
            // 
            // labelSharpnessVal
            // 
            this.labelSharpnessVal.AutoSize = true;
            this.labelSharpnessVal.Location = new System.Drawing.Point(219, 470);
            this.labelSharpnessVal.Name = "labelSharpnessVal";
            this.labelSharpnessVal.Size = new System.Drawing.Size(0, 17);
            this.labelSharpnessVal.TabIndex = 53;
            // 
            // buttonAutoRes
            // 
            this.buttonAutoRes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAutoRes.Location = new System.Drawing.Point(268, 370);
            this.buttonAutoRes.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAutoRes.Name = "buttonAutoRes";
            this.buttonAutoRes.Size = new System.Drawing.Size(28, 24);
            this.buttonAutoRes.TabIndex = 45;
            this.buttonAutoRes.Text = "¤";
            this.buttonAutoRes.UseVisualStyleBackColor = true;
            this.buttonAutoRes.Click += new System.EventHandler(this.buttonAutoRes_Click);
            // 
            // buttonOffset
            // 
            this.buttonOffset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonOffset.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOffset.Image = global::MazeAnalyzer.Properties.Resources.target1;
            this.buttonOffset.Location = new System.Drawing.Point(204, 398);
            this.buttonOffset.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOffset.Name = "buttonOffset";
            this.buttonOffset.Size = new System.Drawing.Size(28, 24);
            this.buttonOffset.TabIndex = 51;
            this.buttonOffset.UseVisualStyleBackColor = true;
            this.buttonOffset.Click += new System.EventHandler(this.buttonOffset_Click);
            // 
            // labelOpacityPercent
            // 
            this.labelOpacityPercent.AutoSize = true;
            this.labelOpacityPercent.Location = new System.Drawing.Point(219, 509);
            this.labelOpacityPercent.Name = "labelOpacityPercent";
            this.labelOpacityPercent.Size = new System.Drawing.Size(0, 17);
            this.labelOpacityPercent.TabIndex = 50;
            // 
            // trackBarSharpness
            // 
            this.trackBarSharpness.Location = new System.Drawing.Point(92, 463);
            this.trackBarSharpness.Name = "trackBarSharpness";
            this.trackBarSharpness.Size = new System.Drawing.Size(121, 56);
            this.trackBarSharpness.TabIndex = 52;
            this.trackBarSharpness.Scroll += new System.EventHandler(this.trackBarSharpness_Scroll);
            // 
            // labelScroll
            // 
            this.labelScroll.BackColor = System.Drawing.SystemColors.Control;
            this.labelScroll.Location = new System.Drawing.Point(0, 0);
            this.labelScroll.Name = "labelScroll";
            this.labelScroll.Size = new System.Drawing.Size(31, 21);
            this.labelScroll.TabIndex = 43;
            // 
            // pictureBoxHeatmap
            // 
            this.pictureBoxHeatmap.BackColor = System.Drawing.Color.White;
            this.pictureBoxHeatmap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxHeatmap.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxHeatmap.Name = "pictureBoxHeatmap";
            this.pictureBoxHeatmap.Size = new System.Drawing.Size(841, 803);
            this.pictureBoxHeatmap.TabIndex = 43;
            this.pictureBoxHeatmap.TabStop = false;
            this.pictureBoxHeatmap.Click += new System.EventHandler(this.pictureBoxHeatmap_Click);
            this.pictureBoxHeatmap.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxHeatmap_Paint);
            this.pictureBoxHeatmap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxHeatmap_MouseDown);
            this.pictureBoxHeatmap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxHeatmap_MouseMove);
            this.pictureBoxHeatmap.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxHeatmap_MouseUp);
            // 
            // panelHeatmap
            // 
            this.panelHeatmap.AutoScroll = true;
            this.panelHeatmap.BackColor = System.Drawing.Color.White;
            this.panelHeatmap.Controls.Add(this.pictureBoxHeatmap);
            this.panelHeatmap.Controls.Add(this.labelScroll);
            this.panelHeatmap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHeatmap.Location = new System.Drawing.Point(3, 0);
            this.panelHeatmap.Name = "panelHeatmap";
            this.panelHeatmap.Size = new System.Drawing.Size(841, 803);
            this.panelHeatmap.TabIndex = 44;
            this.panelHeatmap.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panelHeatmap_Scroll);
            this.panelHeatmap.Resize += new System.EventHandler(this.panelHeatmap_Resize);
            // 
            // Heatmap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1153, 803);
            this.Controls.Add(this.panelHeatmap);
            this.Controls.Add(this.panelSettings);
            this.Controls.Add(this.splitter1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(429, 829);
            this.Name = "Heatmap";
            this.Text = "Heatmap";
            this.Load += new System.EventHandler(this.HeatmapConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMidpoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOpacity)).EndInit();
            this.panelSettings.ResumeLayout(false);
            this.panelSettings.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSharpness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHeatmap)).EndInit();
            this.panelHeatmap.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxRes;
        private System.Windows.Forms.Button buttonMaxColor;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.Button buttonMinColor;
        private System.Windows.Forms.Button buttonBgColor;
        private System.Windows.Forms.Button buttonMidColor;
        private System.Windows.Forms.CheckBox checkBoxShowBg;
        private System.Windows.Forms.CheckBox checkBoxShowMidColor;
        private System.Windows.Forms.TrackBar trackBarMidpoint;
        private System.Windows.Forms.ComboBox comboBoxInterpolation;
        private System.Windows.Forms.Label labelRes;
        private System.Windows.Forms.Label labelShowInterpolation;
        private System.Windows.Forms.Button buttonSavePng;
        private System.Windows.Forms.TrackBar trackBarOpacity;
        private System.Windows.Forms.Label labelOpacity;
        private System.Windows.Forms.TextBox textBoxOffset;
        private System.Windows.Forms.Label labelOffset;
        private System.Windows.Forms.ComboBox comboBoxColorPreset;
        private System.Windows.Forms.Label labelColorPreset;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Button buttonSaveCsv;
        private System.Windows.Forms.RadioButton radioButtonPres;
        private System.Windows.Forms.RadioButton radioButtonTime;
        private System.Windows.Forms.RadioButton radioButtonEntr;
        private System.Windows.Forms.CheckBox checkBoxShowAnaRegns;
        private System.Windows.Forms.CheckBox checkBoxShowMaze;
        private System.Windows.Forms.Label labelDrawSettings;
        private System.Windows.Forms.Label labelQualitySettings;
        private System.Windows.Forms.Label labelColorSettings;
        private System.Windows.Forms.Panel panelSettings;
        private System.Windows.Forms.Label labelScroll;
        private System.Windows.Forms.PictureBox pictureBoxHeatmap;
        private System.Windows.Forms.Panel panelHeatmap;
        private System.Windows.Forms.Label labelOpacityPercent;
        private System.Windows.Forms.Button buttonOffset;
        private System.Windows.Forms.Button buttonAutoRes;
        private System.Windows.Forms.Label labelSharpness;
        private System.Windows.Forms.Label labelSharpnessVal;
        private System.Windows.Forms.TrackBar trackBarSharpness;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_heatmap;
        private System.Windows.Forms.Label labelBgColor;
        private System.Windows.Forms.Label labelMin;
        private System.Windows.Forms.Label labelMax;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxShowPaths;
    }
}