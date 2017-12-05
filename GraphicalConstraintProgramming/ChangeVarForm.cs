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
    public partial class ChangeVarForm : Form
    {
        private Variable var;

        public ChangeVarForm(Variable v)
        {

            

            InitializeComponent();

            varname.Text = v.name;
            vartype.Text = v.type.ToString();
            fixedVal.Text = v.fixedVal;
            var = v;

        }

        private void fixedVal_TextChanged(object sender, EventArgs e)
        {
            if (var == null) return;
            var.fixedVal = fixedVal.Text.Trim();
        }

        private void fixedVal_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                this.Close();
            }
        }
    }
}
