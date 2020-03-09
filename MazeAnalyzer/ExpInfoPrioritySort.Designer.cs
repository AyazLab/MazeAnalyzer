namespace MazeAnalyzer
{
    partial class ExpInfoPrioritySort
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
            this.button_ok = new System.Windows.Forms.Button();
            this.button_default = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox_priorities = new System.Windows.Forms.ListBox();
            this.button_move_up = new System.Windows.Forms.Button();
            this.button_move_down = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(97, 155);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 0;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_default
            // 
            this.button_default.Location = new System.Drawing.Point(16, 155);
            this.button_default.Name = "button_default";
            this.button_default.Size = new System.Drawing.Size(75, 23);
            this.button_default.TabIndex = 1;
            this.button_default.Text = "Default";
            this.button_default.UseVisualStyleBackColor = true;
            this.button_default.Click += new System.EventHandler(this.button_default_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select the Path Grouping Priority";
            // 
            // listBox_priorities
            // 
            this.listBox_priorities.FormattingEnabled = true;
            this.listBox_priorities.Location = new System.Drawing.Point(15, 41);
            this.listBox_priorities.Name = "listBox_priorities";
            this.listBox_priorities.Size = new System.Drawing.Size(76, 95);
            this.listBox_priorities.TabIndex = 3;
            // 
            // button_move_up
            // 
            this.button_move_up.Location = new System.Drawing.Point(97, 41);
            this.button_move_up.Name = "button_move_up";
            this.button_move_up.Size = new System.Drawing.Size(75, 23);
            this.button_move_up.TabIndex = 4;
            this.button_move_up.Text = "Move Up";
            this.button_move_up.UseVisualStyleBackColor = true;
            this.button_move_up.Click += new System.EventHandler(this.button_move_up_Click);
            // 
            // button_move_down
            // 
            this.button_move_down.Location = new System.Drawing.Point(97, 70);
            this.button_move_down.Name = "button_move_down";
            this.button_move_down.Size = new System.Drawing.Size(75, 23);
            this.button_move_down.TabIndex = 5;
            this.button_move_down.Text = "Move Down";
            this.button_move_down.UseVisualStyleBackColor = true;
            this.button_move_down.Click += new System.EventHandler(this.button_move_down_Click);
            // 
            // ExpInfoPrioritySort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(179, 190);
            this.Controls.Add(this.button_move_down);
            this.Controls.Add(this.button_move_up);
            this.Controls.Add(this.listBox_priorities);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_default);
            this.Controls.Add(this.button_ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ExpInfoPrioritySort";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Path Grouping Priority";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_default;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox_priorities;
        private System.Windows.Forms.Button button_move_up;
        private System.Windows.Forms.Button button_move_down;
    }
}