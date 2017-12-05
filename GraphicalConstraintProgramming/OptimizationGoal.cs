using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalConstraintProgramming
{
    public class OptimizationGoal
    {
        public Variable v;
        public String goal;
        public int weight;

        public OptimizationGoal(Variable var, String goalString, int weight_int)
        {
            this.v = var;
            this.goal = goalString;
            this.weight = weight_int;
        }

        public string getCode()
        {
            return "abs(" + goal + " - " + v.name + ")";
        }

        internal string getSerialization()
        {
            String output = "";

            output += v.name;
            output += "||";
            output += goal;
            output += "||";
            output += weight;

            return output;
        }

        internal static Variable loadVar(CPInstance i, string v)
        {
            String[] data = v.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            return i.getVarByName(data[0]);
        }

        internal static string loadValue(string v)
        {
            String[] data = v.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            return data[1];
        }
    }
}
