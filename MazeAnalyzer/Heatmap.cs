using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using MazeLib;

namespace MazeAnalyzer
{
    public partial class Heatmap : Form
    {
        static Color cusMinColor = Color.FromArgb(128, 255, 128);
        static Color cusMaxColor = Color.FromArgb(128, 0, 255);

        static bool cusMidColorShow = true;
        static bool midColorShow;
        static Color cusMidColor = Color.FromArgb(128, 255, 255);

        static bool cusTransparentBg = true;
        static bool transparentBg;
        static Color cusBgColor = Color.White;

        public enum Preset
        {
            Cool, Hot, Gray, Custom
        }
        static Preset selectPreset = Preset.Cool;
        static double midpoint = 0.1;
        static int opacity = 75;
        static int heatmapAlpha = (int)((double)opacity / 100 * 255);
        static int sharpness = 0;

        public enum ClickMode
        {
            None, Offset, ResolutionBox
        }
        ClickMode selectClickMode = ClickMode.None;
        public enum InterpolationModes
        {
            None, Bilinear, HighQualityBilinear, Bicubic, HighQualityBicubic
        }
        static InterpolationModes interpolation = InterpolationModes.None;
        readonly MazeViewer mv;

        static string heatmapTypeStr;
        static HeatmapItem selectHeatmap = new HeatmapItem();
        static double selectMinHeat;
        static double selectMaxHeat;
        string heatmapUnits;
        
        static bool maze = true;
        static bool anaRegns = false;

        static string csvPath; // a string containing the selected paths for the csv files

        public Heatmap(MazeViewer mv)
        {
            InitializeComponent();

            this.mv = mv;
            BuildHtmap(this.mv);

            DoubleBuffered = true;
        }

        private void HeatmapConfig_Load(object sender, EventArgs e)
        // matches the internal and external heatmap settings
        {
            comboBoxPreset.SelectedIndex = (int)selectPreset;
            trackBarMidpoint.Value = Convert.ToInt32(midpoint * 10);
            trackBarOpacity.Value = opacity;
            heatmapAlpha = (int)((double)opacity / 100 * 255);
            labelOpacityPercent.Text = string.Format("{0}%", opacity);


            textBoxRes.Text = Convert.ToString(selectHeatmap.res);
            textBoxRes.BackColor = SystemColors.Control;

            double offsetX = Math.Round(selectHeatmap.offsetX, 2, MidpointRounding.AwayFromZero);
            double offsetZ = Math.Round(selectHeatmap.offsetZ, 2, MidpointRounding.AwayFromZero);
            textBoxOffset.Text = string.Format("{0}, {1}", offsetX, offsetZ);
            textBoxOffset.BackColor = SystemColors.Control;
            buttonOffset.Image = new Bitmap(buttonOffset.Image, 12, 12);

            comboBoxInterpolation.SelectedIndex = (int)interpolation;

            trackBarSharpness.Value = sharpness;
            labelSharpnessVal.Text = string.Format("{0}", sharpness);


            if (selectHeatmap.type == HeatmapItem.Type.Presence)
            {
                radioButtonPres.Checked = true;
            }
            else if (selectHeatmap.type == HeatmapItem.Type.Entrance)
            {
                radioButtonEntr.Checked = true;
            }
            else if (selectHeatmap.type == HeatmapItem.Type.Time)
            {
                radioButtonTime.Checked = true;
            }
            checkBoxMaze.Checked = maze;
            checkBoxAnaRegns.Checked = anaRegns;
        }


        // color settings
        private void buttonMinColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                selectPreset = Preset.Custom;
                comboBoxPreset.SelectedIndex = (int)selectPreset;
                cusMinColor = colorDialog.Color;

                buttonMinColor.BackColor = cusMinColor;
                Refresh();
            }
        }

        private void buttonMaxColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                selectPreset = Preset.Custom;
                comboBoxPreset.SelectedIndex = (int)selectPreset;
                cusMaxColor = colorDialog.Color;

                buttonMaxColor.BackColor = cusMaxColor;
                Refresh();
            }
        }

        private void checkBoxMidColor_Click(object sender, EventArgs e)
        {
            selectPreset = Preset.Custom;
            comboBoxPreset.SelectedIndex = (int)selectPreset;
            midColorShow = !midColorShow;
            checkBoxMidColor.Checked = midColorShow;
            cusMidColorShow = midColorShow;

            Refresh();
        }

        private void buttonMidColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                selectPreset = Preset.Custom;
                comboBoxPreset.SelectedIndex = (int)selectPreset;
                cusMidColor = colorDialog.Color;

                buttonMidColor.BackColor = cusMidColor;
                Refresh();
            }
        }

        private void checkBoxBgTransparent_Click(object sender, EventArgs e)
        {
            selectPreset = Preset.Custom;
            comboBoxPreset.SelectedIndex = (int)selectPreset;
            transparentBg = !transparentBg;
            checkBoxTransparentBg.Checked = transparentBg;
            cusTransparentBg = transparentBg;

            Refresh();
        }

        private void buttonBgColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                selectPreset = Preset.Custom;
                comboBoxPreset.SelectedIndex = (int)selectPreset;
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

                    checkBoxMidColor.Checked = cusMidColorShow;
                    buttonMidColor.BackColor = cusMidColor;

                    checkBoxTransparentBg.Checked = cusTransparentBg;
                    buttonBgColor.BackColor = cusBgColor;
                    break;
                default:
                    break;
            }
            if (newPreset != Preset.Custom)
            {
                midColorShow = true;
                checkBoxMidColor.Checked = midColorShow;
                transparentBg = true;
                checkBoxTransparentBg.Checked = transparentBg;
                buttonBgColor.BackColor = Color.White;
            }
            selectPreset = newPreset;
        }

        private void trackBarMidpoint_Scroll(object sender, EventArgs e)
        {
            midpoint = Convert.ToDouble(trackBarMidpoint.Value) / 10;

            if (checkBoxMidColor.Checked)
            {
                Refresh();
            }
            else
            {
                checkBoxMidColor_Click(sender, e);
            }
        }

        private void trackBarOpacity_Scroll(object sender, EventArgs e)
        {
            opacity = trackBarOpacity.Value;
            heatmapAlpha = (int)((double)opacity / 100 * 255);

            labelOpacityPercent.Text = string.Format("{0}%", opacity);
            Refresh();
        }


        // resolution, offset, interpolation, & sharpness events
        private void textBoxRes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // 0.2 - 15 maze units / pixel, beyond this range, heatmap calculations and colorbar pixel approximations could slow down
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
        {
            string newOffSetX = textBoxOffset.Text.Split(new String[] { ", " }, StringSplitOptions.None)[0];
            string newOffSetZ = textBoxOffset.Text.Split(new String[] { ", " }, StringSplitOptions.None)[1];
            string offsetX = Convert.ToString(Math.Round(selectHeatmap.offsetX, 2, MidpointRounding.AwayFromZero));
            string offsetZ = Convert.ToString(Math.Round(selectHeatmap.offsetZ, 2, MidpointRounding.AwayFromZero));

            if (newOffSetX != offsetX || newOffSetZ != offsetZ)
            {
                mv.SetHeatmapOffset(Convert.ToDouble(newOffSetX), Convert.ToDouble(newOffSetZ));
                RebuildHeatmap();
            }
        }

        void RebuildHeatmap()
        // called when the resolution or offset is changed
        {
            BuildHtmap(mv);

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
        {
            if (selectClickMode == ClickMode.ResolutionBox)
            {
                selectClickMode = ClickMode.None;
                Cursor = Cursors.Default;
            }
            else
            {
                selectClickMode = ClickMode.ResolutionBox;
                Cursor = Cursors.Cross;
            }
        }

        private void pictureBoxHtmap_MouseDown(object sender, MouseEventArgs e)
        {
            MouseEventArgs mouseCoord = e;

            if (selectClickMode == ClickMode.ResolutionBox)
            {
                mouseCoord1 = new Point(mouseCoord.X, mouseCoord.Y - panelHtmap.AutoScrollPosition.Y);
            }
        }

        bool selectRect = false;
        private void pictureBoxHtmap_MouseMove(object sender, MouseEventArgs e)
        {
            MouseEventArgs mouseCoord = e;

            if (selectClickMode == ClickMode.ResolutionBox)
            {
                if (e.Button == MouseButtons.Left)
                {
                    mouseCoord2 = new Point(mouseCoord.X, mouseCoord.Y - panelHtmap.AutoScrollPosition.Y);

                    selectRect = true;
                    Refresh();
                }
            }
        }

        private void pictureBoxHtmap_MouseUp(object sender, MouseEventArgs e)
        {
            MouseEventArgs mouseCoord = e;

            if (selectClickMode == ClickMode.ResolutionBox)
            {
                mouseCoord2 = new Point(mouseCoord.X, mouseCoord.Y);
                PointF mazeCoord1 = MouseToMazeCoord(mouseCoord1.X, mouseCoord1.Y);
                PointF mazeCoord2 = MouseToMazeCoord(mouseCoord2.X, mouseCoord2.Y);

                selectHeatmap.res = Math.Round(Math.Max(Math.Abs(mazeCoord1.X - mazeCoord2.X), Math.Abs(mazeCoord1.Y - mazeCoord2.Y)), 2);
                textBoxRes.Text = Convert.ToString(selectHeatmap.res);
                textBoxOffset.Text = string.Format("{0}, {1}", mazeCoord1.X, mazeCoord2.Y);
                selectRect = false;
                selectClickMode = ClickMode.None;

                textBoxRes.BackColor = SystemColors.Control;
                textBoxOffset.BackColor = SystemColors.Control;
                Cursor = Cursors.Default;
                SetOffset(); // SetOffset calls RebuildHtmap, which calls Refresh
            }
        }

        private void buttonOffset_Click(object sender, EventArgs e)
        {
            if (selectClickMode == ClickMode.Offset)
            {
                selectClickMode = ClickMode.None;
                Cursor = Cursors.Default;
            }
            else
            {
                selectClickMode = ClickMode.Offset;
                Cursor = Cursors.Cross;
            }
        }

        private void pictureBoxHtmap_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouseCoord = (MouseEventArgs)e;
            PointF mazeCoord = MouseToMazeCoord(mouseCoord.X, mouseCoord.Y);

            if (selectClickMode == ClickMode.Offset)
            {
                textBoxOffset.Text = string.Format("{0}, {1}", mazeCoord.X, mazeCoord.Y);
                SetOffset();
                selectClickMode = ClickMode.None;

                textBoxOffset.BackColor = SystemColors.Control;
                Cursor = Cursors.Default;
            }
            else if (selectClickMode == ClickMode.None)
            {
                ToolTip offset = new ToolTip();

                double heatVal = double.NaN;
                Point heatmapCoord = selectHeatmap.MazeToHeatmapCoord(mazeCoord.X, mazeCoord.Y);

                if (heatmapCoord.X >= 0 && heatmapCoord.X < selectHeatmap.xPixels && heatmapCoord.Y >= 0 && heatmapCoord.Y < selectHeatmap.zPixels)
                {
                    heatVal = Math.Round(selectHeatmap.val[heatmapCoord.X, heatmapCoord.Y], 2, MidpointRounding.AwayFromZero);
                }

                offset.SetToolTip(pictureBoxHtmap, string.Format("x:{0}, z:{1}\n{2} {3}", mazeCoord.X, mazeCoord.Y, heatVal, heatmapUnits));
            }
        }

        private PointF MouseToMazeCoord(double x, double z)
        {
            PointF mazeCoord = new PointF();

            double mzTopLeftX = selectHeatmap.heatmapXCenter - selectHeatmap.xBotRadius * selectHeatmap.res - buffer;
            double mzWidth = selectHeatmap.xPixels * selectHeatmap.res + buffer * 2;

            mazeCoord.X = (float)Math.Round(mzTopLeftX + x / width * mzWidth, 2, MidpointRounding.AwayFromZero);

            double mzTopLeftZ = selectHeatmap.heatmapZCenter - selectHeatmap.zBotRadius * selectHeatmap.res - buffer;
            double mzHeight = selectHeatmap.zPixels * selectHeatmap.res + buffer * 2;

            mazeCoord.Y = (float)Math.Round(mzTopLeftZ + (z - panelHtmap.AutoScrollPosition.Y) / height * mzHeight, 2, MidpointRounding.AwayFromZero);

            return mazeCoord;
        }


        // selected heatmap, maze, and analyzer regions events
        public void SetHeatmapType(HeatmapItem.Type newHeatmapType)
        {
            switch (newHeatmapType)
            {
                case HeatmapItem.Type.Presence:
                    heatmapTypeStr = "Presence";
                    selectHeatmap = mv.presHeatmap;
                    heatmapUnits = "% Trials";
                    break;
                case HeatmapItem.Type.Entrance:
                    heatmapTypeStr = "Entrance";
                    selectHeatmap = mv.entrHeatmap;
                    heatmapUnits = "# Times";
                    break;
                case HeatmapItem.Type.Time:
                    heatmapTypeStr = "Time";
                    selectHeatmap = mv.timeHeatmap;
                    heatmapUnits = "s";
                    break;
                default:
                    break;
            }
            selectMinHeat = selectHeatmap.GetMin();
            selectMaxHeat = selectHeatmap.GetMax();

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

        private void checkBoxMaze_Click(object sender, EventArgs e)
        {
            maze = !maze;

            checkBoxMaze.Checked = maze;
            Refresh();
        }

        private void checkBoxAnalyzerRegions_Click(object sender, EventArgs e)
        {
            anaRegns = !anaRegns;

            checkBoxAnaRegns.Checked = anaRegns;
            Refresh();
        }


        // save file events
        private void buttonSaveCsv_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveCsv = new SaveFileDialog();
            saveCsv.Filter = "CSV file(*.csv)| *.csv | All Files(*.*) | *.* ";

            if (saveCsv.ShowDialog() == DialogResult.OK)
            {
                StreamWriter csv = new StreamWriter(saveCsv.FileName);

                csv.Write(string.Format("Maze Analyzer Heatmap Export: {0}\n", heatmapTypeStr));
                csv.Write(string.Format("Date Exported: {0}\n", DateTime.Now));
                csv.Write(string.Format("Project File Name: {0}\n", mv.projectInfo.ProjectFilename));
                csv.Write(string.Format("Project Name: {0}\n", mv.projectInfo.ProjectFilename.Split('\\')[mv.projectInfo.ProjectFilename.Split('\\').Length - 1]));
                csv.Write(string.Format("Maze Name: {0}.maz\n", mv.mazeFileName));
                csv.Write(string.Format("Paths Exported: {0}\n", csvPath));
                csv.Write(string.Format("Heatmap Resolution: {0} Maze Units / Pixel\n", selectHeatmap.res));
                csv.Write("\n");

                csv.Write("xz, ");
                string xs = GetHeatmapBorders(selectHeatmap.offsetX, selectHeatmap.xBotRadius, selectHeatmap.xPixels);
                csv.Write(xs.Substring(0, xs.Length - 2));
                csv.Write("\n");

                string[] zs = GetHeatmapBorders(selectHeatmap.offsetZ, selectHeatmap.zBotRadius, selectHeatmap.zPixels).Split(' ');

                for (int i = 0; i < selectHeatmap.val.GetLength(1); i++)
                {
                    csv.Write(zs[i] + " ");
                    for (int j = 0; j < selectHeatmap.val.GetLength(0); j++)
                    {
                        csv.Write(selectHeatmap.val[j, i]);

                        if (j != selectHeatmap.val.GetLength(0) - 1)
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
        // returns the coordinates borders of all the pixels for the csv file
        {
            string coords = "";

            for (int i = 0; i < pixels + 1; i++)
            {
                double coord = offset + (i - lowerRadius) * selectHeatmap.res;

                if (coord != offset)
                {
                    coords += string.Format("{0}, ", coord);
                }
            }

            return coords;
        }

        private void buttonSavePng_Click(object sender, EventArgs e)
        {
            bool temp = checkBoxTransparentBg.Checked;
            checkBoxTransparentBg.Checked = true;

            SaveFileDialog savePng = new SaveFileDialog();
            savePng.Filter = "Png Image (.png)|*.png";

            if (savePng.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = MakePng();

                bmp.Save(savePng.FileName);
            }

            checkBoxTransparentBg.Checked = temp;
        }

        Bitmap MakePng()
        {
            int resize = 100;
            int width = (int)(selectHeatmap.xPixels * selectHeatmap.res + buffer * 2) * resize;
            int height = (int)(selectHeatmap.zPixels * selectHeatmap.res + buffer * 2) * resize;

            int colorbarWidth = width / 10;
            int colorbarHeight = height / 4;
            int colorbarTopLeftX = width;
            int colorbarTopLeftZ = height / 8;

            Bitmap bmp = new Bitmap(width + colorbarWidth, height);

            using (Graphics png = Graphics.FromImage(bmp))
            {
                pictureBoxHtmap_Paint(png, false, width, height, colorbarWidth, colorbarHeight, colorbarTopLeftX, colorbarTopLeftZ, 170, Color.White);
            }

            return bmp;
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            ToClipboard();
        }

        public void ToClipboard()
        {
            int colorbarWidth = (int)(trackBarMidpoint.Width * 1.4);
            int colorbarHeight = trackBarMidpoint.Height;
            int colorbarTopLeftX = panelSettings.Location.X - colorbarWidth;
            int colorbarTopLeftZ = trackBarMidpoint.Location.Y - panelHtmap.AutoScrollPosition.Y * 2;

            width = panelHtmap.Width - colorbarWidth;
            height = (int)((selectHeatmap.zPixels * selectHeatmap.res + buffer * 2) / (selectHeatmap.xPixels * selectHeatmap.res + buffer * 2) * width);
            scale = 5 + (double)(panelHtmap.Width - panelSettings.Width) / (1302 - panelSettings.Width) * 12;

            Bitmap copy = new Bitmap((int)(width + colorbarWidth * 0.85), height);
            Graphics g = Graphics.FromImage(copy);

            pictureBoxHtmap_Paint(g, true, width, height, colorbarWidth, colorbarHeight, colorbarTopLeftX, colorbarTopLeftZ, 8, Color.Black);

            Clipboard.Clear();
            Clipboard.SetImage(copy.Clone(new Rectangle(0, 0, (int)(width + colorbarWidth * 0.85), height), copy.PixelFormat));
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

        private void PaintSelectRect(Graphics g)
        {
            g.DrawRectangle(new Pen(Brushes.Black, 1F), MouseToRect(mouseCoord1, mouseCoord2));
        }

        void BuildHtmap(MazeViewer mv)
        // makes the heatmaps and adds them together [the numbers]
        {
            // TODO: Make flag to skip build if no changes are made
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

                    // gets paths for the csv file
                    csvPath += string.Format("\"TRI {0} ", mpi.ExpTrial);
                    if (mpi.ExpGroup != "")
                    {
                        csvPath += string.Format("GRO {0} ", mpi.ExpGroup);
                    }
                    if (mpi.ExpCondition != "")
                    {
                        csvPath += string.Format("CON {0} ", mpi.ExpCondition);
                    }
                    csvPath += string.Format("SUB {0} ", mpi.ExpSubjectID);
                    csvPath += string.Format("SES {0}\"; ", mpi.ExpSession);
                }
            }

            mv.presHeatmap /= mv.curMazePaths.cPaths.Count;
            mv.entrHeatmap /= mv.curMazePaths.cPaths.Count;
            mv.timeHeatmap /= mv.curMazePaths.cPaths.Count / 1000; // convert to seconds

            SetHeatmapType(selectHeatmap.type);
        }

        Color ToColor(double heatVal) // takes a heat number and returns a color
        {
            double minR = Convert.ToDouble(buttonMinColor.BackColor.R);
            double minG = Convert.ToDouble(buttonMinColor.BackColor.G);
            double minB = Convert.ToDouble(buttonMinColor.BackColor.B);

            double midR = Convert.ToDouble(buttonMidColor.BackColor.R);
            double midG = Convert.ToDouble(buttonMidColor.BackColor.G);
            double midB = Convert.ToDouble(buttonMidColor.BackColor.B);

            double maxR = Convert.ToDouble(buttonMaxColor.BackColor.R);
            double maxG = Convert.ToDouble(buttonMaxColor.BackColor.G);
            double maxB = Convert.ToDouble(buttonMaxColor.BackColor.B);

            int r;
            int g;
            int b;

            if (checkBoxMidColor.Checked)
            {
                if (heatVal / (selectMaxHeat - selectMinHeat) >= midpoint) // checks heat percentile
                {
                    r = Convert.ToInt32(midR + (maxR - midR) * (heatVal / (selectMaxHeat - selectMinHeat) - midpoint) / (1 - midpoint));
                    g = Convert.ToInt32(midG + (maxG - midG) * (heatVal / (selectMaxHeat - selectMinHeat) - midpoint) / (1 - midpoint));
                    b = Convert.ToInt32(midB + (maxB - midB) * (heatVal / (selectMaxHeat - selectMinHeat) - midpoint) / (1 - midpoint));
                }
                else
                {
                    r = Convert.ToInt32(minR + (midR - minR) * heatVal / (selectMaxHeat - selectMinHeat) / midpoint);
                    g = Convert.ToInt32(minG + (midG - minG) * heatVal / (selectMaxHeat - selectMinHeat) / midpoint);
                    b = Convert.ToInt32(minB + (midB - minB) * heatVal / (selectMaxHeat - selectMinHeat) / midpoint);
                }
            }
            else
            {
                r = Convert.ToInt32(minR + (maxR - minR) * heatVal / (selectMaxHeat - selectMinHeat));
                g = Convert.ToInt32(minG + (maxG - minG) * heatVal / (selectMaxHeat - selectMinHeat));
                b = Convert.ToInt32(minB + (maxB - minB) * heatVal / (selectMaxHeat - selectMinHeat));
            }

            return Color.FromArgb(heatmapAlpha, r, g, b);
        }

        Bitmap ToBitmap(double[,] htmap) // makes a bitmap for the colorbar and heatmap
        {
            Bitmap bmp = new Bitmap(htmap.GetLength(0), htmap.GetLength(1));

            for (int i = 0; i < htmap.GetLength(0); i++)
            {
                for (int j = 0; j < htmap.GetLength(1); j++)
                {
                    bmp.SetPixel(i, j, ToColor(htmap[i, j]));

                    if (htmap[i, j] == 0 && checkBoxTransparentBg.Checked) // sets background to transparent or background color
                    {
                        bmp.SetPixel(i, j, Color.Transparent);
                    }
                    else if (htmap[i, j] == 0)
                    {
                        bmp.SetPixel(i, j, Color.FromArgb(heatmapAlpha, buttonBgColor.BackColor));
                    }
                }
            }

            return bmp;
        }

        Bitmap MakeColorbar()
        {
            // MakeColorbar parameter 0 less than parameter 1, parameter 1 is an approximation of the number of pixels
            double[,] colorbar = new double[Math.Min(selectHeatmap.xPixels, selectHeatmap.zPixels), (selectHeatmap.xPixels + selectHeatmap.zPixels) / 2];

            for (int i = 0; i < colorbar.GetLength(0); i++)
            {
                for (int j = 0; j < colorbar.GetLength(1); j++)
                {
                    colorbar[i, j] = selectMaxHeat - (selectMaxHeat - selectMinHeat) / (colorbar.GetLength(1) - 1) * j;
                }
            }

            return ToBitmap(colorbar);
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

        static int width;
        static int height;
        static double scale; // in pixel per maze units
        static double buffer = 2; // in maze units
        private void pictureBoxHtmap_Paint(object sender, PaintEventArgs e)
        {
            int colorbarWidth = (int)(trackBarMidpoint.Width * 1.4);
            int colorbarHeight = trackBarMidpoint.Height;
            int colorbarTopLeftX = panelSettings.Location.X - colorbarWidth;
            int colorbarTopLeftZ = trackBarMidpoint.Location.Y - panelHtmap.AutoScrollPosition.Y * 2;

            width = panelHtmap.Width - colorbarWidth;
            height = (int)((selectHeatmap.zPixels * selectHeatmap.res + buffer * 2) / (selectHeatmap.xPixels * selectHeatmap.res + buffer * 2) * width);
            scale = 5 + (double)(panelHtmap.Width - panelSettings.Width) / (1302 - panelSettings.Width) * 12;

            pictureBoxHtmap_Paint(e.Graphics, true, width, height, colorbarWidth, colorbarHeight, colorbarTopLeftX, colorbarTopLeftZ, 8, Color.Black);

            if (selectRect)
            {
                PaintSelectRect(e.Graphics);
            }
        }

        static bool scroll = false;
        private void pictureBoxHtmap_Paint(Graphics g, bool autosize, int width, int height, int colorbarWidth, int colorbarHeight, int colorbarTopLeftX, int colorbarTopLeftZ, int colorbarLabelSize, Color colorbarLabelColor)
        {
            // in maze units * scale
            int mazeTopLeftX = (int)(-(selectHeatmap.heatmapXCenter - selectHeatmap.xBotRadius * selectHeatmap.res - buffer) * scale);
            int mazeTopLeftZ = (int)(-(selectHeatmap.heatmapZCenter - selectHeatmap.zBotRadius * selectHeatmap.res - buffer) * scale);
            int mazeWidth = (int)((selectHeatmap.xPixels * selectHeatmap.res + buffer * 2) * scale);
            int mazeHeight = (int)((selectHeatmap.zPixels * selectHeatmap.res + buffer * 2) * scale);

            // in coord system of the paint function
            int htmapTopLeftX = (int)(width * buffer / (selectHeatmap.xPixels * selectHeatmap.res + buffer * 2));
            int htmapTopLeftZ = (int)(htmapTopLeftX / (selectHeatmap.xPixels * selectHeatmap.res + buffer * 2) * (selectHeatmap.zPixels * selectHeatmap.res + buffer * 2));
            int htmapWidth = width - htmapTopLeftX * 2;
            int htmapHeight = height - htmapTopLeftZ * 2;


            // makes maze, heatmap, and colorbar
            Image mazeBmp = mv.PaintMazeToBuffer(mazeTopLeftX, mazeTopLeftZ, mazeWidth, mazeHeight, scale);
            Image anaRegnsBmp = mv.PaintAnalyzerItemsToBuffer(mazeTopLeftX, mazeTopLeftZ, mazeWidth, mazeHeight, scale);
            Bitmap heatBmp = ToBitmap(selectHeatmap.val);
            Bitmap colorbarBmp = MakeColorbar();


            // autosize & scroll settings
            if (autosize)
            {
                if (!scroll)
                {
                    labelScroll.Location = new Point(labelScroll.Location.X, height);
                    scroll = true;
                }
                g.TranslateTransform(panelHtmap.AutoScrollPosition.X, panelHtmap.AutoScrollPosition.Y);
            }


            // draws maze, heatmap, colorbar, and colorbar labels
            SetInterpolation(g);

            Rectangle mazeDest = new Rectangle(0, 0, width, height);
            if (maze)
            {
                g.DrawImage(mazeBmp, mazeDest, 0, 0, mazeBmp.Width, mazeBmp.Height, GraphicsUnit.Pixel);
            }
            if (anaRegns)
            {
                g.DrawImage(anaRegnsBmp, mazeDest, 0, 0, anaRegnsBmp.Width, anaRegnsBmp.Height, GraphicsUnit.Pixel);
            }

            Rectangle htmapDest = new Rectangle(htmapTopLeftX, htmapTopLeftZ, htmapWidth, htmapHeight);
            Bitmap resizeheatBitmap = ResizeBitmap(heatBmp, sharpness, htmapWidth);
            g.DrawImage(resizeheatBitmap, htmapDest, 0, 0, resizeheatBitmap.Width, resizeheatBitmap.Height, GraphicsUnit.Pixel);

            Rectangle colorbarDest = new Rectangle(colorbarTopLeftX, colorbarTopLeftZ, colorbarWidth, colorbarHeight);
            g.DrawImage(colorbarBmp, colorbarDest, 0, 0, colorbarBmp.Width, colorbarBmp.Height, GraphicsUnit.Pixel);

            Font font = new Font(new FontFamily("times"), colorbarLabelSize);
            SolidBrush brush = new SolidBrush(colorbarLabelColor);
            g.DrawString(string.Format("{0}\n({1})", heatmapTypeStr, heatmapUnits), font, brush, colorbarTopLeftX - font.Size, (float)(colorbarTopLeftZ + colorbarHeight * -0.22));
            g.DrawString(string.Format("{0:0.00}", selectMaxHeat), font, brush, colorbarTopLeftX, colorbarTopLeftZ);
            g.DrawString(string.Format("{0:0.00}", selectMinHeat + (selectMaxHeat - selectMinHeat) * .75), font, brush, colorbarTopLeftX, (float)(colorbarTopLeftZ + colorbarHeight * 0.22));
            g.DrawString(string.Format("{0:0.00}", selectMinHeat + (selectMaxHeat - selectMinHeat) * .5), font, brush, colorbarTopLeftX, (float)(colorbarTopLeftZ + colorbarHeight * 0.44));
            g.DrawString(string.Format("{0:0.00}", selectMinHeat + (selectMaxHeat - selectMinHeat) * .25), font, brush, colorbarTopLeftX, (float)(colorbarTopLeftZ + colorbarHeight * 0.66));
            g.DrawString(string.Format("{0:0.00}", selectMinHeat), font, brush, colorbarTopLeftX, (float)(colorbarTopLeftZ + colorbarHeight * 0.88));
        }

        public Bitmap ResizeBitmap(Bitmap bmp, int factor, int destWidth)
        {
            if (factor < 1)
            {
                factor = 1;
            }
            else if (factor * bmp.Width > destWidth)
            {
                factor = (int)Math.Floor((double)destWidth / bmp.Width);
            }

            int newBmpWidth = bmp.Width * factor;
            int newBmpHeight = bmp.Height * factor;
            Bitmap resizeBmp = new Bitmap(newBmpWidth, newBmpHeight);

            using (Graphics g = Graphics.FromImage(resizeBmp))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;

                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.SmoothingMode = SmoothingMode.None;
                g.CompositingQuality = CompositingQuality.AssumeLinear;

                g.DrawImage(bmp, 0, 0, newBmpWidth, newBmpHeight);
            }

            return resizeBmp;
        }

        private void panelHtmap_Resize(object sender, EventArgs e)
        {
            scroll = false;
            panelHtmap.AutoScroll = false;
            panelHtmap.AutoScroll = true;

            Refresh();
        }

        private void panelHtmap_Scroll(object sender, ScrollEventArgs e)
        {
            Refresh();
        }
    }
}