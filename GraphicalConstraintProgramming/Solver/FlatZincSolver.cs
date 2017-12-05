using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalConstraintProgramming
{
    class FlatZincSolver : Solver
    {
        private string instance_path;

        public FlatZincSolver(string instance_path)
        {
            this.instance_path = instance_path;
        }

        public override string solve()
        {
            return MiniZincInterface.ExecCommand(MiniZincInterface.mzinc_path, instance_path);
        }
    }
}
