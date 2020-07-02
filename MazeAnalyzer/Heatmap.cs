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
        static Color cusMinColor = Color.FromArgb(128, 255, 128);
        static Color cusMaxColor = Color.FromArgb(128, 0, 255);

        static bool cusMidColorShow = true;
        static bool midColorShow;
        static Color cusMidColor = Color.FromArgb(128, 255, 255);

        static bool cusTransparentBg = true;
        static bool transparentBg;
        static Color cusBgColor = Color.White;

        static int preset = 0;
        static double midpoint = 0.1;
        static int opacity = 178;

        static int interpolation = 2;

        static bool maze = true;
        static bool anaRegns = false;

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

        static double[,] selectHtmap = new double[0, 0];
        static double selectMinHeat;
        static double selectMaxHeat;

        static string paths; // a string containing the selected paths for the csv files

        public Heatmap(MazeViewer inpMz)
        {
            InitializeComponent();

            curMaze = inpMz;
            AddHtmap(curMaze);

            DoubleBuffered = true;
        }

        private void HeatmapConfig_Load(object sender, EventArgs e) // matches the internal and external heatmap settings
        {
            buttonMinColor.BackColor = cusMinColor;
            buttonMaxColor.BackColor = cusMaxColor;

            checkBoxMidColor.Checked = cusMidColorShow;
            buttonMidColor.BackColor = cusMidColor;

            checkBoxTransparentBg.Checked = cusTransparentBg;
            buttonBgColor.BackColor = cusBgColor;

            comboBoxPreset.SelectedIndex = preset;
            trackBarMidpoint.Value = Convert.ToInt32(midpoint * 10);
            trackBarOpacity.Value = opacity;
            labelOpacityPercent.Text = string.Format("{0}%", Math.Round((double)opacity / 255 * 100, 4, MidpointRounding.AwayFromZero));


            textBoxRes.Text = Convert.ToString(MazePathItem.res);
            textBoxRes.BackColor = SystemColors.Control;

            double offsetX = Math.Round(MazePathItem.offsetX, 2, MidpointRounding.AwayFromZero);
            double offsetZ = Math.Round(MazePathItem.offsetZ, 2, MidpointRounding.AwayFromZero);
            textBoxOffset.Text = string.Format("{0}, {1}", offsetX, offsetZ);
            textBoxOffset.BackColor = SystemColors.Control;
            buttonOffset.Image = new Bitmap(buttonOffset.Image, 12, 12);

            comboBoxInterpolation.SelectedIndex = interpolation;


            if (selectHtmap == presHtmap)
            {
                radioButtonPres.Checked = true;
            }
            else if (selectHtmap == entrHtmap)
            {
                radioButtonEntr.Checked = true;
            }
            else if (selectHtmap == timeHtmap)
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
                preset = 3;
                comboBoxPreset.SelectedIndex = preset;
                cusMinColor = colorDialog.Color;

                buttonMinColor.BackColor = cusMinColor;
                Refresh();
            }
        }

        private void buttonMaxColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                preset = 3;
                comboBoxPreset.SelectedIndex = preset;
                cusMaxColor = colorDialog.Color;

                buttonMaxColor.BackColor = cusMaxColor;
                Refresh();
            }
        }

        private void checkBoxMidColor_Click(object sender, EventArgs e)
        {
            preset = 3;
            comboBoxPreset.SelectedIndex = preset;
            midColorShow = !midColorShow;
            checkBoxMidColor.Checked = midColorShow;
            cusMidColorShow = checkBoxMidColor.Checked;

            Refresh();
        }

        private void buttonMidColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                preset = 3;
                comboBoxPreset.SelectedIndex = preset;
                cusMidColor = colorDialog.Color;

                buttonMidColor.BackColor = cusMidColor;
                Refresh();
            }
        }

        private void checkBoxBgTransparent_Click(object sender, EventArgs e)
        {
            preset = 3;
            comboBoxPreset.SelectedIndex = preset;
            transparentBg = !transparentBg;
            checkBoxTransparentBg.Checked = transparentBg;
            cusTransparentBg = checkBoxTransparentBg.Checked;

            Refresh();
        }

        private void buttonBgColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                preset = 3;
                comboBoxPreset.SelectedIndex = preset;
                cusBgColor = colorDialog.Color;

                buttonBgColor.BackColor = cusBgColor;
                Refresh();
            }
        }

        private void comboBoxPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            preset = comboBoxPreset.SelectedIndex;

            if (preset == 0)
            {
                buttonMinColor.BackColor = Color.FromArgb(128, 255, 128);
                buttonMidColor.BackColor = Color.FromArgb(128, 255, 255);
                buttonMaxColor.BackColor = Color.FromArgb(128, 0, 255);
            }
            else if (preset == 1)
            {
                buttonMinColor.BackColor = Color.White;
                buttonMidColor.BackColor = Color.FromArgb(255, 255, 128);
                buttonMaxColor.BackColor = Color.FromArgb(255, 128, 128);
            }
            else if (preset == 2)
            {
                buttonMinColor.BackColor = Color.FromArgb(128, 128, 128);
                buttonMidColor.BackColor = Color.FromArgb(192, 192, 192);
                buttonMaxColor.BackColor = Color.White;
            }
            else if (preset == 3)
            {
                buttonMinColor.BackColor = cusMinColor;
                buttonMaxColor.BackColor = cusMaxColor;

                checkBoxMidColor.Checked = cusMidColorShow;
                buttonMidColor.BackColor = cusMidColor;

                checkBoxTransparentBg.Checked = cusTransparentBg;
                buttonBgColor.BackColor = cusBgColor;
            }
            if (preset != 3)
            {
                midColorShow = true;
                checkBoxMidColor.Checked = midColorShow;
                transparentBg = true;
                checkBoxTransparentBg.Checked = transparentBg;
                buttonBgColor.BackColor = Color.White;
            }

            Refresh();
        }

        private void trackBarMidpoint_Scroll(object sender, EventArgs e)
        {
            midpoint = Convert.ToDouble(trackBarMidpoint.Value) / 10;

            if (checkBoxMidColor.Checked)
            {
                Refresh();
            }
        }

        private void trackBarOpacity_Scroll(object sender, EventArgs e)
        {
            opacity = trackBarOpacity.Value;

            labelOpacityPercent.Text = string.Format("{0}%", Math.Round((double)opacity / 255 * 100, 4, MidpointRounding.AwayFromZero));
            Refresh();
        }


        // resolution, offset, & interpolation events
        private void textBoxRes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // 0.2 - 15 maze units / pixel, beyond this range, heatmap calculations and colorbar pixel approximations could slow down
                MazePathItem.res = Convert.ToDouble(textBoxRes.Text);

                textBoxRes.BackColor = SystemColors.Control;
                UpdateHtmap();
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
                selectHtmap = new double[0, 0];
                UpdateHtmap();
            }
        }

        bool offsetClickMode = false;
        private void buttonOffset_Click(object sender, EventArgs e)
        {
            if (offsetClickMode)
            {
                offsetClickMode = false;
                Cursor = Cursors.Default;
            }
            else
            {
                offsetClickMode = true;
                Cursor = Cursors.Cross;
            }
        }

        double hm_mzUnits_topLeftX;
        double hm_mzUnits_width;

        double hm_mzUnits_topLeftZ;
        double hm_mzUnits_height;
        private void pictureBoxHtmap_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouseCoord = (MouseEventArgs)e;
            PointF mazeCoord = MouseToMazeCoord(mouseCoord.X, mouseCoord.Y);

            if (offsetClickMode)
            {
                textBoxOffset.Text = string.Format("{0}, {1}", mazeCoord.X, mazeCoord.Y);
                SetOffset();
                offsetClickMode = false;

                textBoxOffset.BackColor = SystemColors.Control;
                Cursor = Cursors.Default;
            }
            else
            {
                ToolTip offset = new ToolTip();

                double hm_mzUnits_botRightX = hm_mzUnits_topLeftX + hm_mzUnits_width;
                double hm_mzUnits_botRightZ = hm_mzUnits_topLeftZ + hm_mzUnits_height;

                double heat = 0;
                if (hm_mzUnits_topLeftX <= mazeCoord.X && mazeCoord.X <= hm_mzUnits_botRightX && hm_mzUnits_topLeftZ <= mazeCoord.Y && mazeCoord.Y <= hm_mzUnits_botRightZ)
                {
                    Point pixelCoord = MazePathItem.MapCoord(mazeCoord.X, mazeCoord.Y);
                    heat = Math.Round(selectHtmap[pixelCoord.X, pixelCoord.Y], 2, MidpointRounding.AwayFromZero);
                }

                offset.SetToolTip(pictureBoxHtmap, string.Format("Coord: ({0}, {1}); Heat: {2}", mazeCoord.X, mazeCoord.Y, heat));
            }
        }

        private PointF MouseToMazeCoord(double x, double z)
        {
            PointF mazeCoord = new PointF();

            hm_mzUnits_topLeftX = MazePathItem.htmapXCenter - MazePathItem.xLowerRadius * MazePathItem.res - buffer;
            hm_mzUnits_width = MazePathItem.xPixels * MazePathItem.res + buffer * 2;

            mazeCoord.X = (float)Math.Round(hm_mzUnits_topLeftX + x / width * hm_mzUnits_width, 2, MidpointRounding.AwayFromZero);

            hm_mzUnits_topLeftZ = MazePathItem.htmapZCenter - MazePathItem.zLowerRadius * MazePathItem.res - buffer;
            hm_mzUnits_height = MazePathItem.zPixels * MazePathItem.res + buffer * 2;

            mazeCoord.Y = (float)Math.Round(hm_mzUnits_topLeftZ + (z - panelHtmap.AutoScrollPosition.Y) / height * hm_mzUnits_height, 2, MidpointRounding.AwayFromZero);

            return mazeCoord;
        }

        void UpdateHtmap()
        {
            MazePathItem.UpdatePixels();

            AddHtmap(curMaze);

            if (radioButtonPres.Checked)
            {
                selectHtmap = presHtmap;
                selectMaxHeat = maxPres;
                selectMinHeat = minPres;
            }
            else if (radioButtonEntr.Checked)
            {
                selectHtmap = entrHtmap;
                selectMaxHeat = maxEntr;
                selectMinHeat = minEntr;
            }
            else if (radioButtonTime.Checked)
            {
                selectHtmap = timeHtmap;
                selectMaxHeat = maxTime;
                selectMinHeat = minTime;
            }

            Refresh();
        }

        private void comboBoxInterpolation_SelectedIndexChanged(object sender, EventArgs e)
        {
            interpolation = comboBoxInterpolation.SelectedIndex;
            Refresh();
        }


        // selected heatmap events
        private void radioButtonPres_CheckedChanged(object sender, EventArgs e)
        {
            selectHtmap = presHtmap;
            selectMaxHeat = maxPres;
            selectMinHeat = minPres;

            Refresh();
        }
        private void radioButtonEntr_CheckedChanged(object sender, EventArgs e)
        {
            selectHtmap = entrHtmap;
            selectMaxHeat = maxEntr;
            selectMinHeat = minEntr;

            Refresh();
        }
        private void radioButtonTime_CheckedChanged(object sender, EventArgs e)
        {
            selectHtmap = timeHtmap;
            selectMaxHeat = maxTime;
            selectMinHeat = minTime;

            Refresh();
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
            if (radioButtonPres.Checked)
            {
                SaveCsv("Presence", presHtmap);
            }
            else if (radioButtonEntr.Checked)
            {
                SaveCsv("Entrance", entrHtmap);
            }
            else if (radioButtonTime.Checked)
            {
                SaveCsv("Time", timeHtmap);
            }
        }

        void SaveCsv(string type, double[,] htmap)
        {
            SaveFileDialog saveCsv = new SaveFileDialog();
            saveCsv.Filter = "CSV file(*.csv)| *.csv | All Files(*.*) | *.* ";

            if (saveCsv.ShowDialog() == DialogResult.OK)
            {
                StreamWriter csv = new StreamWriter(saveCsv.FileName);

                csv.Write(string.Format("Maze Analyzer Heatmap Export: {0}\n", type));
                csv.Write(string.Format("Date Exported: {0}\n", DateTime.Now));
                csv.Write(string.Format("Project File Name: {0}\n", curMaze.projectInfo.ProjectFilename));
                csv.Write(string.Format("Project Name: {0}\n", curMaze.projectInfo.ProjectFilename.Split('\\')[curMaze.projectInfo.ProjectFilename.Split('\\').Length - 1]));
                csv.Write(string.Format("Maze Name: {0}.maz\n", curMaze.mazeFileName));
                csv.Write(string.Format("Paths Exported: {0}\n", paths));
                csv.Write(string.Format("Heatmap Resolution: {0} Maze Units / Pixel\n", MazePathItem.res));
                csv.Write("\n");

                csv.Write("xz, ");
                string xs = GetCoords(MazePathItem.offsetX, MazePathItem.xLowerRadius, MazePathItem.xPixels);
                csv.Write(xs.Substring(0, xs.Length - 2));
                csv.Write("\n");

                string[] zs = GetCoords(MazePathItem.offsetZ, MazePathItem.zLowerRadius, MazePathItem.zPixels).Split(' ');

                for (int i = 0; i < htmap.GetLength(1); i++)
                {
                    csv.Write(zs[i] + " ");
                    for (int j = 0; j < htmap.GetLength(0); j++)
                    {
                        csv.Write(htmap[j, i]);

                        if (j != htmap.GetLength(0) - 1)
                        {
                            csv.Write(", ");
                        }
                    }
                    csv.Write("\n");
                }
                csv.Close();
            }
        }

        string GetCoords(double offset, int lowerRadius, int pixels) // returns the coordinates borders of all the pixels
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
                pictureBoxHtmap_Paint(png, false, width, height, colorbarWidth, colorbarHeight, colorbarTopLeftX, colorbarTopLeftZ, 170, Color.White, 1);
            }

            return bmp;
        }

        void AddHtmap(MazeViewer inpMz) // makes the heatmaps and adds them together [the numbers]
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
                        mzp.MakeHtmap();

                        for (int i = 0; i < MazePathItem.xPixels; i++)
                        {
                            for (int j = 0; j < MazePathItem.zPixels; j++)
                            {
                                presHtmap[i, j] += mzp.presHtmap[i, j]; // bool to int switch
                                entrHtmap[i, j] += mzp.entrHtmap[i, j];
                                timeHtmap[i, j] += mzp.timeHtmap[i, j] / 1000; // millisecond to second conversion
                            }
                        }

                        paths += string.Format("\"TRI {0} ", mzp.ExpTrial);
                        if (mzp.ExpGroup != "")
                        {
                            paths += string.Format("GRO {0} ", mzp.ExpGroup);
                        }
                        if (mzp.ExpCondition != "")
                        {
                            paths += string.Format("CON {0} ", mzp.ExpCondition);
                        }
                        paths += string.Format("SUB {0} ", mzp.ExpSubjectID);
                        paths += string.Format("SES {0}\"; ", mzp.ExpSession);
                    }
                }

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

                // default heatmap, change default here
                selectHtmap = presHtmap;
                selectMinHeat = minPres;
                selectMaxHeat = maxPres;
            }
        }

        Color ToColor(double heat) // takes a heat number and returns a color
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
                if (heat / (selectMaxHeat - selectMinHeat) >= midpoint) // checks heat percentile
                {
                    r = Convert.ToInt32(midR + (maxR - midR) * (heat / (selectMaxHeat - selectMinHeat) - midpoint) / (1 - midpoint));
                    g = Convert.ToInt32(midG + (maxG - midG) * (heat / (selectMaxHeat - selectMinHeat) - midpoint) / (1 - midpoint));
                    b = Convert.ToInt32(midB + (maxB - midB) * (heat / (selectMaxHeat - selectMinHeat) - midpoint) / (1 - midpoint));
                }
                else
                {
                    r = Convert.ToInt32(minR + (midR - minR) * heat / (selectMaxHeat - selectMinHeat) / midpoint);
                    g = Convert.ToInt32(minG + (midG - minG) * heat / (selectMaxHeat - selectMinHeat) / midpoint);
                    b = Convert.ToInt32(minB + (midB - minB) * heat / (selectMaxHeat - selectMinHeat) / midpoint);
                }
            }
            else
            {
                r = Convert.ToInt32(minR + (maxR - minR) * heat / (selectMaxHeat - selectMinHeat));
                g = Convert.ToInt32(minG + (maxG - minG) * heat / (selectMaxHeat - selectMinHeat));
                b = Convert.ToInt32(minB + (maxB - minB) * heat / (selectMaxHeat - selectMinHeat));
            }

            return Color.FromArgb(opacity, r, g, b);
        }

        Bitmap ToBitmap(double[,] htmap) // makes a bitmap for the colorbar and heatmap
        {
            Bitmap bitmap = new Bitmap(htmap.GetLength(0), htmap.GetLength(1));

            for (int i = 0; i < htmap.GetLength(0); i++)
            {
                for (int j = 0; j < htmap.GetLength(1); j++)
                {
                    bitmap.SetPixel(i, j, ToColor(htmap[i, j]));

                    if (htmap[i, j] == 0 && checkBoxTransparentBg.Checked) // sets background to transparent or background color
                    {
                        bitmap.SetPixel(i, j, Color.Transparent);
                    }
                    else if (htmap[i, j] == 0)
                    {
                        bitmap.SetPixel(i, j, Color.FromArgb(opacity, buttonBgColor.BackColor));
                    }
                }
            }

            return bitmap;
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
            if (interpolation == 0)
            {
                png.InterpolationMode = InterpolationMode.Default;
            }
            else if (interpolation == 1)
            {
                png.InterpolationMode = InterpolationMode.HighQualityBicubic;
            }
            else if (interpolation == 2)
            {
                png.InterpolationMode = InterpolationMode.NearestNeighbor;
            }

            png.PixelOffsetMode = PixelOffsetMode.Half; // by default, 2x+ scaled images are drawn offset half a pixel
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

            pictureBoxHtmap_Paint(e.Graphics, true, width, height, colorbarWidth, colorbarHeight, colorbarTopLeftX, colorbarTopLeftZ, 8, Color.Black, 2);
        }

        static bool scroll = false;
        private void pictureBoxHtmap_Paint(Graphics png, bool autosize, int width, int height, int colorbarWidth, int colorbarHeight, int colorbarTopLeftX, int colorbarTopLeftZ, int colorbarLabelSize, Color colorbarLabelColor, int colorbarLabelDisplacement)
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
            Image mazeBitmap = curMaze.PaintMazeToBuffer(mazeTopLeftX, mazeTopLeftZ, mazeWidth, mazeHeight, scale);
            Image anaRegnsBitmap = curMaze.PaintAnalyzerItemsToBuffer(mazeTopLeftX, mazeTopLeftZ, mazeWidth, mazeHeight, scale);
            Bitmap heatBitmap = ToBitmap(selectHtmap);
            Bitmap colorbarBitmap = MakeColorbar();


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
                png.DrawImage(mazeBitmap, mazeDest, 0, 0, mazeBitmap.Width, mazeBitmap.Height, GraphicsUnit.Pixel);
            }
            if (anaRegns)
            {
                png.DrawImage(anaRegnsBitmap, mazeDest, 0, 0, anaRegnsBitmap.Width, anaRegnsBitmap.Height, GraphicsUnit.Pixel);
            }

            Rectangle htmapDest = new Rectangle(htmapTopLeftX, htmapTopLeftZ, htmapWidth, htmapHeight);
            png.DrawImage(heatBitmap, htmapDest, 0, 0, heatBitmap.Width, heatBitmap.Height, GraphicsUnit.Pixel);

            Rectangle colorbarDest = new Rectangle(colorbarTopLeftX, colorbarTopLeftZ, colorbarWidth, colorbarHeight);
            png.DrawImage(colorbarBitmap, colorbarDest, 0, 0, colorbarBitmap.Width, colorbarBitmap.Height, GraphicsUnit.Pixel);

            string units;
            if (radioButtonPres.Checked)
            {
                units = "Presence:\n(probability)";
            }
            else if (radioButtonEntr.Checked)
            {
                units = "  Entrance:\n  (# of times)";
            }
            else
            {
                units = "    Time:\n    (seconds)";
            }
            Font font = new Font(new FontFamily("times"), colorbarLabelSize);
            SolidBrush brush = new SolidBrush(colorbarLabelColor);
            png.DrawString(units, font, brush, colorbarTopLeftX - font.Size * colorbarLabelDisplacement, (float)(colorbarTopLeftZ + colorbarHeight * -0.22));
            png.DrawString(string.Format("{0}", selectMaxHeat), font, brush, colorbarTopLeftX, colorbarTopLeftZ);
            png.DrawString(string.Format("{0}", selectMinHeat + (selectMaxHeat - selectMinHeat) * .75), font, brush, colorbarTopLeftX, (float)(colorbarTopLeftZ + colorbarHeight * 0.22));
            png.DrawString(string.Format("{0}", selectMinHeat + (selectMaxHeat - selectMinHeat) * .5), font, brush, colorbarTopLeftX, (float)(colorbarTopLeftZ + colorbarHeight * 0.44));
            png.DrawString(string.Format("{0}", selectMinHeat + (selectMaxHeat - selectMinHeat) * .25), font, brush, colorbarTopLeftX, (float)(colorbarTopLeftZ + colorbarHeight * 0.66));
            png.DrawString(string.Format("{0}", selectMinHeat), font, brush, colorbarTopLeftX, (float)(colorbarTopLeftZ + colorbarHeight * 0.88));
        }

        private void panelHtmap_Resize(object sender, EventArgs e)
        {
            //((Control)sender).Invalidate();
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