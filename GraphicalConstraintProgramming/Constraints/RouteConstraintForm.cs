using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicalConstraintProgramming.Constraints
{
    public partial class RouteConstraintForm : Form
    {

        public bool accept = false;
        public bool symmetric = true;
        public int[][] distances;
        public List<int> domain;
        public String lengthVar = "";

        public RouteConstraintForm(List<Variable> selectedVars)
        {

            InitializeComponent();

            this.Text = "Route Constraint";

            typeof(DataGridView).InvokeMember(
            "DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView1, new object[] { true });


            domain = new List<int>();

            foreach(Variable v in selectedVars){
                
                for(int i = v.domain_start; i<= v.domain_end; i++)
                {
                    if (!domain.Contains(i)) domain.Add(i);
                }
            }

            domain.Sort();




            foreach (int x in domain)
            {
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn());
                dataGridView1.Columns[dataGridView1.ColumnCount - 1].Name = x.ToString();
                dataGridView1.Columns[dataGridView1.ColumnCount - 1].Width = 50;
                dataGridView1.Columns[dataGridView1.ColumnCount - 1].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (int y in domain)
            {
                DataGridViewRow r = new DataGridViewRow();
                r.HeaderCell.Value = y.ToString();
                r.Height = 50;
                dataGridView1.Rows.Add(r);

            }

            foreach(DataGridViewRow r in dataGridView1.Rows)
                foreach (DataGridViewCell c in r.Cells)
            {
                    c.Style.BackColor = Color.White;
                    if (c.ColumnIndex == c.RowIndex) c.Value = 0;
            }

            checkBox1_CheckedChanged(null, null);


        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            symmetric = checkBox1.Checked;

            if (checkBox1.Checked)
            {
                foreach (DataGridViewRow r in dataGridView1.Rows)
                    foreach (DataGridViewCell c in r.Cells)
                    {
                        if (c.ColumnIndex < c.RowIndex)
                        {
                            c.ReadOnly = true;
                            c.Style.BackColor = Color.DarkGray;

                            dataGridView1.Rows[c.RowIndex].Cells[c.ColumnIndex].Value = dataGridView1.Rows[c.ColumnIndex].Cells[c.RowIndex].Value;
                        }
                    }
            }
            else
            {
                foreach (DataGridViewRow r in dataGridView1.Rows)
                    foreach (DataGridViewCell c in r.Cells)
                    {
                        if (c.ColumnIndex < c.RowIndex) c.ReadOnly = false;
                        c.Style.BackColor = Color.White;
                    }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (checkBox1.Checked)
            {
                dataGridView1.Rows[e.ColumnIndex].Cells[e.RowIndex].Value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewRow r in dataGridView1.Rows)
                foreach (DataGridViewCell c in r.Cells)
                {
                    if (c.Value == null)
                    {
                        Form1.alert("Please fill out all cells!");
                        return;
                    }
                }


            if (textBox1.Text.Trim() == "")
            {
                Form1.alert("Please set length variable (output)!");
                return;
            }

            accept = true;

            distances = new int[domain.Count][];

            for (int x = 0; x < dataGridView1.ColumnCount; x++)
            {
                distances[x] = new int[domain.Count];

                for (int y = 0; y < dataGridView1.RowCount; y++)
                {
                    distances[x][y] = int.Parse( dataGridView1.Rows[y].Cells[x].Value.ToString());
                }


            }

            this.lengthVar = textBox1.Text;

            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "Text Files (*.txt)|*.txt|CSV Files (*.csv*)|*.csv*";
            dlg.Multiselect = false;

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            foreach (var path in dlg.FileNames)
            {
                checkBox1.Checked = false;

                using (var reader = new StreamReader(path))
                {

                    int l = 0;
                    while (!reader.EndOfStream && l < dataGridView1.Rows.Count)
                    {

                            string line = reader.ReadLine();
                            line = System.Text.RegularExpressions.Regex.Replace(line, @"\s+", " ");
                        line = line.Trim();

                        string[] values = line.Split(null);

                            for(int entry = 0; entry < values.Length && entry < dataGridView1.Rows.Count; entry++)
                            {
                                dataGridView1.Rows[l].Cells[entry] .Value= values[entry];
                            }

                            
                        l++;
                    }
                }
            }
        }
    }
}
