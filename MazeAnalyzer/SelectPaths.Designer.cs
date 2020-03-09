namespace MazeAnalyzer
{
    partial class SelectPaths
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
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("Deneme...");
            this.listView1 = new ListViewEx.ListViewEx();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.logFileName_TextBox = new System.Windows.Forms.TextBox();
            this.button_reset = new System.Windows.Forms.Button();
            this.button_unselectAll = new System.Windows.Forms.Button();
            this.button_selectAll = new System.Windows.Forms.Button();
            this.button_copy_condition = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numeric_Session = new System.Windows.Forms.NumericUpDown();
            this.textBox_Group = new System.Windows.Forms.TextBox();
            this.textBox_Subject = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_Session)).BeginInit();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.AllowColumnReorder = true;
            this.listView1.CheckBoxes = true;
            this.listView1.DoubleClickActivation = false;
            this.listView1.FullRowSelect = true;
            listViewItem4.StateImageIndex = 0;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem4});
            this.listView1.Location = new System.Drawing.Point(8, 69);
            this.listView1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(667, 280);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(617, 355);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(56, 25);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Enabled = false;
            this.buttonLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLoad.Location = new System.Drawing.Point(557, 355);
            this.buttonLoad.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(56, 25);
            this.buttonLoad.TabIndex = 2;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(107, 131);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 26);
            this.label1.TabIndex = 4;
            this.label1.Text = "Loading...";
            // 
            // logFileName_TextBox
            // 
            this.logFileName_TextBox.Location = new System.Drawing.Point(11, 358);
            this.logFileName_TextBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.logFileName_TextBox.Name = "logFileName_TextBox";
            this.logFileName_TextBox.ReadOnly = true;
            this.logFileName_TextBox.Size = new System.Drawing.Size(230, 20);
            this.logFileName_TextBox.TabIndex = 5;
            // 
            // button_reset
            // 
            this.button_reset.Location = new System.Drawing.Point(245, 355);
            this.button_reset.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(56, 25);
            this.button_reset.TabIndex = 6;
            this.button_reset.Text = "Reset";
            this.button_reset.UseVisualStyleBackColor = true;
            this.button_reset.Click += new System.EventHandler(this.button_reset_Click);
            // 
            // button_unselectAll
            // 
            this.button_unselectAll.Location = new System.Drawing.Point(308, 355);
            this.button_unselectAll.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_unselectAll.Name = "button_unselectAll";
            this.button_unselectAll.Size = new System.Drawing.Size(56, 25);
            this.button_unselectAll.TabIndex = 7;
            this.button_unselectAll.Text = "Clear";
            this.button_unselectAll.UseVisualStyleBackColor = true;
            this.button_unselectAll.Click += new System.EventHandler(this.button_unselectAll_Click);
            // 
            // button_selectAll
            // 
            this.button_selectAll.Location = new System.Drawing.Point(405, 38);
            this.button_selectAll.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_selectAll.Name = "button_selectAll";
            this.button_selectAll.Size = new System.Drawing.Size(79, 25);
            this.button_selectAll.TabIndex = 8;
            this.button_selectAll.Text = "All to Current";
            this.button_selectAll.UseVisualStyleBackColor = true;
            this.button_selectAll.Click += new System.EventHandler(this.button_selectAll_Click);
            // 
            // button_copy_condition
            // 
            this.button_copy_condition.Location = new System.Drawing.Point(523, 38);
            this.button_copy_condition.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_copy_condition.Name = "button_copy_condition";
            this.button_copy_condition.Size = new System.Drawing.Size(98, 25);
            this.button_copy_condition.TabIndex = 15;
            this.button_copy_condition.Text = "Maze->Condition";
            this.button_copy_condition.UseVisualStyleBackColor = true;
            this.button_copy_condition.Click += new System.EventHandler(this.button_copy_condition_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numeric_Session);
            this.groupBox1.Controls.Add(this.textBox_Group);
            this.groupBox1.Controls.Add(this.textBox_Subject);
            this.groupBox1.Location = new System.Drawing.Point(8, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(356, 45);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Experimental Info";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(246, 21);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Session:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(123, 21);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Subject:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 21);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Group:";
            // 
            // numeric_Session
            // 
            this.numeric_Session.Location = new System.Drawing.Point(297, 18);
            this.numeric_Session.Margin = new System.Windows.Forms.Padding(2);
            this.numeric_Session.Name = "numeric_Session";
            this.numeric_Session.Size = new System.Drawing.Size(48, 20);
            this.numeric_Session.TabIndex = 17;
            this.numeric_Session.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // textBox_Group
            // 
            this.textBox_Group.Location = new System.Drawing.Point(49, 18);
            this.textBox_Group.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_Group.Name = "textBox_Group";
            this.textBox_Group.Size = new System.Drawing.Size(65, 20);
            this.textBox_Group.TabIndex = 16;
            // 
            // textBox_Subject
            // 
            this.textBox_Subject.Location = new System.Drawing.Point(173, 18);
            this.textBox_Subject.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_Subject.Name = "textBox_Subject";
            this.textBox_Subject.Size = new System.Drawing.Size(68, 20);
            this.textBox_Subject.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label5.Location = new System.Drawing.Point(520, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 26);
            this.label5.TabIndex = 17;
            this.label5.Text = "Copy Maze Name as \r\nExperimental Condition";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label6.Location = new System.Drawing.Point(392, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(121, 26);
            this.label6.TabIndex = 18;
            this.label6.Text = "Add All Paths To\r\nCurrently Selected Maze\r\n";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(402, 354);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 26);
            this.label7.TabIndex = 19;
            this.label7.Text = "⇧ Doubleclick to\r\nChoose Maze\r\n";
            // 
            // SelectPaths
            // 
            this.AcceptButton = this.buttonLoad;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(681, 384);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_copy_condition);
            this.Controls.Add(this.button_selectAll);
            this.Controls.Add(this.button_unselectAll);
            this.Controls.Add(this.button_reset);
            this.Controls.Add(this.logFileName_TextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.listView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectPaths";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Paths To Import";
            this.Load += new System.EventHandler(this.SelectPaths_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_Session)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListViewEx.ListViewEx listView1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox logFileName_TextBox;
        private System.Windows.Forms.Button button_reset;
        private System.Windows.Forms.Button button_unselectAll;
        private System.Windows.Forms.Button button_selectAll;
        private System.Windows.Forms.Button button_copy_condition;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numeric_Session;
        private System.Windows.Forms.TextBox textBox_Group;
        private System.Windows.Forms.TextBox textBox_Subject;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}