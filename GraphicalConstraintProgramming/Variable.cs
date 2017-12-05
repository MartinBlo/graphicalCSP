using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicalConstraintProgramming
{
    
    public enum vartype {int_type, float_type, bool_type, string_type};

    

    public class Variable : IComparable<Variable>
    {

        public bool isOptimize = false;

        public vartype type;
        public int domain_start;

        internal bool isNeighbour(Variable possible_neighbour)
        {
            return (this.locationX == possible_neighbour.locationX && Math.Abs(this.locationY - possible_neighbour.locationY) == 1) ||
                (this.locationY == possible_neighbour.locationY && Math.Abs(this.locationX - possible_neighbour.locationX) == 1);
        }

        internal void notUsedInConstraint(Constraint c)
        {
            if (involvedInConstraint.Contains(c)) involvedInConstraint.Remove(c);
        }

        internal IEnumerable<Set> getSets()
        {
            return new HashSet<Set>(sets);
        }

        internal void usedInSet(Set set)
        {
            if(!sets.Contains(set)) this.sets.Add(set);
        }

        internal string getSerialization()
        {
            String output = "";
            output += this.name;
            output += "|";
            output += this.type;
            output += "|";
            output += this.domain_start;
            output += "|";
            output += this.domain_end;
            output += "|";
            output += this.uName;
            output += "|";
            output += this.varIndexX;
            output += "|";
            output += this.varIndexY;
            output += "|";
            output += this.locationX;
            output += "|";
            output += this.locationY;
            output += "|";
            output += this.fixedVal;

            return output;

        }

        public static Variable loadVar(String s, DataGridView grid)
        {
            String[] values = s.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            if (values[1].Equals(vartype.int_type.ToString()))
            {
                DataGridViewCell c = grid.Rows[int.Parse(values[7])].Cells[int.Parse(values[8])];
                Variable v =  new Variable(values[0], vartype.int_type, int.Parse(values[2]), int.Parse(values[3]), grid.Rows[int.Parse(values[8])].Cells[int.Parse(values[7])], values[4], int.Parse(values[5]), int.Parse(values[6]));
                if (values.Length > 9) v.fixedVal = values[9];
                return v;
            }
            return null;
        }

        public int domain_end;
        public string name;

        public int locationX;
        public int locationY;

        public string solvedValue;

        public DataGridViewCell c;
        public int varIndexY;
        public int varIndexX;
        private String uName;

        private HashSet<Constraint> involvedInConstraint;
        private HashSet<Set> sets;
        public string fixedVal = "";

        internal Variable() { } // for serialization

        public Variable(string name, vartype type, int domain_start, int domain_end, DataGridViewCell c, string uName, int varIndexX, int varIndexY)
        {
            if (type == vartype.bool_type || type == vartype.string_type || type == vartype.float_type) throw new ArgumentException("Domain specification only for int variables!");

            this.type = type;
            this.domain_start = domain_start;
            this.domain_end = domain_end;
            this.name = name;
            this.c = c;

            this.uName = uName;
            this.varIndexX = varIndexX;
            this.varIndexY = varIndexY;
            this.solvedValue = "";

            involvedInConstraint = new HashSet<Constraint>();

            this.locationX = c.ColumnIndex;
            this.locationY = c.RowIndex;

            sets = new HashSet<Set>();
        }




        public Variable(string name, vartype type)
        {
            if (type == vartype.int_type) throw new ArgumentException("Int variables must have a specific domain!");

            this.type = type;
            this.name = name;


        }

        internal string getFormattedText()
        {
            return this.uName + " " + StringFormatTools.subtext(this.varIndexX) + " " + StringFormatTools.subtext(this.varIndexY);
        }

        public void usedInConstraint(Constraint c)
        {
            if (!involvedInConstraint.Contains(c)) involvedInConstraint.Add(c);
        }

        public Color getColor()
        {
            Color c;
            if(!isOptimize) c = Color.FromArgb(255, 255, 255);
            else c = Color.FromArgb(255, 150, 150);
            //foreach(Constraint con in involvedInConstraint)
            //{
            //    c = ColorTools.addColor(c, con.getColor());
            //}

            return c;
        }

        internal void drawConstraintsInCell(DataGridViewCellPaintingEventArgs e, CPInstance i)
        {
            foreach(Constraint c in involvedInConstraint)
            {
                CellTools.drawConstraint(this, c, e, i);
            }
        }

        public int CompareTo(Variable other)
        {
            if (other == null) return 1;
            if (other.locationY < this.locationY) return 1;
            if (other.locationY > this.locationY) return -1;
            if (other.locationX < this.locationX) return 1;
            if (other.locationX > this.locationX) return -1;

            return 0;
         }
    }
}
