using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicalConstraintProgramming
{
    internal class SetIntersectionAtMostOneConstraint : SimpleConstraint
    {
        private HashSet<Set> sets;

        public override void addVariables(List<Variable> variables_external)
        {
            this.variables = new List<Variable>();
            foreach (Variable v in variables_external)
            {
                this.variables.Add(v);
                v.notUsedInConstraint(this);
            }

            sets = new HashSet<Set>();

            foreach (Variable v in base.involvedVariables())
            {
                foreach (Set s in v.getSets())
                {
                    if (!sets.Contains(s)) sets.Add(s);
                }
            }

            base.variables.Clear();
            
            foreach (Set set in sets)
            {
                foreach (Variable v in set.involvedVariables())
                {
                    v.usedInConstraint(this);
                    if (!variables.Contains(v)) variables.Add(v);
                }
            }
        }
   

    public override string GetCode()
        {

            String output = "constraint at_most1(" + getSetArray(sets) + ");";



            


            return output;
        }

        private string getSetArray(IEnumerable<Set> sets)
        {
            String output = "[";

            bool first = true;

            foreach (Set s in sets)
            {

                if (!first)
                {
                    output += ",";
                }
                first = false;

                output += s.getName();
            }

            output += "]";

            return output;
        }



        public override Color getColor()
        {
            return Color.Blue;
        }

        public override void drawDragMark(PaintEventArgs e, DataGridView v)
        {
        }

    }
}
