using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalConstraintProgramming.Constraints
{
    class CountXYConstraint : SimpleConstraint
    {
        
        private String selectedVar = "";
        private String countedVar = "";
        private String operatorVar = "";
        private bool isVar = false;

        private static String lastVar = "";
        private static String lastCount = "";
        private static String lastOP = "";
        private static bool lastWasVar = false;

        public CountXYConstraint()
        {
            if (lastVar == "" || lastCount =="" || lastOP == "") throw new Exception("Cannot duplicate constraint!");

            selectedVar = lastVar;
            countedVar = lastCount;
            operatorVar = lastOP;
            isVar = lastWasVar;

            base.variables = new List<Variable>();
        }

        public CountXYConstraint(String selectedVar, String countedVar, String operatorVar, CPInstance i)
        {
            this.selectedVar = selectedVar;
            this.countedVar = countedVar;
            this.operatorVar = operatorVar;

            lastVar = selectedVar;
            lastCount = countedVar;
            lastOP = operatorVar;

            Variable output = null;

            if (i.variableExists(countedVar))
            {
                lastWasVar = true;
                isVar = true;
                output = i.getVarByName(countedVar);
            }
            else
            {
                lastWasVar = false ;
                isVar = false;
            }

            base.variables = new List<Variable>();

            if (output != null)
            {
                variables.Add(output);
                base.involvedVariables().Add(output);
                output.usedInConstraint(this);
            }
        }

        public override string GetCode()
        {

            String output = "constraint  count([";

            bool first = true;

            foreach (Variable v in base.involvedVariables())
            {

                if (isVar && v == involvedVariables()[0]) continue; // first is the output var

                if (!first)
                {
                    output += ",";
                }
                first = false;

                output += v.name;
            }

            if(!isVar) output += "], " + selectedVar + ") " + operatorVar + " " + countedVar + ";";
            else output += "], " + selectedVar + ") " + operatorVar + " " + involvedVariables()[0].name + ";";

            return output;
        }
    }
}

