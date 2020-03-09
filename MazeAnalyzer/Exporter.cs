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
    public partial class Exporter : Form
    {
        MazeViewer curMaze;
        public List<Main.ExpInfoTypes> currentExpPriority;

        public Exporter(ref MazeViewer inpMz)
        {
            InitializeComponent();
            curMaze = inpMz;
        }

        private void Exporter_Load(object sender, EventArgs e)
        {
            UpdatePathList();

            if (curMaze.curRegions.Regions.Count == 0)
            {
                checkBoxRegions.Enabled = false;
                checkBoxDistanceToLocations.Enabled = false;
            }

            checkBoxHeader.Checked = true;
            checkBoxTime.Checked = true;
            checkBoxPath.Checked = true;
        }

        private void UpdatePathList()
        {
            int i = 0;
            listBox1.Items.Clear();
            for (i = 0; i < curMaze.curMazePaths.cPaths.Count; i++)
            {
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
            if (!(checkBoxTime.Checked || checkBoxPath.Checked || checkBoxRegions.Checked || checkBoxHeader.Checked))
            {
                MessageBox.Show("Step 2: Please select at least one parameter!");
                return false;
            }
            return true;
        }

        private bool Check3()
        {
            //if (textBoxFile.Text=="")
            if(outPutFileName=="")
            {
                MessageBox.Show("Step 3: Please select output file name");
                return false;
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
            buttonExport.Enabled = false;
            Export();
            buttonExport.Enabled = true;
        }

        private string outPutFileName="";
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
                outPutFileName = a.FileName.Substring(0,a.FileName.LastIndexOf(".") );
              
                textBoxFile.Text = outPutFileName + "_{pathName+Index}.txt" ;
                string fname;
                int index;
                textBoxOutPutList.Text = "";
                for (int i = 0; i < listBox2.Items.Count; i++)
                {
                    index = GetPathNumber((string)listBox2.Items[i]);
                    fname = outPutFileName + "_" + listBox2.Items[i] + ".txt";
                    //curMaze.curMazePaths.cPaths[index]
                    textBoxOutPutList.Text += fname + "\t";
                }

            }
        }

        private int GetPathNumber(string inp)
        {
            string[] parts = inp.Split('_');
            foreach (string part in parts)
            {
                if (part.StartsWith("Path"))
                    return int.Parse(part.Substring(4));
            }
            return -1;
        }

        private void Export()
        {
            //if (textBoxFile.Text.EndsWith(".txt")==false)
            //{
            //    textBoxFile.Text += ".txt";
            //}
            //using outPutFileName

            int i;
            int index;
            string fname;
            int success=0;
            int failed = 0;
            //for each selected path...
            for (i = 0; i < listBox2.Items.Count; i++)
            {
                index = GetPathNumber((string)listBox2.Items[i]);
                //fname = textBoxFile.Text.Insert(textBoxFile.Text.Length - 4, "_" + listBox2.Items[i]);
                fname = outPutFileName + "_" + listBox2.Items[i] + ".txt";
                //curMaze.curMazePaths.cPaths[index]
                if (SavePath(fname, index))
                    success++;
                else
                    failed++;
            }

            fname = "Export process complete!\n\n";
            if(success>0)
                fname += "Successfully exported " + success + " file" + ((success > 1) ? "s" : "") ;
            if (failed > 0)
                fname +="Failed exporting " + failed + " file" + ((success > 1) ? "s" : "");

            MessageBox.Show(fname,"MazeAnalyzer",MessageBoxButtons.OK,MessageBoxIcon.Information);

        }

        private bool SavePath(string fname, int index)
        {
            try
            {
                StreamWriter fp = new StreamWriter(fname);

                if (fp == null)
                {
                    return false;
                }

                //save header
                if (checkBoxHeader.Checked)
                {
                    fp.WriteLine("Log Information:\t");
                    fp.WriteLine("Maze\t" + curMaze.curMazePaths.cPaths[index].Maze);
                    fp.WriteLine("Log\t" + curMaze.curMazePaths.cPaths[index].logFileName);
                    fp.WriteLine("Index\t" + curMaze.curMazePaths.cPaths[index].MelIndex);
                    fp.WriteLine("Walker\t" + curMaze.curMazePaths.cPaths[index].Walker);
                    fp.WriteLine("Date\t" + curMaze.curMazePaths.cPaths[index].Date);
                    fp.WriteLine();
                    fp.WriteLine("Experiment Information:\t");
                    fp.WriteLine("Group\t" + curMaze.curMazePaths.cPaths[index].ExpGroup);
                    fp.WriteLine("Condition\t" + curMaze.curMazePaths.cPaths[index].ExpCondition);
                    fp.WriteLine("Subject\t" + curMaze.curMazePaths.cPaths[index].ExpSubjectID);
                    fp.WriteLine("Session\t" + curMaze.curMazePaths.cPaths[index].ExpSession.ToString());
                    fp.WriteLine("Trial\t" + curMaze.curMazePaths.cPaths[index].ExpTrial.ToString());
                    fp.WriteLine();
                    if (checkBoxTime.Checked)
                        fp.Write("Time\t");
                    if (checkBoxPath.Checked)
                        fp.Write("X\tY\tZ\t");
                    if (checkBox2DViewAngle.Checked)
                        fp.Write("ViewAngle\t");

                    if (checkBoxDistanceToLocations.Checked)
                    {
                        foreach (MeasurementRegion m in curMaze.curRegions.Regions)
                        {
                            fp.Write(m.Name + "_Distance\t");
                        }
                    }
                    if (checkBoxRegions.Checked)
                    {
                        foreach (MeasurementRegion m in curMaze.curRegions.Regions)
                        {
                            fp.Write(m.Name + "_Inside?\t");
                        }
                    }
                    fp.WriteLine();
                }
                int i;
                if (checkBoxRegions.Checked || checkBoxDistanceToLocations.Checked)
                {
                    //if there are region calculations requested, initialize them
                    foreach (MeasurementRegion m in curMaze.curRegions.Regions)
                    {
                        m.InitializeDistanceCalculations();
                    }
                }
                for (i = 0; i< curMaze.curMazePaths.cPaths[index].PathPoints.Count; i++)
                {
                    if (checkBoxTime.Checked)
                        fp.Write(curMaze.curMazePaths.cPaths[index].PathTimes[i] + "\t");
                    if (checkBoxPath.Checked)
                    {
                        fp.Write(curMaze.curMazePaths.cPaths[index].PathPoints[i].X + "\t" + curMaze.curMazePaths.cPaths[index].PathPoints[i].Y + "\t" + curMaze.curMazePaths.cPaths[index].PathPoints[i].Z + "\t");
                    }
                    if(checkBox2DViewAngle.Checked)
                    {
                        MPoint vPos = new MPoint(curMaze.curMazePaths.cPaths[index].PathPoints[i].X, curMaze.curMazePaths.cPaths[index].PathPoints[i].Y, curMaze.curMazePaths.cPaths[index].PathPoints[i].Z);
                        MPoint vView = new MPoint(curMaze.curMazePaths.cPaths[index].PathViewPoints[i].X, curMaze.curMazePaths.cPaths[index].PathViewPoints[i].Y, curMaze.curMazePaths.cPaths[index].PathViewPoints[i].Z);
                        MPoint viewVector = vView - vPos;
                        float viewAngle = Tools.GetAngleDegree(new PointF(0,0),new PointF((float)viewVector.X,(float)viewVector.Z));
                        viewAngle = (viewAngle+180) % 360;
                        fp.Write(viewAngle + "\t");
                    }
                    if (checkBoxDistanceToLocations.Checked)
                    {
                        foreach (MeasurementRegion m in curMaze.curRegions.Regions)
                        {
                            fp.Write((m.Distance(curMaze.curMazePaths.cPaths[index].PathPoints[i])) + "\t");
                        }
                    }
                    if (checkBoxRegions.Checked)
                    {
                        foreach (MeasurementRegion m in curMaze.curRegions.Regions)
                        {
                            fp.Write((m.IsPointIn(curMaze.curMazePaths.cPaths[index].PathPoints[i]) ? "1" : "0") + "\t");
                        }
                    }
                    fp.WriteLine();
                }
                fp.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Add();
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Remove();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button_moveAllRight_Click(object sender, EventArgs e)
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
