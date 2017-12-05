using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalConstraintProgramming.HelperTools
{
    class OptimizationGoalHelper
    {
        public static string getOptimizationString(List<OptimizationGoal> list)
        {
            if(list.Count == 0) return "\nsolve satisfy;";

            String goal = "\nsolve minimize 0 ";

            foreach(OptimizationGoal g in list)
            {
                goal += "+ " + g.getCode() + " ";
            }

            goal += ";";

            return goal;
        }
    }
}
