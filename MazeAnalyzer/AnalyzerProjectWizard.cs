using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using MazeAnalyzer;

namespace MazeAnalyzer
{
    public partial class AnalyzerProjectWizard : Form
    {
        public string projectName = "Untitled Project";
        public string projectDescription = "";
        public List<String> mazeFileNames = new List<string>();

        public AnalyzerProjectWizard(bool editMode=false)
        {
            InitializeComponent();
            mazeFileNames = new List<string>();
            listBox_mazefiles.Items.Clear();

            if (editMode)
            { 
                button_finish.Text = "Ok";
                this.Text = "Edit Project Information";
                listBox_mazefiles.Enabled = false;
                button_Ok.Enabled = false;
                button_remove.Enabled = false;
            }
            else
            { 
                
            }
        }

        public void updateMazesList()
        {
            foreach (string file in mazeFileNames)
            {
                listBox_mazefiles.Items.Add(file);
                //LoadMazeFile(file);
            }
        }

        private void button_Ok_Click(object sender, EventArgs e)
        {
            OpenFileDialog a = new OpenFileDialog();
            //switch (type)
            //{
                //case mzFileType.Maze:
                a.Filter = "Maze files (*.maz) |*.maz";
                a.FilterIndex = 1;
                a.RestoreDirectory = true;
                a.Multiselect = true;
                //if (curMazeViewer.projMazeList.Count == 0)
                //    a.Title = "Add First Maze File";
                if (a.ShowDialog() == DialogResult.OK)
                {
                    if (a.FileNames.Length > 1)
                        foreach (string file in a.FileNames)
                        {
                        listBox_mazefiles.Items.Add(file);
                        //LoadMazeFile(file);
                        }
                    else
                        listBox_mazefiles.Items.Add(a.FileName);
                            //LoadMazeFile(a.FileName);
                }

                    //break;
            }

        private void AnalyzerProjectWizard_Load(object sender, EventArgs e)
        {
            textBox_ProjName.Text = projectName;
            textBox_Description.Text = projectDescription;
            
        }

        private void button_finish_Click(object sender, EventArgs e)
        {
            if (listBox_mazefiles.Items.Count > 0)
            {
                projectName = textBox_ProjName.Text;
                projectDescription = textBox_Description.Text;
                if (listBox_mazefiles.Enabled)
                    mazeFileNames = listBox_mazefiles.Items.Cast<string>().ToList();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
                MessageBox.Show("Must have at least one mazefile");
        }

        private void button_remove_Click(object sender, EventArgs e)
        {
            
            foreach(int index in listBox_mazefiles.SelectedIndices)
            { 
                listBox_mazefiles.Items.RemoveAt(index);
            }
        }

        private void listBox_mazefiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] a = (string[])e.Data.GetData(DataFormats.FileDrop);
            for (int i = 0; i < a.Length; i++)
            {
                if (a.Length >= 1)
                {
                    int index = a[i].LastIndexOf(".");
                    string ext = a[i].Substring(index + 1);
                    ext = ext.ToLower();
                    if (ext.CompareTo("maz") == 0)
                        listBox_mazefiles.Items.Add(a[i]);

                }
            }
        }

        private void listBox_mazefiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
    }
}
