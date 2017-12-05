using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GraphicalConstraintProgramming
{
    abstract class SimpleConstraint : Constraint
    {

        protected List<Variable> variables;
        int rightmost_index = 0;
        int lowest_index = 0;

        public SimpleConstraint()
        {
            this.variables = new List<Variable>();

        }

        public override void addVariables(List<Variable> variables)
        {
            if(this.variables == null) this.variables = new List<Variable>();
            foreach (Variable v in variables)
            {
                this.variables.Add(v);
                v.usedInConstraint(this);
                if (v.locationX > rightmost_index) rightmost_index = v.locationX;
                if (v.locationY > lowest_index) lowest_index = v.locationY;
            }
        }

        public override void drawDragMark(PaintEventArgs e, DataGridView v)
        {
            if(getDragMarkRectangle(v).Location.X != 0)
            {
                e.Graphics.DrawRectangle(new Pen(new SolidBrush(getColor())), getDragMarkRectangle(v));
                e.Graphics.DrawString("+", new Font(FontFamily.GenericMonospace, 10), Brushes.Black, getDragMarkRectangle(v).Left, getDragMarkRectangle(v).Top);
            }
           
        }

        public override Rectangle getDragMarkRectangle(DataGridView v)
        {
            int cellindexX = rightmost_index + 1;
            int cellindexY = lowest_index;

            Point p  = v.GetCellDisplayRectangle(cellindexX, cellindexY, false).Location;


            return new Rectangle(p.X +10 , p.Y +10, Form1.getSize() / 2, Form1.getSize() / 2);
        }

        public override string GetCode()
        {
            

            return "";
        }

        public override Color getColor()
        {
            return Color.Black;
        }



        public override int getDrawingOffset()
        {
            return 0;
        }

        public override List<Variable> involvedVariables()
        {
            return variables.ToList();
        }

        public override String getSerialization()
        {
            return "";
        }
        

    }
}
