using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalConstraintProgramming
{
    class BreakSymmetryByAscendingConstraint : SimpleConstraint
    {

        public override string GetCode()
        {

            variables.Sort();

            String output = "constraint increasing([";

            bool first = true;

            foreach (Variable v in variables)
            {

                if (!first)
                {
                    output += ",";
                }
                first = false;

                output += v.name;
            }

            output += "]);";

            return output;
        }

        public override Color getColor()
        {
            return Color.BlanchedAlmond;
        }

    }
}
