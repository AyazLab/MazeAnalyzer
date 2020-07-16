using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

using MazeLib;

namespace MazeAnalyzer
{
    public partial class Heatmap : Form
    {
        // effected by preset
        static Color cusMinColor = Color.FromArgb(128, 255, 128);
        static Color cusMaxColor = Color.FromArgb(128, 0, 255);

        static bool showCusMidColor = true;
        static bool showMidColor;
        static Color cusMidColor = Color.FromArgb(128, 255, 255);

        static bool showCusTransparentBg = true;
        static bool showTransparentBg;
        static Color cusBgColor = Color.White;

        public enum Preset
        {
            Cool, Hot, Gray, Custom
        }
        static Preset preset = Preset.Cool;
        static double midpoint = 0.1;
        static int opacity = 75;
        static int heatmapAlpha = (int)(opacity / 100.0 * 255);
        static int sharpness = 0;

        public enum ClickMode
        {
            None, Offset, ResolutionBox
        }
        ClickMode clickMode = ClickMode.None;
        public enum InterpolationModes
        {
            None, Bilinear, HighQualityBilinear, Bicubic, HighQualityBicubic
        }
        static InterpolationModes interpolation = InterpolationModes.None;
        readonly MazeViewer mv;

        static string heatmapTypeStr;
        static HeatmapItem selectedHeatmap = new HeatmapItem();
        static double minHeatVal;
        static double maxHeatVal;
        string heatmapUnits;
        
        static bool showMaze = true;
        static bool showAnaRegns = false;

        static string csvPaths; // the selected paths for the csv

        public Heatmap(MazeViewer mv)
        {
            InitializeComponent();

            this.mv = mv;
            BuildHeatmap(this.mv);

            DoubleBuffered = true;
        }

        private void HeatmapConfig_Load(object sender, EventArgs e)
        // syncs internal and external settings
        {
            comboBoxPreset.SelectedIndex = (int)preset; // calls SetPreset
            trackBarMidpoint.Value = (int)(midpoint * 10);
            trackBarOpacity.Value = opacity;
            heatmapAlpha = (int)(opacity / 100.0 * 255);
            labelOpacityPercent.Text = string.Format("{0}%", opacity);


            textBoxRes.Text = Convert.ToString(selectedHeatmap.res); // changes textBoxRes.BackColor to white
            textBoxRes.BackColor = SystemColors.Control;

            double offsetX = Math.Round(selectedHeatmap.offsetX, 2, MidpointRounding.AwayFromZero);
            double offsetZ = Math.Round(selectedHeatmap.offsetZ, 2, MidpointRounding.AwayFromZero);
            textBoxOffset.Text = string.Format("{0}, {1}", offsetX, offsetZ);
            textBoxOffset.BackColor = SystemColors.Control;
            buttonOffset.Image = new Bitmap(buttonOffset.Image, 12, 12);

            comboBoxInterpolation.SelectedIndex = (int)interpolation;

            trackBarSharpness.Value = sharpness;
            labelSharpnessVal.Text = string.Format("{0}", sharpness);


            if (selectedHeatmap.type == HeatmapItem.Type.Presence)
            {
                radioButtonPres.Checked = true;
            }
            else if (selectedHeatmap.type == HeatmapItem.Type.Entrance)
            {
                radioButtonEntr.Checked = true;
            }
            else if (selectedHeatmap.type == HeatmapItem.Type.Time)
            {
                radioButtonTime.Checked = true;
            }
            checkBoxShowMaze.Checked = showMaze;
            checkBoxShowAnaRegns.Checked = showAnaRegns;
        }


        // color settings
        private void buttonMinColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                preset = Preset.Custom;
                comboBoxPreset.SelectedIndex = (int)preset; // calls SetPreset
                cusMinColor = colorDialog.Color;

                buttonMinColor.BackColor = cusMinColor;
                Refresh();
            }
        }

        private void buttonMaxColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                preset = Preset.Custom;
                comboBoxPreset.SelectedIndex = (int)preset;
                cusMaxColor = colorDialog.Color;

                buttonMaxColor.BackColor = cusMaxColor;
                Refresh();
            }
        }

        private void checkBoxShowMidColor_Click(object sender, EventArgs e)
        {
            preset = Preset.Custom;
            comboBoxPreset.SelectedIndex = (int)preset;
            showMidColor = !showMidColor;
            checkBoxShowMidColor.Checked = showMidColor;
            showCusMidColor = showMidColor;

            Refresh();
        }

        private void buttonMidColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                preset = Preset.Custom;
                comboBoxPreset.SelectedIndex = (int)preset;
                cusMidColor = colorDialog.Color;

                buttonMidColor.BackColor = cusMidColor;
                Refresh();
            }
        }

        private void checkBoxShowBgTransparent_Click(object sender, EventArgs e)
        {
            preset = Preset.Custom;
            comboBoxPreset.SelectedIndex = (int)preset;
            showTransparentBg = !showTransparentBg;
            checkBoxShowTransparentBg.Checked = showTransparentBg;
            showCusTransparentBg = showTransparentBg;

            Refresh();
        }

        private void buttonBgColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                preset = Preset.Custom;
                comboBoxPreset.SelectedIndex = (int)preset;
                cusBgColor = colorDialog.Color;

                buttonBgColor.BackColor = cusBgColor;
                Refresh();
            }
        }

        private void comboBoxPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPreset((Preset)comboBoxPreset.SelectedIndex);

            Refresh();
        }

        private void SetPreset(Preset newPreset)
        {
            switch (newPreset)
            {
                case Preset.Cool:
                    buttonMinColor.BackColor = Color.FromArgb(128, 255, 128);
                    buttonMidColor.BackColor = Color.FromArgb(128, 255, 255);
                    buttonMaxColor.BackColor = Color.FromArgb(128, 0, 255);
                    break;

                case Preset.Hot:
                    buttonMinColor.BackColor = Color.White;
                    buttonMidColor.BackColor = Color.FromArgb(255, 255, 128);
                    buttonMaxColor.BackColor = Color.FromArgb(255, 128, 128);
                    break;

                case Preset.Gray:
                    buttonMinColor.BackColor = Color.FromArgb(128, 128, 128);
                    buttonMidColor.BackColor = Color.FromArgb(192, 192, 192);
                    buttonMaxColor.BackColor = Color.White;
                    break;

                case Preset.Custom:
                    buttonMinColor.BackColor = cusMinColor;
                    buttonMaxColor.BackColor = cusMaxColor;

                    checkBoxShowMidColor.Checked = showCusMidColor;
                    buttonMidColor.BackColor = cusMidColor;

                    checkBoxShowTransparentBg.Checked = showCusTransparentBg;
                    buttonBgColor.BackColor = cusBgColor;
                    break;

                default:
                    break;
            }

            if (newPreset != Preset.Custom)
            {
                showMidColor = true;
                checkBoxShowMidColor.Checked = showMidColor;
                showTransparentBg = true;
                checkBoxShowTransparentBg.Checked = showTransparentBg;
                buttonBgColor.BackColor = Color.White;
            }
            
            preset = newPreset;
        }

        private void trackBarMidpoint_Scroll(object sender, EventArgs e)
        {
            midpoint = (double)trackBarMidpoint.Value / 10;

            if (checkBoxShowMidColor.Checked)
            {
                Refresh();
            }
            else
            {
                checkBoxShowMidColor_Click(sender, e);
            }
        }

        private void trackBarOpacity_Scroll(object sender, EventArgs e)
        {
            opacity = trackBarOpacity.Value;
            heatmapAlpha = (int)(opacity / 100.0 * 255);

            labelOpacityPercent.Text = string.Format("{0}%", opacity);
            Refresh();
        }


        // quality settings [resolution, offset, interpolation, & sharpness]
        private void textBoxRes_KeyDown(object sender, KeyEventArgs e)
        // 0.2 - 15 maze coord / pixel recommended
        {
            if (e.KeyCode == Keys.Enter)
            {
                mv.SetHeatmapRes(Convert.ToDouble(textBoxRes.Text));

                textBoxRes.BackColor = SystemColors.Control;
                RebuildHeatmap();
            }
        }

        private void textBoxRes_TextChanged(object sender, EventArgs e)
        {
            textBoxRes.BackColor = Color.White;
        }

        private void textBoxOffset_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetOffset();
            }

            textBoxOffset.BackColor = SystemColors.Control;
        }

        private void textBoxOffset_TextChanged(object sender, EventArgs e)
        {
            textBoxOffset.BackColor = Color.White;
        }

        void SetOffset()
        // called when textBoxOffset is changed or after an active ClickMode
        {
            string newOffSetX = textBoxOffset.Text.Split(new String[] { ", " }, StringSplitOptions.None)[0];
            string newOffSetZ = textBoxOffset.Text.Split(new String[] { ", " }, StringSplitOptions.None)[1];
            string offsetX = Convert.ToString(Math.Round(selectedHeatmap.offsetX, 2, MidpointRounding.AwayFromZero));
            string offsetZ = Convert.ToString(Math.Round(selectedHeatmap.offsetZ, 2, MidpointRounding.AwayFromZero));

            if (newOffSetX != offsetX || newOffSetZ != offsetZ)
            {
                mv.SetHeatmapOffset(Convert.ToDouble(newOffSetX), Convert.ToDouble(newOffSetZ));
                RebuildHeatmap();
            }
        }

        void RebuildHeatmap()
        // call when the resolution or offset is changed
        {
            BuildHeatmap(mv);

            Refresh();
        }

        private void comboBoxInterpolation_SelectedIndexChanged(object sender, EventArgs e)
        {
            interpolation = (InterpolationModes)comboBoxInterpolation.SelectedIndex;

            Refresh();
        }

        private void trackBarSharpness_Scroll(object sender, EventArgs e)
        {
            sharpness = trackBarSharpness.Value;

            labelSharpnessVal.Text = string.Format("{0}", sharpness);
            Refresh();
        }

        Point mouseCoord1;
        Point mouseCoord2;
        private void buttonAutoRes_Click(object sender, EventArgs e)
        // auto resolution feature
        {
            if (clickMode == ClickMode.ResolutionBox)
            {
                clickMode = ClickMode.None;
                Cursor = Cursors.Default;
            }
            else
            {
                clickMode = ClickMode.ResolutionBox;
                Cursor = Cursors.Cross;
            }
        }

        private void pictureBoxHeatmap_MouseDown(object sender, MouseEventArgs e)
        // auto resolution feature
        {
            MouseEventArgs mouseCoord = e;

            if (clickMode == ClickMode.ResolutionBox)
            {
                mouseCoord1 = new Point(mouseCoord.X, mouseCoord.Y - panelHeatmap.AutoScrollPosition.Y);
            }
        }

        bool showResBox = false;
        private void pictureBoxHeatmap_MouseMove(object sender, MouseEventArgs e)
        // auto resolution feature
        {
            MouseEventArgs mouseCoord = e;

            if (clickMode == ClickMode.ResolutionBox)
            {
                if (e.Button == MouseButtons.Left)
                {
                    mouseCoord2 = new Point(mouseCoord.X, mouseCoord.Y - panelHeatmap.AutoScrollPosition.Y);

                    showResBox = true;
                    Refresh();
                }
            }
        }

        private void pictureBoxHeatmap_MouseUp(object sender, MouseEventArgs e)
        // auto resolution feature
        {
            MouseEventArgs mouseCoord = e;

            if (clickMode == ClickMode.ResolutionBox)
            {
                mouseCoord2 = new Point(mouseCoord.X, mouseCoord.Y);
                PointF mazeCoord1 = MouseToMazeCoord(mouseCoord1.X, mouseCoord1.Y);
                PointF mazeCoord2 = MouseToMazeCoord(mouseCoord2.X, mouseCoord2.Y);

                selectedHeatmap.res = Math.Round(Math.Max(Math.Abs(mazeCoord1.X - mazeCoord2.X), Math.Abs(mazeCoord1.Y - mazeCoord2.Y)), 2);
                textBoxRes.Text = Convert.ToString(selectedHeatmap.res);
                textBoxOffset.Text = string.Format("{0}, {1}", mazeCoord1.X, mazeCoord2.Y);
                showResBox = false;
                clickMode = ClickMode.None;

                textBoxRes.BackColor = SystemColors.Control;
                textBoxOffset.BackColor = SystemColors.Control;
                Cursor = Cursors.Default;
                SetOffset(); // SetOffset calls RebuildHeatmap, which calls Refresh
            }
        }

        private void buttonOffset_Click(object sender, EventArgs e)
        // offset click mode feature
        {
            if (clickMode == ClickMode.Offset)
            {
                clickMode = ClickMode.None;
                Cursor = Cursors.Default;
            }
            else
            {
                clickMode = ClickMode.Offset;
                Cursor = Cursors.Cross;
            }
        }

        private void pictureBoxHeatmap_Click(object sender, EventArgs e)
        // offset click mode feature
        {
            MouseEventArgs mouseCoord = (MouseEventArgs)e;
            PointF mazeCoord = MouseToMazeCoord(mouseCoord.X, mouseCoord.Y);

            if (clickMode == ClickMode.Offset)
            {
                textBoxOffset.Text = string.Format("{0}, {1}", mazeCoord.X, mazeCoord.Y);
                SetOffset();
                clickMode = ClickMode.None;

                textBoxOffset.BackColor = SystemColors.Control;
                Cursor = Cursors.Default;
            }
            else if (clickMode == ClickMode.None)
            {
                ToolTip offset = new ToolTip();

                double heatVal = double.NaN;
                Point heatmapCoord = selectedHeatmap.MazeToHeatmapCoord(mazeCoord.X, mazeCoord.Y);

                if (heatmapCoord.X >= 0 && heatmapCoord.X < selectedHeatmap.xPixels && heatmapCoord.Y >= 0 && heatmapCoord.Y < selectedHeatmap.zPixels)
                {
                    heatVal = Math.Round(selectedHeatmap.val[heatmapCoord.X, heatmapCoord.Y], 2, MidpointRounding.AwayFromZero);
                }

                offset.SetToolTip(pictureBoxHeatmap, string.Format("x:{0}, z:{1}\n{2} {3}", mazeCoord.X, mazeCoord.Y, heatVal, heatmapUnits));
            }
        }

        private PointF MouseToMazeCoord(double x, double z)
        {
            PointF mazeCoord = new PointF();

            double mzTopLeftX = selectedHeatmap.hmXCenter - selectedHeatmap.xBotRadius * selectedHeatmap.res - buffer;
            double mzWidth = selectedHeatmap.xPixels * selectedHeatmap.res + buffer * 2;

            mazeCoord.X = (float)Math.Round(mzTopLeftX + x / mzHmWidth * mzWidth, 2, MidpointRounding.AwayFromZero);

            double mzTopLeftZ = selectedHeatmap.hmZCenter - selectedHeatmap.zBotRadius * selectedHeatmap.res - buffer;
            double mzHeight = selectedHeatmap.zPixels * selectedHeatmap.res + buffer * 2;

            mazeCoord.Y = (float)Math.Round(mzTopLeftZ + (z - panelHeatmap.AutoScrollPosition.Y) / mzHmHeight * mzHeight, 2, MidpointRounding.AwayFromZero);

            return mazeCoord;
        }


        // draw settings [select heatmap type, show maze & analyzer regions]
        public void SetHeatmapType(HeatmapItem.Type newType)
        {
            switch (newType)
            {
                case HeatmapItem.Type.Presence:
                    heatmapTypeStr = "Presence";
                    selectedHeatmap = mv.presHeatmap;
                    heatmapUnits = "% Trials";
                    break;
                case HeatmapItem.Type.Entrance:
                    heatmapTypeStr = "Entrance";
                    selectedHeatmap = mv.entrHeatmap;
                    heatmapUnits = "# Times";
                    break;
                case HeatmapItem.Type.Time:
                    heatmapTypeStr = "Time";
                    selectedHeatmap = mv.timeHeatmap;
                    heatmapUnits = "s";
                    break;
                default:
                    break;
            }
            minHeatVal = selectedHeatmap.GetMin();
            maxHeatVal = selectedHeatmap.GetMax();

            Refresh();
        }

        private void radioButtonPres_CheckedChanged(object sender, EventArgs e)
        {
            SetHeatmapType(HeatmapItem.Type.Presence);
        }
        
        private void radioButtonEntr_CheckedChanged(object sender, EventArgs e)
        {
            SetHeatmapType(HeatmapItem.Type.Entrance);
        }

        private void radioButtonTime_CheckedChanged(object sender, EventArgs e)
        {
            SetHeatmapType(HeatmapItem.Type.Time);
        }

        private void checkBoxShowMaze_Click(object sender, EventArgs e)
        {
            showMaze = !showMaze;

            checkBoxShowMaze.Checked = showMaze;
            Refresh();
        }

        private void checkBoxShowAnalyzerRegions_Click(object sender, EventArgs e)
        {
            showAnaRegns = !showAnaRegns;

            checkBoxShowAnaRegns.Checked = showAnaRegns;
            Refresh();
        }


        // save csv & png, & copy to clipboard
        private void buttonSaveCsv_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV file(*.csv)| *.csv | All Files(*.*) | *.* ";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter csv = new StreamWriter(sfd.FileName);

                csv.Write(string.Format("Maze Analyzer Heatmap Export: {0}\n", heatmapTypeStr));
                csv.Write(string.Format("Date Exported: {0}\n", DateTime.Now));
                csv.Write(string.Format("Project File Name: {0}\n", mv.projectInfo.ProjectFilename));
                csv.Write(string.Format("Project Name: {0}\n", mv.projectInfo.ProjectFilename.Split('\\')[mv.projectInfo.ProjectFilename.Split('\\').Length - 1]));
                csv.Write(string.Format("Maze Name: {0}.maz\n", mv.mazeFileName));
                csv.Write(string.Format("Paths Exported: {0}\n", csvPaths));
                csv.Write(string.Format("Heatmap Resolution: {0} Maze Units / Pixel\n", selectedHeatmap.res));
                csv.Write("\n");

                csv.Write("xz, ");
                string hmXBorders = GetHeatmapBorders(selectedHeatmap.offsetX, selectedHeatmap.xBotRadius, selectedHeatmap.xPixels);
                csv.Write(hmXBorders.Substring(0, hmXBorders.Length - 2));
                csv.Write("\n");

                string[] hmZBorders = GetHeatmapBorders(selectedHeatmap.offsetZ, selectedHeatmap.zBotRadius, selectedHeatmap.zPixels).Split(' ');

                for (int i = 0; i < selectedHeatmap.val.GetLength(1); i++)
                {
                    csv.Write(hmZBorders[i] + " ");
                    for (int j = 0; j < selectedHeatmap.val.GetLength(0); j++)
                    {
                        csv.Write(selectedHeatmap.val[j, i]);

                        if (j != selectedHeatmap.val.GetLength(0) - 1)
                        {
                            csv.Write(", ");
                        }
                    }
                    csv.Write("\n");
                }
                csv.Close();
            }
        }

        string GetHeatmapBorders(double offset, int lowerRadius, int pixels)
        // gets the coordinates borders of all the heatmap pixels for the csv
        {
            string hmBorders = "";

            for (int i = 0; i < pixels + 1; i++)
            {
                double coord = offset + (i - lowerRadius) * selectedHeatmap.res;

                if (coord != offset)
                {
                    hmBorders += string.Format("{0}, ", coord);
                }
            }

            return hmBorders;
        }

        private void buttonSavePng_Click(object sender, EventArgs e)
        {
            bool temp = checkBoxShowTransparentBg.Checked;
            checkBoxShowTransparentBg.Checked = true;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Png Image (.png)|*.png";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = MakePng();

                bmp.Save(sfd.FileName);
            }

            checkBoxShowTransparentBg.Checked = temp;
        }

        Bitmap MakePng()
        {
            int resize = 100;
            int mzHmWidth = (int)(selectedHeatmap.xPixels * selectedHeatmap.res + buffer * 2) * resize;
            int mzHmHeight = (int)(selectedHeatmap.zPixels * selectedHeatmap.res + buffer * 2) * resize;

            int cbWidth = mzHmWidth / 10;
            int cbHeight = mzHmHeight / 4;
            int cbTopLeftX = mzHmWidth;
            int cbTopLeftZ = mzHmHeight / 8;

            Bitmap bmp = new Bitmap(mzHmWidth + cbWidth, mzHmHeight);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                pictureBoxHeatmap_Paint(g, false, mzHmWidth, mzHmHeight, cbWidth, cbHeight, cbTopLeftX, cbTopLeftZ, 170, Color.White);
            }

            return bmp;
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            ToClipboard();
        }

        public void ToClipboard()
        {
            int cbWidth = (int)(trackBarMidpoint.Width * 1.4);
            int cbHeight = trackBarMidpoint.Height;
            int cbTopLeftX = panelSettings.Location.X - cbWidth;
            int cbTopLeftZ = trackBarMidpoint.Location.Y;

            mzHmWidth = panelHeatmap.Width - cbWidth;
            mzHmHeight = (int)((selectedHeatmap.zPixels * selectedHeatmap.res + buffer * 2) / (selectedHeatmap.xPixels * selectedHeatmap.res + buffer * 2) * mzHmWidth);
            scale = 5 + ((double)panelHeatmap.Width - panelSettings.Width) / (1302.0 - panelSettings.Width) * 12;

            Bitmap copy = new Bitmap((int)(mzHmWidth + cbWidth * 0.85), mzHmHeight);
            Graphics g = Graphics.FromImage(copy);

            pictureBoxHeatmap_Paint(g, false, mzHmWidth, mzHmHeight, cbWidth, cbHeight, cbTopLeftX, cbTopLeftZ, 8, Color.Black);

            Clipboard.Clear();
            Clipboard.SetImage(copy.Clone(new Rectangle(0, 0, (int)(mzHmWidth + cbWidth * 0.85), mzHmHeight), copy.PixelFormat));
            copy.Dispose();
        }


        // painting events
        private Rectangle MouseToRect(Point topLeft, Point botRight)
        {
            int top = topLeft.Y < botRight.Y ? topLeft.Y : botRight.Y;
            int bot = topLeft.Y > botRight.Y ? topLeft.Y : botRight.Y;
            int left = topLeft.X < botRight.X ? topLeft.X : botRight.X;
            int right = topLeft.X > botRight.X ? topLeft.X : botRight.X;

            return new Rectangle(left, top, right - left, bot - top);
        }

        private void PaintResBox(Graphics g)
        // paints resolution box
        {
            g.DrawRectangle(new Pen(Brushes.Black, 1F), MouseToRect(mouseCoord1, mouseCoord2));
        }

        void BuildHeatmap(MazeViewer mv)
        // each path has a heatmap, which will be added together such that each maze has a heatmap
        {
            mv.presHeatmap = new HeatmapItem(HeatmapItem.Type.Presence);
            mv.entrHeatmap = new HeatmapItem(HeatmapItem.Type.Entrance);
            mv.timeHeatmap = new HeatmapItem(HeatmapItem.Type.Time);

            foreach (MazePathItem mpi in mv.curMazePaths.cPaths)
            {
                if (mpi.selected)
                {
                    mpi.MakePathHeatmap();

                    mv.presHeatmap += mpi.presHeatmap;
                    mv.entrHeatmap += mpi.entrHeatmap;
                    mv.timeHeatmap += mpi.timeHeatmap;

                    // gets the selected paths for the csv
                    csvPaths += string.Format("\"TRI {0} ", mpi.ExpTrial);
                    if (mpi.ExpGroup != "")
                    {
                        csvPaths += string.Format("GRO {0} ", mpi.ExpGroup);
                    }
                    if (mpi.ExpCondition != "")
                    {
                        csvPaths += string.Format("CON {0} ", mpi.ExpCondition);
                    }
                    csvPaths += string.Format("SUB {0} ", mpi.ExpSubjectID);
                    csvPaths += string.Format("SES {0}\"; ", mpi.ExpSession);
                }
            }

            mv.presHeatmap /= mv.curMazePaths.cPaths.Count;
            mv.entrHeatmap /= mv.curMazePaths.cPaths.Count;
            mv.timeHeatmap = mv.timeHeatmap / mv.curMazePaths.cPaths.Count / 1000.0; // milliseconds to seconds

            SetHeatmapType(selectedHeatmap.type);
        }

        Color HeatValToColor(double heatVal)
        // called by HeatmapToBitmap
        {
            byte minR = buttonMinColor.BackColor.R;
            byte minG = buttonMinColor.BackColor.G;
            byte minB = buttonMinColor.BackColor.B;

            byte midR = buttonMidColor.BackColor.R;
            byte midG = buttonMidColor.BackColor.G;
            byte midB = buttonMidColor.BackColor.B;

            byte maxR = buttonMaxColor.BackColor.R;
            byte maxG = buttonMaxColor.BackColor.G;
            byte maxB = buttonMaxColor.BackColor.B;

            byte r;
            byte g;
            byte b;

            if (checkBoxShowMidColor.Checked)
            {
                if (heatVal / (maxHeatVal - minHeatVal) >= midpoint) // checks heat percentile
                {
                    r = (byte)(midR + (maxR - midR) * (heatVal / (maxHeatVal - minHeatVal) - midpoint) / (1 - midpoint));
                    g = (byte)(midG + (maxG - midG) * (heatVal / (maxHeatVal - minHeatVal) - midpoint) / (1 - midpoint));
                    b = (byte)(midB + (maxB - midB) * (heatVal / (maxHeatVal - minHeatVal) - midpoint) / (1 - midpoint));
                }
                else
                {
                    r = (byte)(minR + (midR - minR) * heatVal / (maxHeatVal - minHeatVal) / midpoint);
                    g = (byte)(minG + (midG - minG) * heatVal / (maxHeatVal - minHeatVal) / midpoint);
                    b = (byte)(minB + (midB - minB) * heatVal / (maxHeatVal - minHeatVal) / midpoint);
                }
            }
            else
            {
                r = (byte)(minR + (maxR - minR) * heatVal / (maxHeatVal - minHeatVal));
                g = (byte)(minG + (maxG - minG) * heatVal / (maxHeatVal - minHeatVal));
                b = (byte)(minB + (maxB - minB) * heatVal / (maxHeatVal - minHeatVal));
            }

            return Color.FromArgb(heatmapAlpha, r, g, b);
        }

        Bitmap HeatmapToBitmap(double[,] heatmap)
        // heatmap & colorbar to bitmap
        {
            Bitmap bmp = new Bitmap(heatmap.GetLength(0), heatmap.GetLength(1));

            for (int i = 0; i < heatmap.GetLength(0); i++)
            {
                for (int j = 0; j < heatmap.GetLength(1); j++)
                {
                    bmp.SetPixel(i, j, HeatValToColor(heatmap[i, j]));

                    if (heatmap[i, j] == 0 && checkBoxShowTransparentBg.Checked) // sets background to transparent or background color
                    {
                        bmp.SetPixel(i, j, Color.Transparent);
                    }
                    else if (heatmap[i, j] == 0)
                    {
                        bmp.SetPixel(i, j, Color.FromArgb(heatmapAlpha, buttonBgColor.BackColor));
                    }
                }
            }

            return bmp;
        }

        Bitmap MakeColorbar()
        {
            // new double[] parameter 0 less than parameter 1
            // new double[] parameter 1: average of heatmap xPixels & zPixels
            double[,] colorbar = new double[Math.Min(selectedHeatmap.xPixels, selectedHeatmap.zPixels), (selectedHeatmap.xPixels + selectedHeatmap.zPixels) / 2];

            for (int i = 0; i < colorbar.GetLength(0); i++)
            {
                for (int j = 0; j < colorbar.GetLength(1); j++)
                {
                    colorbar[i, j] = maxHeatVal - (maxHeatVal - minHeatVal) / (colorbar.GetLength(1) - 1) * j;
                }
            }

            return HeatmapToBitmap(colorbar);
        }

        void SetInterpolation(Graphics g)
        {
            trackBarSharpness.Enabled = true;

            switch (interpolation)
            {
                case InterpolationModes.None:
                    trackBarSharpness.Enabled = false;
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    break;

                case InterpolationModes.Bilinear:
                    g.InterpolationMode = InterpolationMode.Bilinear;
                    break;

                case InterpolationModes.HighQualityBilinear:
                    g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                    break;

                case InterpolationModes.Bicubic:
                    g.InterpolationMode = InterpolationMode.Bicubic;
                    break;

                case InterpolationModes.HighQualityBicubic:
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    break;

                default:
                    break;
            }

            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.SmoothingMode = SmoothingMode.None;
            g.CompositingQuality = CompositingQuality.AssumeLinear;
        }

        static int mzHmWidth;
        static int mzHmHeight;
        static double scale; // in pixel / maze coord
        static double buffer = 2; // in maze coord
        private void pictureBoxHeatmap_Paint(object sender, PaintEventArgs e)
        {
            int cbWidth = (int)(trackBarMidpoint.Width * 1.4);
            int cbHeight = trackBarMidpoint.Height;
            int cbTopLeftX = panelSettings.Location.X - cbWidth;
            int cbTopLeftZ = trackBarMidpoint.Location.Y - panelHeatmap.AutoScrollPosition.Y * 2;

            mzHmWidth = panelHeatmap.Width - cbWidth;
            mzHmHeight = (int)((selectedHeatmap.zPixels * selectedHeatmap.res + buffer * 2) / (selectedHeatmap.xPixels * selectedHeatmap.res + buffer * 2) * mzHmWidth);
            scale = 5 + ((double)panelHeatmap.Width - panelSettings.Width) / (1302.0 - panelSettings.Width) * 12;

            pictureBoxHeatmap_Paint(e.Graphics, true, mzHmWidth, mzHmHeight, cbWidth, cbHeight, cbTopLeftX, cbTopLeftZ, 8, Color.Black);

            if (showResBox) // paints resolution box
            {
                PaintResBox(e.Graphics);
            }
        }

        static bool scroll = false;
        private void pictureBoxHeatmap_Paint(Graphics g, bool autosize, int mzHmWidth, int mzHmHeight, int cbWidth, int cbHeight, int cbTopLeftX, int cbTopLeftZ, int cbLabelSize, Color cbLabelColor)
        // overload for MakePng
        {
            // in maze coord * scale
            int mzTopLeftX = (int)(-(selectedHeatmap.hmXCenter - selectedHeatmap.xBotRadius * selectedHeatmap.res - buffer) * scale);
            int mzTopLeftZ = (int)(-(selectedHeatmap.hmZCenter - selectedHeatmap.zBotRadius * selectedHeatmap.res - buffer) * scale);
            int mzWidth = (int)((selectedHeatmap.xPixels * selectedHeatmap.res + buffer * 2) * scale);
            int mzHeight = (int)((selectedHeatmap.zPixels * selectedHeatmap.res + buffer * 2) * scale);

            // in mouse coord
            int hmTopLeftX = (int)(mzHmWidth * buffer / (selectedHeatmap.xPixels * selectedHeatmap.res + buffer * 2));
            int hmTopLeftZ = (int)(hmTopLeftX / (selectedHeatmap.xPixels * selectedHeatmap.res + buffer * 2) * (selectedHeatmap.zPixels * selectedHeatmap.res + buffer * 2));
            int hmWidth = mzHmWidth - hmTopLeftX * 2;
            int hmHeight = mzHmHeight - hmTopLeftZ * 2;


            // makes maze, heatmap, & colorbar
            Image mazeBmp = mv.PaintMazeToBuffer(mzTopLeftX, mzTopLeftZ, mzWidth, mzHeight, scale);
            Image anaRegnsBmp = mv.PaintAnalyzerItemsToBuffer(mzTopLeftX, mzTopLeftZ, mzWidth, mzHeight, scale);
            Bitmap heatmapBmp = HeatmapToBitmap(selectedHeatmap.val);
            Bitmap colorbarBmp = MakeColorbar();


            // autosize & scroll settings
            if (autosize)
            {
                if (!scroll)
                {
                    labelScroll.Location = new Point(labelScroll.Location.X, mzHmHeight);
                    scroll = true;
                }
                g.TranslateTransform(panelHeatmap.AutoScrollPosition.X, panelHeatmap.AutoScrollPosition.Y);
            }


            // paints maze, heatmap, & colorbar
            SetInterpolation(g);

            Rectangle mazeDest = new Rectangle(0, 0, mzHmWidth, mzHmHeight);
            if (showMaze)
            {
                g.DrawImage(mazeBmp, mazeDest, 0, 0, mazeBmp.Width, mazeBmp.Height, GraphicsUnit.Pixel);
            }
            if (showAnaRegns)
            {
                g.DrawImage(anaRegnsBmp, mazeDest, 0, 0, anaRegnsBmp.Width, anaRegnsBmp.Height, GraphicsUnit.Pixel);
            }

            Rectangle heatmapDest = new Rectangle(hmTopLeftX, hmTopLeftZ, hmWidth, hmHeight);
            Bitmap resizedHeatmapBmp = ResizeBitmap(heatmapBmp, sharpness, hmWidth);
            g.DrawImage(resizedHeatmapBmp, heatmapDest, 0, 0, resizedHeatmapBmp.Width, resizedHeatmapBmp.Height, GraphicsUnit.Pixel);

            Rectangle colorbarDest = new Rectangle(cbTopLeftX, cbTopLeftZ, cbWidth, cbHeight);
            g.DrawImage(colorbarBmp, colorbarDest, 0, 0, colorbarBmp.Width, colorbarBmp.Height, GraphicsUnit.Pixel);

            Font font = new Font(new FontFamily("times"), cbLabelSize);
            SolidBrush brush = new SolidBrush(cbLabelColor);
            g.DrawString(string.Format("{0}\n({1})", heatmapTypeStr, heatmapUnits), font, brush, cbTopLeftX - font.Size, (float)(cbTopLeftZ + cbHeight * -0.22));
            g.DrawString(string.Format("{0:0.00}", maxHeatVal), font, brush, cbTopLeftX, cbTopLeftZ);
            g.DrawString(string.Format("{0:0.00}", minHeatVal + (maxHeatVal - minHeatVal) * .75), font, brush, cbTopLeftX, (float)(cbTopLeftZ + cbHeight * 0.22));
            g.DrawString(string.Format("{0:0.00}", minHeatVal + (maxHeatVal - minHeatVal) * .5), font, brush, cbTopLeftX, (float)(cbTopLeftZ + cbHeight * 0.44));
            g.DrawString(string.Format("{0:0.00}", minHeatVal + (maxHeatVal - minHeatVal) * .25), font, brush, cbTopLeftX, (float)(cbTopLeftZ + cbHeight * 0.66));
            g.DrawString(string.Format("{0:0.00}", minHeatVal), font, brush, cbTopLeftX, (float)(cbTopLeftZ + cbHeight * 0.88));
        }

        public Bitmap ResizeBitmap(Bitmap bmp, int factor, int destWidth)
        // sharpness feature
        {
            if (factor < 1)
            {
                factor = 1;
            }
            else if (factor * bmp.Width > destWidth)
            {
                factor = (int)Math.Floor((double)destWidth / bmp.Width);
            }

            int newWidth = bmp.Width * factor;
            int newHeight = bmp.Height * factor;
            Bitmap resizeBmp = new Bitmap(newWidth, newHeight);

            using (Graphics g = Graphics.FromImage(resizeBmp))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;

                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.SmoothingMode = SmoothingMode.None;
                g.CompositingQuality = CompositingQuality.AssumeLinear;

                g.DrawImage(bmp, 0, 0, newWidth, newHeight);
            }

            return resizeBmp;
        }

        private void panelHeatmap_Resize(object sender, EventArgs e)
        {
            scroll = false;
            panelHeatmap.AutoScroll = false;
            panelHeatmap.AutoScroll = true;

            Refresh();
        }

        private void panelHeatmap_Scroll(object sender, ScrollEventArgs e)
        {
            Refresh();
        }
    }
}