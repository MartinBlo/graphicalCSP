using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicalConstraintProgramming
{
    public partial class CustomSumConstraintForm : Form
    {

        public static String op = "";
        public static String var = "";
        public static Boolean accept = false;

        public CustomSumConstraintForm()
        {
            InitializeComponent();
            accept = false;
            comboBox1.SelectedIndex = 0;
            textBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "")
            {
                op = comboBox1.SelectedItem.ToString();
                var = textBox1.Text;
                accept = true;
                this.Close();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            textBox1_KeyDown(sender, e);
        }
    }
}
