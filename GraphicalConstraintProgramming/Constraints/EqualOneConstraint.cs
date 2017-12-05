using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalConstraintProgramming
{
    class EqualOneConstraint : SimpleConstraint
    {
        public override string GetCode()
        {

            String output = "constraint (";

            bool first = true;

            foreach (Variable v in base.involvedVariables())
            {

                if (!first)
                {
                    output += "+";
                }
                first = false;

                output += v.name;
            }

            output += ") = 1;";

            return output;
        }

        public override Color getColor()
        {
            return Color.Blue;
        }
    }
}
