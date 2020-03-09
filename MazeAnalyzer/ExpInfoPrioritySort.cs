using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MazeAnalyzer
{
    public partial class ExpInfoPrioritySort : Form
    {
        private List<Main.ExpInfoTypes> expInfoPrioity;
        public ExpInfoPrioritySort(List<Main.ExpInfoTypes> expInfoPrioity)
        {
            InitializeComponent();
            this.expInfoPrioity = expInfoPrioity;
            listBox_priorities.Items.Clear();
            foreach (Main.ExpInfoTypes ex in expInfoPrioity)
            {
                listBox_priorities.Items.Add(ex);
            }
        }

        private void button_default_Click(object sender, EventArgs e)
        {
            expInfoPrioity.Clear();
            expInfoPrioity.Add(Main.ExpInfoTypes.Group);
            expInfoPrioity.Add(Main.ExpInfoTypes.Condition);
            expInfoPrioity.Add(Main.ExpInfoTypes.Subject);
            expInfoPrioity.Add(Main.ExpInfoTypes.Session);
            expInfoPrioity.Add(Main.ExpInfoTypes.Trial);
            
            listBox_priorities.Items.Clear();
            foreach (Main.ExpInfoTypes ex in expInfoPrioity)
            {
                listBox_priorities.Items.Add(ex);
            }
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            expInfoPrioity.Clear();
            foreach (Main.ExpInfoTypes ex in listBox_priorities.Items)
            {
                expInfoPrioity.Add(ex);
            }
            this.Close();
        }

        private void button_move_up_Click(object sender, EventArgs e)
        {
            MoveItem(-1);
        }


        private void button_move_down_Click(object sender, EventArgs e)
        {
            MoveItem(1);
        }

        public void MoveItem(int direction)
        {
            // Checking selected item
            if (listBox_priorities.SelectedItem == null || listBox_priorities.SelectedIndex < 0)
                return; // No selected item - nothing to do

            // Calculate new index using move direction
            int newIndex = listBox_priorities.SelectedIndex + direction;

            // Checking bounds of the range
            if (newIndex < 0 || newIndex >= listBox_priorities.Items.Count)
                return; // Index out of range - nothing to do

            object selected = listBox_priorities.SelectedItem;

            // Removing removable element
            listBox_priorities.Items.Remove(selected);
            // Insert it in new position
            listBox_priorities.Items.Insert(newIndex, selected);
            // Restore selection
            listBox_priorities.SetSelected(newIndex, true);
        }

    }
}
