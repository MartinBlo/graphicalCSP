using System;
using System.Diagnostics;

namespace GraphicalConstraintProgramming
{
    internal class ORToolsSolver : Solver
    {
        private string instance_path;

        public ORToolsSolver(string instance_path)
        {
            this.instance_path = instance_path;
        }

        public override string solve()
        {
            String flatzinc_path = instance_path.Substring(0, instance_path.Length - 4) + ".fzn";
            String ozn_path = instance_path.Substring(0, instance_path.Length - 4) + ".ozn";

            //long rand_seed = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            //rand_seed = rand_seed % 100000;

            String result;

            MiniZincInterface.ExecCommand(MiniZincInterface.mzn2fzn_path, instance_path);
            result = MiniZincInterface.ExecCommand(MiniZincInterface.or_tools_path, flatzinc_path );

            return MiniZincInterface.ExecCommand(MiniZincInterface.solns2out_path, ozn_path, result);
        }
    }
}