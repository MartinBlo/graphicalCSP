using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalConstraintProgramming
{
    class ColorTools
    {
        public static Color addColor(Color c1, Color c2)
        {

            Color x = Color.FromArgb(255, Math.Max(0, Math.Min(c1.R - 15, c2.R - 15)), Math.Max(0, Math.Min(c1.G - 15, c2.G - 15)), Math.Max(0, Math.Min(c1.B - 15, c2.B - 15)));


            return x;


        }
    }
}
