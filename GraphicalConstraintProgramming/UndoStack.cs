using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalConstraintProgramming
{
    public class UndoStack
    {
        Stack<Action> actions;
        Stack<Action> redoActions;

        public UndoStack()
        {
            clear();
        }

        public void clear()
        {
            actions = new Stack<Action>();
            redoActions = new Stack<Action>();
        }

        public void doAction(Action a)
        {
            doAction(a, true);
        }

        private void doAction(Action a, Boolean clearRedo)
        {
            actions.Push(a);
            a.doAction();
            if (clearRedo) redoActions.Clear();
        }
        
        public void undo()
        {
            Action last = actions.Pop();
            last.undoAction();
            redoActions.Push(last);

        }

        public void redo()
        {
            Action last = redoActions.Pop();
            doAction(last, false);
        }


        public Boolean getUndoPossible()
        {
            return actions.Count()> 0;
        }

        public Boolean getRedoPossible()
        {
            return redoActions.Count > 0;
        }


    }
}
