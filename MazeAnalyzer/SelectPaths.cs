using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MazeLib;

namespace MazeAnalyzer
{
    public partial class SelectPaths : Form
    {
        string iFolder;
        string iFile;
        private NumericUpDown numericTrial;
        private TextBox textBoxCondition;
        private ComboBox comboBoxMazeTo;
        private Control[] Editors;
        private string[] curProjMazes;
        private string curMaze;
        private int[] curMazesTrials;
        private List<MazePathItem> mpiList;

        bool currentlyEditing = false;

        public SelectPaths(string folder, string file, string[] loadedMazeNames,string currentMaze)
        {
            InitializeComponent();
            iFolder = folder;
            iFile = file;
            curProjMazes = new string[loadedMazeNames.Length + 1];
            curProjMazes[0] = "Do Not Import";
            curMazesTrials = new int[curProjMazes.Length];
            curMazesTrials[0] = 0;
            this.curMaze = currentMaze;
            for (int i = 1; i < loadedMazeNames.Length + 1; i++)
            {
                curProjMazes[i] = loadedMazeNames[i - 1];

                curMazesTrials[i] = 0;
            }
            listView1.CheckBoxes = false;

            listView1.SubItemClicked += new ListViewEx.SubItemEventHandler(listView1_SubItemClicked);
            listView1.SubItemEndEditing += new ListViewEx.SubItemEndEditingEventHandler(listView1_SubItemEndEditing);
            listView1.DoubleClickActivation = true;

            this.Text = "Select Paths for " + folder;
        }

        private int[] selects = new int[1];

        private void SelectPaths_Load(object sender, EventArgs e)
        {


            comboBoxMazeTo = new ComboBox();
            comboBoxMazeTo.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxMazeTo.Name = "comboBoxMazeTo";
            comboBoxMazeTo.Visible = false;
            comboBoxMazeTo.Items.AddRange(curProjMazes);
            comboBoxMazeTo.SelectedIndexChanged += new EventHandler(control_SelectedValueChanged);

            numericTrial = new NumericUpDown();
            numericTrial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numericTrial.Location = new System.Drawing.Point(32, 152);
            numericTrial.Maximum = new System.Decimal(new int[] {
                                                                           250,
                                                                           0,
                                                                           0,
                                                                           0});
            numericTrial.Minimum = new System.Decimal(new int[] {
                                                                           0,
                                                                           0,
                                                                           0,
                                                                           0});
            numericTrial.Name = "numericTrial";
            numericTrial.Size = new System.Drawing.Size(80, 20);
            numericTrial.TabIndex = 5;
            numericTrial.Value = new System.Decimal(new int[] {
                                                                         1,
                                                                         0,
                                                                         0,
                                                                         0});
            numericTrial.Visible = false;

            ((System.ComponentModel.ISupportInitialize)(this.numericTrial)).EndInit();

            textBoxCondition = new TextBox();
            textBoxCondition.Location = new Point(-100, -100);

            this.Controls.Add(this.numericTrial);
            this.Controls.Add(this.textBoxCondition);
            this.Controls.Add(this.comboBoxMazeTo);

            //arrowBox = new Button();
            //arrowBox.BackgroundImage = Properties.Resources.LoadToIcon;
            //arrowBox.Enabled = false;

   



            Editors = new Control[] {null,null,null,
                                    null,			// for column 3
                                    comboBoxMazeTo, //for Column 4
									textBoxCondition,		// for column 5
									numericTrial,	// for column 6
									};

            ImageList listViewBG = new ImageList();
            listViewBG.Images.Add(Properties.Resources.LoadToIcon);

            listView1.Items.Clear();
            listView1.LargeImageList = listViewBG;
            listView1.SmallImageList = listViewBG;

            listView1.Columns.Add("Index");
            listView1.Columns.Add("Date/Time");
            listView1.Columns.Add("Maze File");
            listView1.Columns.Add(" ");
            listView1.Columns.Add("Import To");
            listView1.Columns.Add("Condition");
            listView1.Columns.Add("Trial");
            listView1.Columns[0].Width = 50;
            listView1.Columns[1].Width = 140;
            listView1.Columns[2].Width = 150;
            listView1.Columns[3].Width = 32;
            listView1.Columns[3].ImageIndex = 0;

            listView1.Columns[4].Width = 125;
            listView1.Columns[5].Width = 100;
            listView1.Columns[6].Width = 50;
            listView1.View = View.Details;



            mpiList = new List<MazePathItem>();
            if (MazePathCollection.ScanLogFile(iFolder, iFile, mpiList))
            {
                selects = new int[mpiList.Count];
                int i = 0;
                foreach (MazePathItem mz in mpiList)
                {
                    textBox_Subject.Text = mz.Walker;

                    int mazeIndex = 0;
                    string mazeMatched = "Maze Not Found";
                    bool mazeMatches=false;
                    foreach (string mzString in curProjMazes)
                    {
                        mazeIndex++;
                        if (string.Compare(mzString.ToLower(), mz.Maze.ToLower()) == 0)
                        {
                            mazeMatched = curProjMazes[mazeIndex - 1];
                            mazeMatches = true;
                            break;
                        }

                    }
                    if (mazeIndex == curProjMazes.Length)
                    {
                        mazeIndex = 0;

                    }
                    else
                        mazeIndex = mazeIndex - 1;
                    curMazesTrials[mazeIndex]++;


                    ListViewItem a = new ListViewItem(new string[] { mz.melIndex.ToString(), mz.Date, mz.Maze,"", mazeMatched, mz.ExpCondition, curMazesTrials[mazeIndex].ToString() });
                    if (mazeMatches)
                    {
                        a.BackColor = Color.PaleGreen;
                        a.ForeColor = Color.DarkGreen;
                        a.Checked = true;
                    }
                    else
                    {
                        a.BackColor = Color.LightGray;
                        a.ForeColor = Color.Black;
                        a.Checked = false;
                    }
                    if (a.Checked)
                        selects[i++] = mazeIndex;
                    else
                        selects[i++] = 0;
                    //listView1.Items.Add(mz.logIndex + "\t" + mz.Maze + "\t" + mz.Date);
                    listView1.Items.Add(a);
                }
                if (mpiList.Count > 0)
                {
                    
                    if (mpiList[0].Mel.Length > 0)
                        logFileName_TextBox.Text = "Log from " + Path.GetFileName(mpiList[0].Mel);
                    else
                        logFileName_TextBox.Visible = false;
                }
            }
            label1.Hide();
            buttonLoad.Enabled = true;
            buttonCancel.Enabled = true;

            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Selected Log file contains no valid Paths", "Import Error");
                this.Close();
            }
        }

        private void control_SelectedValueChanged(object sender, System.EventArgs e)
        {
            listView1.EndEditing(true);
            listView1_updateItems();


        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            if(!listView1.currentlyEditing()&& listView1.Items.Count>0)
            { 

                int numSelected = 0;
                //MessageBox.Show(listView1.Items.Count.ToString());
                for (int i = 0; i < selects.Length; i++)
                {

                    if (listView1.Items[i].Checked)
                    {
                        selects[i] = 1;
                        numSelected++;
                    }
                    else
                        selects[i] = 0;
                }
                if (numSelected == 0)
                { 
                    MessageBox.Show("No Paths Assigned to Mazes", "Import Error");
                
                    return;
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }


        }

        public int[] GetSelectedItems()
        {
            return selects;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView1.EndEditing(true);
            listView1_updateItems();
        }

        private void listView1_SubItemClicked(object sender, ListViewEx.SubItemEventArgs e)
        {

            listView1.StartEditing(Editors[e.SubItem], e.Item, e.SubItem);
        }

        private void listView1_SubItemEndEditing(object sender, ListViewEx.SubItemEndEditingEventArgs e)
        {
            listView1_updateItems();
        }

        private void listView1_resetItems()
        {

        }

        private void listView1_unselectAllItems()
        {

        }

        private void listView1_oneMazeAllItems()
        {

        }

        private void listView1_updateItems()
        {
            foreach (ListViewItem lvItem in listView1.Items)
            {
                string mazeMatched = "Maze Not Found";
                bool bMazeMatches = false;
                bool ignorePathImport = false;

                if (string.Compare(lvItem.SubItems[2].Text.ToLower(), lvItem.SubItems[4].Text.ToLower()) == 0)
                {
                    bMazeMatches = true;

                }
                else if (lvItem.SubItems[4].Text.Length > 1 && (string.Compare(curProjMazes[0], lvItem.SubItems[4].Text) == 0|| string.Compare(mazeMatched, lvItem.SubItems[4].Text) == 0))
                {
                    ignorePathImport = true;
                }




                //ListViewItem a = new ListViewItem(new string[] { mz.melIndex.ToString(), mz.Date, mz.Maze, mazeMatched, mz.ExpCondition, curMazesTrials[mazeIndex].ToString() });
                if (bMazeMatches && !ignorePathImport)
                {
                    lvItem.BackColor = Color.PaleGreen;
                    lvItem.ForeColor = Color.DarkGreen;
                    lvItem.Checked = true;
                }
                else if (!bMazeMatches && !ignorePathImport)
                {
                    lvItem.BackColor = Color.LavenderBlush;
                    lvItem.ForeColor = Color.Crimson;
                    lvItem.Checked = true;
                }
                else
                {
                    lvItem.BackColor = Color.LightGray;
                    lvItem.ForeColor = Color.Black;
                    lvItem.Checked = false;
                }
            }
        }


        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            listView1_updateItems();
        }

        private void reset_trialCount()
        {
            int i = 0;
            foreach (string mzString in curProjMazes)
            {
                curMazesTrials[i] = 0;
                i++;
            }
        }

        private void update_trialCount()
        {
            reset_trialCount();

            foreach (ListViewItem lvItem in listView1.Items)
            {
                int mazeIndex = 0;
       
                foreach (string mzString in curProjMazes)
                {
                    
                    if (string.Compare(mzString, lvItem.SubItems[4].Text) == 0)
                    {
                        break;
                    }
                    mazeIndex++;
                }
  
                curMazesTrials[mazeIndex]++;
                lvItem.SubItems[6].Text = curMazesTrials[mazeIndex].ToString();


            }
        }

        private void button_reset_Click(object sender, EventArgs e)
        {

            foreach (ListViewItem lvItem in listView1.Items)
            {
                lvItem.SubItems[4].Text = curProjMazes[0];
                foreach (string x in curProjMazes)
                {
                    if (string.Compare(lvItem.SubItems[2].Text.ToLower(),x.ToLower())==0)
                    {
                        lvItem.SubItems[4].Text = x;
                    }
                }
                

            }
            listView1_updateItems();
            update_trialCount();
        }

        private void button_unselectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvItem in listView1.Items)
            {
                lvItem.SubItems[4].Text = curProjMazes[0];

            }
            listView1_updateItems();
            update_trialCount();
        }

        private void button_selectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvItem in listView1.Items)
            {
                lvItem.SubItems[4].Text = curMaze;

            }
            listView1_updateItems();
            update_trialCount();
        }

        private void button_copy_condition_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvItem in listView1.Items)
            {
                lvItem.SubItems[5].Text = lvItem.SubItems[2].Text;

            }
        }

        public List<List<MazePathItem>> GetSelectedPaths()
        {
            List<List<MazePathItem>> selectedPaths = new List<List<MazePathItem>>();
            foreach (MazePathItem mpi in mpiList)
            {
                ListViewItem lvItem = listView1.Items[mpi.melIndex];
                mpi.ExpCondition = lvItem.SubItems[5].Text;
                mpi.ExpSubjectID = textBox_Subject.Text;
                mpi.ExpGroup = textBox_Group.Text;
                mpi.ExpTrial = int.Parse(lvItem.SubItems[6].Text);
                mpi.ExpSession = (int)numeric_Session.Value;
            }

            List<MazePathItem> mpiListTemp = new List<MazePathItem>();
            foreach(string mazeStr in curProjMazes)
            {
                mpiListTemp = new List<MazePathItem>();

                if (mazeStr == curProjMazes[0]) //skip  the first
                    continue;

                foreach (MazePathItem mpi in mpiList)
                {
                    ListViewItem lvItem = listView1.Items[mpi.melIndex];
                    if(string.Compare(lvItem.SubItems[4].Text,mazeStr)==0)
                    {
                        mpiListTemp.Add(mpi);
                    }
                }

                selectedPaths.Add(mpiListTemp);
                //mpiListTemp.Clear();

            }
            return selectedPaths;
        }
    }
}
