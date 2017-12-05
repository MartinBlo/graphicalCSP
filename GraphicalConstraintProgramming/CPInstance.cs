using GraphicalConstraintProgramming.Actions;
using GraphicalConstraintProgramming.HelperTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicalConstraintProgramming
{
    public class CPInstance
    {
        public  bool solved;
        public  bool unsatisfiable;
        //internal Rectangle dragRectangle;

        public Dictionary<String,Variable> variables;
        
        private Dictionary<DataGridViewCell, Variable> cell_var_mapping;
        
        public List<Constraint> constraints;
        
        private List<List<Variable>> dragConstraints; //constraints for drag duplication
        
        public List<OptimizationGoal> optimizationGoals;

        private Variable[,] grid;

        public Constraint lastConstraint = null;

        public string code;


        internal CPInstance() { }

        internal void removeConstraint(Constraint c)
        {
            constraints.Remove(c);
            //dragRectangle = c.getDragMarkRectangle(v);
            lastConstraint = null;
        }
        

        public CPInstance(int sizeX, int sizeY)
        {
            clear(sizeX, sizeY);
        }

        public bool variableExists(string name)
        {
            return variables.ContainsKey(name) || variables.ContainsKey(name + "_1_1");
            
        }

        public Variable getVarByName(string name)
        {
            return variables[name];
        }
        

        public int getConstraintCount()
        {
            return constraints.Count;
        }

        public void clearOptimization()
        {
            if(optimizationGoals != null) foreach(OptimizationGoal g in optimizationGoals)
            {
                g.v.isOptimize = false;
            }
            optimizationGoals = new List<OptimizationGoal>();
                                
        }
        
        public int setOptTo(Variable v, String value)
        {
            var goal = new OptimizationGoal(v, value, 1);
            int id = goal.GetHashCode();
            optimizationGoals.Add(goal);
            v.isOptimize = true;
            return id;
        }

        public void removeOptTo(int id)
        {

            List<OptimizationGoal> copy = new List<OptimizationGoal>(optimizationGoals);

            foreach(OptimizationGoal g in copy)
            {
                if(g.GetHashCode() == id)
                {
                    optimizationGoals.Remove(g);
                    g.v.isOptimize = false;

                }
            }
        }

        public Variable[] getNeighbours(Variable v) // LEFT, TOP, RIGHT, BOTTOM
        {
            Variable[] n = new Variable[4];

            if (v.locationX > 0 && grid[v.locationX - 1, v.locationY] != null) n[0] = (grid[v.locationX - 1, v.locationY]);
            if (v.locationX < grid.GetLength(0) -1 && grid[v.locationX + 1, v.locationY] != null) n[2] = (grid[v.locationX + 1, v.locationY]);

            if (v.locationY > 0 && grid[v.locationX, v.locationY-1] != null) n[1] = (grid[v.locationX , v.locationY-1]);
            if (v.locationY < grid.GetLength(1) - 1 && grid[v.locationX, v.locationY+1] != null) n[3] = (grid[v.locationX , v.locationY+1]);

            return n;


        }

        public Variable[] getNeighbours(Variable v, Constraint c) // LEFT, TOP, RIGHT, BOTTOM
        {
            Variable[] n = new Variable[4];

            if (v.locationX > 0 && grid[v.locationX - 1, v.locationY] != null && c.involvedVariables().Contains(grid[v.locationX - 1, v.locationY])) n[0] = (grid[v.locationX - 1, v.locationY]);
            if (v.locationX < grid.GetLength(0) - 1 && grid[v.locationX + 1, v.locationY] != null && c.involvedVariables().Contains(grid[v.locationX + 1, v.locationY])) n[2] = (grid[v.locationX + 1, v.locationY]);

            if (v.locationY > 0 && grid[v.locationX, v.locationY - 1] != null && c.involvedVariables().Contains(grid[v.locationX , v.locationY-1])) n[1] = (grid[v.locationX, v.locationY - 1]);
            if (v.locationY < grid.GetLength(1) - 1 && grid[v.locationX, v.locationY + 1] != null && c.involvedVariables().Contains(grid[v.locationX, v.locationY+1])) n[3] = (grid[v.locationX, v.locationY + 1]);

            return n;


        }

        public void addVariable(Variable v)
        {
            if (variableExists(v.name)) throw new Exception("Variable exitsts!");
            if (cell_var_mapping.ContainsKey(v.c)) throw new Exception("Cell occupied!");

            variables[v.name] = v;
            cell_var_mapping[v.c] = v;
            grid[v.locationX, v.locationY] = v;

        }

        public void removeVariable(Variable v, DataGridViewCell c)
        {
            variables.Remove(v.name);
            cell_var_mapping.Remove(c);
            grid[v.locationX, v.locationY] = null;
        }

        internal string writeFile()
        {

            


            // Write the string to a file.
            String path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" +  "output.mzn";
            System.IO.StreamWriter file = new System.IO.StreamWriter(path);
            file.WriteLine(getCode());

            Console.WriteLine(getCode());

            file.Close();

            return path;
        }

        internal string getCode()
        {
            code = "include \"globals.mzn\";";

            foreach (Variable v in variables.Values)
            {
                v.fixedVal = v.fixedVal.Trim();

                if (v.type == vartype.bool_type)
                {

                }
                if (v.type == vartype.float_type)
                {

                }
                if (v.type == vartype.int_type)
                {

                    if(v.fixedVal != "")
                    {

                        code += "\n";
                        code += "var " + v.domain_start + ".." + v.domain_end + ": " + v.name + " = " + v.fixedVal + " ;";
                    }
                    else
                    {

                        code += "\n";
                        code += "var " + v.domain_start + ".." + v.domain_end + ": " + v.name + ";";
                    }

                }
                if (v.type == vartype.string_type)
                {

                }

            }


            foreach (Constraint c in constraints)
            {
                String c_code = c.GetCode().Trim();
                if (c.involvedVariables().Count != 0 && c_code != "")
                {
                    code += "\n";
                    code += c_code;
                }
            }


            code += OptimizationGoalHelper.getOptimizationString(optimizationGoals);

            code += "\noutput[";

            
            bool first = true;

            foreach (Variable v in variables.Values)
            {

                if (!first)
                {
                    code += ",";
                }
                first = false;

                code += '"' + v.name +  " = "+ '"' + ", show(" +v.name + ")" + ","+ '"' + ";\\n" + '"' + "\n";
            }

                    

            code += "];";
            
            return code;
        }

        internal void solve(MiniZincInterface mzinc, SolverName solverName)
        {
            string result = mzinc.solve(this, solverName);

            if (result.Contains("UNSATISFIABLE") || result.Trim() == "")
            {
                solved = false;
            }
          
            else
            {
                solved = true;

                String[] solutions = result.Split(new string[] { "==========" }, StringSplitOptions.None);

                if(solutions.Length >= 2) result = solutions[solutions.Length - 2];



                using (StringReader reader = new StringReader(result))
                {
                    string line = string.Empty;
                    do
                    {
                        line = reader.ReadLine();
                        if (line != null)
                        {
                            if (line.Contains("=") && !line.Contains("set_"))
                            {
                                string[] x = line.Split('=');

                                Variable v = variables[x[0].Trim()];

                                if (v != null)
                                {
                                    v.solvedValue = x[1].Trim().Split(';')[0];
                                    
                                }


                            }
                        }

                    } while (line != null);
                }
            }


        }

        internal void addConstraint(Constraint c)
        {

            constraints.Add(c);
            //dragRectangle = c.getDragMarkRectangle(v);
            lastConstraint = c;
            
        }

        internal Variable getVar(DataGridViewCell c)
        {
            if (!cell_var_mapping.ContainsKey(c)) return null;
            return cell_var_mapping[c];
        }

        internal void clear(int sizeX, int sizeY)
        {
            grid = new Variable[sizeX, sizeY]; // TODO DYNAMIC CHANGE;
            variables = new Dictionary<string, Variable>();
            cell_var_mapping = new Dictionary<DataGridViewCell, Variable>();
            constraints = new List<Constraint>();
            solved = false;
            unsatisfiable = false;
            dragConstraints = new List<List<Variable>>();
            //dragRectangle = new Rectangle(0,0,0,0);
            clearOptimization();

        }

        internal void drawConstraintLabel(PaintEventArgs e, DataGridView v)
        {
            if(lastConstraint != null) lastConstraint.drawDragMark(e, v);
        }

        internal Rectangle getDragRectangle(DataGridView v)
        {
            if (lastConstraint == null) return new Rectangle(0, 0, 0, 0);
            return lastConstraint.getDragMarkRectangle(v);
        }

        internal void drawDuplicateConstraint(Point diff, Constraint last)
        {
            if (last != null) { 

                List<Variable> vars = getVarsByOffset(diff, last.involvedVariables());

                dragConstraints.Add(vars);
                
            }
        }


        internal Constraint duplicateConstraint(Point diff, Constraint last)
        {
            if(last != null)
            {
                Type t = last.GetType();
                Constraint c;

                try
                {
                    c = (Constraint) t.GetConstructor(new Type[] { }).Invoke(new object[] { });
                }
                catch
                {
                    c =  null;
                }



                if (c != null)
                {
                    Form1.stack.doAction(new AddConstraintAction(c,this, getVarsByOffset(diff, last.involvedVariables())));
                    c.duplication_distance = (int) Math.Abs( diff.X) + Math.Abs(diff.Y);
                    return c;
                }
            }
            return null;

        }
        

        private List<Variable> getVarsByOffset(Point diff, List<Variable> list)
        {
            List<Variable> offset = new List<Variable>();

            foreach(Variable v in list)
            {
                if (v.locationX + diff.X < 0 || v.locationX + diff.X >= grid.GetLength(0)) continue;
                if (v.locationY + diff.Y < 0 || v.locationY + diff.Y >= grid.GetLength(1)) continue;

                Variable n = grid[v.locationX + diff.X, v.locationY + diff.Y];
                if (n != null) offset.Add(n);

            }

            return offset;
        }

        internal void drawPossibleConstraints(PaintEventArgs e)
        {
            foreach(List<Variable> list in dragConstraints)
            {

                CellTools.drawPossibleConstraint(e, list, lastConstraint.getColor());

            }
        }

        internal void clearPossibleConstraints()
        {
            dragConstraints.Clear();
        }
    }
}
