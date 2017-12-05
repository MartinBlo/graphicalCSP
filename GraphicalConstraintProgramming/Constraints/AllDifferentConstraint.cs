using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GraphicalConstraintProgramming
{
    class AllDifferentConstraint : SimpleConstraint
    {
        public static Constraint fromSerialization(string s, CPInstance i)
        {
            AllDifferentConstraint c =  new AllDifferentConstraint();

            String[] values = s.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            List<Variable> involvedV = new List<Variable>();

            for(int index= 1; index < values.Length; index++)
            {
                Variable v = i.getVarByName(values[index]);
                if(v != null)
                {
                    involvedV.Add(v);
                }
            }

            c.addVariables(involvedV);

            return c;

        }

        public override string getSerialization()
        {
            String o =  this.GetType().ToString();
            

            foreach(Variable v in involvedVariables())
            {
                o += "|" + v.name;
            }

            return o;



        }

        public override string GetCode()
        {

            String output = "constraint all_different([";

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
            return Color.Green;
        }


    }
}
