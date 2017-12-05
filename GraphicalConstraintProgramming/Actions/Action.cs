using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalConstraintProgramming
{
    public abstract class Action
    {
        public abstract void doAction();

        public abstract void undoAction();

    }
}
