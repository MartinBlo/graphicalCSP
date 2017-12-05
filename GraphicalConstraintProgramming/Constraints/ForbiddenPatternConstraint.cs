using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalConstraintProgramming.Constraints
{
    
    class ForbiddenPatternConstraint : SimpleConstraint
    {

        private static String lastValue = "";

        private String forbiddenPatterns = "";

        public ForbiddenPatternConstraint(String forbiddenPatterns)
        {
            this.forbiddenPatterns = forbiddenPatterns;
            lastValue = forbiddenPatterns;
        }

        public ForbiddenPatternConstraint()
        {
            if (lastValue.Trim() == "") throw new Exception("Cannot duplicate Constraint - no previous constraint");
            this.forbiddenPatterns = lastValue;
        }


        public override string GetCode()
        {
            String code = "";
            List<Variable> sortedVars = new List<Variable>(variables);
            sortedVars.Sort();

            using (StringReader reader = new StringReader(forbiddenPatterns))
            {
                string line = string.Empty;
                do
                {
                    line = reader.ReadLine();
                    if (line != null && line.Trim().Length > 0)
                    {
                        line = line.Trim();
                        String[] pattern = line.Split(' ');
                        if(pattern.Length > 0)
                        {
                            for(int i = 0; i< sortedVars.Count; i++)
                            {
                                //Variable[] forbiddenVars = new Variable[pattern.Length];

                                String currentConstraint = "constraint ";

                                Boolean first = true;

                                for(int k = 0; k < pattern.Length; k++)
                                {

                                    if(!first)
                                    {
                                        currentConstraint += @" \/ ";
                                        
                                    }

                                    first = false;

                                    //forbiddenVars[k] = sortedVars[(i + k) % sortedVars.Count];
                                    Variable currentVar = sortedVars[(i + k) % sortedVars.Count];

                                    if (pattern[k].StartsWith("~"))
                                    {
                                        currentConstraint += currentVar.name + " = " + pattern[k].Substring(1);

                                    }
                                    else if (pattern[k].StartsWith(">"))
                                    {
                                        currentConstraint += currentVar.name + " <= " + pattern[k].Substring(1);
                                    }
                                    else if (pattern[k].StartsWith("<"))
                                    {
                                        currentConstraint += currentVar.name + " >= " + pattern[k].Substring(1);
                                    }
                                    else
                                    {
                                        currentConstraint += currentVar.name + " != " + pattern[k];

                                    }


                                }

                                currentConstraint += ";";

                                code += currentConstraint + "\n";

                            }
                        }

                        
                    }

                } while (line != null);
            }


            return code;
        }


        public override Color getColor()
        {
            return Color.DarkOliveGreen;
        }
    }
}
