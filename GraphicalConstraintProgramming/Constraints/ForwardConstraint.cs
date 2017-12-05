using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalConstraintProgramming.Constraints
{
    class ForwardConstraint : SimpleConstraint
    {

        private static String lastValue = "";

        private String constraints = "";

        public ForwardConstraint(String constraints)
        {
            this.constraints = constraints;
            lastValue = constraints;
        }

        public ForwardConstraint()
        {
            if (lastValue.Trim() == "") throw new Exception("Cannot duplicate Constraint - no previous constraint");
            this.constraints = lastValue;
        }


        public override string GetCode()
        {
            String code = "";
            List<Variable> sortedVars = new List<Variable>(variables);
            sortedVars.Sort();

            using (StringReader reader = new StringReader(constraints))
            {
                string line = string.Empty;
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        
                        String[] implication = line.Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries);

          



                        if (implication.Length == 2)
                        {

                            String occuringVars = line;
                            occuringVars = occuringVars.Replace("(", " ");
                            occuringVars = occuringVars.Replace(")", " ");
                            occuringVars = occuringVars.Replace("->", ",");


                            String[] oV = occuringVars.Split(',');



                            for(int i = 0; i< sortedVars.Count; i++)
                            {

                                String currentline = line;

                                foreach (String s in oV)
                                {
                                    String s_new = s;
                                    s_new = s_new.Replace('<', '=');
                                    s_new = s_new.Replace('>', '=');
                                    s_new = s_new.Replace('!', '=');
                                    s_new = s_new.Trim();


                                    String tmp = s_new.Split('=')[0].Trim();
                                    tmp = tmp.Replace('<', ' ');
                                    tmp = tmp.Replace('>', ' ');
                                    tmp = tmp.Replace('!', ' ');
                                    tmp = tmp.Trim();
                                    currentline = currentline.Replace(tmp, convertVarToName(tmp, sortedVars, i));

                                }

                                currentline = currentline.Replace(",", @" /\ ");




                                code += "\n" + "constraint " + currentline + ";";
                            }

                        }




                    }

                } while (line != null);
            }


            return code;
        }


        public override Color getColor()
        {
            return Color.Honeydew;
        }

        private String convertVarToName(String var, List<Variable> sortedVars, int offset)
        {
            if (var.StartsWith("C"))
            {
                int index = 0;

                if (var.Length != 1)
                {


                    try
                    {

                        index = int.Parse(var.Substring(1));

                    }
                    catch (Exception) { };
                 }

                return sortedVars[(offset + index) % sortedVars.Count].name;



            }

            return "";
        }
    }
}
