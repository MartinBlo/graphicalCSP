using System;
using System.Collections.Generic;
using System.Drawing;

namespace GraphicalConstraintProgramming
{
    internal class Set : SimpleConstraint
    {


        public override string GetCode()
        {
            string output = "";
            

            output += "var set of int: " + getName() + " = " + getVariableSet(involvedVariables()) +";";

            return output;


        }

        public string getName()
        {
            return "set_" + GetHashCode();
        }

        public override void addVariables(List<Variable> variables)
        {
            base.addVariables(variables);

            foreach(Variable v in variables)
            {
                v.usedInSet(this);
            }


        }

        public override Color getColor()
        {
            return Color.Yellow;
        }

        private string getVariableSet(List<Variable> list)
        {
            String output = "{";

            bool first = true;

            foreach (Variable v in list)
            {

                if (!first)
                {
                    output += ",";
                }
                first = false;

                output += v.name;
            }

            output += "}";

            return output;
        }
    }
}