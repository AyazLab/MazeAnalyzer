namespace MazeAnalyzer
{
    partial class AnalyzerProjectWizard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnalyzerProjectWizard));
            this.button_Ok = new System.Windows.Forms.Button();
            this.textBox_ProjName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_Description = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_finish = new System.Windows.Forms.Button();
            this.listBox_mazefiles = new System.Windows.Forms.ListBox();
            this.button_remove = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_Ok
            // 
            this.button_Ok.Location = new System.Drawing.Point(333, 190);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(95, 23);
            this.button_Ok.TabIndex = 2;
            this.button_Ok.Text = "Add Maze Files";
            this.button_Ok.UseVisualStyleBackColor = true;
            this.button_Ok.Click += new System.EventHandler(this.button_Ok_Click);
            // 
            // textBox_ProjName
            // 
            this.textBox_ProjName.Location = new System.Drawing.Point(249, 35);
            this.textBox_ProjName.Name = "textBox_ProjName";
            this.textBox_ProjName.Size = new System.Drawing.Size(179, 20);
            this.textBox_ProjName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(171, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Project Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(171, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Description:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(171, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(257, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Please Enter a Name and Description for your project";
            // 
            // textBox_Description
            // 
            this.textBox_Description.Location = new System.Drawing.Point(249, 65);
            this.textBox_Description.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_Description.Multiline = true;
            this.textBox_Description.Name = "textBox_Description";
            this.textBox_Description.Size = new System.Drawing.Size(179, 94);
            this.textBox_Description.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::MazeAnalyzer.Properties.Resources.AnalyzerLogo;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Location = new System.Drawing.Point(8, 8);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(152, 151);
            this.panel1.TabIndex = 15;
            // 
            // button_finish
            // 
            this.button_finish.Location = new System.Drawing.Point(330, 366);
            this.button_finish.Name = "button_finish";
            this.button_finish.Size = new System.Drawing.Size(98, 23);
            this.button_finish.TabIndex = 16;
            this.button_finish.Text = "Finish";
            this.button_finish.UseVisualStyleBackColor = true;
            this.button_finish.Click += new System.EventHandler(this.button_finish_Click);
            // 
            // listBox_mazefiles
            // 
            this.listBox_mazefiles.AllowDrop = true;
            this.listBox_mazefiles.FormattingEnabled = true;
            this.listBox_mazefiles.HorizontalScrollbar = true;
            this.listBox_mazefiles.Location = new System.Drawing.Point(13, 190);
            this.listBox_mazefiles.Name = "listBox_mazefiles";
            this.listBox_mazefiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox_mazefiles.Size = new System.Drawing.Size(314, 199);
            this.listBox_mazefiles.TabIndex = 17;
            this.listBox_mazefiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBox_mazefiles_DragDrop);
            this.listBox_mazefiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBox_mazefiles_DragEnter);
            // 
            // button_remove
            // 
            this.button_remove.Location = new System.Drawing.Point(333, 219);
            this.button_remove.Name = "button_remove";
            this.button_remove.Size = new System.Drawing.Size(95, 23);
            this.button_remove.TabIndex = 8;
            this.button_remove.Text = "Remove";
            this.button_remove.UseVisualStyleBackColor = true;
            this.button_remove.Click += new System.EventHandler(this.button_remove_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 174);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Project Mazes:";
            // 
            // AnalyzerProjectWizard
            // 
            this.AcceptButton = this.button_Ok;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(440, 396);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_remove);
            this.Controls.Add(this.listBox_mazefiles);
            this.Controls.Add(this.button_finish);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBox_Description);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_ProjName);
            this.Controls.Add(this.button_Ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AnalyzerProjectWizard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Enter Project Information and Add Mazes";
            this.Load += new System.EventHandler(this.AnalyzerProjectWizard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.TextBox textBox_ProjName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_Description;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_finish;
        private System.Windows.Forms.ListBox listBox_mazefiles;
        private System.Windows.Forms.Button button_remove;
        private System.Windows.Forms.Label label3;
    }
}