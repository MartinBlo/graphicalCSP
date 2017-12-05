using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicalConstraintProgramming
{
    public partial class NewVariableForm : Form
    {

        static int val_min = 0;
        static int val_max = 9;

        public NewVariableForm()
        {
            InitializeComponent();
        }

        private void NewVariableForm_Load(object sender, EventArgs e)
        {

            foreach (vartype val in Enum.GetValues(typeof(vartype)))
            {
                comboBox1.Items.Add(val);
            }

            comboBox1.SelectedIndex = 0;

            numericUpDown1.Minimum = -99999;
            numericUpDown1.Maximum = 99999;

            numericUpDown2.Minimum = -99999;
            numericUpDown2.Maximum = 99999;

            numericUpDown1.Value = val_min;
            numericUpDown2.Value = val_max;

            textBox1.Select();


        }

        private void button1_Click(object sender, EventArgs e)
        {

            textBox1.Text = textBox1.Text.Trim();

            if(textBox1.Text == "")
            {
                MessageBox.Show("Please enter a valid variable name!", "Error!",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            }

            else if (Form1.instance.variableExists(textBox1.Text))
            {
                MessageBox.Show("Variable name already exists!", "Error!",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            else
            {


                if(comboBox1.SelectedIndex == 0) // Int type
                {

                    List<Variable> vars = new List<Variable>();

                    foreach (DataGridViewCell c in Form1.selected_cells)
                    {

                        Variable v = new Variable(textBox1.Text + "_" + (c.ColumnIndex + 1 - Form1.selected_cells.First().ColumnIndex) + "_" + (c.RowIndex + 1 - Form1.selected_cells.First().RowIndex), (vartype)comboBox1.SelectedIndex, (int)numericUpDown1.Value, (int)numericUpDown2.Value, c,
                            textBox1.Text, (c.ColumnIndex + 1 - Form1.selected_cells.First().ColumnIndex), (c.RowIndex + 1 - Form1.selected_cells.First().RowIndex));

                        //Form1.instance.addVariable(v,c);
                        vars.Add(v);
                        
                    }


                    if(vars.Count > 0)
                    {
                        Form1.addVariables(vars);
                    }

                    this.Close();

                }
                else 
                {
                    throw new NotImplementedException();
                    

                }


            }



        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //textBox1.Text = textBox1.Text.ToLower();
            if (textBox1.Text.Length > 4) textBox1.Text =  textBox1.Text.Substring(0, 4);
            if(textBox1.Text.Length > 0) textBox1.SelectionStart = textBox1.Text.Length; // add some logic if length is 0
            textBox1.SelectionLength = 0;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            val_min = (int) ((NumericUpDown)sender).Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            val_max = (int)((NumericUpDown)sender).Value;
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
        }

        private void numericUpDown1_KeyDown(object sender, KeyEventArgs e)
        {
            textBox1_KeyUp(sender, e);
        }

        private void numericUpDown2_KeyDown(object sender, KeyEventArgs e)
        {
            textBox1_KeyUp(sender, e);
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            textBox1_KeyUp(sender, e);
        }
    }
}
