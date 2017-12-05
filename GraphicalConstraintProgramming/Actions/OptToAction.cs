using System;

namespace GraphicalConstraintProgramming
{
    internal class OptToAction : Action
    {
        private Variable variable;
        private string value;
        private CPInstance instance;
        private int id = 0;

        public OptToAction(Variable variable, string value, CPInstance instance)
        {
            this.variable = variable;
            this.value = value;
            this.instance = instance;

        }
        

        public override void doAction()
        {
            id = instance.setOptTo(variable, value);
        }

        public override void undoAction()
        {
            instance.removeOptTo(id);
        }
    }
}