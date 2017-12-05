using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalConstraintProgramming
{
    class CustomConstraint : SimpleConstraint
    {
        private String constraint_code;

        public CustomConstraint(String constraint_code)
        {
            this.constraint_code = constraint_code;

            base.variables = new List<Variable>();
        }

        public override string GetCode()
        {


            return constraint_code;
        }
    }
}
