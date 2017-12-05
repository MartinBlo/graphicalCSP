using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalConstraintProgramming
{
    class CustomSumConstraint : SimpleConstraint
    {
        private static String lastConstraintCode;

        public static String last_opI;
        public static String last_varI;

        private String opI = null;
        private String varI = null;


        private bool isVar = false;
        public static bool last_isVar = false;


        public CustomSumConstraint()
        {
            if (lastConstraintCode == "") throw new Exception("Cannot duplicate constraint!");
            base.variables = new List<Variable>();
        }

        public CustomSumConstraint(String op, String var, CPInstance i)

        {
            varI = var.Trim();
            opI = op.Trim();


            Variable output = null;

            if (i.variableExists(var))
            {
                isVar = true;
                output = i.getVarByName(varI);
            }
            else
            {
                isVar = false;
            }

            base.variables = new List<Variable>();

            if (output != null)
            {
                variables.Add(output);
                base.involvedVariables().Add(output);
                output.usedInConstraint(this);
            }

            last_varI = varI;
            last_opI = opI;
            last_isVar = isVar;


        }

        public override string GetCode()
        {

            if(varI == null || opI == null)
            {
                varI = last_varI;
                opI = last_opI;
                isVar = last_isVar;
            }



            String output = "constraint (";

            bool first = true;

            List<Variable> vars = new List<Variable>(base.involvedVariables());

            Variable o = null;

            if (isVar)
            {
                o = vars[0];
                vars.Remove(o);
            }

            foreach (Variable v in vars)
            {

                if (!first)
                {
                    output += "+";
                }
                first = false;

                output += v.name;
            }

            if(isVar) output += ") " + opI + " " + o.name + ";";
            else output += ") " + opI + " " + varI + ";";

            return output;
        }
    }
}
