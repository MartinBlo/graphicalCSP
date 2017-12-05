using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace GraphicalConstraintProgramming
{
    class AllEqualConstraint : SimpleConstraint
    {
        public override string GetCode()
        {

            String output = "constraint all_equal([";

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
            return Color.BlueViolet;
        }
        
    }
}
