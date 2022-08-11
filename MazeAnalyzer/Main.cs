using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MazeLib;
using MazeMaker;
using System.Xml;

namespace MazeAnalyzer
{
    

    public partial class Main : Form
    {
        MazeLib.MazeViewer curMazeViewer = new MazeLib.MazeViewer();
        MazeLib.MazeItemThemeLibrary mazeThemeLibrary = new MazeItemThemeLibrary();
        //MazeLib.MazePath curMazePaths = new MazeLib.MazePath();

        Heatmap hmViewer;

        public enum mzFileType
        {
            Project, Maze, LogFile, RegionFile
        }

        public enum ExpInfoTypes
        {
            Group, Condition, Subject, Session, Trial
        }

        bool projectChanged = false;

        public List<ExpInfoTypes> expInfoPrioity = new List<ExpInfoTypes>(6);
        public List<ExpInfoTypes> altInfoPriority;

        bool bCommandLineFormat = false;
        string sCommandLineFile = "";

        bool bMeasuringMode = false;
        bool bMeasuringInitialized = false;
        PointF lastClickedPosition;

        public Main()
        {
            InitializeComponent();
            tabPage1.Controls.Add(curMazeViewer);
         
            curMazeViewer.Visible = true;
            //curMaze.Dock = DockStyle.Fill;      

            MainInitialize();

        }

        void MainInitialize()
        {
            curMazeViewer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tabPage_MouseMove);
            curMazeViewer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabPage_MouseDown);
            curMazeViewer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tabPage_MouseUp);

            curMazeViewer.MouseClick += new MouseEventHandler(this.tabControl1_MouseClick);
            curMazeViewer.KeyDown += new KeyEventHandler(this.Main_KeyDown);
            curMazeViewer.Scroll += new ScrollEventHandler(this.tabControl1_Scroll);
            curMazeViewer.MouseWheel += new MouseEventHandler(this.tabControl1_MouseWheel);

            tabControlTree.AutoSize = true;
            treeViewPaths.AutoSize = true;
            treeViewProject.AutoSize = true;
            treeViewProject.AllowDrop = true;

            foreach (MazeItemTheme m in mazeThemeLibrary.themeItems)
            {
                ToolStripMenuItem t = new ToolStripMenuItem(m.name);
                t.Click += themeButton_Click;
                themeToolStripMenuItem.DropDownItems.Add(t);

                t = new ToolStripMenuItem(m.name);
                t.Click += themeButton_Click;
                toolStripDropdown_Theme.DropDownItems.Add(t);
            }

            hmViewer=new Heatmap(curMazeViewer);
        }

        public Main(string inp)
        {
            InitializeComponent();
            tabPage1.Controls.Add(curMazeViewer);
            curMazeViewer.Visible = true;
            bCommandLineFormat = true;
            sCommandLineFile = inp;

            MainInitialize();

        }



        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            SplashScreen.SetStatus("Reading settings");
            //initialize
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            UpdatePathList();
            UpdateProjectList();
            UpdateTheSize();

            CurrentSettings.ReadSettings();

            splitContainer2.Panel1Collapsed = !CurrentSettings.showListPane;
            splitContainer1.Panel2Collapsed = !CurrentSettings.showPropertiesPane;
            setTheme(CurrentSettings.themeIndex);

            projectPaneToolStripMenuItem.Checked = CurrentSettings.showListPane;
            propertyPaneToolStripMenuItem.Checked= CurrentSettings.showPropertiesPane;

            toolStrip2.Enabled = false;

            if (bCommandLineFormat)
            {
                timer1.Enabled = true;
            }
            if (this.WindowState != FormWindowState.Minimized)
            {
                this.Activate();
                this.Select();
                this.Focus();
                this.BringToFront();
            }
            
            updateLastProjectButtons();
            PositionWelcomePanel();
            CloseAnalyzerProject();
            SplashScreen.CloseForm();
        }

        private void updateLastProjectButtons()
        {

            if (CurrentSettings.previousProjectFiles.Count > 0)
            {
                String lastProject = CurrentSettings.previousProjectFiles[CurrentSettings.previousProjectFiles.Count - 1];
                if (System.IO.File.Exists(lastProject))
                {
                    buttonWelcomeLoadLast.Text = "Open Last Project\n(" + System.IO.Path.GetFileName(lastProject) + ")";
                    buttonWelcomeLoadLast.Enabled = true;
                    buttonWelcomeIconLoadLast.Enabled = true;
                }
                else
                {
                    buttonWelcomeLoadLast.Text = "Last Project Not Found (" + System.IO.Path.GetFileName(lastProject) + ")";
                    buttonWelcomeLoadLast.Enabled = false;
                    buttonWelcomeIconLoadLast.Enabled = false;
                }
            }
            else
            {
                buttonWelcomeLoadLast.Text = "No Previous Projects";
                buttonWelcomeLoadLast.Enabled = false;
                buttonWelcomeIconLoadLast.Enabled = false;
            }
        }

        private void tabPage1_Paint(object sender, PaintEventArgs e)
        {
            //
        }


        /// <summary>
        /// type 0 = Maze
        /// type 1 = Log
        /// </summary>
        /// <param name="type"></param>
        public void Open(mzFileType type)
        {
            EnterMeasureMode(false); //exit measurement mode if any



            //Open Maze...  
            OpenFileDialog a = new OpenFileDialog();
            switch (type)
            {
                case mzFileType.Maze:
                    a.Filter = "Maze files (*.maz) |*.maz";
                    a.FilterIndex = 1;
                    a.RestoreDirectory = true;
                    a.Multiselect = true;
                    if (curMazeViewer.projMazeList.Count == 0)
                        a.Title = "Add First Maze File";
                    if (a.ShowDialog() == DialogResult.OK)
                    {
                        if(a.FileNames.Length>1)
                            foreach(string file in a.FileNames)
                            {
                                LoadMazeFile(file);
                            }
                        else
                            LoadMazeFile(a.FileName);
                    }

                    break;
                case mzFileType.LogFile:
                    a.Filter = "Maze Log files (*.txt) |*.txt";
                    a.FilterIndex = 1;
                    a.RestoreDirectory = true;
                    a.Multiselect = true;
                    if (a.ShowDialog() == DialogResult.OK)
                    {
                        if (a.FileNames.Length > 1)
                            foreach (string file in a.FileNames)
                            {
                                LoadLogFile(file);
                            }
                        else
                            LoadLogFile(a.FileName);
                    }

                    break;
                case mzFileType.Project:
                    DialogResult dRes;
                    if (projectChanged)
                    {
                        dRes = MessageBox.Show("Current project (" + Path.GetFileName(curMazeViewer.projectInfo.ProjectFilename) + ") has been changed.\t\n\nDo you want to save?", "MazeAnalyzer", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

                        if (dRes == DialogResult.Yes)
                            SaveAnalyzerProject();
                        else if (dRes == DialogResult.Cancel)
                            return;
                    }
                    a.Filter = "Maze Analyzer Project (*.mzproj) |*.mzproj";
                    a.FilterIndex = 1;
                    a.RestoreDirectory = true;
                    if (a.ShowDialog() == DialogResult.OK)
                    {
                        LoadProjectFile(a.FileName);
                    }

                    break;
                case mzFileType.RegionFile:
                    a.Filter = "Maze Region Definition files(*.mzReg) | *.mzReg;*.dat";
                    a.FilterIndex = 1;
                    a.RestoreDirectory = true;
                    if (a.ShowDialog() == DialogResult.OK)
                    {
                        LoadRegionFile(a.FileName);
                    }

                    break;
                default:
                    a.Filter = "Maze files (*.maz) |*.maz| Maze Log files (*.txt) |*.txt| Maze Region Definition files (*.mzReg) |*.mzReg|Maze Analyzer Project(*.mzproj) | *.mzproj";
                    break;
            }



        }

        public void EditProjectInfo()
        {
            AnalyzerProjectWizard projWizard = new AnalyzerProjectWizard(true);
            projWizard.projectDescription = curMazeViewer.projectInfo.ProjectDescription;
            projWizard.projectName = curMazeViewer.projectInfo.ProjectName;
            

            foreach (MazeLib.ExtendedMaze em in curMazeViewer.projMazeList)
            {
                projWizard.mazeFileNames.Add(em.FileName);
            }
            projWizard.updateMazesList();

            if (projWizard.ShowDialog() == DialogResult.OK)
            {
                curMazeViewer.projectInfo.ProjectName = projWizard.projectName;
                curMazeViewer.projectInfo.ProjectDescription = projWizard.projectDescription;
            }
            else
                return;
        }


        private void LoadMazeFile(string inp)
        {
            if (curMazeViewer.curMaze == null)
                Reset();
            if (System.IO.File.Exists(inp) == false)
            {
                MessageBox.Show("Couldn't find file: \n\n" + inp, "MazeAnalyzer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (CurrentSettings.previousMazeFiles.Contains(inp))
                {
                    CurrentSettings.previousMazeFiles.Remove(inp);
                    CurrentSettings.SaveSettings();
                }
                return;
            }
            if (curMazeViewer.ReadMazeFile(inp))
            {
                UpdateTabs();
                
                SetTabByIndex(curMazeViewer.projMazeList.Count - 1);
                
                //CurrentSettings.AddPreviousFile(inp);
                CurrentSettings.AddMazeFileToPrevious(inp);
                toolStrip2.Enabled = true;
                UpdatePathList();
                UpdateProjectList();
                EnableProjectButtons(true);
                projectChanged = true;
            }
            else
            {
                if (CurrentSettings.previousMazeFiles.Contains(inp))
                {
                    CurrentSettings.previousMazeFiles.Remove(inp);
                }
            }
            CurrentSettings.SaveSettings();
        }

        private void LoadLogFile(string inp)
        {
            
            if (System.IO.File.Exists(inp) == false)
            {
                MessageBox.Show("Can not find log file: " + inp, "MazeAnalyzer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (CurrentSettings.previousLogFiles.Contains(inp))
                {
                    CurrentSettings.previousLogFiles.Remove(inp);
                    CurrentSettings.SaveSettings();
                }
                return;
            }
            //first scan the log file
            string[] curProjMazes = new string[curMazeViewer.projMazeList.Count];

            int i = 0;
            foreach(ExtendedMaze exMaze in curMazeViewer.projMazeList)
            {
                curProjMazes[i] = Path.GetFileNameWithoutExtension(exMaze.FileName);
                i++;
            }

            if (i == 0) //if there are no mazes in the project, ignore
            {
                MessageBox.Show("No mazes present in the current project", "MazeAnalyzer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SelectPaths sel = new SelectPaths(inp, curMazeViewer.mazeFileName,curProjMazes,Path.GetFileNameWithoutExtension(curMazeViewer.curMaze.FileName));
            if (sel.ShowDialog() == DialogResult.Cancel)
                return;

            if (curMazeViewer.pathInCurrentPaths(inp))
            { 
                MessageBox.Show("Logfile: "+inp+"\nPath already assigned to current Maze.", "MazeAnalyzer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
            }

            if (curMazeViewer.ImportLogFile(inp, sel.GetSelectedPaths()))
            {
                
                
                curMazeViewer.bShowPaths = true;
                pathsToolStripMenuItem.Checked = true;
                curMazeViewer.curMazePaths.SelectAll();
                UpdatePathList();
                UpdateProjectList();
                CurrentSettings.AddLogFileToPrevious(inp);
                projectChanged = true;
                toolStrip2.Enabled = true;
            }
            else
            {
                if (CurrentSettings.previousLogFiles.Contains(inp))
                {
                    CurrentSettings.previousLogFiles.Remove(inp);
                }
            }
            CurrentSettings.SaveSettings();
        }

        private void EnableProjectButtons(bool enable)
        {
            saveAsToolStripMenuItem.Enabled = enable;
            saveProjectFileToolStripButton.Enabled = enable;
            toolStripButtonOpen.Enabled = enable;
            toolStripButtonOpenLog.Enabled = enable;
            openLogFileToolStripMenuItem.Enabled = enable;
            openRegionDefinitionFileToolStripMenuItem.Enabled = enable;
            openToolStripMenuItem.Enabled = enable;
            recentLogsToolStripMenuItem.Enabled = enable;
            recentMazesToolStripMenuItem.Enabled = enable;
            recentRegionDefinitionsToolStripMenuItem.Enabled = enable;
            definePointToolStripMenuItem.Enabled = enable;
            defineRegionToolStripMenuItem.Enabled = enable;
            measureToolStripMenuItem.Enabled = enable;
            manageRegionsToolStripMenuItem.Enabled = enable;
            analyzeToolStripMenuItem.Enabled = enable;
            exportToolStripMenuItem.Enabled = enable;
            editProjectInformationToolStripMenuItem.Enabled = enable;
            zoomInToolStripMenuItem.Enabled = enable;
            zoomOutToolStripMenuItem.Enabled = enable;
            resetZoomToolStripMenuItem.Enabled = enable;
            nextMazeToolStripMenuItem.Enabled = enable;
            centerViewOnStartToolStripMenuItem.Enabled = enable;
            previousMazeToolStripMenuItem.Enabled = enable;
            toolStripButtonPlay.Enabled = enable;
            toolStripDropDown_ZoomIn_Button.Enabled = enable;
            toolStripDropDown_ZoomOut_Button.Enabled = enable;
            toolStripDropdown_Theme.Enabled = enable;
            toolStripDropDown_CenterView.Enabled = enable;

        }

        private void LoadRegionFile(string inp)
        {
            if (curMazeViewer.curMaze != null && curMazeViewer.curRegions.ReadFromFile(inp))
            {
                toolStrip2.Enabled = true;
                CurrentSettings.AddRegionFileToPrevious(inp);
                projectChanged = true;
                RefreshMazeView();
                CurrentSettings.SaveSettings();
            }

        }

        private void LoadProjectFile(string inp)
        {
            if (System.IO.File.Exists(inp) == false)
            {
                MessageBox.Show("Couldn't find file: \n\n" + inp, "MazeAnalyzer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (CurrentSettings.previousProjectFiles.Contains(inp))
                {
                    CurrentSettings.previousProjectFiles.Remove(inp);
                    CurrentSettings.SaveSettings();
                }
                return;
            }
            else
            {

                // Project File should have Name of Maze(s), Name of Log files, and study data (subject num, group etc)
                Reset();
                if (curMazeViewer.ReadProject(inp))
                {

                    // CurrentSettings.AddRegionFileToPrevious(inp);

                    pathsToolStripMenuItem.Checked = true;
                    toolStrip2.Enabled = true;
                    curMazeViewer.projectInfo.ProjectFilename = inp;
                    UpdatePathList();
                    UpdateTabs();
                    UpdateProjectList();
                    CurrentSettings.AddProjectFileToPrevious(inp);
                    projectChanged = false;
                    SetTabByIndex(0);
                    EnableProjectButtons(true);

                    // CurrentSettings.SaveSettings();
                }
                else
                {

                    if (CurrentSettings.previousMazeFiles.Contains(inp))
                    {
                        CurrentSettings.previousMazeFiles.Remove(inp);
                    }
                }
            }
            CurrentSettings.SaveSettings();
        }
        


     

        public static string BuildPathNameFromExp(MazePathItem mpi,List<ExpInfoTypes> infoPriority)
        {
            string outName = "";
            foreach(ExpInfoTypes exptype in infoPriority)
            {
                switch(exptype)
                {
                    case ExpInfoTypes.Group:
                        outName += "Group=" + mpi.ExpGroup+", ";
                        break;
                    case ExpInfoTypes.Condition:
                        outName += "Condition=" + mpi.ExpCondition + ", ";
                        break;
                    case ExpInfoTypes.Subject:
                        outName += "Subject=" + mpi.ExpSubjectID + ", ";
                     break;
                    case ExpInfoTypes.Trial:
                        outName += "Trial=" + mpi.ExpTrial + ", ";
                        break;
                    case ExpInfoTypes.Session:
                        outName += "Session=" + mpi.ExpSession + ", ";
                        break;
                }
            }
            outName += "Path" + mpi.logIndex;
            return outName;

        }

        private void Reset()
        {
            propertyGrid1.SelectedObject = null;
            curMazeViewer.NewMaze();
            UpdatePathList();
            panelWelcome.Visible = false;
            EnterMeasureMode(false); //exit measurement mode if any

            //curMazeViewer.projectInfo = new MazeViewer.ProjectInfo();
            curMazeViewer.projectInfo.ProjectFilename = "UnsavedProject";

            expInfoPrioity.Clear();
            expInfoPrioity.Add(ExpInfoTypes.Group);
            expInfoPrioity.Add(ExpInfoTypes.Condition);
            expInfoPrioity.Add(ExpInfoTypes.Subject);
            expInfoPrioity.Add(ExpInfoTypes.Session);
            expInfoPrioity.Add(ExpInfoTypes.Trial);

            projectChanged = false;
        }

        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            Open(mzFileType.Maze);
        }

        private void toolStripButtonOpenLog_Click(object sender, EventArgs e)
        {
            if (curMazeViewer.IsEmpty())
            {
                MessageBox.Show("No maze loaded! Please first open a maze file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                Open(mzFileType.LogFile);
            }

        }

        public Bitmap CreateBitmapAtRuntime(Color clr)
        {

            Bitmap bitColor = new Bitmap(4, 4);
            for (int x=0;x<4;x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    bitColor.SetPixel(x, y, clr);
                }
            }
            
            Size newbitsize = new Size(20, 20);

            return new Bitmap(bitColor, newbitsize);
        }

        private void UpdateProjectList()
        {
            if(curMazeViewer.bNewRegionFlag)
                projectChanged = true;

            curMazeViewer.bNewRegionFlag = false;

            curMazeViewer.UpdateIndicies();
            treeViewProject.Nodes.Clear();

            TreeNode root = new TreeNode("Analyzer Project");
            if (curMazeViewer.curMaze == null)
                return;

            root.Text += ":" + curMazeViewer.projectInfo.ProjectName;
            Font parentFont = new Font(treeViewProject.Font.FontFamily, treeViewProject.Font.Size, FontStyle.Bold);
            Font fileFont = new Font(treeViewProject.Font.FontFamily, treeViewProject.Font.Size, FontStyle.Italic);
            root.NodeFont = parentFont;

            treeViewProject.Nodes.Add(root);

            TreeNode maze, path,region;



            foreach (ExtendedMaze exMaze in curMazeViewer.projMazeList)
            {
                maze = new TreeNode("Maze: "+Path.GetFileNameWithoutExtension(exMaze.FileName));
                maze.ToolTipText = exMaze.FileName;
                maze.Name = "MZ$" + exMaze.Index;

                List<string> regionFiles = new List<string>();
                List<MazeLib.MeasurementRegion> mzRegions = curMazeViewer.GetRegionsByIndex(exMaze.Index).Regions;
                int varIndex = 0;
                foreach (MazeLib.MeasurementRegion mzR in mzRegions)
                {
                    // = new Uri(Directory.GetCurrentDirectory());

                    //regionFiles.Add(mzR.filename);

                    region = new TreeNode("Region: " + mzR.Name);
                    region.ToolTipText = exMaze.FileName;
                    region.Name = "MZ$" + exMaze.Index + "$REG$" + varIndex;
                    region.NodeFont = fileFont;
                    maze.Nodes.Add(region);
                    varIndex++;
                }
                //var unique_items = new HashSet<string>(regionFiles);

                //int varIndex = 0;
                //foreach (string mrc in unique_items)
                //{
                //    region = new TreeNode("Region: " + Path.GetFileNameWithoutExtension(mrc));
                //    region.ToolTipText = exMaze.FileName;
                //    region.Name = "MZ$"+exMaze.Index+"$REG$" + varIndex;
                //    region.NodeFont = fileFont;
                //    maze.Nodes.Add(region);
                //    varIndex++;
                //}
                foreach (MazePathItem mpi in curMazeViewer.GetPathsByIndex(exMaze.Index).cPaths)
                {
                    path = new TreeNode("Path "+mpi.logIndex+": " + Path.GetFileNameWithoutExtension(mpi.logFileName)+":"+mpi.melIndex);
                    path.ToolTipText = mpi.logFileName;
                    path.Name = "MZ$"+exMaze.Index+"$MZP$" + mpi.logIndex;
                    path.NodeFont = fileFont;
                    maze.Nodes.Add(path);
                    
                }
                maze.NodeFont = fileFont;


                root.Nodes.Add(maze);

            }
            treeViewProject.Update();
            root.Expand();
        }

        private void UpdatePathList()
        {
            curMazeViewer.UpdateIndicies();
            if (curMazeViewer.projectInfo.ProjectName.Length > 0)
            {
                this.Text = Application.ProductName + " - " + curMazeViewer.projectInfo.ProjectName + " - " + Path.GetFileNameWithoutExtension(curMazeViewer.projectInfo.ProjectFilename) + ".mzproj";

            }
            else
                this.Text = Application.ProductName;
            if (curMazeViewer.curMaze!=null)
                this.Text+= " - " + Path.GetFileNameWithoutExtension(curMazeViewer.curMaze.FileName);

            curMazeViewer.selectedPath = -1;
            curMazeViewer.Refresh(); //Must run first to initialize colors

            treeViewPaths.Nodes.Clear();

            treeViewPaths.ImageList = new ImageList();
            treeViewPaths.ImageList.Images.Add(CreateBitmapAtRuntime(Color.White));

            List<string> groupNames = new List<string>();
            List<string> subjectNames = new List<string>();
            List<string> conditionNames = new List<string>();
            List<int> trialNums = new List<int>();
            List<int> sessionNums = new List<int>();

            int index = 0;
            foreach (MazeLib.MazePathItem mzR in curMazeViewer.curMazePaths.cPaths)
            {
                groupNames.Add(mzR.ExpGroup);
                subjectNames.Add(mzR.ExpSubjectID);
                conditionNames.Add(mzR.ExpCondition);
                trialNums.Add(mzR.ExpTrial);
                sessionNums.Add(mzR.ExpSession);
                mzR.logIndex = index;
                index++;
            }

            var unique_groups = new SortedSet<string>(groupNames);
            var unique_subjects = new SortedSet<string>(subjectNames);
            var unique_conditions = new SortedSet<string>(conditionNames);
            var unique_trials = new SortedSet<int>(trialNums);
            var unique_sessions = new SortedSet<int>(sessionNums);

           

            altInfoPriority = new List<ExpInfoTypes>(expInfoPrioity);
            List<TreeNode>[] layers = new List<TreeNode>[altInfoPriority.Count];
            int layerNum = 0;

            foreach (ExpInfoTypes e in expInfoPrioity)
            {
                
                switch (e)
                {
                    case ExpInfoTypes.Group:

                        if (unique_groups.Count <= 1)
                            altInfoPriority.Remove(e);
                        else
                        {
                            layers[layerNum] = new List<TreeNode>();
                            foreach (string groupName in unique_groups)
                            {
                                layers[layerNum].Add(new TreeNode("Group: "+groupName));
                            }
                            layerNum++;
                        }
                        break;
                    case ExpInfoTypes.Condition:
                        if (unique_conditions.Count<= 1)
                            altInfoPriority.Remove(e);
                        else
                        {
                            layers[layerNum] = new List<TreeNode>();
                            foreach (string conditionName in unique_conditions)
                            {
                                layers[layerNum].Add(new TreeNode("Condition: " + conditionName));
                            }
                            layerNum++;
                        }
                        break;
                    case ExpInfoTypes.Subject:
                        if (unique_subjects.Count <= 1)
                            altInfoPriority.Remove(e);
                        else
                        {
                            layers[layerNum] = new List<TreeNode>();
                            foreach (string subjectName in unique_subjects)
                            {
                                layers[layerNum].Add(new TreeNode("Subject: "+subjectName));
                            }
                            layerNum++;
                        }
                        break;
                    case ExpInfoTypes.Trial:
                        if (unique_trials.Count <= 1)
                            altInfoPriority.Remove(e);
                        else
                        {
                            layers[layerNum] = new List<TreeNode>();
                            foreach (int trialNum in unique_trials)
                            {
                                layers[layerNum].Add(new TreeNode("Trial "+trialNum.ToString()));
                            }
                            layerNum++;
                        }
                        break;
                    case ExpInfoTypes.Session:
                        if (unique_sessions.Count <= 1)
                            altInfoPriority.Remove(e);
                        else
                        {
                            layers[layerNum] = new List<TreeNode>();
                            foreach (int sessionNum in unique_sessions)
                            {
                                layers[layerNum].Add(new TreeNode("Session "+sessionNum));
                            }
                            layerNum++;
                        }
                        break;
                    default:
                        break;
                }
            }

            Font parentFont = new Font(treeViewPaths.Font.FontFamily, treeViewPaths.Font.Size, FontStyle.Bold);
            Font pathFont = new Font(treeViewPaths.Font.FontFamily, treeViewPaths.Font.Size, FontStyle.Italic);

            TreeNode root = new TreeNode("Paths");
            root.NodeFont = parentFont;
            root.Text = "Paths  ";

            if(layerNum > 0)
                foreach (TreeNode tN0 in layers[0])
                {
                    tN0.Nodes.Clear();
                    if (layerNum > 1)
                        foreach (TreeNode tN1 in layers[1])
                        {
                            tN1.Nodes.Clear();
                            if (layerNum > 2)
                                foreach (TreeNode tN2 in layers[2])
                                {
                                    tN2.Nodes.Clear();
                                    if (layerNum > 3)
                                        foreach (TreeNode tN3 in layers[3])
                                        {
                                            tN3.Nodes.Clear();
                                            if (layerNum > 4)
                                                foreach (TreeNode tN4 in layers[4])
                                                {
                                                    tN4.Nodes.Clear();
                                                    tN3.Nodes.Add((TreeNode)tN4.Clone());
                                                }
                                            tN2.Nodes.Add((TreeNode)tN3.Clone());
                                        }
                                    tN1.Nodes.Add((TreeNode)tN2.Clone());
                                }
                            tN0.Nodes.Add((TreeNode)tN1.Clone());
                        }
                    root.Nodes.Add((TreeNode)tN0.Clone());
                }

            
            int i = 0;

            TreeNode pathNode;// = new TreeNode("Walls");
            //paths.NodeFont = parentFont;
            //paths.Text = "Walls   ";
            for (i = 0; i < curMazeViewer.curMazePaths.cPaths.Count; i++)
            {
                bool found = false;
                foreach(TreeNode tN0 in root.Nodes)
                {
                    if (!found && altInfoPriority.Count>0&&matchesExpInfo(curMazeViewer.curMazePaths.cPaths[i], altInfoPriority[0], tN0.Text))
                    {
                        foreach (TreeNode tN1 in tN0.Nodes)
                        {
                            if (!found && altInfoPriority.Count > 1 && matchesExpInfo(curMazeViewer.curMazePaths.cPaths[i], altInfoPriority[1], tN1.Text))
                            {
                                foreach (TreeNode tN2 in tN1.Nodes)
                                {
                                    if (!found && altInfoPriority.Count > 2 && matchesExpInfo(curMazeViewer.curMazePaths.cPaths[i], altInfoPriority[2], tN2.Text))
                                    {
                                        foreach (TreeNode tN3 in tN2.Nodes)
                                        {
                                            if (!found && altInfoPriority.Count > 3 && matchesExpInfo(curMazeViewer.curMazePaths.cPaths[i], altInfoPriority[3], tN3.Text))
                                            {
                                                foreach (TreeNode tN4 in tN3.Nodes)
                                                {
                                                    if (!found&&altInfoPriority.Count > 4 && matchesExpInfo(curMazeViewer.curMazePaths.cPaths[i], altInfoPriority[4], tN4.Text))
                                                    {
                                                        pathNode = new TreeNode();
                                                        pathNode.Text = "Path " + i.ToString();
                                                        pathNode.Name = "MZP$ " + i.ToString();
                                                        pathNode.NodeFont = pathFont;
                                                        treeViewPaths.ImageList.Images.Add(CreateBitmapAtRuntime(curMazeViewer.curMazePaths.cPaths[i].GetAutoColor()));
                                                        pathNode.ImageIndex = treeViewPaths.ImageList.Images.Count-1;
                                                        pathNode.SelectedImageIndex = pathNode.ImageIndex;
                                                        tN4.Nodes.Add(pathNode);
                                                        found = true;
                                                    }
                                                }
                                                if(!found)
                                                {
                                                    pathNode = new TreeNode();
                                                    pathNode.Text = "Path " + i.ToString();
                                                    pathNode.Name = "MZP$ " + i.ToString();
                                                    pathNode.NodeFont = pathFont;
                                                    treeViewPaths.ImageList.Images.Add(CreateBitmapAtRuntime(curMazeViewer.curMazePaths.cPaths[i].GetAutoColor()));
                                                    pathNode.ImageIndex = treeViewPaths.ImageList.Images.Count - 1;
                                                    pathNode.SelectedImageIndex = pathNode.ImageIndex;
                                                    tN3.Nodes.Add(pathNode);
                                                    found = true;
                                                }
                                            }
                                        }
                                        if (!found)
                                        {
                                            pathNode = new TreeNode();
                                            pathNode.Text = "Path " + i.ToString();
                                            pathNode.Name = "MZP$ " + i.ToString();
                                            pathNode.NodeFont = pathFont;
                                            treeViewPaths.ImageList.Images.Add(CreateBitmapAtRuntime(curMazeViewer.curMazePaths.cPaths[i].GetAutoColor()));
                                            pathNode.ImageIndex = treeViewPaths.ImageList.Images.Count - 1;
                                            pathNode.SelectedImageIndex = pathNode.ImageIndex;
                                            tN2.Nodes.Add(pathNode);
                                            found = true;
                                        }

                                    }
                                }
                                if (!found)
                                {
                                    pathNode = new TreeNode();
                                    pathNode.Text = "Path " + i.ToString();
                                    pathNode.Name = "MZP$ " + i.ToString();
                                    pathNode.NodeFont = pathFont;
                                    treeViewPaths.ImageList.Images.Add(CreateBitmapAtRuntime(curMazeViewer.curMazePaths.cPaths[i].GetAutoColor()));
                                    pathNode.ImageIndex = treeViewPaths.ImageList.Images.Count - 1;
                                    pathNode.SelectedImageIndex = pathNode.ImageIndex;
                                    tN1.Nodes.Add(pathNode);
                                    found = true;
                                }
                            }
                        }
                        if (!found)
                        {
                            pathNode = new TreeNode();
                            pathNode.Text = "Path " + i.ToString();
                            pathNode.Name = "MZP$ " + i.ToString();
                            pathNode.NodeFont = pathFont;
                            treeViewPaths.ImageList.Images.Add(CreateBitmapAtRuntime(curMazeViewer.curMazePaths.cPaths[i].GetAutoColor()));
                            pathNode.ImageIndex = treeViewPaths.ImageList.Images.Count - 1;
                            pathNode.SelectedImageIndex = pathNode.ImageIndex;
                            tN0.Nodes.Add(pathNode);
                            found = true;
                        }
                    }
                }
                if (!found)
                {
                    pathNode = new TreeNode();
                    pathNode.Text = "Path " + i.ToString();
                    pathNode.Name = "MZP$ " + i.ToString();
                    pathNode.NodeFont = pathFont;
                    treeViewPaths.ImageList.Images.Add(CreateBitmapAtRuntime(curMazeViewer.curMazePaths.cPaths[i].GetAutoColor()));
                    pathNode.ImageIndex = treeViewPaths.ImageList.Images.Count - 1;
                    pathNode.SelectedImageIndex = pathNode.ImageIndex;
                    root.Nodes.Add(pathNode);
                    found = true;
                }

            }

            for (int i0=0;i0<root.Nodes.Count;i0++)
            {
                TreeNode tN0 = root.Nodes[i0];
                for (int i1 = 0; i1 < tN0.Nodes.Count; i1++)
                {
                    TreeNode tN1 = tN0.Nodes[i1];
                    for (int i2 = 0; i2 < tN1.Nodes.Count; i2++)
                    {
                        TreeNode tN2 = tN1.Nodes[i2];
                        for (int i3 = 0; i3 < tN2.Nodes.Count; i3++)
                        {
                            TreeNode tN3 = tN2.Nodes[i3];
                            for (int i4 = 0; i4 < tN3.Nodes.Count; i4++)
                            {
                                TreeNode tN4 = tN3.Nodes[i4];
                                if (tN4.Nodes.Count == 0 && !tN4.Name.Contains("MZP$"))
                                { 
                                    tN4.Remove();
                                    i4--;
                                }
                            }
                            if (tN3.Nodes.Count == 0 && !tN3.Name.Contains("MZP$"))
                            {
                                tN3.Remove();
                                i3--;
                            }
                        }
                        if (tN2.Nodes.Count == 0 && !tN2.Name.Contains("MZP$"))
                        {
                            tN2.Remove();
                            i2--;
                        }
                    }
                    if (tN1.Nodes.Count == 0 && !tN1.Name.Contains("MZP$"))
                    {
                        tN1.Remove();
                        i1--;
                    }
                }
                if (tN0.Nodes.Count == 0 && !tN0.Name.Contains("MZP$"))
                {
                    tN0.Remove();
                    i0--;
                }

            }
            root.ExpandAll();
            treeViewPaths.Nodes.Add(root);

            
            propertyGrid1.SelectedObjects = curMazeViewer.curMazePaths.cPaths.ToArray();
        }

        private static bool matchesExpInfo(MazePathItem msp, ExpInfoTypes eType,string match)
        {
            int value = -1;
            if (match.Contains(":"))
            {
                string[] parts = match.Split(':');
                if (parts.Length > 1)
                    match = parts[1].Substring(1);
                else
                    return false;
            }
            switch (eType)
            {
                case ExpInfoTypes.Group:
                    return string.Compare(msp.ExpGroup,match) == 0;
                case ExpInfoTypes.Condition:
                    return string.Compare(msp.ExpCondition, match) == 0;
                case ExpInfoTypes.Subject:
                    return string.Compare(msp.ExpSubjectID, match) == 0;
                case ExpInfoTypes.Trial:
                    int.TryParse(match.Substring(6), out value);
                    return msp.ExpTrial == value;
                case ExpInfoTypes.Session:
                    int.TryParse(match.Substring(7), out value);
                    return msp.ExpSession == value;
                default:
                    return false;
            }
        }

        private void toolStripButtonList_Click(object sender, EventArgs e)
        {
            splitContainer2.Panel1Collapsed = !splitContainer2.Panel1Collapsed;
            UpdatePathList();

            if (splitContainer2.Panel1Collapsed)
                CurrentSettings.showListPane = false;
            else
                CurrentSettings.showListPane = true;
            CurrentSettings.SaveSettings();
        }

        private void treeViewPaths_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int index = 0;
            
            //this is a wall
            if (e.Node.Name.Contains("MZP$"))
            {
                curMazeViewer.curMazePaths.UnselectAll();
                index = int.Parse(e.Node.Name.Substring(4));
                curMazeViewer.curMazePaths.cPaths[index].selected = true;
                propertyGrid1.SelectedObject = curMazeViewer.curMazePaths.cPaths[index];
                curMazeViewer.selectedPath = index;
                curMazeViewer.Refresh();
            }

            else if (e.Node.Text.Contains("Paths"))
            {
                curMazeViewer.curMazePaths.SelectAll();
                curMazeViewer.selectedPath = -1;
                curMazeViewer.Refresh();
                propertyGrid1.SelectedObjects = curMazeViewer.curMazePaths.GetSelected();
            }
            else if(e.Node.Text.Length>1)
            {
                curMazeViewer.curMazePaths.UnselectAll();
                foreach (TreeNode t0 in e.Node.Nodes)
                {
                    if (t0.Name.Contains("MZP$"))
                    {
                        index = int.Parse(t0.Name.Substring(4));
                        propertyGrid1.SelectedObject = curMazeViewer.curMazePaths.cPaths[index];
                        curMazeViewer.curMazePaths.cPaths[index].selected = true;
                    }
                    foreach (TreeNode t1 in t0.Nodes)
                    {
                        if (t1.Name.Contains("MZP$"))
                        {
                            index = int.Parse(t1.Name.Substring(4));
                            propertyGrid1.SelectedObject = curMazeViewer.curMazePaths.cPaths[index];
                            curMazeViewer.curMazePaths.cPaths[index].selected = true;
                        }
                        foreach (TreeNode t2 in t1.Nodes)
                        {
                            if (t2.Name.Contains("MZP$"))
                            {
                                index = int.Parse(t2.Name.Substring(4));
                                propertyGrid1.SelectedObject = curMazeViewer.curMazePaths.cPaths[index];
                                curMazeViewer.curMazePaths.cPaths[index].selected = true;
                            }
                            foreach (TreeNode t3 in t2.Nodes)
                            {
                                if (t3.Name.Contains("MZP$"))
                                {
                                    index = int.Parse(t3.Name.Substring(4));
                                    propertyGrid1.SelectedObject = curMazeViewer.curMazePaths.cPaths[index];
                                    curMazeViewer.curMazePaths.cPaths[index].selected = true;
                                }
                                foreach (TreeNode t4 in t3.Nodes)
                                {
                                    if (t4.Name.Contains("MZP$"))
                                    {
                                        index = int.Parse(t4.Name.Substring(4));
                                        propertyGrid1.SelectedObject = curMazeViewer.curMazePaths.cPaths[index];
                                        curMazeViewer.curMazePaths.cPaths[index].selected = true;
                                    }
                                }
                            }
                        }
                    }
                }
                propertyGrid1.SelectedObjects = curMazeViewer.curMazePaths.GetSelected();
                curMazeViewer.Refresh();
            }


        }

        private void logProcessorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogProcessor a = new LogProcessor();
            a.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About ab = new About();
            ab.ShowDialog();
        }


        private void RefreshMazeView(bool redrawMaze = false)
        {
            //curMaze.OffScreenDrawing();
            UpdateTheSize();
            curMazeViewer.RefreshView(redrawMaze);
            if (curMazeViewer.bNewRegionFlag)
                UpdateProjectList();
            Invalidate();

            
        }



        private void tabControl1_Resize(object sender, EventArgs e)
        {
            RefreshMazeView(true);
        }

        private void tabPage1_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void tabPage1_DragDrop(object sender, DragEventArgs e)
        {


        }

        private int CheckInputFileExtension(string inp)
        {
            int index = inp.LastIndexOf(".");
            string ext = inp.Substring(index + 1);
            ext=ext.ToLower();
            if (ext.CompareTo("maz") == 0)
                return 1;
            if (ext.CompareTo("mazx") == 0)
                return 1;
            if (ext.CompareTo("txt") == 0)
                return 2;
            if (ext.CompareTo("dat") == 0)
                return 3;
            if (ext.CompareTo("mzreg") == 0)
                return 3;
            if (ext.CompareTo("mzproj") == 0)
                return 4;
            return 1; //Make default to Maze files...
        }

        private void Main_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void toolStripButtonPlay_Click(object sender, EventArgs e)
        {
            Run();
        }
        private bool running = false;
        System.Diagnostics.Process presentationProcess;

        private void Run()
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath);

            try
            {
                //if (curMaze.FileName == "")
                //{
                //    SaveAs();
                //    return;
                //}
                //if (curMaze.changed)
                //{
                //    SaveMaze();
                //}

                if (curMazeViewer.selectedPath < 0)
                {
                    MessageBox.Show("No path is selected!");
                    return;
                }
                else if (curMazeViewer.selectedPath < curMazeViewer.curMazePaths.cPaths.Count)
                {
                    string param = '\"' + curMazeViewer.curMaze.FileName + "\" -f +r 600 800 +v \"" + curMazeViewer.curMazePaths.cPaths[curMazeViewer.selectedPath].logFileName + "\":" + curMazeViewer.curMazePaths.cPaths[curMazeViewer.selectedPath].melIndex;
                    presentationProcess = System.Diagnostics.Process.Start(path + "\\MazeWalker.exe", param);

                }
                else
                {
                    MessageBox.Show("No path is available!");
                    return;
                }

                //presentationProcess = System.Diagnostics.Process.Start(path + "MazeWalker.exe", '\"' + curMaze.curMaze.FileName + "\" -f +r 600 800 -v \"" + curMaze.curMazePaths.cPaths[0].logFileName + '\"');
                //presentationProcess = System.Diagnostics.Process.Start(path + "MazeWalker.exe", curMaze.FileName);

            }
            catch (System.Exception ex)
            {
                running = false;
                MessageBox.Show("Couldn't initiate Run!\n\n" + ex.Message, "MazeAnalyzer");
                return;
            }

            //Thread.Sleep(a.Timeout * 1000);                    
            //presentationProcess.EnableRaisingEvents = true;
            //presentationProcess.Exited += new EventHandler(presentationProcess_Exited);

            running = false;

        }

        private void toolStripButtonProperties_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;

            if (splitContainer1.Panel2Collapsed)
                CurrentSettings.showPropertiesPane = false;
            else
                CurrentSettings.showPropertiesPane = true;
            CurrentSettings.SaveSettings();
        }

        private void recentMazesToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            recentMazesToolStripMenuItem.DropDownItems.Clear();
            if (CurrentSettings.previousMazeFiles.Count == 0)
            {
                recentMazesToolStripMenuItem.DropDownItems.Add("No previous items");
            }

            //foreach (string s in CurrentSettings.previousFiles)
            //{
            //    //MenuItem temp = new MenuItem(s);
            //    //temp.Click += new EventHandler(LoadPreviousMaze_Click);
            //    //ToolStripItem temp = new MenuItem(s);
            //    //previousToolStripMenuItem.DropDownItems.Add(s);
            //    recentMazesToolStripMenuItem.DropDownItems.Add(s, null, new EventHandler(LoadPreviousMaze_Click));
            //}            

            //string[] str = new string[CurrentSettings.previousFiles.Count];
            //CurrentSettings.previousFiles.CopyTo(str,0);
            //for (int i = CurrentSettings.previousFiles.Count - 1; i >= 0; i--)
            //{
            //    recentMazesToolStripMenuItem.DropDownItems.Add(str[i], null, new EventHandler(LoadPreviousMaze_Click));
            //}

            for (int i = CurrentSettings.previousMazeFiles.Count - 1; i >= 0; i--)
            {
                recentMazesToolStripMenuItem.DropDownItems.Add(CurrentSettings.previousMazeFiles[i], null, new EventHandler(LoadPreviousMaze_Click));
            }

        }

        void LoadPreviousMaze_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //MessageBox.Show(e.ToString());
            LoadMazeFile(sender.ToString());
        }

        void LoadPreviousLog_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //MessageBox.Show(e.ToString());
            LoadLogFile(sender.ToString());
        }

        void LoadPreviousRegion_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //MessageBox.Show(e.ToString());
            LoadRegionFile(sender.ToString());
        }

        void LoadPreviousProject_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //MessageBox.Show(e.ToString());
            LoadProjectFile(sender.ToString());
        }

        private void recentLogsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            recentLogsToolStripMenuItem.DropDownItems.Clear();
            if (CurrentSettings.previousLogFiles.Count == 0)
            {
                recentLogsToolStripMenuItem.DropDownItems.Add("No previous items");
            }

            for (int i = CurrentSettings.previousLogFiles.Count - 1; i >= 0; i--)
            {
                recentLogsToolStripMenuItem.DropDownItems.Add(CurrentSettings.previousLogFiles[i], null, new EventHandler(LoadPreviousLog_Click));
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (bCommandLineFormat)
            {
                LoadMazeFile(sCommandLineFile);
                //Invalidate();
            }
        }

        private void toolStripButtonDefine_Click(object sender, EventArgs e)
        {
            EnterMeasureMode(false); //exit measurement mode if any
            //define a region
            curMazeViewer.EnterDefineRegionMode(MazeViewer.AnalyzerMode.customRegion);
        }

        private void toolStripButtonManage_Click(object sender, EventArgs e)
        {
            EnterMeasureMode(false); //exit measurement mode if any
            //manage regions
            curMazeViewer.ShowMeasurementRegionManager();
            UpdateProjectList();
            RefreshMazeView();
        }

        private void toolStripButtonAnalyze_Click(object sender, EventArgs e)
        {
            EnterMeasureMode(false); //exit measurement mode if any

            //measure with regions
            if (curMazeViewer.curMazePaths.cPaths.Count == 0)
            {
                MessageBox.Show("There are no paths! Please load paths for measurement!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            //else if (curMaze.curRegions.Regions.Count == 0)
            //{
            //    MessageBox.Show("There are no defined regions! Please define region of interest for measurement!",Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            //}           
            else
            {
                //measure
                Measure exp = new Measure(ref curMazeViewer);
                exp.currentExpPriority = altInfoPriority;
                exp.ShowDialog();
            }
        }

        private void viewToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if (curMazeViewer.IsEmpty())
            {
                floorToolStripMenuItem.Enabled = false;
            }
            else
            {
                floorToolStripMenuItem.Enabled = true;
            }
        }

        private void floorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            curMazeViewer.bShowFloor = !curMazeViewer.bShowFloor;
            floorToolStripMenuItem.Checked = curMazeViewer.bShowFloor;
            curMazeViewer.RefreshView(true);
        }

        private void toolStripButtonExport_Click(object sender, EventArgs e)
        {
            EnterMeasureMode(false); //exit measurement mode if any
            //export
            Exporter exp = new Exporter(ref curMazeViewer);
            exp.currentExpPriority = altInfoPriority;
            exp.ShowDialog();
        }

        private void mazeUpdateUtilityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\")) + "\\MazeUpdate.exe");
                this.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Couldn't find MazeUpdate Utility. Please re-install MazeSuite with latest setup", "MazeAnalyzer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            curMazeViewer.RefreshView();
            UpdatePathList();
            projectChanged = true;
        }

        private void openRegionDefinitionFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (curMazeViewer.IsEmpty())
            {
                MessageBox.Show("No maze loaded! Please first open a maze file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                OpenFileDialog a = new OpenFileDialog();
                a.Filter = "Maze Region Definition Files (*.dat) |*.dat| All files (*.*) |*.*";
                a.FilterIndex = 1;
                a.RestoreDirectory = true;
                if (a.ShowDialog() == DialogResult.OK)
                {
                    LoadRegionFile(a.FileName);
                }
            }
        }

        private void recentRegionDefinitionsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            recentRegionDefinitionsToolStripMenuItem.DropDownItems.Clear();
            if (CurrentSettings.previousRegionDefinitionFiles.Count == 0)
            {
                recentRegionDefinitionsToolStripMenuItem.DropDownItems.Add("No previous items");
            }

            for (int i = CurrentSettings.previousRegionDefinitionFiles.Count - 1; i >= 0; i--)
            {
                recentRegionDefinitionsToolStripMenuItem.DropDownItems.Add(CurrentSettings.previousRegionDefinitionFiles[i], null, new EventHandler(LoadPreviousRegion_Click));
            }
        }

        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string str = Path.GetDirectoryName(Application.ExecutablePath) + "\\MazeSuiteManual.pdf";
                //if (File.Exists(str))
                // {
                System.Diagnostics.Process.Start(str);
                //}
            }
            catch //(System.Exception ex)
            {
                MessageBox.Show("Manual file cannot be found!\r\n\r\nPlease see http://www.mazesuite.com/ for documentation.", "MazeSuite");
            }
        }

        private void buttonPanelGallery2_Click(object sender, EventArgs e)
        {
            LaunchLink("http://www.mazesuite.com/gallery/");
        }

        private void buttonPanelForum1_Click(object sender, EventArgs e)
        {
            LaunchLink("http://www.mazesuite.com/forum/");
        }

        void LaunchLink(string link)
        {
            try
            {
                System.Diagnostics.Process.Start(link);
            }
            catch//(Exception ex)
            {
                MessageBox.Show("Can not launch default browser to visit web link: " + link);
            }
        }
        void PositionWelcomePanel()
        {
            if (panelWelcome.Visible)
            {
                panelWelcome.Left = (tabControl1.Width - panelWelcome.Width) / 2;
                panelWelcome.Top = (tabControl1.Height - panelWelcome.Height) / 2;
            }
        }

        private void toolStripButtonDefinePoint_Click(object sender, EventArgs e)
        {
            EnterMeasureMode(false); //exit measurement mode if any
            //define a region
            curMazeViewer.EnterDefineRegionMode(MazeViewer.AnalyzerMode.point);
        }

        private void tabPage1_Resize(object sender, EventArgs e)
        {
            PositionWelcomePanel();
            UpdateTheSize();
        }

        private void pictureBoxMainWindowRightTopLogo_Click(object sender, EventArgs e)
        {
            LaunchLink("http://www.mazesuite.com");
        }

        private void pictureBoxMainWindowRightTopLogo_MouseEnter(object sender, EventArgs e)
        {
            this.pictureBoxMainWindowRightTopLogo.Image = ((System.Drawing.Image)(Properties.Resources.MazeSuite_MouseOver));
        }

        private void pictureBoxMainWindowRightTopLogo_MouseLeave(object sender, EventArgs e)
        {
            this.pictureBoxMainWindowRightTopLogo.Image = ((System.Drawing.Image)(Properties.Resources.MazeSuite));
        }

        private void toolStripButtonTapeMeasure_Click(object sender, EventArgs e)
        {
            EnterMeasureMode(!bMeasuringMode);

        }

        
        protected Point clickPosition;
        protected Point scrollPosition;
        protected Point lastPosition;

        private void tabPage_MouseDown(object sender, MouseEventArgs e)
        {
            if (curMazeViewer.curMaze == null)
                return;
            if (curMazeViewer.curMaze != null && curMazeViewer.curMode == MazeViewer.AnalyzerMode.none && (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle))
            {
                curMazeViewer.curMode = MazeViewer.AnalyzerMode.viewpan;
                clickPosition.X = e.X;
                clickPosition.Y = e.Y;
                lastPosition.X = tabControl1.SelectedTab.AutoScrollPosition.X;
                lastPosition.Y = tabControl1.SelectedTab.AutoScrollPosition.Y;

                Cursor = Cursors.Hand;
            }
            //else
              //  curMazeViewer.MazeViewer_MouseDown(sender, e);

        }

        private void tabPage_MouseUp(object sender, MouseEventArgs e)
        {
            if (curMazeViewer.curMaze == null)
                return;
            if (curMazeViewer.curMode == MazeViewer.AnalyzerMode.viewpan && (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle))
            {
                curMazeViewer.curMode = MazeViewer.AnalyzerMode.none;
                Cursor = Cursors.Default;

            }
            //else
            //  curMazeViewer.MazeViewer_MouseUp(sender, e);
        }

        private void tabPage_MouseMove(object sender, MouseEventArgs e)
        {
            if (curMazeViewer.curMaze == null)
                return;
            curMouseLocation = e.Location;
            UpdateToolStripReport();
            if (curMazeViewer.curMode == MazeViewer.AnalyzerMode.viewpan && (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle))
            {

                Cursor = Cursors.Hand;
                int diffX = Math.Abs(scrollPosition.X - lastPosition.X);
                int diffY = Math.Abs(scrollPosition.Y - lastPosition.Y);
                scrollPosition.X = clickPosition.X - e.X - lastPosition.X;
                scrollPosition.Y = clickPosition.Y - e.Y - lastPosition.Y;
                if (diffX > 2 || diffY > 2)
                {

                    tabControl1.SelectedTab.AutoScrollPosition = scrollPosition;
                    lastPosition.X = tabControl1.SelectedTab.AutoScrollPosition.X;
                    lastPosition.Y = tabControl1.SelectedTab.AutoScrollPosition.Y;
                }
            }
        }


        private const int WM_SCROLL = 276; // Horizontal scroll 
        private const int SB_LINELEFT = 0; // Scrolls one cell left 
        private const int SB_LINERIGHT = 1; // Scrolls one line right
        private void tabControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            double curScale = curMazeViewer.curMaze.Scale;
            var direction = e.Delta > 0 ? SB_LINELEFT : SB_LINERIGHT;
            if (ModifierKeys == Keys.Control)
            {
                if (curScale > 17 * Math.Pow(1.1, 15))
                    curScale = 17 * Math.Pow(1.1, 15);
                else if((curScale < 17 * Math.Pow(1.1, -10)))
                    curScale = 17 * Math.Pow(1.1, -10);
                if (direction == 0)
                    curMazeViewer.SetScale(curScale * 1.1f);
                else
                    curMazeViewer.SetScale(curScale * 0.9f);
                RefreshMazeView(true);
            }
        }

        Point curMouseLocation = new Point(0, 0);
        private PointF Mouse2Maze(float X, float Y)
        {
            
            return curMazeViewer.Mouse2Maze(X,Y);
        }
        private void UpdateToolStripReport()
        {
            if (curMazeViewer.curMaze != null)
            {
                PointF mouseLoc = Mouse2Maze(curMouseLocation.X, curMouseLocation.Y);
                PointF viewOffset = Mouse2Maze((float)curMazeViewer.curMaze.minX, (float)curMazeViewer.curMaze.minY);
                toolStripStatusLabel1.Text = ""; // Offset: " + viewOffset.X.ToString("F3") + ", " + viewOffset.Y.ToString("F3"); Offset not really used in Maze Analyzer
                if (curMouseLocation.X > 0 && curMouseLocation.Y > 0 /*&& curMouseLocation.X < tabPage1.Width && curMouseLocation.Y < tabPage1.Height*/)
                { 
                    toolStripStatusLabel1.Text += "Position: " + (mouseLoc.X).ToString("F3") + "," + (mouseLoc.Y + viewOffset.Y).ToString("F3"); //Maze Position

                }
                if (bMeasuringMode)
                {
                    if (bMeasuringInitialized)
                    {
                        PointF diff = new PointF(mouseLoc.X - lastClickedPosition.X, mouseLoc.Y - lastClickedPosition.Y);
                        //toolStripStatusLabel1.Text += "     DistanceXZ: " + Math.Round(diff.X, 3) + "," + Math.Round(diff.Y, 3) + "     DistanceEuclidean: " + Math.Round(Math.Sqrt(Math.Pow(diff.X, 2) + Math.Pow(diff.Y, 2)), 4);
                        toolStripStatusLabel1.Text += "     DistanceXZ: " + diff.X.ToString("F3") + "," + diff.Y.ToString("F3") + "     DistanceEuclidean: " + Math.Sqrt(Math.Pow(diff.X, 2) + Math.Pow(diff.Y, 2)).ToString("F4") + "   (ctrl+c to copy, right-click to reset)";

                    }
                    else
                    {
                        toolStripStatusLabel1.Text += "     Click somewhere on the maze to select reference point; (Right-click to reset measurement mode)";
                    }
                }
            }
            else
                toolStripStatusLabel1.Text = "";
        }


        private void EnterMeasureMode(bool inp)
        {
            if (inp && curMazeViewer.curMaze != null)
            {
                //enter measuring mode
                tabPage1.Cursor = Cursors.Cross;
                bMeasuringMode = true;
                bMeasuringInitialized = false;
                toolStripButtonMeasure.Checked = true;
                if (curMazeViewer.bNewRegionFlag)
                    UpdateProjectList();
                curMazeViewer.EnterDefineRegionMode(MazeViewer.AnalyzerMode.ruler);

            }
            else
            {
                //exit measuring mode
                tabPage1.Cursor = Cursors.Default;
                bMeasuringMode = false;
                bMeasuringInitialized = false;
                toolStripButtonMeasure.Checked = false;
                UpdateToolStripReport();
                if (curMazeViewer.bNewRegionFlag)
                    UpdateProjectList();
                curMazeViewer.EnterDefineRegionMode(MazeViewer.AnalyzerMode.none);
            }
        }

        private void tabPage1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (curMazeViewer.curMaze == null)
                return;

            if (e.KeyCode == Keys.Escape)
            {
                EnterMeasureMode(false);
                return;
            }

            if (e.KeyCode == Keys.C && e.Control == true)
            {
                if (bMeasuringMode && bMeasuringInitialized)
                {
                    if (toolStripStatusLabel1.Text.Contains("("))
                        Clipboard.SetText(toolStripStatusLabel1.Text.Substring(0, toolStripStatusLabel1.Text.IndexOf('(')));
                    else
                        Clipboard.SetText(toolStripStatusLabel1.Text);
                }
                else
                    curMazeViewer.toClipboard();
            }
        }


        private void buttonWelcomeLoadLast_Click(object sender, EventArgs e)
        {

            if (CurrentSettings.previousProjectFiles.Count > 0)
            {
                String lastProjectFile = CurrentSettings.previousProjectFiles[CurrentSettings.previousProjectFiles.Count - 1];
                LoadProjectFile(lastProjectFile);
            }
            updateLastProjectButtons();

        }

        private void pathsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            curMazeViewer.bShowPaths = !curMazeViewer.bShowPaths;
            pathsToolStripMenuItem.Checked = curMazeViewer.bShowPaths;
            curMazeViewer.RefreshView(true);
        }

        private void regionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            curMazeViewer.bShowRegions = !curMazeViewer.bShowRegions;
            regionsToolStripMenuItem.Checked = curMazeViewer.bShowRegions;
            curMazeViewer.RefreshView(true);
        }

        private void fillRegionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            curMazeViewer.bFillRegions = !curMazeViewer.bFillRegions;
            fillRegionsToolStripMenuItem.Checked = curMazeViewer.bFillRegions;
            curMazeViewer.RefreshView(true);
        }

        private void endRegionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            curMazeViewer.bShowEndRegions = !curMazeViewer.bShowEndRegions;
            curMazeViewer.bShowEndRegions = !curMazeViewer.bShowEndRegions;
            endRegionsToolStripMenuItem.Checked = curMazeViewer.bShowEndRegions;
            curMazeViewer.RefreshView(true);
        }

        private void lightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            curMazeViewer.bShowLights = !curMazeViewer.bShowLights;
            lightsToolStripMenuItem.Checked = curMazeViewer.bShowLights;
            curMazeViewer.RefreshView(true);
        }

        private void modelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            curMazeViewer.bShowModels = !curMazeViewer.bShowModels;
            modelsToolStripMenuItem.Checked = curMazeViewer.bShowModels;
            curMazeViewer.RefreshView(true);
        }

        private void toolStripButtonDefineRectRegion_Click(object sender, EventArgs e)
        {
            EnterMeasureMode(false); //exit measurement mode if any
            //define a region
            curMazeViewer.EnterDefineRegionMode(MazeViewer.AnalyzerMode.rectRegion);
        }

        private void startPositionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            curMazeViewer.bShowStartPositions = !curMazeViewer.bShowStartPositions;
            startPositionsToolStripMenuItem.Checked = curMazeViewer.bShowStartPositions;
            curMazeViewer.RefreshView(true);
        }

        private void activeRegionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            curMazeViewer.bShowActiveRegions = !curMazeViewer.bShowActiveRegions;
            activeRegionsToolStripMenuItem.Checked = curMazeViewer.bShowActiveRegions;
            curMazeViewer.RefreshView(true);
        }

        private void newAnalyzerProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewAnalyzerProject();
        }

        private void openAnalyzerProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open(mzFileType.Project);
        }

        private void nextMazeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            curMazeViewer.NextMaze();
            if(curMazeViewer.curMaze!=null)
                SetTabByIndex(curMazeViewer.curMaze.Index);
            UpdatePathList();
            RefreshMazeView(true);
        }



        private void projectToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            ToolStripMenuItem projMazeSelect, projMazeRemove;
            projectToolStripMenuItem.DropDownItems.Clear();
            projectToolStripMenuItem.DropDownItems.Add(nextMazeToolStripMenuItem);
            projectToolStripMenuItem.DropDownItems.Add(previousMazeToolStripMenuItem);
          
            int listIndex = 0;
            foreach (MazeLib.ExtendedMaze exMaze in curMazeViewer.projMazeList)
            {

                projMazeSelect = new System.Windows.Forms.ToolStripMenuItem();
                projMazeSelect.Name = listIndex + "$" + exMaze.FileName;
                projMazeSelect.Size = new System.Drawing.Size(152, 22);
                projMazeSelect.Text = listIndex + 1 + ": " + exMaze.FileName;
                projMazeSelect.Click += new System.EventHandler(this.projMazeSelectMenuItem_Click);

                projMazeRemove = new System.Windows.Forms.ToolStripMenuItem();
                projMazeRemove.Name = "Remove$" + exMaze.Index + "$" + exMaze.FileName;
                projMazeRemove.Size = new System.Drawing.Size(152, 22);
                projMazeRemove.Text = "Remove from Project";
                projMazeRemove.Click += new System.EventHandler(this.projMazeRemoveMenuItem_Click);

                projMazeSelect.DropDownItems.Add(projMazeRemove);

                projectToolStripMenuItem.DropDownItems.Add(projMazeSelect);
                listIndex++;
            }
        }

        private void projMazeSelectMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem button = (ToolStripMenuItem)sender;
            string[] parts = button.Name.Split('$');
            if (parts.Length > 0)
            {
                int listIndex = 0;
                int.TryParse(parts[0], out listIndex);
                SetTabByIndex(listIndex);
                UpdatePathList();
                projectToolStripMenuItem.HideDropDown();
            }
        }

        private void projMazeRemoveMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem button = (ToolStripMenuItem)sender;
            string[] parts = button.Name.Split('$');
            if (parts.Length > 1)
            {
                int listIndex = 0;
                int.TryParse(parts[1], out listIndex);
                curMazeViewer.RemoveMazeByIndex(listIndex);
                UpdatePathList();
                UpdateTabs();
                UpdateProjectList();
                if (curMazeViewer.projMazeList.Count == 0)
                    CloseAnalyzerProject();
                else { 
                      SetTabByIndex(curMazeViewer.curMaze.Index);
                //projectToolStripMenuItem_DropDownOpening(sender,e);
                
                     projectChanged = true;
                }

            }
        }

        private void closeAnalyzerProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dRes;
            if(projectChanged)
            {
                dRes = MessageBox.Show("Current project (" + Path.GetFileName(curMazeViewer.projectInfo.ProjectFilename) + ") has been changed.\t\n\nDo you want to save?", "MazeAnalyzer", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

                if (dRes == DialogResult.Yes)
                    SaveAnalyzerProject();
                else if (dRes == DialogResult.Cancel)
                    return;
            }
            CloseAnalyzerProject();
        }

        private void NewAnalyzerProject()
        {
            //if (curMaze.changed && MessageBox.Show("Current maze (" + curMaze.Name + ") has been changed.\t\n\nDo you want to save?", "MazeMaker", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            //{
            //    SaveMaze();
            //}
            //if (MessageBox.Show("Do you want to close current maze!", "MazeMaker", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            //{
            DialogResult dRes;
            if (projectChanged)
            {
                dRes = MessageBox.Show("Current project (" + Path.GetFileName(curMazeViewer.projectInfo.ProjectFilename) + ") has been changed.\t\n\nDo you want to save?", "MazeAnalyzer", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

                if (dRes == DialogResult.Yes)
                    SaveAnalyzerProject();
                else if (dRes == DialogResult.Cancel)
                    return;
            }


            AnalyzerProjectWizard projWizard = new AnalyzerProjectWizard();
            List<string> mazeFiles = new List<string>();

            if (projWizard.ShowDialog() == DialogResult.OK)
            {
                CloseAnalyzerProject();
                curMazeViewer.projectInfo.ProjectName = projWizard.projectName;
                curMazeViewer.projectInfo.ProjectDescription = projWizard.projectDescription;
                mazeFiles = projWizard.mazeFileNames;
            }
            else
                return;

            foreach (string file in mazeFiles)
            {
                LoadMazeFile(file);
            }

            if (curMazeViewer.curMaze!=null)
            {

                EnableProjectButtons(true);
            }
        }

        
        private bool SaveAsAnalyzerProject()
        {
            //save to file..
            SaveFileDialog a = new SaveFileDialog();
            a.Filter = "Maze Analyzer Project Files | *.mzproj";
            a.FilterIndex = 1;
            a.DefaultExt = ".mzproj";
            a.RestoreDirectory = true;


            if (a.ShowDialog() == DialogResult.OK)
            {
                curMazeViewer.projectInfo.ProjectFilename = a.FileName;
                this.Text = Application.ProductName + " - " + Path.GetFileNameWithoutExtension(curMazeViewer.projectInfo.ProjectFilename);
                if (curMazeViewer.curMaze != null)
                    this.Text += " - " + Path.GetFileNameWithoutExtension(curMazeViewer.curMaze.FileName);
                return SaveAnalyzerProject();
            }
            else
                return false;
        }



        private bool SaveAnalyzerProject()
        {
            if (String.Compare(curMazeViewer.projectInfo.ProjectFilename,"UnsavedProject")==0|| curMazeViewer.projectInfo.ProjectFilename.Length <= 1)
            {
                return SaveAsAnalyzerProject();
               
            }
            string filename = curMazeViewer.projectInfo.ProjectFilename;
            string path = Path.GetDirectoryName(filename);

            XmlElement regionNode, mazeNode, pathNode;
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            XmlElement projectXMLnode = doc.CreateElement(string.Empty, "MazeAnalyzerProject", string.Empty);
            doc.AppendChild(projectXMLnode);
            projectXMLnode.SetAttribute("version", "1.1");
            projectXMLnode.SetAttribute("url", "http://www.mazesuite.com");

            if (curMazeViewer.projectInfo.ProjectName.Length < 1)
                curMazeViewer.projectInfo.ProjectName = Path.GetFileNameWithoutExtension(filename);

            XmlElement infoNode = doc.CreateElement(string.Empty, "Info", string.Empty);
            projectXMLnode.AppendChild(infoNode);
            infoNode.SetAttribute("ProjectName", curMazeViewer.projectInfo.ProjectName);
            infoNode.InnerText = curMazeViewer.projectInfo.ProjectDescription;



            Directory.SetCurrentDirectory(path);
            Uri uri = new Uri(filename);

    
            string relativePath;

            foreach (MazeLib.ExtendedMaze exmz in curMazeViewer.projMazeList)
            {
                //tring relativeMazePath = Path.GetFileName(exmz.FileName);

                mazeNode = doc.CreateElement(string.Empty, "Maze", string.Empty);
                projectXMLnode.AppendChild(mazeNode);
                

                var relativeMazePath = uri.MakeRelativeUri(new Uri(exmz.FileName)).ToString();
                mazeNode.SetAttribute("file", relativeMazePath);

                XmlElement logNode,regionCollectionNode,vertexNode;
                regionCollectionNode = doc.CreateElement(string.Empty, "Regions", string.Empty);
                mazeNode.AppendChild(regionCollectionNode);
                //List<string> regionFiles=new List<string>();
                List<MazeLib.MeasurementRegion> mzRegions = curMazeViewer.GetRegionsByIndex(exmz.Index).Regions;
                foreach (MazeLib.MeasurementRegion mzR in mzRegions)
                {
                    // = new Uri(Directory.GetCurrentDirectory());
                    regionNode = doc.CreateElement(string.Empty, "Region", string.Empty);
                    regionCollectionNode.AppendChild(regionNode);
                    regionNode.SetAttribute("Name", mzR.Name);
                    regionNode.SetAttribute("Ymax", mzR.Ymax.ToString());
                    regionNode.SetAttribute("Ymin", mzR.Ymin.ToString());
                    regionNode.SetAttribute("RegionGroup", mzR.RegionGroup);
                    regionNode.SetAttribute("ROI", mzR.ROI.ToString());
                    foreach (PointF vertex in mzR.Vertices)
                    {
                        vertexNode = doc.CreateElement(string.Empty, "Vertex", string.Empty);
                        regionNode.AppendChild(vertexNode);

                        vertexNode.SetAttribute("X", vertex.X.ToString());
                        vertexNode.SetAttribute("Y", vertex.Y.ToString());
                    }

                    //    regionNode.SetAttribute("file", relativePath);
                    //    regionFiles.Add(mzR.filename);
                }
                //var unique_items = new HashSet<string>(regionFiles);
                //bool hasNewItems = false;
                //foreach (string s in unique_items)
                //{
                //    if(string.Compare(s,"new")==0)
                //    {
                //        hasNewItems = true;
                //    }
                //}

                //if (hasNewItems)
                //{
                //    curMazeViewer.GetRegionsByIndex(exmz.Index).MzFile = exmz.FileName;
                //    string newFilename= curMazeViewer.GetRegionsByIndex(exmz.Index).saveRegionDefinitionFile("Save Unsaved Region Definitions for " + exmz.FileName);
                //    if (newFilename.Length < 1)
                //        return false;
                //    relativePath = uri.MakeRelativeUri(new Uri(newFilename)).ToString();
                //    regionNode = doc.CreateElement(string.Empty, "RegionDefinitionFile", string.Empty);
                //    regionCollectionNode.AppendChild(regionNode);
                //    regionNode.SetAttribute("file", relativePath);
                //}
                //else { 
                //    foreach (string s in unique_items)
                //    {

                //        //uri = new Uri(Directory.GetCurrentDirectory());
                //        relativePath = uri.MakeRelativeUri(new Uri(s)).ToString();
                //        regionNode = doc.CreateElement(string.Empty, "RegionDefinitionFile", string.Empty);
                //        regionCollectionNode.AppendChild(regionNode);
                //        regionNode.SetAttribute("file", relativePath);
                //    }
                //}


                List<MazeLib.MazePathItem> mzPaths = curMazeViewer.GetPathsByIndex(exmz.Index).cPaths ;
                List<string> logFiles = new List<string>();

                foreach (MazeLib.MazePathItem mzP in mzPaths)
                {
                    // = new Uri(Directory.GetCurrentDirectory());

                    logFiles.Add(mzP.logFileName);
                }
                
                var unique_items_log = new HashSet<string>(logFiles);
                foreach (string logName in unique_items_log)
                {
                    relativePath = uri.MakeRelativeUri(new Uri(logName)).ToString();
                    logNode = doc.CreateElement(string.Empty, "LogFile", string.Empty);
                    mazeNode.AppendChild(logNode);
                    logNode.SetAttribute("file", relativePath);

                    foreach (MazeLib.MazePathItem mzP in mzPaths)
                    {

                        if(string.Compare(mzP.logFileName,logName)==0)
                        { 
                            pathNode = doc.CreateElement(string.Empty, "PathInfo", string.Empty);
                            logNode.AppendChild(pathNode);
                            
                            pathNode.SetAttribute("melIndex", mzP.melIndex.ToString());
                            pathNode.SetAttribute("Group", mzP.ExpGroup.ToString());
                            pathNode.SetAttribute("Subject", mzP.ExpSubjectID.ToString());
                            pathNode.SetAttribute("Condition", mzP.ExpCondition.ToString());
                            pathNode.SetAttribute("Session", mzP.ExpSession.ToString());
                            pathNode.SetAttribute("Trial", mzP.ExpTrial.ToString());
                            if(mzP.MazeColorRegular != Color.Transparent)
                                pathNode.SetAttribute("Color", mzP.MazeColorRegular.R + "," + mzP.MazeColorRegular.G + "," + mzP.MazeColorRegular.B);
                        }
                    }
                }

            }

            try
            { 
                doc.Save(filename);
            }
            catch {
                if (CurrentSettings.previousLogFiles.Contains(filename))
                {
                    CurrentSettings.previousLogFiles.Remove(filename);
                    CurrentSettings.SaveSettings();
                }
                return false;
            }
            projectChanged = false;


            CurrentSettings.AddProjectFileToPrevious(filename);

            return true;
        }

        //private bool SaveAnalyzerProject()
        //{
        //    if (String.Compare(curMazeViewer.projectInfo.ProjectFilename, "UnsavedProject") == 0 || curMazeViewer.projectInfo.ProjectFilename.Length <= 1)
        //    {
        //        return SaveAsAnalyzerProject();

        //    }
        //    string filename = curMazeViewer.projectInfo.ProjectFilename;
        //    string path = Path.GetDirectoryName(filename);

        //    Directory.SetCurrentDirectory(path);
        //    Uri uri = new Uri(filename);

        //    string fpOut = "";

        //    string relativePath;
        //    fpOut += "MazeAnalyzer Project File v1.0\n";
        //    if (curMazeViewer.projectInfo.ProjectName.Length < 1)
        //        curMazeViewer.projectInfo.ProjectName = Path.GetFileNameWithoutExtension(filename);
        //    fpOut += "ProjectName\t" + curMazeViewer.projectInfo.ProjectName + "\n";
        //    string[] descripOut = curMazeViewer.projectInfo.ProjectDescription.Split('\n');
        //    foreach (string dOut in descripOut)
        //    {
        //        if (dOut.Length > 0)
        //            fpOut += "ProjectDescription\t" + dOut + "\n";
        //    }

        //    fpOut += "Type\tFilename\tGroup\tSubject\tCondition\tSession\tTrial\tColor\n";//\tField1\tField2\tField3\tField4\tField5");
        //    foreach (MazeLib.ExtendedMaze exmz in curMazeViewer.projMazeList)
        //    {
        //        //tring relativeMazePath = Path.GetFileName(exmz.FileName);


        //        var relativeMazePath = uri.MakeRelativeUri(new Uri(exmz.FileName)).ToString();
        //        fpOut += "maze\t" + relativeMazePath + "\n";

        //        List<string> regionFiles = new List<string>();
        //        List<MazeLib.MeasurementRegion> mzRegions = curMazeViewer.GetRegionsByIndex(exmz.Index).Regions;
        //        foreach (MazeLib.MeasurementRegion mzR in mzRegions)
        //        {
        //            // = new Uri(Directory.GetCurrentDirectory());

        //            regionFiles.Add(mzR.filename);
        //        }
        //        var unique_items = new HashSet<string>(regionFiles);
        //        bool hasNewItems = false;
        //        foreach (string s in unique_items)
        //        {
        //            if (string.Compare(s, "new") == 0)
        //            {
        //                hasNewItems = true;
        //            }
        //        }

        //        if (hasNewItems)
        //        {
        //            curMazeViewer.GetRegionsByIndex(exmz.Index).MzFile = exmz.FileName;
        //            string newFilename = curMazeViewer.GetRegionsByIndex(exmz.Index).saveRegionDefinitionFile("Save Unsaved Region Definitions for " + exmz.FileName);
        //            if (newFilename.Length < 1)
        //                return false;
        //            relativePath = uri.MakeRelativeUri(new Uri(newFilename)).ToString();
        //            fpOut += "regions\t" + relativePath + "\n";
        //        }
        //        else
        //        {
        //            foreach (string s in unique_items)
        //            {
        //                //uri = new Uri(Directory.GetCurrentDirectory());
        //                relativePath = uri.MakeRelativeUri(new Uri(s)).ToString();
        //                fpOut += "regions\t" + relativePath + "\n";
        //            }
        //        }


        //        List<MazeLib.MazePathItem> mzPaths = curMazeViewer.GetPathsByIndex(exmz.Index).cPaths;
        //        foreach (MazeLib.MazePathItem mzP in mzPaths)
        //        {
        //            relativePath = uri.MakeRelativeUri(new Uri(mzP.logFileName)).ToString();
        //            fpOut += "logfile\t" + relativePath + ":" + mzP.MelIndex + "\t" + mzP.ExpGroup + "\t" + mzP.ExpSubjectID + "\t" + mzP.ExpCondition + "\t" + mzP.ExpSession + "\t" + mzP.ExpTrial + "\t";
        //            if (mzP.MazeColorRegular == Color.Transparent)
        //                fpOut += "\n";
        //            else
        //                fpOut += mzP.MazeColorRegular.R + "," + mzP.MazeColorRegular.G + "," + mzP.MazeColorRegular.B + "\n";
        //        }

        //    }


        //    StreamWriter fp = new StreamWriter(filename);

        //    if (fp == null)
        //    {
        //        if (CurrentSettings.previousLogFiles.Contains(filename))
        //        {
        //            CurrentSettings.previousLogFiles.Remove(filename);
        //            CurrentSettings.SaveSettings();
        //        }
        //        return false;
        //    }

        //    fp.Write(fpOut);
        //    fp.Close();

        //    projectChanged = false;


        //    CurrentSettings.AddProjectFileToPrevious(filename);

        //    return true;
        //}


        private void CloseAnalyzerProject()
        {
            curMazeViewer.ClearAll();

            propertyGrid1.SelectedObject = null;
            //curMazeViewer.NewMaze();
            panelWelcome.Visible = true;            
            EnterMeasureMode(false); //exit measurement mode if any
            this.Text = Application.ProductName;
            //CurrentSettings.AddPreviousFile(inp);
            toolStrip2.Enabled = false;

            UpdateTabs();
            
            updateLastProjectButtons();
            tabPage1.Text = "Welcome to MazeAnalyer";
            tabPage1.Name = "WelcomePage";
            EnableProjectButtons(false);
            RefreshMazeView();
            UpdatePathList();
            UpdateProjectList();
        }

        private void tabControl1_Scroll(object sender, ScrollEventArgs e)
        {

        }



        private void newProjectFileToolStripButton_Click(object sender, EventArgs e)
        {
            NewAnalyzerProject();
        }

        private void openProjectFileToolStripButton_Click(object sender, EventArgs e)
        {
            Open(mzFileType.Project);
        }

        private void saveAsProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAsAnalyzerProject();
        }

        private void saveProjectFileToolStripButton_Click(object sender, EventArgs e)
        {
            SaveAnalyzerProject();
        }

        private void toolStripButton_ExpInfoPriority_Click(object sender, EventArgs e)
        {
            ExpInfoPrioritySort expPrioritySort = new ExpInfoPrioritySort(expInfoPrioity);
            var dialogResult = expPrioritySort.ShowDialog();
            UpdatePathList();
        }

        private void copyMazeToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (curMazeViewer.curMaze!=null)
            {
                    curMazeViewer.toClipboard();
            }
        }

        private void changeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExpInfoPrioritySort expPrioritySort = new ExpInfoPrioritySort(expInfoPrioity);
            var dialogResult = expPrioritySort.ShowDialog();
            UpdatePathList();
        }

        private void tabControl1_DragDrop(object sender, DragEventArgs e)
        {
            string[] a = (string[])e.Data.GetData(DataFormats.FileDrop);
            for (int i = 0; i < a.Length; i++)
            {
                if (a.Length >= 1)
                {
                    int ext = CheckInputFileExtension(a[i]);
                    if (ext == 1)
                    {
                        //Reset();
                        //curMaze.OpenMazeFile(a[0], this.tabControl1.Width, this.tabControl1.Height);
                        //this.Text = Application.ProductName + " - " + a[0];
                        LoadMazeFile(a[i]);

                    }
                    else if (ext == 2)
                    {
                        LoadLogFile(a[i]);
                    }
                    else if (ext == 3)
                    {
                        LoadRegionFile(a[i]);
                    }
                    else if (ext == 4)
                    {
                        LoadProjectFile(a[i]);
                    }
                }
            }
        }

        private void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (curMazeViewer.curMaze == null) return;

            if (e.Button == MouseButtons.Right && bMeasuringMode)
            {

                EnterMeasureMode(true);
            }
            else if (e.Button == MouseButtons.Left)
            {
                lastClickedPosition = Mouse2Maze(e.X, e.Y);
                bMeasuringInitialized = true;

            }
        }

        private void tabControl1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void tabControl1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        //private void tabControl1_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (curMazeViewer.curMaze == null)
        //        return;
        //    curMouseLocation = e.Location;
        //    UpdateToolStripReport();
        //}

        private void UpdateTabs()
        {
            //if(curMazeViewer.curMaze!=null)
            //{
                
            int i = 0;
            List<TabPageNoScroll> tabCollection = new List<TabPageNoScroll>();
            curMazeViewer.UpdateIndicies();
            foreach(ExtendedMaze e in curMazeViewer.projMazeList)
            {
                    
                if (i > 0)
                {
                    TabPageNoScroll newTabPage = new TabPageNoScroll();
                    
                    newTabPage.Controls.Add(curMazeViewer);
                    tabCollection.Add(newTabPage);
                    newTabPage.Name = "MZFILE$" + i.ToString();
                    newTabPage.Text = Path.GetFileNameWithoutExtension(e.FileName)+".maz";

                    newTabPage.DragDrop += new DragEventHandler(tabControl1_DragDrop);
                    newTabPage.DragEnter += new DragEventHandler(tabControl1_DragEnter);
                    newTabPage.AutoSize = true;
                    newTabPage.AllowDrop = true;
                    
                    newTabPage.AutoScroll = true;

                }
                else
                {
                    tabPage1.Text = Path.GetFileNameWithoutExtension(e.FileName) + ".maz";
                    tabPage1.Name = "MZFILE$" + i.ToString();

                    tabCollection.Add((TabPageNoScroll)tabPage1);
                    this.tabPage1.BackColor = System.Drawing.Color.LightGray;
                    this.tabPage1.BackgroundImage = null;
                }
                i++;
            }
            if(i==0)
            {
                tabPage1.Text = "Welcome";
                tabPage1.Name = "WelcomePage";
                tabPage1.AutoSize = true;
                tabPage1.BackgroundImage = Properties.Resources.mazeAnalyzerBG;
                tabCollection.Add((TabPageNoScroll)tabPage1);
            }
            tabControl1.TabPages.Clear();
            tabControl1.TabPages.AddRange(tabCollection.ToArray());

            RefreshMazeView(true);
            //}
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            AddViewerToSelectedTab();
        }

        private void AddViewerToSelectedTab()
        {
            curMazeViewer.EnterDefineRegionMode(MazeViewer.AnalyzerMode.none);
            TabPageNoScroll currentTabPage = (TabPageNoScroll)tabControl1.SelectedTab;
            if (currentTabPage != null)
            {
                currentTabPage.Controls.Add(curMazeViewer);

                curMazeViewer.Location = new Point(0, 0);
                
                //ToolStripMenuItem button = (ToolStripMenuItem)sender;
                string[] parts = currentTabPage.Name.Split('$');
                if (curMazeViewer.curMaze != null)
                {
                    int listIndex = curMazeViewer.curMaze.Index;
                    if (parts.Length > 1)
                    {

                        int.TryParse(parts[1], out listIndex);
                    }
                    if (listIndex != curMazeViewer.curMaze.Index)
                    {
                        curMazeViewer.SetMazeByListIndex(listIndex);


                        UpdatePathList();
                    }
                }
                //projectToolStripMenuItem.HideDropDown();
                CenterOnStart();
                RefreshMazeView(true);

            }
        }

        private void UpdateTheSize()
        {
            TabPageNoScroll currentTabPage = (TabPageNoScroll) tabControl1.SelectedTab;
            curMazeViewer.UpdateTheSize(currentTabPage.Width, currentTabPage.Height);
        }

        private void SetTabByIndex(int index)
        {
            foreach(TabPage tPage in tabControl1.TabPages)
            {
                string[] parts = tPage.Name.Split('$');
                if (parts.Length > 1)
                {
                    int listIndex = 0;
                    int.TryParse(parts[1], out listIndex);

                    if (listIndex == index)
                    { 
                        tabControl1.SelectedTab = tPage;
                        AddViewerToSelectedTab();
                        return;
                    }
                }
            }

            tabControl1.SelectedTab = tabPage1;
            AddViewerToSelectedTab();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAnalyzerProject();
        }

        private void filePaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer2.Panel1Collapsed = !splitContainer2.Panel1Collapsed;
            projectPaneToolStripMenuItem.Checked = !splitContainer2.Panel1Collapsed;
            UpdatePathList();

            if (splitContainer2.Panel1Collapsed)
                CurrentSettings.showListPane = false;
            else
                CurrentSettings.showListPane = true;
            CurrentSettings.SaveSettings();
        }

        private void propertyPaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;
            propertyPaneToolStripMenuItem.Checked = !splitContainer1.Panel2Collapsed;

            if (splitContainer1.Panel2Collapsed)
                CurrentSettings.showPropertiesPane = false;
            else
                CurrentSettings.showPropertiesPane = true;
            CurrentSettings.SaveSettings();
        }

        private void recentProjectsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            recentProjectsToolStripMenuItem.DropDownItems.Clear();
            if (CurrentSettings.previousProjectFiles.Count == 0)
            {
                recentProjectsToolStripMenuItem.DropDownItems.Add("No previous items");
            }

            //foreach (string s in CurrentSettings.previousFiles)
            //{
            //    //MenuItem temp = new MenuItem(s);
            //    //temp.Click += new EventHandler(LoadPreviousMaze_Click);
            //    //ToolStripItem temp = new MenuItem(s);
            //    //previousToolStripMenuItem.DropDownItems.Add(s);
            //    recentMazesToolStripMenuItem.DropDownItems.Add(s, null, new EventHandler(LoadPreviousMaze_Click));
            //}            

            //string[] str = new string[CurrentSettings.previousFiles.Count];
            //CurrentSettings.previousFiles.CopyTo(str,0);
            //for (int i = CurrentSettings.previousFiles.Count - 1; i >= 0; i--)
            //{
            //    recentMazesToolStripMenuItem.DropDownItems.Add(str[i], null, new EventHandler(LoadPreviousMaze_Click));
            //}

            for (int i = CurrentSettings.previousProjectFiles.Count - 1; i >= 0; i--)
            {
                recentProjectsToolStripMenuItem.DropDownItems.Add(CurrentSettings.previousProjectFiles[i], null, new EventHandler(LoadPreviousProject_Click));
            }

        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dRes;
            if (projectChanged)
            {
                dRes = MessageBox.Show("Current project (" + Path.GetFileName(curMazeViewer.projectInfo.ProjectFilename) + ") has been changed.\t\n\nDo you want to save?", "MazeAnalyzer", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

                if (dRes == DialogResult.Yes)
                    SaveAnalyzerProject();
                else if (dRes == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void previousMazeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            curMazeViewer.PrevMaze();
            if (curMazeViewer.curMaze != null)
                SetTabByIndex(curMazeViewer.curMaze.Index);
            
            RefreshMazeView(true);
            UpdatePathList();
        }

        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            
        }

        private void removePathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = 0;
            TreeNode selectedNode = treeViewPaths.SelectedNode;
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (item.Name.Contains("ProjectContext"))
                selectedNode = treeViewProject.SelectedNode;

            //this is a wall
            if (selectedNode.Name.Contains("MZP$"))
            {
                string[] parts = selectedNode.Name.Split('$');
                if(parts.Length>1)
                    index = int.Parse(parts[parts.Length-1]);

                curMazeViewer.curMazePaths.UnselectAll();
                
                curMazeViewer.curMazePaths.cPaths[index].selected = true;
                curMazeViewer.curMazePaths.RemoveSelected();
                
                //UpdateTabs();
                projectChanged = true;
                curMazeViewer.Refresh();
                UpdatePathList();
                UpdateProjectList();

            }

            else if (selectedNode.Text.Contains("Paths"))
            {
                curMazeViewer.curMazePaths.SelectAll();
                curMazeViewer.selectedPath = -1;
                curMazeViewer.curMazePaths.RemoveSelected();
                
                //UpdateTabs();
                projectChanged = true;
                curMazeViewer.Refresh();
                UpdatePathList();
                UpdateProjectList();

            }
            else if (selectedNode.Text.Length > 1)
            {
                curMazeViewer.curMazePaths.UnselectAll();
                foreach (TreeNode t0 in selectedNode.Nodes)
                {
                    if (t0.Name.Contains("MZP$"))
                    {
                        index = int.Parse(t0.Name.Substring(4));
                        propertyGrid1.SelectedObject = curMazeViewer.curMazePaths.cPaths[index];
                        curMazeViewer.curMazePaths.cPaths[index].selected = true;
                    }
                    foreach (TreeNode t1 in t0.Nodes)
                    {
                        if (t1.Name.Contains("MZP$"))
                        {
                            index = int.Parse(t1.Name.Substring(4));
                            propertyGrid1.SelectedObject = curMazeViewer.curMazePaths.cPaths[index];
                            curMazeViewer.curMazePaths.cPaths[index].selected = true;
                        }
                        foreach (TreeNode t2 in t1.Nodes)
                        {
                            if (t2.Name.Contains("MZP$"))
                            {
                                index = int.Parse(t2.Name.Substring(4));
                                propertyGrid1.SelectedObject = curMazeViewer.curMazePaths.cPaths[index];
                                curMazeViewer.curMazePaths.cPaths[index].selected = true;
                            }
                            foreach (TreeNode t3 in t2.Nodes)
                            {
                                if (t3.Name.Contains("MZP$"))
                                {
                                    index = int.Parse(t3.Name.Substring(4));
                                    propertyGrid1.SelectedObject = curMazeViewer.curMazePaths.cPaths[index];
                                    curMazeViewer.curMazePaths.cPaths[index].selected = true;
                                }
                                foreach (TreeNode t4 in t3.Nodes)
                                {
                                    if (t4.Name.Contains("MZP$"))
                                    {
                                        index = int.Parse(t4.Name.Substring(4));
                                        propertyGrid1.SelectedObject = curMazeViewer.curMazePaths.cPaths[index];
                                        curMazeViewer.curMazePaths.cPaths[index].selected = true;
                                    }
                                }
                            }
                        }
                    }
                }
                curMazeViewer.curMazePaths.RemoveSelected();
                
                //UpdateTabs();
                projectChanged = true;
                curMazeViewer.Refresh();
                UpdatePathList();
                UpdateProjectList();

            }
        }

        private void treeViewPaths_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeViewPaths.SelectedNode = e.Node;
        }

        private void treeViewProject_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeViewProject.SelectedNode = e.Node;
            if (curMazeViewer.bNewRegionFlag)
                UpdateProjectList();
        }

        private void treeViewProject_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int index = -1;

            string[] parts= e.Node.Name.Split('$');

            if(parts.Length>1)
            {
                index = int.Parse(parts[1]);
                if (index>=0)
                {
                    if(curMazeViewer.curMaze.Index!=index)
                        SetTabByIndex(index);
                    curMazeViewer.curMazePaths.SelectAll();
                    e.Node.Expand();
                    curMazeViewer.selectedPath = -1;
                    //curMazeViewer.Refresh();
                    //propertyGrid1.SelectedObjects = curMazeViewer.curMazePaths.GetSelected();
                    index = -1;
                }

                if(parts.Length>2)
                {
                    
                    if (string.Compare(parts[2], "MZP") == 0)
                    {
                        curMazeViewer.curMazePaths.UnselectAll();
                        
                        string[] nodeParts;
                        if (e.Node.Nodes.Count>0)
                        {
                            foreach (TreeNode t0 in e.Node.Nodes)
                            {
                                nodeParts = t0.Name.Split('$');
                                if (t0.Name.Contains("MZP$")&&nodeParts.Length>3)
                                {
                                    index = int.Parse(nodeParts[3]);
                                    propertyGrid1.SelectedObject = curMazeViewer.curMazePaths.cPaths[index];
                                    curMazeViewer.curMazePaths.cPaths[index].selected = true;
                                }
                                //foreach (TreeNode t1 in t0.Nodes)
                                //{
                                //    if (t1.Name.Contains("MZP$"))
                                //    {
                                //        index = int.Parse(t1.Name.Substring(4));
                                //        propertyGrid1.SelectedObject = curMazeViewer.curMazePaths.cPaths[index];
                                //        curMazeViewer.curMazePaths.cPaths[index].selected = true;
                                //    }
                                //    foreach (TreeNode t2 in t1.Nodes)
                                //    {
                                //        if (t2.Name.Contains("MZP$"))
                                //        {
                                //            index = int.Parse(t2.Name.Substring(4));
                                //            propertyGrid1.SelectedObject = curMazeViewer.curMazePaths.cPaths[index];
                                //            curMazeViewer.curMazePaths.cPaths[index].selected = true;
                                //        }
                                //        foreach (TreeNode t3 in t2.Nodes)
                                //        {
                                //            if (t3.Name.Contains("MZP$"))
                                //            {
                                //                index = int.Parse(t3.Name.Substring(4));
                                //                propertyGrid1.SelectedObject = curMazeViewer.curMazePaths.cPaths[index];
                                //                curMazeViewer.curMazePaths.cPaths[index].selected = true;
                                //            }
                                //            foreach (TreeNode t4 in t3.Nodes)
                                //            {
                                //                if (t4.Name.Contains("MZPS"))
                                //                {
                                //                    index = int.Parse(t4.Name.Substring(4));
                                //                    propertyGrid1.SelectedObject = curMazeViewer.curMazePaths.cPaths[index];
                                //                    curMazeViewer.curMazePaths.cPaths[index].selected = true;
                                //                }
                                //            }
                                //        }
                                //    }
                                //}
                            }
                        }
                        else if(parts.Length>3) //single path
                        {
                            index = int.Parse(parts[3]);
                            propertyGrid1.SelectedObject = curMazeViewer.curMazePaths.cPaths[index];
                            curMazeViewer.curMazePaths.cPaths[index].selected = true;
                        }
                    }
                }
                propertyGrid1.SelectedObjects = curMazeViewer.curMazePaths.GetSelected();
                curMazeViewer.Refresh();
            }
        }

        private void contextMenuStrip_Opening(object e,CancelEventArgs args)
        {
            ToolStripMenuItem remove = new ToolStripMenuItem("Remove");
            ToolStripMenuItem projectClose = new ToolStripMenuItem("Close Project");
            ToolStripMenuItem projectSaveAs = new ToolStripMenuItem("Save As");
            ToolStripMenuItem projectInfo = new ToolStripMenuItem("Edit Project Information");
            ToolStripMenuItem manageRegions = new ToolStripMenuItem("Manage Locations");
            //ToolStripMenuItem removeRegion = new ToolStripMenuItem("Remove Region");

            TreeNode selNode = treeViewProject.SelectedNode;
            contextMenuStripProject.Items.Clear();

            if (selNode == null)
                return;

            string[] parts = selNode.Name.Split('$');

            if(parts.Length<2)//Project
            {
                contextMenuStripProject.Items.Add(projectInfo);
                projectInfo.Click += new System.EventHandler(editProjectInformationToolStripMenuItem_Click);
                contextMenuStripProject.Items.Add(projectSaveAs);
                projectSaveAs.Click += new System.EventHandler(saveAsProjectToolStripMenuItem_Click);
                contextMenuStripProject.Items.Add(projectClose);
                projectClose.Click += new System.EventHandler(closeAnalyzerProjectToolStripMenuItem_Click);
            }
            else if(parts.Length==2)// Maze
            {
                remove.Text += " Maze";
                remove.Click += new System.EventHandler(projMazeRemoveMenuItem_Click);
                remove.Name = selNode.Name;
                contextMenuStripProject.Items.Add(remove);
            }
            else if(parts.Length==3)// Path File or Region Collection
            {

            }
            else if(parts.Length==4)// Path or Region
            {
                if(parts[2].Contains("MZP"))
                {
                    remove.Text += " Path";
                    remove.Click += new System.EventHandler(removePathToolStripMenuItem_Click);
                    remove.Name = "ProjectContext#" + selNode.Name;
                    contextMenuStripProject.Items.Add(remove);
                }
                else if(parts[2].Contains("REG"))
                {
                    manageRegions.Click += new System.EventHandler(toolStripButtonManage_Click);
                    contextMenuStripProject.Items.Add(manageRegions);
                }
                
            }

        }

        private void editProjectInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditProjectInfo();
        }

        private void resetPathColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            curMazeViewer.curMazePaths.ResetColors();
            RefreshMazeView();
            UpdatePathList();
            projectChanged = true;
        }

        public void themeButton_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem t = (ToolStripMenuItem)sender;

            int i = 0;
            foreach (ToolStripMenuItem t2 in themeToolStripMenuItem.DropDownItems)
            {
                if (String.Compare(t2.Text, t.Text) == 0)
                    break;
                i++;
            }

            setTheme(i);
        }

        public void setTheme(int index)
        {
            markAllThemesFalse();

            curMazeViewer.curMazeTheme = mazeThemeLibrary.GetThemeByIndex(index);
            curMazeViewer.BackColor = curMazeViewer.curMazeTheme.bgColor;
            RefreshMazeView(true);
            CurrentSettings.themeIndex = curMazeViewer.curMazeTheme.themeIndex;
            CurrentSettings.SaveSettings();

            ToolStripMenuItem t = (ToolStripMenuItem)themeToolStripMenuItem.DropDownItems[index];
            t.Checked = true;
            t = (ToolStripMenuItem)toolStripDropdown_Theme.DropDownItems[index];
            t.Checked = true;
        }

        private void markAllThemesFalse()
        {
            foreach (ToolStripMenuItem t in themeToolStripMenuItem.DropDownItems)
            {
                t.Checked = false;
            }

            foreach (ToolStripMenuItem t in toolStripDropdown_Theme.DropDownItems)
            {
                t.Checked = false;
            }
        }
        private void Main_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (curMazeViewer.curMaze == null)
                return;
            double curScale = curMazeViewer.curMaze.Scale;
                if (curScale > 17 * Math.Pow(1.1, 15))
                    curScale = 17 * Math.Pow(1.1, 15);
                else if ((curScale < 17 * Math.Pow(1.1, -10)))
                    curScale = 17 * Math.Pow(1.1, -10);

                    curMazeViewer.SetScale(curScale * 1.1f);
                RefreshMazeView(true);
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (curMazeViewer.curMaze == null)
                return;
            double curScale = curMazeViewer.curMaze.Scale;
            if (curScale > 17 * Math.Pow(1.1, 15))
                curScale = 17 * Math.Pow(1.1, 15);
            else if ((curScale < 17 * Math.Pow(1.1, -10)))
                curScale = 17 * Math.Pow(1.1, -10);

            curMazeViewer.SetScale(curScale * 0.9f);
            RefreshMazeView(true);
        }

        private void resetZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (curMazeViewer.curMaze == null)
                return;
            curMazeViewer.SetScale(17);
            RefreshMazeView(true);
        }

        private void toolStripDropDown_ZoomIn_Button_Click(object sender, EventArgs e)
        {
            this.zoomInToolStripMenuItem_Click(sender, e);
        }

        private void toolStripDropDownButton2_Click(object sender, EventArgs e)
        {
            this.zoomOutToolStripMenuItem_Click(sender, e);
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripDropDown_CenterView_ButtonClick(object sender, EventArgs e)
        {
            CenterOnStart();
        }

        private void CenterOnStart()
        {
            TabPageNoScroll currentTabPage = (TabPageNoScroll)tabControl1.SelectedTab;
            if (currentTabPage != null)
            { 
                if (curMazeViewer.curMaze == null)
                    return;
                else
                    currentTabPage.AutoScrollPosition = new Point((int)curMazeViewer.curMaze.midStartX - currentTabPage.Width / 2, (int)curMazeViewer.curMaze.midStartY - currentTabPage.Height / 2);
            }
        }

        private void centerViewOnStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CenterOnStart();
        }



        private void toolStripButtonHeatmap_Click(object sender, EventArgs e)
        {
            if (!hmViewer.Visible)
            { 
                hmViewer = new Heatmap(curMazeViewer);
                hmViewer.ShowDialog();
            }
        }
    }

    public class TabPageNoScroll : TabPage
    {


        protected override Point ScrollToControl(Control activeControl)
        {
            return this.AutoScrollPosition;
        }
    }
}

