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
    public partial class ForwardConstraintForm : Form
    {

        public static Boolean accept = false;
        public static String content = "";

        public ForwardConstraintForm()
        {
            InitializeComponent();

            accept = false;
            content = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            content = richTextBox1.Text;
            accept = true;
            this.Close();
        }
    }
}
