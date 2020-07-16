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
            this.textBoxRes = new System.Windows.Forms.TextBox();
            this.buttonMaxColor = new System.Windows.Forms.Button();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.buttonMinColor = new System.Windows.Forms.Button();
            this.buttonBgColor = new System.Windows.Forms.Button();
            this.buttonMidColor = new System.Windows.Forms.Button();
            this.checkBoxShowTransparentBg = new System.Windows.Forms.CheckBox();
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
            this.comboBoxPreset = new System.Windows.Forms.ComboBox();
            this.labelPreset = new System.Windows.Forms.Label();
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
            this.buttonCopy = new System.Windows.Forms.Button();
            this.labelSharpness = new System.Windows.Forms.Label();
            this.labelSharpnessVal = new System.Windows.Forms.Label();
            this.trackBarSharpness = new System.Windows.Forms.TrackBar();
            this.buttonAutoRes = new System.Windows.Forms.Button();
            this.buttonOffset = new System.Windows.Forms.Button();
            this.labelOpacityPercent = new System.Windows.Forms.Label();
            this.labelScroll = new System.Windows.Forms.Label();
            this.pictureBoxHeatmap = new System.Windows.Forms.PictureBox();
            this.panelHeatmap = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMidpoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOpacity)).BeginInit();
            this.panelSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSharpness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHeatmap)).BeginInit();
            this.panelHeatmap.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxRes
            // 
            this.textBoxRes.Location = new System.Drawing.Point(216, 455);
            this.textBoxRes.Name = "textBoxRes";
            this.textBoxRes.Size = new System.Drawing.Size(57, 22);
            this.textBoxRes.TabIndex = 1;
            this.textBoxRes.TextChanged += new System.EventHandler(this.textBoxRes_TextChanged);
            this.textBoxRes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxRes_KeyDown);
            // 
            // buttonMaxColor
            // 
            this.buttonMaxColor.Location = new System.Drawing.Point(67, 81);
            this.buttonMaxColor.Name = "buttonMaxColor";
            this.buttonMaxColor.Size = new System.Drawing.Size(84, 23);
            this.buttonMaxColor.TabIndex = 2;
            this.buttonMaxColor.Text = "Max Color";
            this.buttonMaxColor.UseVisualStyleBackColor = true;
            this.buttonMaxColor.Click += new System.EventHandler(this.buttonMaxColor_Click);
            // 
            // buttonMinColor
            // 
            this.buttonMinColor.Location = new System.Drawing.Point(67, 260);
            this.buttonMinColor.Name = "buttonMinColor";
            this.buttonMinColor.Size = new System.Drawing.Size(84, 23);
            this.buttonMinColor.TabIndex = 3;
            this.buttonMinColor.Text = "Min Color";
            this.buttonMinColor.UseVisualStyleBackColor = true;
            this.buttonMinColor.Click += new System.EventHandler(this.buttonMinColor_Click);
            // 
            // buttonBgColor
            // 
            this.buttonBgColor.Location = new System.Drawing.Point(15, 376);
            this.buttonBgColor.Name = "buttonBgColor";
            this.buttonBgColor.Size = new System.Drawing.Size(143, 23);
            this.buttonBgColor.TabIndex = 4;
            this.buttonBgColor.Text = "Background Color";
            this.buttonBgColor.UseVisualStyleBackColor = true;
            this.buttonBgColor.Click += new System.EventHandler(this.buttonBgColor_Click);
            // 
            // buttonMidColor
            // 
            this.buttonMidColor.Location = new System.Drawing.Point(67, 181);
            this.buttonMidColor.Name = "buttonMidColor";
            this.buttonMidColor.Size = new System.Drawing.Size(84, 23);
            this.buttonMidColor.TabIndex = 10;
            this.buttonMidColor.Text = "Mid Color";
            this.buttonMidColor.UseVisualStyleBackColor = true;
            this.buttonMidColor.Click += new System.EventHandler(this.buttonMidColor_Click);
            // 
            // checkBoxShowTransparentBg
            // 
            this.checkBoxShowTransparentBg.AutoSize = true;
            this.checkBoxShowTransparentBg.Location = new System.Drawing.Point(15, 349);
            this.checkBoxShowTransparentBg.Name = "checkBoxShowTransparentBg";
            this.checkBoxShowTransparentBg.Size = new System.Drawing.Size(188, 21);
            this.checkBoxShowTransparentBg.TabIndex = 17;
            this.checkBoxShowTransparentBg.Text = "Transparent Background";
            this.checkBoxShowTransparentBg.UseVisualStyleBackColor = true;
            this.checkBoxShowTransparentBg.Click += new System.EventHandler(this.checkBoxShowBgTransparent_Click);
            // 
            // checkBoxShowMidColor
            // 
            this.checkBoxShowMidColor.AutoSize = true;
            this.checkBoxShowMidColor.Location = new System.Drawing.Point(67, 154);
            this.checkBoxShowMidColor.Name = "checkBoxShowMidColor";
            this.checkBoxShowMidColor.Size = new System.Drawing.Size(89, 21);
            this.checkBoxShowMidColor.TabIndex = 18;
            this.checkBoxShowMidColor.Text = "Mid Color";
            this.checkBoxShowMidColor.UseVisualStyleBackColor = true;
            this.checkBoxShowMidColor.Click += new System.EventHandler(this.checkBoxShowMidColor_Click);
            // 
            // trackBarMidpoint
            // 
            this.trackBarMidpoint.Location = new System.Drawing.Point(15, 81);
            this.trackBarMidpoint.Maximum = 9;
            this.trackBarMidpoint.Name = "trackBarMidpoint";
            this.trackBarMidpoint.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarMidpoint.Size = new System.Drawing.Size(56, 202);
            this.trackBarMidpoint.TabIndex = 19;
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
            this.comboBoxInterpolation.Location = new System.Drawing.Point(104, 517);
            this.comboBoxInterpolation.Name = "comboBoxInterpolation";
            this.comboBoxInterpolation.Size = new System.Drawing.Size(169, 24);
            this.comboBoxInterpolation.TabIndex = 21;
            this.comboBoxInterpolation.SelectedIndexChanged += new System.EventHandler(this.comboBoxInterpolation_SelectedIndexChanged);
            // 
            // labelRes
            // 
            this.labelRes.AutoSize = true;
            this.labelRes.Location = new System.Drawing.Point(12, 455);
            this.labelRes.Name = "labelRes";
            this.labelRes.Size = new System.Drawing.Size(198, 17);
            this.labelRes.TabIndex = 22;
            this.labelRes.Text = "Resolution [Maze Units / Pixel]";
            // 
            // labelShowInterpolation
            // 
            this.labelShowInterpolation.AutoSize = true;
            this.labelShowInterpolation.Location = new System.Drawing.Point(12, 517);
            this.labelShowInterpolation.Name = "labelShowInterpolation";
            this.labelShowInterpolation.Size = new System.Drawing.Size(86, 17);
            this.labelShowInterpolation.TabIndex = 24;
            this.labelShowInterpolation.Text = "Interpolation";
            // 
            // buttonSavePng
            // 
            this.buttonSavePng.Location = new System.Drawing.Point(107, 718);
            this.buttonSavePng.Name = "buttonSavePng";
            this.buttonSavePng.Size = new System.Drawing.Size(93, 23);
            this.buttonSavePng.TabIndex = 26;
            this.buttonSavePng.Text = "Save PNG";
            this.buttonSavePng.UseVisualStyleBackColor = true;
            this.buttonSavePng.Click += new System.EventHandler(this.buttonSavePng_Click);
            // 
            // trackBarOpacity
            // 
            this.trackBarOpacity.Location = new System.Drawing.Point(67, 314);
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
            this.labelOpacity.Location = new System.Drawing.Point(5, 314);
            this.labelOpacity.Name = "labelOpacity";
            this.labelOpacity.Size = new System.Drawing.Size(56, 17);
            this.labelOpacity.TabIndex = 29;
            this.labelOpacity.Text = "Opacity";
            // 
            // textBoxOffset
            // 
            this.textBoxOffset.Location = new System.Drawing.Point(99, 484);
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
            this.labelOffset.Location = new System.Drawing.Point(12, 484);
            this.labelOffset.Name = "labelOffset";
            this.labelOffset.Size = new System.Drawing.Size(81, 17);
            this.labelOffset.TabIndex = 36;
            this.labelOffset.Text = "Offset (x, z)";
            this.labelOffset.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBoxPreset
            // 
            this.comboBoxPreset.FormattingEnabled = true;
            this.comboBoxPreset.Items.AddRange(new object[] {
            "cool scale",
            "hot scale",
            "gray scale",
            "custom"});
            this.comboBoxPreset.Location = new System.Drawing.Point(67, 40);
            this.comboBoxPreset.Name = "comboBoxPreset";
            this.comboBoxPreset.Size = new System.Drawing.Size(121, 24);
            this.comboBoxPreset.TabIndex = 38;
            this.comboBoxPreset.SelectedIndexChanged += new System.EventHandler(this.comboBoxPreset_SelectedIndexChanged);
            // 
            // labelPreset
            // 
            this.labelPreset.AutoSize = true;
            this.labelPreset.Location = new System.Drawing.Point(12, 43);
            this.labelPreset.Name = "labelPreset";
            this.labelPreset.Size = new System.Drawing.Size(49, 17);
            this.labelPreset.TabIndex = 39;
            this.labelPreset.Text = "Preset";
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 767);
            this.splitter1.TabIndex = 40;
            this.splitter1.TabStop = false;
            // 
            // labelDrawSettings
            // 
            this.labelDrawSettings.AutoSize = true;
            this.labelDrawSettings.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.labelDrawSettings.Location = new System.Drawing.Point(12, 607);
            this.labelDrawSettings.Name = "labelDrawSettings";
            this.labelDrawSettings.Size = new System.Drawing.Size(95, 17);
            this.labelDrawSettings.TabIndex = 49;
            this.labelDrawSettings.Text = "Draw Settings";
            // 
            // labelQualitySettings
            // 
            this.labelQualitySettings.AutoSize = true;
            this.labelQualitySettings.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.labelQualitySettings.Location = new System.Drawing.Point(12, 426);
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
            this.checkBoxShowAnaRegns.Location = new System.Drawing.Point(107, 665);
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
            this.checkBoxShowMaze.Location = new System.Drawing.Point(107, 638);
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
            this.radioButtonTime.Location = new System.Drawing.Point(12, 691);
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
            this.radioButtonEntr.Location = new System.Drawing.Point(12, 664);
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
            this.radioButtonPres.Location = new System.Drawing.Point(12, 637);
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
            this.buttonSaveCsv.Location = new System.Drawing.Point(8, 718);
            this.buttonSaveCsv.Name = "buttonSaveCsv";
            this.buttonSaveCsv.Size = new System.Drawing.Size(93, 23);
            this.buttonSaveCsv.TabIndex = 40;
            this.buttonSaveCsv.Text = "Save CSV";
            this.buttonSaveCsv.UseVisualStyleBackColor = true;
            this.buttonSaveCsv.Click += new System.EventHandler(this.buttonSaveCsv_Click);
            // 
            // panelSettings
            // 
            this.panelSettings.BackColor = System.Drawing.Color.White;
            this.panelSettings.Controls.Add(this.buttonCopy);
            this.panelSettings.Controls.Add(this.labelSharpness);
            this.panelSettings.Controls.Add(this.labelSharpnessVal);
            this.panelSettings.Controls.Add(this.trackBarSharpness);
            this.panelSettings.Controls.Add(this.buttonAutoRes);
            this.panelSettings.Controls.Add(this.buttonOffset);
            this.panelSettings.Controls.Add(this.labelOpacityPercent);
            this.panelSettings.Controls.Add(this.checkBoxShowTransparentBg);
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
            this.panelSettings.Controls.Add(this.labelPreset);
            this.panelSettings.Controls.Add(this.buttonMidColor);
            this.panelSettings.Controls.Add(this.textBoxRes);
            this.panelSettings.Controls.Add(this.textBoxOffset);
            this.panelSettings.Controls.Add(this.comboBoxPreset);
            this.panelSettings.Controls.Add(this.buttonMinColor);
            this.panelSettings.Controls.Add(this.buttonMaxColor);
            this.panelSettings.Controls.Add(this.labelOffset);
            this.panelSettings.Controls.Add(this.trackBarMidpoint);
            this.panelSettings.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelSettings.Location = new System.Drawing.Point(844, 0);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Size = new System.Drawing.Size(309, 767);
            this.panelSettings.TabIndex = 42;
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
            this.labelSharpness.Location = new System.Drawing.Point(12, 554);
            this.labelSharpness.Name = "labelSharpness";
            this.labelSharpness.Size = new System.Drawing.Size(76, 17);
            this.labelSharpness.TabIndex = 54;
            this.labelSharpness.Text = "Sharpness";
            // 
            // labelSharpnessVal
            // 
            this.labelSharpnessVal.AutoSize = true;
            this.labelSharpnessVal.Location = new System.Drawing.Point(221, 554);
            this.labelSharpnessVal.Name = "labelSharpnessVal";
            this.labelSharpnessVal.Size = new System.Drawing.Size(0, 17);
            this.labelSharpnessVal.TabIndex = 53;
            // 
            // trackBarSharpness
            // 
            this.trackBarSharpness.Location = new System.Drawing.Point(94, 547);
            this.trackBarSharpness.Name = "trackBarSharpness";
            this.trackBarSharpness.Size = new System.Drawing.Size(121, 56);
            this.trackBarSharpness.TabIndex = 52;
            this.trackBarSharpness.Scroll += new System.EventHandler(this.trackBarSharpness_Scroll);
            // 
            // buttonAutoRes
            // 
            this.buttonAutoRes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAutoRes.Location = new System.Drawing.Point(276, 455);
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
            this.buttonOffset.Location = new System.Drawing.Point(206, 482);
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
            this.labelOpacityPercent.Location = new System.Drawing.Point(194, 314);
            this.labelOpacityPercent.Name = "labelOpacityPercent";
            this.labelOpacityPercent.Size = new System.Drawing.Size(0, 17);
            this.labelOpacityPercent.TabIndex = 50;
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
            this.pictureBoxHeatmap.Size = new System.Drawing.Size(841, 767);
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
            this.panelHeatmap.Size = new System.Drawing.Size(841, 767);
            this.panelHeatmap.TabIndex = 44;
            this.panelHeatmap.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panelHeatmap_Scroll);
            this.panelHeatmap.Resize += new System.EventHandler(this.panelHeatmap_Resize);
            // 
            // Heatmap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1153, 767);
            this.Controls.Add(this.panelHeatmap);
            this.Controls.Add(this.panelSettings);
            this.Controls.Add(this.splitter1);
            this.MinimumSize = new System.Drawing.Size(429, 807);
            this.Name = "Heatmap";
            this.Text = "Heatmap";
            this.Load += new System.EventHandler(this.HeatmapConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMidpoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOpacity)).EndInit();
            this.panelSettings.ResumeLayout(false);
            this.panelSettings.PerformLayout();
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
        private System.Windows.Forms.CheckBox checkBoxShowTransparentBg;
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
        private System.Windows.Forms.ComboBox comboBoxPreset;
        private System.Windows.Forms.Label labelPreset;
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
    }
}