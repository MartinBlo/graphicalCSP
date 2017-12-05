using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicalConstraintProgramming.Constraints
{
    public partial class CountXYConstraintForm : Form
    {
        public static String selectedVar = "";
        public static String countedVar = "";
        public static String operatorVar = "";

        public static Boolean accept = false;
        public static String a;

        public CountXYConstraintForm()
        {
            InitializeComponent();

            comboBox1.SelectedIndex = 0;

            accept = false;

            selectedVar = "";
            countedVar = "";
            operatorVar = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {

            selectedVar = textBox1.Text.Trim();
            countedVar = textBox2.Text.Trim();
            operatorVar = comboBox1.SelectedItem.ToString().Trim();

            if(selectedVar != "" && countedVar != "")
            {

                accept = true;
                this.Close();
            }

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
        }
    }
}
