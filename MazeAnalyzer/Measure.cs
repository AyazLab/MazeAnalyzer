using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MazeLib;
using MazeMaker;
using System.IO;

namespace MazeAnalyzer
{
    public partial class Measure : Form
    {
        MazeViewer curMaze;
        bool bOutput2File = false;

        public List<Main.ExpInfoTypes> currentExpPriority;
        public Measure(ref MazeViewer inpMz)
        {
            InitializeComponent();
            curMaze = inpMz;
        }

        double startTime = 0;
        double endTime = 0;

        private void Measure_Load(object sender, EventArgs e)
        {
            UpdatePathList();

            //checkBoxHeader.Checked = true;
            checkBoxTime.Checked = true;
            checkBoxPath.Checked = true;
            checkBoxVelocity.Checked = true;
            checkBoxReEntry.Checked = true;

            if (curMaze.curRegions.Regions.Count == 0)
            {
                checkBoxReEntry.Enabled = false;
                checkBoxReEntry.Checked = false;
            }

            ToogleVisibilityAdvancedEntry(false);

            tabControl1.TabPages.Remove(tabPage4);
        }

        private void UpdatePathList()
        {
            int i = 0;
            listBox1.Items.Clear();
            for (i = 0; i < curMaze.curMazePaths.cPaths.Count; i++)
            {
                //string pathName = curMaze.curMazePaths.cPaths[i].;
                listBox1.Items.Add(Main.BuildPathNameFromExp(curMaze.curMazePaths.cPaths[i], currentExpPriority));
            }
            
        }

        private bool Check1()
        {
            if (listBox2.Items.Count < 1)
            {
                MessageBox.Show("Step 1: Please select at least one path!");
                return false;
            }
            return true;
        }

        private bool Check2()
        {
            if (!(checkBoxTime.Checked || checkBoxPath.Checked || checkBoxVelocity.Checked || checkBoxReEntry.Checked))
            {
                MessageBox.Show("Step 2: Please select at least one parameter!");
                return false;
            }
            return true;
        }

        private bool Check3()
        {
            //if (textBoxFile.Text=="")
            //{
            //    MessageBox.Show("Step 3: Please select output file name");
            //    return false;
            //}
            if (checkBoxTimeInterval.Checked)
            {
                if (string.IsNullOrEmpty(textBoxStart.Text))
                {
                    startTime = 0;
                }
                else
                {
                    startTime = double.Parse(textBoxStart.Text) * 1000;
                }
                if (string.IsNullOrEmpty(textBoxEnd.Text))
                {
                    endTime = 0;
                }
                else
                {
                    endTime = double.Parse(textBoxEnd.Text) * 1000;                    
                }
                if (endTime < startTime)
                {                    
                    MessageBox.Show("Inconsistent time interval!");
                    return false;
                }
            }
            return true;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (!Check1()) return;
            tabControl1.SelectedIndex++;
        }

        private void buttonNext2_Click(object sender, EventArgs e)
        {
            if (!Check2()) return;
            tabControl1.SelectedIndex++;
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            Remove();
        }
        private void Remove()
        {
            if (listBox2.SelectedItems.Count > 0)
            {
                List<string> inp = new List<string>();
                int i = 0;
                for (i = 0; i < listBox2.SelectedItems.Count; i++)
                {
                    inp.Add(listBox2.SelectedItems[i].ToString());
                }
                for (i = 0; i < inp.Count; i++)
                {
                    listBox1.Items.Add(inp[i]);
                    listBox2.Items.Remove(inp[i]);
                }

            }
        }
        private void Add()
        {
            if (listBox1.SelectedItems.Count > 0)
            {
                List<string> inp = new List<string>();
                int i = 0;
                for (i = 0; i < listBox1.SelectedItems.Count; i++)
                {
                    inp.Add(listBox1.SelectedItems[i].ToString());
                }
                for (i = 0; i < inp.Count; i++)
                {
                    listBox2.Items.Add(inp[i]);
                    listBox1.Items.Remove(inp[i]);
                }
            }
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            Add();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //listBox1.SelectedIndex = -1;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //listBox2.SelectedIndex = -1;
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            if (!Check1()) return;
            if (!Check2()) return;
            if (!Check3()) return;            
            if (string.IsNullOrEmpty(textBoxFile.Text))
                bOutput2File = false;
            else
                bOutput2File = true;

            buttonExport.Enabled = false;
            //MessageBox.Show("This feature is not yet available!", "MazeAnalyzer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (DoMeasure())
            {
                if (tabControl1.TabPages.Contains(tabPage4) == false)
                {
                    tabControl1.TabPages.Add(tabPage4);
                }
                tabControl1.SelectedTab = tabPage4;
                
            }
            buttonExport.Enabled = true;
        }

        private void buttonSelectFileName_Click(object sender, EventArgs e)
        {
            //save to file..
            SaveFileDialog a = new SaveFileDialog();
            a.Filter = "Tab Seperated Text File | *.txt";
            a.FilterIndex = 1;
            a.DefaultExt = ".txt";
            a.RestoreDirectory = true;

            if (a.ShowDialog() == DialogResult.OK)
            {
                textBoxFile.Text = a.FileName;
            }
        }

        private int GetPathNumber(string inp)
        {
            string[] parts = inp.Split(',');
            foreach (string part in parts)
            {
                if (part.StartsWith("Path"))
                    return int.Parse(part.Substring(4));
                else if (part.StartsWith(" Path"))
                    return int.Parse(part.Substring(5));
            }
            return -1;
        }

        private string ListStringToDelimitedString(List<string> input,string delimit="\t")
        {
            string output = "";
            bool firstRun = true;
            foreach(string str in input)
            {
                if (!firstRun)
                    output += delimit;
                else
                    firstRun = false;
                output += str ;
            }
            return output;
        }

        private bool DoMeasure()
        {
            if (bOutput2File && textBoxFile.Text.EndsWith(".txt") == false)
            {
                textBoxFile.Text += ".txt";
            }

            int i;
            int index;
            string fname;
            int success=0;
            int failed = 0;

            richTextBox1.Text = "MazeAnalyzer Measurement Summary Report\nStarted at " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + "\n\n";

            List<string> rowText;
            StreamWriter fp = null;

            if (bOutput2File)
            {
                

               fp = new StreamWriter(textBoxFile.Text);
                if (fp == null)
                {
                    bOutput2File = false; 
                }
                else
                {
                    fp.WriteLine("MazeAnalyzer Measurement Summary v1");
                    fp.WriteLine(curMaze.mazeFileName);
                    rowText = BuildHeader();
                    fp.WriteLine(ListStringToDelimitedString(rowText));
                }
            }

            //for each selected path...
            for (i = 0; i < listBox2.Items.Count; i++)
            {
                index = GetPathNumber((string)listBox2.Items[i]);
                if (bOutput2File)
                    fname = textBoxFile.Text.Insert(textBoxFile.Text.Length - 4, "_" + listBox2.Items[i]);
                else
                    fname = listBox2.Items[i].ToString();
                //mzPath

                rowText = CalculateAndSaveForPath(fname, index);
                if (rowText!=null)
                { 
                    success++;
                    if (bOutput2File)
                        fp.WriteLine(ListStringToDelimitedString(rowText));
                }
                else
                    failed++;
            }

            fname = "Measurement complete!\n\n";
            if(success>0)
                fname += "Successfully processed " + success + " path" + ((success > 1) ? "s" : "") ;
            if (failed > 0)
                fname +="Failed measuring " + failed + " path" + ((success > 1) ? "s" : "");

            MessageBox.Show(fname,"MazeAnalyzer",MessageBoxButtons.OK,MessageBoxIcon.Information);
            if (bOutput2File)
                fp.Close();

            return true;
        }

        private List<string> CalculateAndSaveForPath(string fname, int index)
        {
            List<string> output = new List<string>();
            try
            {
                StreamWriter fp = null;
                if (checkBox_ExportDetailed.Checked)
                { 
                    if (bOutput2File)
                    {
                        fp = new StreamWriter(fname);
                        if (fp == null)
                        {
                            return null;
                        }
                    }
                }
                int i, j;
                double t=0, p=0;

                MazePathItem mzPath = curMaze.curMazePaths.cPaths[index];

                output.Add(mzPath.ExpGroup);
                output.Add(mzPath.ExpSubjectID);
                output.Add(mzPath.ExpCondition);
                output.Add(mzPath.ExpSession.ToString());
                output.Add(mzPath.ExpTrial.ToString());

                output.Add(mzPath.PathTime.ToString());
                output.Add(mzPath.PathLength.ToString());
                output.Add(mzPath.PathLengthXZ.ToString());
                

                MeasureItem[] measured = new MeasureItem[curMaze.curRegions.Regions.Count];
                for (j = 0; j < curMaze.curRegions.Regions.Count; j++)
                {
                    measured[j] = new MeasureItem();
                }
                MeasureItem total = new MeasureItem();
                total.reEntry = 1;

                for (i = 0; i < mzPath.PathPoints.Count; i++)
                {
                    //Check if we looking a custom time interval
                    if (checkBoxTimeInterval.Checked)
                    {
                        if (mzPath.PathTimes[i] <= startTime)
                            continue;
                        else if (mzPath.PathTimes[i] > endTime)
                            break;
                    }
                    
                    //calculate delta distance and delta time
                    if (i > 0)
                    {
                        t = (mzPath.PathTimes[i] - mzPath.PathTimes[i - 1]);
                        t = t / 1000;
                        p = mzPath.PathPoints[i].GetDistance(mzPath.PathPoints[i - 1]);                        
                        total.time += t;
                        total.pathLength += p;                        
                    }
                    
                    //Process each region with the current point
                    for (j = 0; j < curMaze.curRegions.Regions.Count; j++)
                    {
                        if (curMaze.curRegions.Regions[j].IsPointIn(mzPath.PathPoints[i]))
                        {
                            if (measured[j].lastPosIn && i > 0)
                            {
                                measured[j].time += t;
                                measured[j].pathLength += p;
                            }
                            else
                            {
                                measured[j].reEntry++;
                                measured[j].accessTimes.Add(total.time);
                                measured[j].accessTimePathLength.Add(total.pathLength);
                                measured[j].accessDescription.Add("Entered");
                            }
                            measured[j].lastPosIn = true;
                        }
                        else
                        {
                            if (measured[j].lastPosIn==true)
                            {
                                measured[j].accessTimes.Add(total.time-t);
                                measured[j].accessTimePathLength.Add(total.pathLength-p);
                                measured[j].accessDescription.Add("Exited");
                            }
                            measured[j].lastPosIn = false;
                        }
                        if (i + 1 >= mzPath.PathPoints.Count)
                        {
                            //end of path, should also mark as exit.
                            if (measured[j].lastPosIn == true)
                            {
                                measured[j].accessTimes.Add(total.time);
                                measured[j].accessTimePathLength.Add(total.pathLength);
                                measured[j].accessDescription.Add("Exited");
                            }
                        }
                    }
                }
                
                for (j = 0; j < curMaze.curRegions.Regions.Count; j++)
                {
                    if (measured[j].time>0)
                        measured[j].velocity =  measured[j].pathLength / measured[j].time;
                }
                total.velocity = total.pathLength / total.time;
                string str = "\n" + fname + "\n\n";
                if (bOutput2File&&checkBox_ExportDetailed.Checked)
                {
                    fp.WriteLine("MazeAnalyzer Measurement Summary File\tv1");
                    fp.WriteLine("Generated at\t" + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
                    fp.WriteLine();
                    fp.WriteLine("Group\tCondition\tSubject\tSession\tTrial");
                    fp.WriteLine(mzPath.ExpGroup + "\t" + mzPath.ExpCondition + "\t" + mzPath.ExpSubjectID + "\t" + mzPath.ExpSession.ToString() + "\t" + mzPath.ExpTrial.ToString());
                    fp.WriteLine();
                    fp.Write("Region\t");
                }
                str += "Region\t";
                if (checkBoxTime.Checked)
                {
                    if (bOutput2File && checkBox_ExportDetailed.Checked)
                        fp.Write("Time\t");
                    str += "Time\t";
                }
                if (checkBoxPath.Checked)
                {
                    if (bOutput2File && checkBox_ExportDetailed.Checked)
                        fp.Write("Path\t");
                    str += "Path\t";
                }
                if (checkBoxVelocity.Checked)
                {
                    if (bOutput2File && checkBox_ExportDetailed.Checked)
                        fp.Write("Velocity\t");
                    str += "Velocity\t";
                }
                if (checkBoxReEntry.Checked)
                {
                    if (bOutput2File && checkBox_ExportDetailed.Checked)
                        fp.Write("Access\t");
                    str += "Access\t";
                }
                if (bOutput2File && checkBox_ExportDetailed.Checked)
                    fp.WriteLine();
                str += "\n";
                for (j = 0; j < curMaze.curRegions.Regions.Count; j++)
                {
                    if (bOutput2File && checkBox_ExportDetailed.Checked)
                        fp.Write(curMaze.curRegions.Regions[j].Name + "\t");
                    str += curMaze.curRegions.Regions[j].Name + "\t";
                    if (checkBoxTime.Checked)
                    {
                        if (bOutput2File && checkBox_ExportDetailed.Checked)
                            fp.Write(measured[j].time.ToString("#.###") + "\t");
                        str += measured[j].time.ToString("#.###") + "\t";
                        if(curMaze.curRegions.Regions[j].Vertices.Count>2)
                            output.Add(measured[j].time.ToString("#.###"));

                    }
                    if (checkBoxPath.Checked)
                    {
                        if (bOutput2File && checkBox_ExportDetailed.Checked)
                            fp.Write(measured[j].pathLength.ToString("#.###") + "\t");
                        str += measured[j].pathLength.ToString("#.###") + "\t";
                        if (curMaze.curRegions.Regions[j].Vertices.Count > 2)
                            output.Add(measured[j].pathLength.ToString("#.###"));
                    }
                    if (checkBoxVelocity.Checked)
                    {
                        if (bOutput2File && checkBox_ExportDetailed.Checked)
                            fp.Write(measured[j].velocity.ToString("#.###") + "\t");
                        str += measured[j].velocity.ToString("#.###") + "\t";
                        if (curMaze.curRegions.Regions[j].Vertices.Count > 2)
                            output.Add(measured[j].velocity.ToString("#.###"));
                    }
                    if (checkBoxReEntry.Checked)
                    {
                        if (bOutput2File && checkBox_ExportDetailed.Checked)
                            fp.Write(measured[j].reEntry + "\t");
                        str += measured[j].reEntry + "\t";
                        if (curMaze.curRegions.Regions[j].Vertices.Count > 2)
                            output.Add(measured[j].reEntry.ToString("#.###"));
                    }
                    str += "\n";
                    if (bOutput2File && checkBox_ExportDetailed.Checked)
                        fp.WriteLine();
                }
                str += "---------------------------------------------------------------------\n";
                #region report overall for the path..
                if (bOutput2File && checkBox_ExportDetailed.Checked)
                    fp.Write("Overall\t");
                str += "Overall\t";
                if (checkBoxTime.Checked)
                {
                    if (bOutput2File && checkBox_ExportDetailed.Checked)
                        fp.Write(total.time.ToString("#.###") + "\t");
                    str += total.time.ToString("#.###") + "\t";
                }
                if (checkBoxPath.Checked)
                {
                    if (bOutput2File && checkBox_ExportDetailed.Checked)
                        fp.Write(total.pathLength.ToString("#.###") + "\t");
                    str += total.pathLength.ToString("#.###") + "\t";
                }
                if (checkBoxVelocity.Checked)
                {
                    if (bOutput2File && checkBox_ExportDetailed.Checked)
                        fp.Write(total.velocity.ToString("#.###") + "\t");
                    str += total.velocity.ToString("#.###") + "\t";
                }
                if (checkBoxReEntry.Checked)
                {
                    if (bOutput2File && checkBox_ExportDetailed.Checked)
                        fp.Write(total.reEntry + "\t");
                    str += total.reEntry + "\t";
                }
                str += "\n";
                if (bOutput2File && checkBox_ExportDetailed.Checked)
                    fp.WriteLine();
                #endregion
                //print total...

                if (bOutput2File && checkBox_ExportDetailed.Checked)
                    fp.Write("\r\n===========================================\r\n\r\n");
                str += "\r\n===========================================\r\n\r\n";

                if (bOutput2File && checkBox_ExportDetailed.Checked)
                    fp.Write("Detailed entry/exit for each region\r\n(time of access  path length at access  type of access)\r\n");
                str += "Detailed entry/exit for each region\n(time of access  path length at access  type of access)\n";

                bool accessLogFound = false;
                for (j = 0; j < curMaze.curRegions.Regions.Count; j++)
                {
                    if (bOutput2File && checkBox_ExportDetailed.Checked)
                        fp.Write("\r\n" + curMaze.curRegions.Regions[j].Name + "\r\n");
                    str += "\n"+ curMaze.curRegions.Regions[j].Name + "\n";

                    accessLogFound = false;
                    if (measured[j].accessTimes.Count > 0 && measured[j].accessTimePathLength.Count > 0 && measured[j].accessDescription.Count > 0)
                    {
                        if ((measured[j].accessTimes.Count == measured[j].accessTimePathLength.Count) && (measured[j].accessTimePathLength.Count == measured[j].accessDescription.Count))
                        {
                            accessLogFound = true;
                            for (i = 0; i < measured[j].accessTimes.Count; i++)
                            {
                                if (bOutput2File && checkBox_ExportDetailed.Checked)
                                    fp.Write(measured[j].accessTimes[i].ToString("#.###") + "\t" + measured[j].accessTimePathLength[i].ToString("#.###") + "\t" + measured[j].accessDescription[i] + "\r\n");
                                str += measured[j].accessTimes[i].ToString("#.###") + "\t" + measured[j].accessTimePathLength[i].ToString("#.###") + "\t" + measured[j].accessDescription[i] + "\n";
                            }
                        }
                    }
                    if (accessLogFound == false)
                    {
                        if (bOutput2File && checkBox_ExportDetailed.Checked)
                            fp.Write("No access log for this region\r\n");
                        str += "No access log for this region\n";
                    }
                    
                }

                output.Add("\n");
                if (bOutput2File && checkBox_ExportDetailed.Checked)
                    fp.Close();
                richTextBox1.Text += str;
                return output;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private List<string> BuildHeader()
        {
            List<string> output = new List<string>();
            try
            {
               

                output.Add("Group");
                output.Add("Subject");
                output.Add("Condition");
                output.Add("Session");
                output.Add("Trial");

                output.Add("PathTime");
                output.Add("PathLength");
                output.Add("PathLengthXZ");


                foreach(MeasurementRegion mzR in curMaze.curRegions.Regions)
                {
                    if(mzR.Vertices.Count>2)
                    { 
                        //output.Add(mzR.Name);
                        if (checkBoxTime.Checked)
                        {
                            output.Add(mzR.Name + "_Time");
                        }
                        if (checkBoxPath.Checked)
                        {
                            output.Add(mzR.Name + "_PathLength");
                        }
                        if (checkBoxVelocity.Checked)
                        {
                            output.Add(mzR.Name + "_AvgVelocity");
                        }
                        if (checkBoxReEntry.Checked)
                        {
                            output.Add(mzR.Name + "_TimesEntered");
                        }
                    }

                }

                return output;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private void ToogleVisibilityAdvancedEntry(bool inp)
        {
            label13.Visible = inp;
            label14.Visible = inp;
            textBoxStart.Visible = inp;
            textBoxEnd.Visible = inp;
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Add();
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Remove();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTimeInterval.Checked)
            {
                ToogleVisibilityAdvancedEntry(true);
            }
            else
                ToogleVisibilityAdvancedEntry(false);
        }

        private void button_MoveAllRight_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                List<string> inp = new List<string>();
                int i = 0;
                for (i = 0; i < listBox1.Items.Count; i++)
                {
                    inp.Add(listBox1.Items[i].ToString());
                }
                for (i = 0; i < inp.Count; i++)
                {
                    listBox2.Items.Add(inp[i]);
                    listBox1.Items.Remove(inp[i]);
                }
            }
        }
    }
}
