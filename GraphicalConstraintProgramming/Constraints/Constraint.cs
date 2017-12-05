using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicalConstraintProgramming
{
    public abstract class Constraint
    {
        public abstract string GetCode();

        public abstract Color getColor();

        public abstract List<Variable> involvedVariables();

        public abstract void addVariables(List<Variable> variables);

        public abstract int getDrawingOffset();

        public abstract void drawDragMark(PaintEventArgs e, DataGridView v);

        public abstract Rectangle getDragMarkRectangle(DataGridView v);

        public abstract string getSerialization();

        public int duplication_distance = 0;
    }

}
