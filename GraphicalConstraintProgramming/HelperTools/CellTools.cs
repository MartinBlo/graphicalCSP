using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GraphicalConstraintProgramming
{
    internal class CellTools
    {
        internal static void drawConstraint(Variable variable, Constraint c, DataGridViewCellPaintingEventArgs e, CPInstance i)
        {

            Pen p = new Pen(Color.FromArgb(Math.Max(0,255 - c.duplication_distance*10), c.getColor()));

            if (c.duplication_distance == 0)
            {
                p.Width = 4f;
            }
            else
            {
                p.Width = 1.5f + 1.0f / c.duplication_distance;
            }

            Variable[] n = i.getNeighbours(variable , c);


            for (int k = 0; k < 4; k++)
            {
                if (n[k] == null)// && n[(k + 4 - 1) % 4] != null && n[(k + 4 + 1) % 4] != null)
                {
                    CellTools.drawStraight(e, k, p);
                }
            }
            
        }

        private static void drawStraight(DataGridViewCellPaintingEventArgs e, int k, Pen p)
        {
            if (k == 0)
            {
                e.Graphics.DrawLine(p, e.CellBounds.Left + p.Width/2,
                e.CellBounds.Bottom , e.CellBounds.Left + p.Width / 2,
                e.CellBounds.Top );
            }
            if (k == 1)
            {
                e.Graphics.DrawLine(p, e.CellBounds.Left,
                e.CellBounds.Top + p.Width / 2, e.CellBounds.Right ,
                e.CellBounds.Top + p.Width / 2);

            }
            if (k == 2)
            {
                e.Graphics.DrawLine(p, e.CellBounds.Right -p.Width/2,
                e.CellBounds.Top , e.CellBounds.Right  - p.Width/2,
                e.CellBounds.Bottom );
            }
            if (k == 3)
            {
                e.Graphics.DrawLine(p, e.CellBounds.Left,
                e.CellBounds.Bottom  - p.Width/2, e.CellBounds.Right ,
                e.CellBounds.Bottom  - p.Width/2);
            }




        }

        internal static void drawGrid(DataGridViewCellPaintingEventArgs e, Color gridColor)
        {
            Pen gridLinePen = new Pen(gridColor);


            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left,
            e.CellBounds.Bottom - 1, e.CellBounds.Right - 1,
            e.CellBounds.Bottom - 1);
            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1,
                e.CellBounds.Top, e.CellBounds.Right - 1,
                e.CellBounds.Bottom);
        }

        internal static void drawPossibleConstraint(PaintEventArgs e, List<Variable> list, Color c)
        {

            foreach (Variable v in list)
            {
                e.Graphics.DrawRectangle(new Pen(new SolidBrush(c)), v.c.DataGridView.GetCellDisplayRectangle(v.c.ColumnIndex, v.c.RowIndex, false));
            }


        }
    }
}