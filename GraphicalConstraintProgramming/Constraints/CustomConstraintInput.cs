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
    public partial class CustomConstraintInput : Form
    {

        public static String content = "";
        public static Boolean accept = false;

        public CustomConstraintInput()
        {
            InitializeComponent();
            accept = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            accept = true;
            this.Close();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            content = richTextBox1.Text.Trim();
        }
    }
}
