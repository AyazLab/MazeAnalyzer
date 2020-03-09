using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using MazeMaker;


namespace MazeAnalyzer
{
    public partial class LogProcessor : Form
    {        
        bool preparing = false;
        int counter = 0;
        private volatile bool continueOperation = true;
        private Thread trd=null;


        public LogProcessor()
        {
            InitializeComponent();
        }
        private void LogProcessor_Load(object sender, EventArgs e)
        {
            Reset();
            //trd = new Thread(new ThreadStart(this.Process));
            //trd.IsBackground = true;
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {            
            if(preparing)
            {
                if (MessageBox.Show("Do you want to quit without processing the log file", "Close?", MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }
            this.Close();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            //start
            //if (!preparing || textBoxInput.Text == "" || textBoxOutput.Text == "")
            //    return;
            if (textBoxInput.Text == "")
            {
                MessageBox.Show("No input file, please select a log file to parse!");
                return;
            }
            if (textBoxOutput.Text == "")
            {
                MessageBox.Show("No output directory set, please select an output directory!");
                return;
            }
            SetButtons(false);
            preparing = false;
            labelResult.Text = "Please wait...";
            //timer1.Start(); 

            trd = new Thread(new ThreadStart(this.Process));
            trd.IsBackground = true;
            trd.Start();
        }

        private void buttonInput_Click(object sender, EventArgs e)
        {
            OpenFileDialog a = new OpenFileDialog();
            a.Filter = "Text files (*.txt)| *.txt| Excel files (*.xls) | *.xls ";
            if(DialogResult.OK==a.ShowDialog())
            {
                textBoxInput.Text = a.FileName;
                preparing = true;
                counter = 0;
                labelResult.Text = "No results.";
            }
        }

        private void buttonOutput_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog a = new FolderBrowserDialog();
            a.Description = "Please select a folder to save output files";
            if(DialogResult.OK == a.ShowDialog())
            {
                textBoxOutput.Text = a.SelectedPath;
                preparing = true;
                labelResult.Text = "No results.";
            }
        }

        private void Reset()
        {
            textBoxInput.Text = "";
            textBoxOutput.Text = "";
            labelResult.Text = "No results.";
            preparing = false;
            counter = 0;
            progressBar1.Visible = false;
            buttonCancel.Visible = false;
        }




        private void Process()
        {            
            try
            {
                string fname =  Tools.GetShortFileName(textBoxInput.Text);
                string maze="";
                string buf="", line;
                string ret="";
                string eventBuf = "";
                StreamReader st = new StreamReader(textBoxInput.Text);
                bool started = false;
                StreamWriter elog = new StreamWriter(textBoxOutput.Text + fname + "_report.txt");
                StreamWriter elog2 = new StreamWriter(textBoxOutput.Text + fname + "_list.txt");
                StreamWriter elog3 = new StreamWriter(textBoxOutput.Text + fname + "_Eventlist.txt");

                elog.WriteLine("Maze Suite - LogProcessor Tool Report File\r\n");
                elog.WriteLine("Maze Suite - LogProcessor Tool Report List File\r\n");
                elog2.WriteLine("Index\tMaze Time\tPath Len\tMaze File\tReturn Value");
                long curTime=0;
                bool mazeEnded = false;
                bool mazeTimeStarted = false;
               
                MPoint startPoint = new MPoint();
                MPoint endPoint = new MPoint();
                double pathLen = 0;

                long mazeTime=0;
                long totalTime = 0;

                StreamWriter a,b;
#if !DEBUG
                progressBar1.Visible = true;
                buttonCancel.Visible = true;
#endif
                counter=0;
                while (!st.EndOfStream)
                {
                    st.ReadLine();
                    counter++;
                }
#if !DEBUG
                progressBar1.Minimum = 0;
                progressBar1.Maximum = counter;
                progressBar1.Value = 0;
#endif
                st.Close();
                counter = 0;
                continueOperation = true;
                st = new StreamReader(textBoxInput.Text);

                while(!st.EndOfStream)
                {
                    line = st.ReadLine();
#if !DEBUG
                    progressBar1.Value++;
#endif
                    if (continueOperation==false)
                        break;
                    if (line.Contains("Walker"))
                    {
                        if (buf.Length > 2)
                        {
                            if (mazeTimeStarted)
                            {
                                elog.WriteLine("Maze " + (counter).ToString());
                                elog.WriteLine("Start Time :\t" + mazeTime.ToString());
                                elog.WriteLine("End Time   :\t" + curTime.ToString());
                                elog.WriteLine("Time       :\t" + (curTime - mazeTime).ToString() + "\r\n\r\n");
                                totalTime += curTime - mazeTime;
                                mazeTimeStarted = false;
                                elog2.WriteLine((counter).ToString() + "\t" + (curTime - mazeTime).ToString() + "\t" + pathLen.ToString(".00;.00;0") + "\t" + maze + "\t" + ret);
                            }

                            a = new StreamWriter(textBoxOutput.Text + fname + "_" + (counter).ToString() + "_" + maze + ".txt");
                            a.WriteLine("");
                            a.Write(buf);
                            a.Close();

                            b = new StreamWriter(textBoxOutput.Text + fname + "_" + (counter).ToString() + "_" + maze + "_Events.txt");
                            b.Write(eventBuf);
                            b.Close();

                            counter++;

                            buf = "";
                            maze = "";
                            ret = "";
                            eventBuf = "";
                            pathLen = 0;
                            startPoint = new MPoint(0,0,0); //PointF(0, 0);
                            endPoint = new MPoint(startPoint);
                        }
                        started = true;
                        mazeEnded = true;
                    }
                    else if (line.Contains("Maze\t:"))
                    {
                        maze = Tools.GetShortFileName(line).Substring(1);
                    }
                    //else if (line.Contains("Time")) 
                    //{
                    //    //do nothing...
                    //}
                    else
                    {                        
                        try
                        {                            
                            //long temp = long.Parse(line.Substring(0, line.IndexOf('\t')));
                            string[] p = line.Split('\t');
                            if (p.Length == 8)
                            {
                                for (int i = 1; i < p.Length; i++)
                                    p[i - 1] = p[i];
                            }

                            if (p.Length > 0 )
                            {
                                long temp = long.Parse(p[0]);
                                if (temp != -1) curTime = temp;
                                else ret = p[1];

                                mazeTimeStarted = true;

                                if (p[1].Contains("Event"))
                                {
                                    //if (p[2] == "5" && p[4] != "0" )
                                    //{
                                        eventBuf += line + "\r\n";
                                        elog3.WriteLine(line);
                                    //}
                                }                                

                                if (mazeEnded && mazeTimeStarted)
                                {
                                    mazeTime = curTime;
                                    mazeEnded = false;
                                    //if (p.Length == 5)
                                    //{                                    
                                        startPoint.X = double.Parse(p[1]);
                                        startPoint.Y = double.Parse(p[2]);
                                        startPoint.Z = double.Parse(p[3]);
                                    //}
                                }
                                else
                                {
                                    startPoint = new MPoint(endPoint);
                                }
                                endPoint.X = double.Parse(p[1]);
                                endPoint.Y = double.Parse(p[2]);
                                endPoint.Z = double.Parse(p[3]);

                                pathLen += endPoint.GetDistance(startPoint);
                                //pathLen += Math.Sqrt(Math.Pow(endPoint.X - startPoint.X, 2) + Math.Pow(endPoint.Y - startPoint.Y, 2) + Math.Pow(endPoint.Z - startPoint.Z, 2) );
                            }                            
                        }
                        catch
                        {
                            //mazeEnded = false;
                        }


                    }
                    if(started)
                    {
                        buf += line + "\r\n";
                    }
                }
                a = new StreamWriter(textBoxOutput.Text + fname + "_" + (counter).ToString() + "_" + maze + ".txt");
                a.WriteLine("");
                a.Write(buf);
                a.Close();


                b = new StreamWriter(textBoxOutput.Text + fname + "_" + (counter).ToString() + "_" + maze + "_Events.txt");
                b.Write(eventBuf);
                b.Close();

                counter++;

                st.Close();
                
                if (mazeTimeStarted)
                {
                    elog.WriteLine("Maze " + (counter-1).ToString());
                    elog.WriteLine("Start Time :\t" + mazeTime.ToString());
                    elog.WriteLine("End Time   :\t" + curTime.ToString());
                    elog.WriteLine("Time       :\t" + (curTime - mazeTime).ToString() + "\r\n\r\n");
                    totalTime += curTime - mazeTime;
                    elog2.WriteLine((counter-1).ToString() + "\t" + (curTime - mazeTime).ToString() + "\t" + pathLen.ToString(".00;.00;0") + "\t" + maze + "\t" + ret);
                }
                elog.WriteLine("Total Maze Time :\t " + totalTime.ToString() + " ms");
                elog.WriteLine("\t\t\t(" + ((double)totalTime / 1000).ToString("#.#") + " sec)");
                elog.WriteLine("\t\t\t(" + ((double)totalTime / 60000).ToString("#.#") + " min)");
                elog.Close();
                elog2.Close();
                elog3.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
#if !DEBUG
            progressBar1.Visible = false;
            buttonCancel.Visible = false;
            Finish();
#endif
        }

        private void Finish()
        {
            //labelResult.Text = "Created " + counter + " files in the output directory";
            labelResult.Text = "Found " + counter + " session" + ((counter>1)?"s":"") + " and created " +  (counter*2+3) + " files.";
            SetButtons(true);
        }

        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //    //timer1.Stop();
        //    //Process();
        //    //labelResult.Text = "Created " + counter + " files in the output directory";
        //    //SetButtons(true);
        //}


        void SetButtons(bool enable)
        {
            buttonStart.Enabled = enable;
            buttonInput.Enabled = enable;
            buttonOutput.Enabled = enable;
            buttonClose.Enabled = enable;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (trd.IsAlive)
            {
                if (MessageBox.Show("Do you want to stop current operation?", "MazeAnalyzer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //stop the operations...
                    continueOperation = false;
                }
            }
        }


    }
}