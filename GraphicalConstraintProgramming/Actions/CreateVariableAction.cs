using System;
using System.Collections.Generic;

namespace GraphicalConstraintProgramming
{
    internal class CreateVariableAction : Action
    {
        private List<Variable> vars;
        private CPInstance i;

        public CreateVariableAction(List<Variable> vars, CPInstance i)
        {
            this.vars = vars;
            this.i = i;
        }

        public override void doAction()
        {
            foreach(Variable v in vars)
            {
                i.addVariable(v);
            }
        }

        public override void undoAction()
        {
            foreach (Variable v in vars)
            {
                i.removeVariable(v, v.c);
            }
        }
    }
}