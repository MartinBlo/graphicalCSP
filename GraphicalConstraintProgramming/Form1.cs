using GraphicalConstraintProgramming.Actions;
using GraphicalConstraintProgramming.Constraints;
using GraphicalConstraintProgramming.HelperTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicalConstraintProgramming
{
    public partial class Form1 : Form
    {

        private const int sizeX = 200;
        private const int sizeY = 200;

        private int mouseX = 0;
        private int mouseY = 0;
        private bool mouseDown = false;
        private bool selectDown = false;
        private bool rectDrag = false;

        private Point rectDragStartCell;

        public static CPInstance instance;
        public static MiniZincInterface mzinc;
        public static IOrderedEnumerable<DataGridViewCell> selected_cells;
        private static int size = 60;

        private Constraint lastConstraint = null;

        private static Color gridColor = Color.Gainsboro;

        public static Boolean drawConstraints = true;
        private SolverName solver;

        public static UndoStack stack;

        public static int getSize()
        {
            return size;
        }

        public Form1()
        {
            InitializeComponent();

            initializeForm(new CPInstance(sizeX, sizeY));



        }


        public void initializeForm(CPInstance instance)
        {
            if (mzinc == null) mzinc = new MiniZincInterface();

            this.initializeGrid();

            Form1.instance = instance;

            comboBox1.SelectedIndex = comboBox1.Items.Count - 1;

            stack = new UndoStack();
            refreshUndoButtons();

            lastConstraint = null;

        }

        private void initializeGrid()
        {
            
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            dataGridView1.GridColor = gridColor;

            typeof(DataGridView).InvokeMember(
            "DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView1, new object[] { true });


            for (int i = 0; i < sizeX; i++)
            {
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn());


                dataGridView1.Columns[i].Name = GetExcelColumnName(i + 1);
                dataGridView1.Columns[i].Width = size;
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            for (int i = 0; i < sizeY; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                dataGridView1.Rows[i].Height = size;

            }

            
        }

        

        private void new_var_Click_1(object sender, EventArgs e)
        {
            NewVariableForm f = new NewVariableForm();
            f.ShowDialog();

            invalidateInstance();

            dataGridView1.ClearSelection();

            refreshGrid();


        }

        private void solve_Click_1(object sender, EventArgs e)
        {
            instance.solve(mzinc, solver);

            if (!instance.unsatisfiable)
            {
                refreshGrid();
            }
            else
            {
                Form1.alert("Unsatisfiable!");
            }
        }

        public static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        internal static void alert(string v)
        {
            MessageBox.Show(v, "Alert",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

     
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = instance.getCode();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            // Ignore if a column or row header is clicked
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                if (e.Button == MouseButtons.Right)
                {
                    DataGridViewCell clickedCell = (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex];

                    // Here you can do whatever you want with the cell

                    if (!clickedCell.Selected) this.dataGridView1.CurrentCell = clickedCell;  // Select the clicked cell, for instance
                    clickedCell.Selected = true;

                    ContextMenu m = new ContextMenu();

                    if (dataGridView1.SelectedCells.Count == 1 && instance.getVar(clickedCell) != null)
                    {
                        MenuItem min_max = new MenuItem("Minimize / Maximize this Variable");
                        MenuItem minimize = new MenuItem("Minimize this Variable");
                        minimize.Click += new EventHandler(minimize_Click);
                        MenuItem maximize = new MenuItem("Maximize this Variable");
                        maximize.Click += new EventHandler(maximize_Click);
                        MenuItem closeToInt = new MenuItem("Bring Variable close to ...");
                        closeToInt.Click += new EventHandler(optTo_Click);
                        min_max.MenuItems.Add(minimize);
                        min_max.MenuItems.Add(maximize);
                        min_max.MenuItems.Add(closeToInt);
                        m.MenuItems.Add(min_max);
                    }

                    MenuItem item = new MenuItem("New Variable(s)...");
                    item.Click += new EventHandler(new_var_Click_1);
                    m.MenuItems.Add(item);

                    MenuItem c = new MenuItem("All different Constraint");
                    c.Click += new EventHandler(allDiff_Click);
                    m.MenuItems.Add(c);

                    MenuItem d = new MenuItem("All equal Constraint");
                    d.Click += new EventHandler(allEqual_Click);
                    m.MenuItems.Add(d);

                    MenuItem sum = new MenuItem("Sum");

                    MenuItem f = new MenuItem("Sum <= 1 Constraint");
                    f.Click += new EventHandler(sumLEQ1_Click);
                    sum.MenuItems.Add(f);

                    MenuItem x = new MenuItem("Sum = 1 Constraint");
                    x.Click += new EventHandler(equalOne_Click);
                    sum.MenuItems.Add(x);

                    MenuItem l = new MenuItem("Custom Sum constraint...");
                    l.Click += new EventHandler(custom_sum_Click);
                    sum.MenuItems.Add(l);

                    m.MenuItems.Add(sum);

                    MenuItem set = new MenuItem("Set Tools");
                    m.MenuItems.Add(set);


                    MenuItem create_set = new MenuItem("Create Set");
                    create_set.Click += new EventHandler(create_set_Click);
                    set.MenuItems.Add(create_set);

                    MenuItem set_intersection_atmost_one = new MenuItem("Set intersection at most one");
                    set_intersection_atmost_one.Click += new EventHandler(set_in_atm_one);
                    set.MenuItems.Add(set_intersection_atmost_one);

                    MenuItem symmetry = new MenuItem("Break Symmetry");
                    m.MenuItems.Add(symmetry);

                    MenuItem breakSymmetryByAscending = new MenuItem("By Ascending Order");
                    breakSymmetryByAscending.Click += new EventHandler(symmetry_ascending_Click);
                    symmetry.MenuItems.Add(breakSymmetryByAscending);

                    MenuItem cyclicConstraints = new MenuItem("Pattern Constraints");
                    m.MenuItems.Add(cyclicConstraints);

                    MenuItem ncForbiddenPatterns = new MenuItem("Forbidden patterns...");
                    ncForbiddenPatterns.Click += new EventHandler(ncforbiddenPatterns_Click);
                    cyclicConstraints.MenuItems.Add(ncForbiddenPatterns);

                    MenuItem forbiddenPatterns = new MenuItem("Cyclic forbidden patterns...");
                    forbiddenPatterns.Click += new EventHandler(forbiddenPatterns_Click);
                    cyclicConstraints.MenuItems.Add(forbiddenPatterns);


                    MenuItem ncForwardConstraint = new MenuItem("Forward constraint...");
                    ncForwardConstraint.Click += new EventHandler(ncforwardConstraint_Click);
                    cyclicConstraints.MenuItems.Add(ncForwardConstraint);

                    MenuItem forwardConstraint = new MenuItem("Cyclic forward constraint...");
                    forwardConstraint.Click += new EventHandler(forwardConstraint_Click);
                    cyclicConstraints.MenuItems.Add(forwardConstraint);


                    MenuItem countXY = new MenuItem("Number of occurrences...");
                    countXY.Click += new EventHandler(countXY_Click);
                    m.MenuItems.Add(countXY);

                    MenuItem routeConstraint = new MenuItem("Route...");
                    routeConstraint.Click += new EventHandler(routeConstraint_Click);
                    m.MenuItems.Add(routeConstraint);
                    


                    MenuItem custom_constraint = new MenuItem("Custom Constraint...");
                    custom_constraint.Click += new EventHandler(custom_constraint_Click);

                    m.MenuItems.Add(custom_constraint);

                    



                    // Get mouse position relative to the vehicles grid
                    var relativeMousePosition = dataGridView1.PointToClient(Cursor.Position);

                    // Show the context menu

                    m.Show(dataGridView1, relativeMousePosition);
                }
               
            }

        }

        private void routeConstraint_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedCellCollection x = dataGridView1.SelectedCells;

            List<Variable> variables = new List<Variable>();

            foreach(DataGridViewCell c in x)
            {

                Variable v = instance.getVar(c);
                if(v != null) variables.Add(v);

            }

            if (variables.Count > 0)
            {
                RouteConstraintForm form = new RouteConstraintForm(variables);
                form.ShowDialog();
                if (form.accept)
                {
                    addConstraint(dataGridView1.SelectedCells, new RouteConstraint(form.lengthVar, form.distances, form.domain, instance));
                }
            }
        }

        private void maximize_Click(object sender, EventArgs e)
        {
            Variable v = instance.getVar(dataGridView1.SelectedCells[0]);
            stack.doAction(new OptToAction(v, v.domain_end.ToString(), instance));
            dataGridView1.ClearSelection();
            invalidateInstance();

        }
        private void optTo_Click(object sender, EventArgs e)
        {
            OptToForm f = new OptToForm();
            f.ShowDialog();

            if (f.accept && f.value.Length > 0)
            {
                stack.doAction(new OptToAction(instance.getVar(dataGridView1.SelectedCells[0]), f.value, instance));
                dataGridView1.ClearSelection();
            }
        }




        private void minimize_Click(object sender, EventArgs e)
        {
            Variable v = instance.getVar(dataGridView1.SelectedCells[0]);
            stack.doAction(new OptToAction(v, v.domain_start.ToString(), instance));
            dataGridView1.ClearSelection();
            invalidateInstance();

        }

        private void countXY_Click(object sender, EventArgs e)
        {

            CountXYConstraintForm c = new CountXYConstraintForm();
            c.ShowDialog();

            if (CountXYConstraintForm.accept && CountXYConstraintForm.selectedVar.Trim() != "" && CountXYConstraintForm.countedVar.Trim() != "" && CountXYConstraintForm.operatorVar.Trim() != "")
            {
                addConstraint(dataGridView1.SelectedCells, new CountXYConstraint(CountXYConstraintForm.selectedVar, CountXYConstraintForm.countedVar, CountXYConstraintForm.operatorVar, instance));

            }

        }

        private void ncforwardConstraint_Click(object sender, EventArgs e)
        {

            ForwardConstraintForm c = new ForwardConstraintForm();
            c.ShowDialog();

            if (ForwardConstraintForm.accept && ForwardConstraintForm.content.Trim() != "")
            {
                addConstraint(dataGridView1.SelectedCells, new ForwardConstraintNonCyclic(ForwardConstraintForm.content));

            }

        }

        private void forwardConstraint_Click(object sender, EventArgs e)
        {

            ForwardConstraintForm c = new ForwardConstraintForm();
            c.ShowDialog();

            if (ForwardConstraintForm.accept && ForwardConstraintForm.content.Trim() != "")
            {
                addConstraint(dataGridView1.SelectedCells, new ForwardConstraint(ForwardConstraintForm.content));

            }

        }

        private void ncforbiddenPatterns_Click(object sender, EventArgs e)
        {

            ForbiddenPatternForm c = new ForbiddenPatternForm();
            c.ShowDialog();

            if (ForbiddenPatternForm.accept && ForbiddenPatternForm.content.Trim() != "")
            {
                addConstraint(dataGridView1.SelectedCells, new ForbiddenPatternNonCyclicConstraint(ForbiddenPatternForm.content));

            }

        }


        private void forbiddenPatterns_Click(object sender, EventArgs e)
        {

            ForbiddenPatternForm c = new ForbiddenPatternForm();
            c.ShowDialog();

            if (ForbiddenPatternForm.accept && ForbiddenPatternForm.content.Trim() != "")
            {
                addConstraint(dataGridView1.SelectedCells, new ForbiddenPatternConstraint(ForbiddenPatternForm.content));

            }

        }

        private void custom_constraint_Click(object sender, EventArgs e)
        {

            CustomConstraintInput c = new CustomConstraintInput();
            c.ShowDialog();

            if(CustomConstraintInput.accept && CustomConstraintInput.content != "")
            {
                addConstraint(dataGridView1.SelectedCells, new CustomConstraint(CustomConstraintInput.content));

            }

        }


        private void custom_sum_Click(object sender, EventArgs e)
        {
            CustomSumConstraintForm c = new CustomSumConstraintForm();
            c.ShowDialog();

            if (CustomSumConstraintForm.accept)
            {
                addConstraint(dataGridView1.SelectedCells, new CustomSumConstraint(CustomSumConstraintForm.op, CustomSumConstraintForm.var, instance));

            }
        }


        private void symmetry_ascending_Click(object sender, EventArgs e)
        {
            addConstraint(dataGridView1.SelectedCells, new BreakSymmetryByAscendingConstraint());
        }

        private void set_in_atm_one(object sender, EventArgs e)
        {
            addConstraint(dataGridView1.SelectedCells, new SetIntersectionAtMostOneConstraint());
        }

        private void create_set_Click(object sender, EventArgs e)
        {
            addConstraint(dataGridView1.SelectedCells, new Set());
        }

        private void equalOne_Click(object sender, EventArgs e)
        {
            addConstraint(dataGridView1.SelectedCells, new EqualOneConstraint());
        }

        private void sumLEQ1_Click(object sender, EventArgs e)
        {
            addConstraint(dataGridView1.SelectedCells, new SumLEQ1Constraint());
        }

        

        public static void addVariables(List<Variable> vars)
        {
            stack.doAction(new CreateVariableAction(vars, instance));
        }

        private void addConstraint(DataGridViewSelectedCellCollection selected_cells,  Constraint c)
        {

            

            List<Variable> selectedVars = new List<Variable>();

            foreach (DataGridViewCell cell in selected_cells)
            {
                if (instance.getVar(cell) != null)
                {
                    selectedVars.Add(instance.getVar(cell));
                }

            }

            if (selectedVars.Count == 0)
            {
                selectedVars.Clear();
                dataGridView1.ClearSelection();
                return;
            }
            else
            {
                lastConstraint = c;
                stack.doAction(new AddConstraintAction(c, instance, selectedVars));

                invalidateInstance();

                selectedVars.Clear();
                dataGridView1.ClearSelection();
            }
        }

        private void allEqual_Click(object sender, EventArgs e)
        {
            addConstraint(dataGridView1.SelectedCells, new AllEqualConstraint());
        }

        private void allDiff_Click(object sender, EventArgs e)
        {
            addConstraint(dataGridView1.SelectedCells, new AllDifferentConstraint());

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

            selected_cells = dataGridView1.SelectedCells.Cast<DataGridViewCell>().OrderBy(r => r.RowIndex).ThenBy(c => c.ColumnIndex);

        }



        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            DataGridViewCell currentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (instance.getVar(currentCell) == null)
            {
                // empty cell
                //e.Handled = false; <- let event be handled by default handler. (done implicit)

                return;


            }

            else
            {
                Variable currentVar = instance.getVar(currentCell);


                if(rectDrag) dataGridView1.ClearSelection();


                // Erase the cell.
                e.Graphics.FillRectangle(new SolidBrush(currentVar.getColor()), e.CellBounds);

                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected)

                {
                    e.Graphics.FillRectangle(Brushes.DarkKhaki, e.CellBounds);
                }

                CellTools.drawGrid(e, gridColor);



                if(drawConstraints) currentVar.drawConstraintsInCell(e, instance);


                e.Graphics.DrawString(currentVar.getFormattedText(), e.CellStyle.Font,
                    Brushes.Black, e.CellBounds.X + 2,
                    e.CellBounds.Y + 2, StringFormat.GenericDefault);

                currentVar.fixedVal = currentVar.fixedVal.Trim();

                if(currentVar.fixedVal != "")
                {
                    if (instance.solved)
                    {
                        e.Graphics.DrawString(currentVar.solvedValue, e.CellStyle.Font,
                     Brushes.Crimson, e.CellBounds.X + 2,
                     e.CellBounds.Y + 20, StringFormat.GenericDefault);
                    }
                    else
                    {
                        e.Graphics.DrawString(currentVar.fixedVal, e.CellStyle.Font,
                     Brushes.Crimson, e.CellBounds.X + 2,
                     e.CellBounds.Y + 20, StringFormat.GenericDefault);
                    }
                

                }
                else
                {


                    if (instance.solved) e.Graphics.DrawString(currentVar.solvedValue, e.CellStyle.Font,
                         Brushes.Black, e.CellBounds.X + 2,
                         e.CellBounds.Y + 20, StringFormat.GenericDefault);

                    if (!instance.solved) e.Graphics.DrawString(currentVar.solvedValue, e.CellStyle.Font,
                         Brushes.LightGray, e.CellBounds.X + 2,
                         e.CellBounds.Y + 20, StringFormat.GenericDefault);


                }



                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to delete all variables and constraints?",
                                     "Confirm Delete",
                                     MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                instance.clear(sizeX, sizeY);
                refreshGrid();
                stack.clear();
                refreshUndoButtons();
                richTextBox1.Text = instance.getCode();
            }
            else
            {
                // If 'No', do something here.
            }

            

        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            if (rectDrag)
            {
                instance.drawPossibleConstraints(e);

            }
            else
            {
                instance.clearPossibleConstraints();
            }

            if (mouseDown && selectDown && dataGridView1.SelectedCells.Count > 1)
            {
                Rectangle rect = new Rectangle(mouseX + 30, mouseY+10, 75, 55);
                e.Graphics.DrawRectangle(new Pen(Brushes.Black), mouseX+29, mouseY+9, 76, 56);
                e.Graphics.FillRectangle(Brushes.White, rect);
                Font f = new Font(FontFamily.GenericMonospace, 10);

                int width = 0;
                int height = 0;
                bool regular = true;
                DataGridViewCell prev = null;


                width = selected_cells.Last().ColumnIndex - selected_cells.First().ColumnIndex+1;
                height = selected_cells.Last().RowIndex - selected_cells.First().RowIndex+1;

                foreach (DataGridViewCell c in selected_cells)
                {
                    if (prev != null && !(c.ColumnIndex == prev.ColumnIndex + 1 || c.ColumnIndex == selected_cells.First().ColumnIndex && c.RowIndex == prev.RowIndex + 1))
                    {
                        regular = false;
                        break;
                    }
                    
                        
                  
                    if(prev != null && c.RowIndex == prev.RowIndex + 1 && prev.ColumnIndex - selected_cells.First().ColumnIndex+1 != width)
                    {
                        regular = false;
                        break;
                    }

                    prev = c;


                 }

                if(regular) e.Graphics.DrawString(width.ToString() + " x " + height.ToString(), f, Brushes.Black, mouseX + 29, mouseY + 9);
                e.Graphics.DrawString((dataGridView1.SelectedCells.Count.ToString() + " Cells"), f, Brushes.Black, mouseX + 29, mouseY + 39);

            }
            if(instance.getConstraintCount() != 0 && drawConstraints)
            {
                instance.drawConstraintLabel(e, dataGridView1);
            }
        }

        private void dataGridView1_MouseMove(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;
            if (mouseDown) refreshGrid();
            
            
        }
        
        
        private void invalidateInstance()
        {
            instance.solved = false;
            instance.unsatisfiable = false;
            refreshGrid();
            refreshUndoButtons();
            if(tabControl1.SelectedIndex == 1)
            {
                richTextBox1.Text = instance.getCode();
            }
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 's' || e.KeyChar == 'S')
            {
                create_set_Click(null, null);
            }
        }

        private void refreshGrid()
        {
            dataGridView1.Invalidate();
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            
            Point p = new Point(e.ColumnIndex, e.RowIndex);
            mouseX = e.X;
            mouseY = e.Y;

            Point mouse = new Point(e.X, e.Y);

            Rectangle r = instance.getDragRectangle(dataGridView1);
            

            if (e.Button == MouseButtons.Left) {

                mouseDown = true;

                if (drawConstraints &&r != null && r.Contains(cellPointToDataGridSpace(e.ColumnIndex, e.RowIndex, mouse)))
                {
                    rectDrag = true;
                    rectDragStartCell = p;
                    
                }
                else
                {
                    selectDown = true;
                }

                refreshGrid();


            }
        }

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (rectDrag == true)
            {

                Constraint lastDuplicated = null;

                Point diff = new Point(e.ColumnIndex - rectDragStartCell.X, e.RowIndex - rectDragStartCell.Y);
                if (diff.X != 0 || diff.Y != 0)
                {
                    int x = 0;
                    int y = 0;

                    while (Math.Abs(x) <= Math.Abs(diff.X) && Math.Abs(y) <= Math.Abs(diff.Y) && (x != diff.X || y != diff.Y))
                    {
                        if (x != diff.X) x += Math.Sign(diff.X);
                        if (y != diff.Y) y += Math.Sign(diff.Y);

                        lastDuplicated = instance.duplicateConstraint(new Point(x, y), lastConstraint);

                        invalidateInstance();

                    }

                }

                if (lastDuplicated != null) lastConstraint = lastDuplicated;

            }

            mouseDown = false;
            selectDown = false;
            rectDrag = false;

            refreshGrid();
        }

        private Point cellPointToDataGridSpace(int columnindex, int rowindex, Point cellPoint)
        {
            Point p = dataGridView1.GetCellDisplayRectangle(columnindex, rowindex, false).Location;
            return new Point(p.X + cellPoint.X, p.Y + cellPoint.Y);
        }

        private void dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (rectDrag == true)
            {

                instance.clearPossibleConstraints();

                Point diff = new Point(e.ColumnIndex - rectDragStartCell.X, e.RowIndex - rectDragStartCell.Y);
                if (diff.X != 0 || diff.Y != 0)
                {
                    int x = 0;
                    int y = 0;

                    while (Math.Abs(x) <= Math.Abs(diff.X) && Math.Abs(y) <= Math.Abs(diff.Y) && (x != diff.X || y != diff.Y))
                    {
                        if (x != diff.X) x += Math.Sign(diff.X);
                        if (y != diff.Y) y += Math.Sign(diff.Y);

                        instance.drawDuplicateConstraint(new Point(x, y), lastConstraint);


                    }
                
                }
            }
        }

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            refreshGrid();
        }

        private void row_column_numbers_CheckedChanged(object sender, EventArgs e)
        {
            if (row_column_numbers.Checked)
            {
                dataGridView1.RowHeadersVisible = true;
                dataGridView1.ColumnHeadersVisible = true;
                refreshGrid();
            }
            else
            {
                dataGridView1.RowHeadersVisible = false;
                dataGridView1.ColumnHeadersVisible = false;
                refreshGrid();
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Variable v = instance.getVar(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex]);

            if (v != null)
            {

                ChangeVarForm f = new ChangeVarForm(v);
                f.ShowDialog();

                invalidateInstance();

                dataGridView1.ClearSelection();

                refreshGrid();

            }
            else
            {
                new_var_Click_1(null, null);
            }
        }

        private void checkconstraints_CheckedChanged(object sender, EventArgs e)
        {
            drawConstraints = checkconstraints.Checked;
            refreshGrid();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0) solver = SolverName.gecode;
            if (comboBox1.SelectedIndex == 1) solver = SolverName.minizinc;
            if (comboBox1.SelectedIndex == 2) solver = SolverName.or_tools;
            if (comboBox1.SelectedIndex == 3) solver = SolverName.chuffed;
            if (comboBox1.SelectedIndex == 4) solver = SolverName.all;
            

        }
        
        private void refreshUndoButtons()
        {
            undo_button.Enabled = stack.getUndoPossible();
            redo_button.Enabled = stack.getRedoPossible();
        }

        private void undo_button_Click(object sender, EventArgs e)
        {
            if (stack.getUndoPossible()) stack.undo();
            invalidateInstance();
        }

        private void redo_button_Click(object sender, EventArgs e)
        {
            if (stack.getRedoPossible()) stack.redo();
            invalidateInstance();
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            SaveLoadTools.save(instance);
        }

        private void load_button_Click(object sender, EventArgs e)
        {
            CPInstance i = SaveLoadTools.load(dataGridView1);
            if(i != null)
            {
                instance = i;
            }
        }
    }
}





