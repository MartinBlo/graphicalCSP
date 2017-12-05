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
    public partial class ForbiddenPatternForm : Form
    {

        public static Boolean accept = false;
        public static String content = "";

        public ForbiddenPatternForm()
        {
            InitializeComponent();

            accept = false;
            content = "";
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            content = forbiddenPatternsText.Text;
            accept = true;
            this.Close();
        }
    }
}
