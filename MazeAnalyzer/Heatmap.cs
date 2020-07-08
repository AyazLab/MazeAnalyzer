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
        static int htmapAlpha = (int)((double)opacity / 100 * 255);
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

        readonly MazeViewer curMaze;

        static double[,] presHtmap;
        static double minPres;
        static double maxPres;

        static double[,] entrHtmap;
        static double minEntr;
        static double maxEntr;

        static double[,] timeHtmap;
        static double minTime;
        static double maxTime;

        public enum HtmapType
        {
            Presence, Entrance, Time
        }
        static HtmapType selectHtmapType = HtmapType.Presence;
        static string htmapTypeStr;
        static double[,] selectHtmap = new double[0, 0];
        static double selectMinHeat;
        static double selectMaxHeat;
        string htmapUnits;
        
        static bool maze = true;
        static bool anaRegns = false;

        static string csvPathHeaderInfo; // a string containing the selected paths for the csv files

        public Heatmap(MazeViewer inpMz)
        {
            InitializeComponent();

            curMaze = inpMz;
            BuildHtmap(curMaze);

            DoubleBuffered = true;
        }

        private void HeatmapConfig_Load(object sender, EventArgs e)
        // matches the internal and external heatmap settings
        {
            comboBoxPreset.SelectedIndex = (int)selectPreset;
            trackBarMidpoint.Value = Convert.ToInt32(midpoint * 10);
            trackBarOpacity.Value = opacity;
            htmapAlpha = (int)((double)opacity / 100 * 255);
            labelOpacityPercent.Text = string.Format("{0}%", opacity);


            textBoxRes.Text = Convert.ToString(MazePathItem.res);
            textBoxRes.BackColor = SystemColors.Control;

            double offsetX = Math.Round(MazePathItem.offsetX, 2, MidpointRounding.AwayFromZero);
            double offsetZ = Math.Round(MazePathItem.offsetZ, 2, MidpointRounding.AwayFromZero);
            textBoxOffset.Text = string.Format("{0}, {1}", offsetX, offsetZ);
            textBoxOffset.BackColor = SystemColors.Control;
            buttonOffset.Image = new Bitmap(buttonOffset.Image, 12, 12);

            comboBoxInterpolation.SelectedIndex = (int)interpolation;

            trackBarSharpness.Value = sharpness;
            labelSharpnessVal.Text = string.Format("{0}", sharpness);


            if (selectHtmapType == HtmapType.Presence)
            {
                radioButtonPres.Checked = true;
            }
            else if (selectHtmapType == HtmapType.Entrance)
            {
                radioButtonEntr.Checked = true;
            }
            else if (selectHtmapType == HtmapType.Time)
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
            htmapAlpha = (int)((double)opacity / 100 * 255);

            labelOpacityPercent.Text = string.Format("{0}%", opacity);
            Refresh();
        }


        // resolution, offset, interpolation, & sharpness events
        private void textBoxRes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // 0.2 - 15 maze units / pixel, beyond this range, heatmap calculations and colorbar pixel approximations could slow down
                MazePathItem.res = Convert.ToDouble(textBoxRes.Text);

                textBoxRes.BackColor = SystemColors.Control;
                RebuildHtmap();
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
            string offsetX = Convert.ToString(Math.Round(MazePathItem.offsetX, 2, MidpointRounding.AwayFromZero));
            string offsetZ = Convert.ToString(Math.Round(MazePathItem.offsetZ, 2, MidpointRounding.AwayFromZero));

            if (newOffSetX != offsetX || newOffSetZ != offsetZ)
            {
                MazePathItem.offsetX = Convert.ToDouble(newOffSetX);
                MazePathItem.offsetZ = Convert.ToDouble(newOffSetZ);
                // BuildHtmap only runs if the number of heatmap pixels has changed and changing the offset doesn't necessarily change the number of heatmap pixels
                selectHtmap = new double[0, 0];
                RebuildHtmap();
            }
        }

        void RebuildHtmap()
        // called when the resolution or offset is changed
        {
            MazePathItem.UpdateHtmapPixels();

            BuildHtmap(curMaze);

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

                MazePathItem.res = Math.Round(Math.Max(Math.Abs(mazeCoord1.X - mazeCoord2.X), Math.Abs(mazeCoord1.Y - mazeCoord2.Y)), 2);
                textBoxRes.Text = Convert.ToString(MazePathItem.res);
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
                Point pixelCoord = MazePathItem.MapCoord(mazeCoord.X, mazeCoord.Y);

                if (pixelCoord.X >= 0 && pixelCoord.X < MazePathItem.xPixels && pixelCoord.Y >= 0 && pixelCoord.Y < MazePathItem.zPixels)
                {
                    heatVal = Math.Round(selectHtmap[pixelCoord.X, pixelCoord.Y], 2, MidpointRounding.AwayFromZero);
                }

                offset.SetToolTip(pictureBoxHtmap, string.Format("x:{0}, z:{1}\n{2} {3}", mazeCoord.X, mazeCoord.Y, heatVal, htmapUnits));
            }
        }

        private PointF MouseToMazeCoord(double x, double z)
        {
            PointF mazeCoord = new PointF();

            double hm_mzUnits_topLeftX = MazePathItem.htmapXCenter - MazePathItem.xLowerRadius * MazePathItem.res - buffer;
            double hm_mzUnits_width = MazePathItem.xPixels * MazePathItem.res + buffer * 2;

            mazeCoord.X = (float)Math.Round(hm_mzUnits_topLeftX + x / width * hm_mzUnits_width, 2, MidpointRounding.AwayFromZero);

            double hm_mzUnits_topLeftZ = MazePathItem.htmapZCenter - MazePathItem.zLowerRadius * MazePathItem.res - buffer;
            double hm_mzUnits_height = MazePathItem.zPixels * MazePathItem.res + buffer * 2;

            mazeCoord.Y = (float)Math.Round(hm_mzUnits_topLeftZ + (z - panelHtmap.AutoScrollPosition.Y) / height * hm_mzUnits_height, 2, MidpointRounding.AwayFromZero);

            return mazeCoord;
        }


        // selected heatmap, maze, and analyzer regions events
        public void SetHtmapType(HtmapType newHtmapType)
        {
            switch (newHtmapType)
            {
                case HtmapType.Presence:
                    htmapTypeStr = "Presence";
                    selectHtmap = presHtmap;
                    selectMaxHeat = maxPres;
                    selectMinHeat = minPres;
                    htmapUnits = "% Trials";
                    break;
                case HtmapType.Entrance:
                    htmapTypeStr = "Entrance";
                    selectHtmap = entrHtmap;
                    selectMaxHeat = maxEntr;
                    selectMinHeat = minEntr;
                    htmapUnits = "# Times";
                    break;
                case HtmapType.Time:
                    htmapTypeStr = "Time";
                    selectHtmap = timeHtmap;
                    selectMaxHeat = maxTime;
                    selectMinHeat = minTime;
                    htmapUnits = "s";
                    break;
                default:
                    break;
            }
            selectHtmapType = newHtmapType;

            Refresh();
        }

        private void radioButtonPres_CheckedChanged(object sender, EventArgs e)
        {
            SetHtmapType(HtmapType.Presence);
        }
        
        private void radioButtonEntr_CheckedChanged(object sender, EventArgs e)
        {
            SetHtmapType(HtmapType.Entrance);
        }

        private void radioButtonTime_CheckedChanged(object sender, EventArgs e)
        {
            SetHtmapType(HtmapType.Time);
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

                csv.Write(string.Format("Maze Analyzer Heatmap Export: {0}\n", htmapTypeStr));
                csv.Write(string.Format("Date Exported: {0}\n", DateTime.Now));
                csv.Write(string.Format("Project File Name: {0}\n", curMaze.projectInfo.ProjectFilename));
                csv.Write(string.Format("Project Name: {0}\n", curMaze.projectInfo.ProjectFilename.Split('\\')[curMaze.projectInfo.ProjectFilename.Split('\\').Length - 1]));
                csv.Write(string.Format("Maze Name: {0}.maz\n", curMaze.mazeFileName));
                csv.Write(string.Format("Paths Exported: {0}\n", csvPathHeaderInfo));
                csv.Write(string.Format("Heatmap Resolution: {0} Maze Units / Pixel\n", MazePathItem.res));
                csv.Write("\n");

                csv.Write("xz, ");
                string xs = GetCoords(MazePathItem.offsetX, MazePathItem.xLowerRadius, MazePathItem.xPixels);
                csv.Write(xs.Substring(0, xs.Length - 2));
                csv.Write("\n");

                string[] zs = GetCoords(MazePathItem.offsetZ, MazePathItem.zLowerRadius, MazePathItem.zPixels).Split(' ');

                for (int i = 0; i < selectHtmap.GetLength(1); i++)
                {
                    csv.Write(zs[i] + " ");
                    for (int j = 0; j < selectHtmap.GetLength(0); j++)
                    {
                        csv.Write(selectHtmap[j, i]);

                        if (j != selectHtmap.GetLength(0) - 1)
                        {
                            csv.Write(", ");
                        }
                    }
                    csv.Write("\n");
                }
                csv.Close();
            }
        }

        string GetCoords(double offset, int lowerRadius, int pixels)
        // returns the coordinates borders of all the pixels for the csv file
        {
            string coords = "";

            for (int i = 0; i < pixels + 1; i++)
            {
                double coord = offset + (i - lowerRadius) * MazePathItem.res;

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
            int width = (int)(MazePathItem.xPixels * MazePathItem.res + buffer * 2) * resize;
            int height = (int)(MazePathItem.zPixels * MazePathItem.res + buffer * 2) * resize;

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
            height = (int)((MazePathItem.zPixels * MazePathItem.res + buffer * 2) / (MazePathItem.xPixels * MazePathItem.res + buffer * 2) * width);
            scale = 5 + (double)(panelHtmap.Width - panelSettings.Width) / (1302 - panelSettings.Width) * 12;

            Bitmap copy = new Bitmap((int)(width + colorbarWidth * 0.85), height);
            Graphics png = Graphics.FromImage(copy);

            pictureBoxHtmap_Paint(png, true, width, height, colorbarWidth, colorbarHeight, colorbarTopLeftX, colorbarTopLeftZ, 8, Color.Black);

            Clipboard.Clear();
            Clipboard.SetImage(copy.Clone(new Rectangle(0, 0, (int)(width + colorbarWidth * 0.85), height), copy.PixelFormat));
            copy.Dispose();
        }


        // painting events
        private Rectangle PointsToRect(Point beginPoint, Point endPoint)
        {
            int top = beginPoint.Y < endPoint.Y ? beginPoint.Y : endPoint.Y;
            int bottom = beginPoint.Y > endPoint.Y ? beginPoint.Y : endPoint.Y;
            int left = beginPoint.X < endPoint.X ? beginPoint.X : endPoint.X;
            int right = beginPoint.X > endPoint.X ? beginPoint.X : endPoint.X;

            return new Rectangle(left, top, right - left, bottom - top);
        }

        private void PaintSelectRect(Graphics png)
        {
            png.DrawRectangle(new Pen(Brushes.Black, 1F), PointsToRect(mouseCoord1, mouseCoord2));
        }

        void BuildHtmap(MazeViewer inpMz)
        // makes the heatmaps and adds them together [the numbers]
        {
            if (selectHtmap.Length != MazePathItem.xPixels * MazePathItem.zPixels)
            {
                presHtmap = new double[MazePathItem.xPixels, MazePathItem.zPixels];
                entrHtmap = new double[MazePathItem.xPixels, MazePathItem.zPixels];
                timeHtmap = new double[MazePathItem.xPixels, MazePathItem.zPixels];

                foreach (MazePathItem mzp in inpMz.curMazePaths.cPaths)
                {
                    if (mzp.selected)
                    {
                        mzp.MakePathHtmap();

                        for (int i = 0; i < MazePathItem.xPixels; i++)
                        {
                            for (int j = 0; j < MazePathItem.zPixels; j++)
                            {
                                presHtmap[i, j] += mzp.presHtmap[i, j]; // bool to int switch
                                entrHtmap[i, j] += mzp.entrHtmap[i, j];
                                timeHtmap[i, j] += mzp.timeHtmap[i, j] / 1000; // millisecond to second conversion
                            }
                        }

                        // gets paths for the csv file
                        csvPathHeaderInfo += string.Format("\"TRI {0} ", mzp.ExpTrial);
                        if (mzp.ExpGroup != "")
                        {
                            csvPathHeaderInfo += string.Format("GRO {0} ", mzp.ExpGroup);
                        }
                        if (mzp.ExpCondition != "")
                        {
                            csvPathHeaderInfo += string.Format("CON {0} ", mzp.ExpCondition);
                        }
                        csvPathHeaderInfo += string.Format("SUB {0} ", mzp.ExpSubjectID);
                        csvPathHeaderInfo += string.Format("SES {0}\"; ", mzp.ExpSession);
                    }
                }

                // gets min, max, & average for each heatmap
                for (int i = 0; i < MazePathItem.xPixels; i++)
                {
                    for (int j = 0; j < MazePathItem.zPixels; j++)
                    {
                        presHtmap[i, j] /= curMaze.curMazePaths.cPaths.Count;
                        minPres = Math.Min(presHtmap[i, j], minPres);
                        maxPres = Math.Max(presHtmap[i, j], maxPres);

                        entrHtmap[i, j] /= curMaze.curMazePaths.cPaths.Count;
                        minEntr = Math.Min(entrHtmap[i, j], minEntr);
                        maxEntr = Math.Max(entrHtmap[i, j], maxEntr);

                        timeHtmap[i, j] /= curMaze.curMazePaths.cPaths.Count;
                        minTime = Math.Min(timeHtmap[i, j], minTime);
                        maxTime = Math.Max(timeHtmap[i, j], maxTime);
                    }
                }

                SetHtmapType(selectHtmapType);
            }
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

            return Color.FromArgb(htmapAlpha, r, g, b);
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
                        bmp.SetPixel(i, j, Color.FromArgb(htmapAlpha, buttonBgColor.BackColor));
                    }
                }
            }

            return bmp;
        }

        Bitmap MakeColorbar()
        {
            // MakeColorbar parameter 0 less than parameter 1, parameter 1 is an approximation of the number of pixels
            double[,] colorbar = new double[Math.Min(MazePathItem.xPixels, MazePathItem.zPixels), (MazePathItem.xPixels + MazePathItem.zPixels) / 2];

            for (int i = 0; i < colorbar.GetLength(0); i++)
            {
                for (int j = 0; j < colorbar.GetLength(1); j++)
                {
                    colorbar[i, j] = selectMaxHeat - (selectMaxHeat - selectMinHeat) / (colorbar.GetLength(1) - 1) * j;
                }
            }

            return ToBitmap(colorbar);
        }

        void SetInterpolation(Graphics png)
        {
            trackBarSharpness.Enabled = true;

            switch (interpolation)
            {
                case InterpolationModes.None:
                    trackBarSharpness.Enabled = false;
                    png.InterpolationMode = InterpolationMode.NearestNeighbor;
                    break;
                case InterpolationModes.Bilinear:
                    png.InterpolationMode = InterpolationMode.Bilinear;
                    break;
                case InterpolationModes.HighQualityBilinear:
                    png.InterpolationMode = InterpolationMode.HighQualityBilinear;
                    break;
                case InterpolationModes.Bicubic:
                    png.InterpolationMode = InterpolationMode.Bicubic;
                    break;
                case InterpolationModes.HighQualityBicubic:
                    png.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    break;
                default:
                    break;
            }

            png.PixelOffsetMode = PixelOffsetMode.HighQuality;
            png.SmoothingMode = SmoothingMode.None;
            png.CompositingQuality = CompositingQuality.AssumeLinear;
        }

        static int width;
        static int height;
        static double scale;
        static double buffer = 2;
        private void pictureBoxHtmap_Paint(object sender, PaintEventArgs e)
        {
            int colorbarWidth = (int)(trackBarMidpoint.Width * 1.4);
            int colorbarHeight = trackBarMidpoint.Height;
            int colorbarTopLeftX = panelSettings.Location.X - colorbarWidth;
            int colorbarTopLeftZ = trackBarMidpoint.Location.Y - panelHtmap.AutoScrollPosition.Y * 2;

            width = panelHtmap.Width - colorbarWidth;
            height = (int)((MazePathItem.zPixels * MazePathItem.res + buffer * 2) / (MazePathItem.xPixels * MazePathItem.res + buffer * 2) * width);
            scale = 5 + (double)(panelHtmap.Width - panelSettings.Width) / (1302 - panelSettings.Width) * 12;

            pictureBoxHtmap_Paint(e.Graphics, true, width, height, colorbarWidth, colorbarHeight, colorbarTopLeftX, colorbarTopLeftZ, 8, Color.Black);

            if (selectRect)
            {
                PaintSelectRect(e.Graphics);
            }
        }

        static bool scroll = false;
        private void pictureBoxHtmap_Paint(Graphics png, bool autosize, int width, int height, int colorbarWidth, int colorbarHeight, int colorbarTopLeftX, int colorbarTopLeftZ, int colorbarLabelSize, Color colorbarLabelColor)
        {
            float mazeTopLeftX = (float)(-(MazePathItem.htmapXCenter - MazePathItem.xLowerRadius * MazePathItem.res - buffer) * scale);
            float mazeTopLeftZ = (float)(-(MazePathItem.htmapZCenter - MazePathItem.zLowerRadius * MazePathItem.res - buffer) * scale);
            int mazeWidth = (int)((MazePathItem.xPixels * MazePathItem.res + buffer * 2) * scale);
            int mazeHeight = (int)((MazePathItem.zPixels * MazePathItem.res + buffer * 2) * scale);

            int htmapTopLeftX = (int)(width * buffer / (MazePathItem.xPixels * MazePathItem.res + buffer * 2));
            int htmapTopLeftZ = (int)(htmapTopLeftX / (MazePathItem.xPixels * MazePathItem.res + buffer * 2) * (MazePathItem.zPixels * MazePathItem.res + buffer * 2));
            int htmapWidth = width - htmapTopLeftX * 2;
            int htmapHeight = height - htmapTopLeftZ * 2;


            // makes maze, heatmap, and colorbar
            Image mazeBmp = curMaze.PaintMazeToBuffer(mazeTopLeftX, mazeTopLeftZ, mazeWidth, mazeHeight, scale);
            Image anaRegnsBmp = curMaze.PaintAnalyzerItemsToBuffer(mazeTopLeftX, mazeTopLeftZ, mazeWidth, mazeHeight, scale);
            Bitmap heatBmp = ToBitmap(selectHtmap);
            Bitmap colorbarBmp = MakeColorbar();


            // autosize & scroll settings
            if (autosize)
            {
                if (!scroll)
                {
                    labelScroll.Location = new Point(labelScroll.Location.X, height);
                    scroll = true;
                }
                png.TranslateTransform(panelHtmap.AutoScrollPosition.X, panelHtmap.AutoScrollPosition.Y);
            }


            // draws maze, heatmap, colorbar, and colorbar labels
            SetInterpolation(png);

            Rectangle mazeDest = new Rectangle(0, 0, width, height);
            if (maze)
            {
                png.DrawImage(mazeBmp, mazeDest, 0, 0, mazeBmp.Width, mazeBmp.Height, GraphicsUnit.Pixel);
            }
            if (anaRegns)
            {
                png.DrawImage(anaRegnsBmp, mazeDest, 0, 0, anaRegnsBmp.Width, anaRegnsBmp.Height, GraphicsUnit.Pixel);
            }

            Rectangle htmapDest = new Rectangle(htmapTopLeftX, htmapTopLeftZ, htmapWidth, htmapHeight);
            Bitmap resizeheatBitmap = ResizeBitmap(heatBmp, sharpness, htmapWidth);
            png.DrawImage(resizeheatBitmap, htmapDest, 0, 0, resizeheatBitmap.Width, resizeheatBitmap.Height, GraphicsUnit.Pixel);

            Rectangle colorbarDest = new Rectangle(colorbarTopLeftX, colorbarTopLeftZ, colorbarWidth, colorbarHeight);
            png.DrawImage(colorbarBmp, colorbarDest, 0, 0, colorbarBmp.Width, colorbarBmp.Height, GraphicsUnit.Pixel);

            Font font = new Font(new FontFamily("times"), colorbarLabelSize);
            SolidBrush brush = new SolidBrush(colorbarLabelColor);
            png.DrawString(string.Format("{0}\n({1})", htmapTypeStr, htmapUnits), font, brush, colorbarTopLeftX - font.Size, (float)(colorbarTopLeftZ + colorbarHeight * -0.22));
            png.DrawString(string.Format("{0:0.00}", selectMaxHeat), font, brush, colorbarTopLeftX, colorbarTopLeftZ);
            png.DrawString(string.Format("{0:0.00}", selectMinHeat + (selectMaxHeat - selectMinHeat) * .75), font, brush, colorbarTopLeftX, (float)(colorbarTopLeftZ + colorbarHeight * 0.22));
            png.DrawString(string.Format("{0:0.00}", selectMinHeat + (selectMaxHeat - selectMinHeat) * .5), font, brush, colorbarTopLeftX, (float)(colorbarTopLeftZ + colorbarHeight * 0.44));
            png.DrawString(string.Format("{0:0.00}", selectMinHeat + (selectMaxHeat - selectMinHeat) * .25), font, brush, colorbarTopLeftX, (float)(colorbarTopLeftZ + colorbarHeight * 0.66));
            png.DrawString(string.Format("{0:0.00}", selectMinHeat), font, brush, colorbarTopLeftX, (float)(colorbarTopLeftZ + colorbarHeight * 0.88));
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

            using (Graphics png = Graphics.FromImage(resizeBmp))
            {
                png.InterpolationMode = InterpolationMode.NearestNeighbor;

                png.PixelOffsetMode = PixelOffsetMode.HighQuality;
                png.SmoothingMode = SmoothingMode.None;
                png.CompositingQuality = CompositingQuality.AssumeLinear;

                png.DrawImage(bmp, 0, 0, newBmpWidth, newBmpHeight);
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