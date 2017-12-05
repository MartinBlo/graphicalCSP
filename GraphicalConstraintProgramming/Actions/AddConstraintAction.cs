using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalConstraintProgramming.Actions
{
    public class AddConstraintAction : Action
    {

        private Constraint c;
        private CPInstance i;
        private List<Variable> selectedVars;

        public AddConstraintAction(Constraint c, CPInstance i, List<Variable> selectedVars)
        {
            this.c = c;
            this.i = i;
            this.selectedVars = new List<Variable>(selectedVars);
            c.addVariables(selectedVars);

        }

        public override void doAction()
        {

            foreach (Variable v in selectedVars) v.usedInConstraint(c);
            i.addConstraint(c);
        }

        public override void undoAction()
        {
            i.removeConstraint(c);
            foreach(Variable v in c.involvedVariables())
            {
                v.notUsedInConstraint(c);
            }
            
        }
    }
}
