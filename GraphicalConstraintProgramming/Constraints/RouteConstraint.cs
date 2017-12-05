using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalConstraintProgramming.Constraints
{
    class RouteConstraint : SimpleConstraint
    {
        public string lengthVar;
        public int[][] distances;
        public List<int> domain;

        public static bool lastWasVar = false;
        public static string lastLengthVar;
        public static int[][] lastDistances;
        public static List<int> lastDomain;
        
        public RouteConstraint()
        {
            lengthVar = lastLengthVar;
            distances = lastDistances;
            domain = lastDomain;
        }


        public RouteConstraint(string lengthVar, int[][] distances, List<int> domain, CPInstance i)
      
        {
            this.lengthVar = lengthVar;
            this.distances = distances;
            this.domain = domain;


            lastLengthVar = lengthVar;
            lastDistances = distances;
            lastDomain = domain;

            Variable output = null;

            if (i.variableExists(lengthVar.Trim()))
            {
                lastWasVar = true;
                output = i.getVarByName(lengthVar.Trim());
            }
            else
            {
                lastWasVar = false;
            }

            base.variables = new List<Variable>();

            if (output != null)
            {
                variables.Add(output);
                base.involvedVariables().Add(output);
                output.usedInConstraint(this);
            }
        }



        public override string GetCode()
        {
            String distArray = "array[int,int] of int: " + getDistanceArrayName() + " = array2d(" +
                domain[0] + ".." + domain[domain.Count - 1] + ", " + domain[0] + ".." + domain[domain.Count - 1] + ", [";

            for(int i = domain[0]; i<= domain[domain.Count-1]; i++)
            {
                distArray += "|";
                
                bool first = true;

                for (int o = domain[0]; o <= domain[domain.Count - 1]; o++)
                {
                    if (first)
                    {
                        first = false;
                        distArray += " "; 
                    }
                    else {
                        distArray += ", ";
                    }

                    if(domain.Contains(o) && domain.Contains(i))
                    {
                        distArray += distances[domain.IndexOf(o)][domain.IndexOf(i)];
                    }
                    else
                    {
                        distArray += 0;
                    }
                }
            }
            distArray += "|]);";





            List<Variable> sortedVars = new List<Variable>(variables);


            Variable output = null;

            if (lastWasVar)
            {
                output = sortedVars[0];
                sortedVars.Remove(output);
            }

            sortedVars.Sort();

            string constraint = "constraint (";

            bool first_var = true;

            for(int index = 0; index < sortedVars.Count-1; index++)
            {
                if (first_var)
                {
                    constraint += getDistanceArrayName() + "[" + (sortedVars[index].name) + " , " + (sortedVars[index + 1].name) + "]";
                    first_var = false;
                }
                else constraint += " + " + getDistanceArrayName() + "[" + (sortedVars[index].name) + " , " + (sortedVars[index + 1].name) + "]";
            }

            if(output != null) constraint += ") = " + output.name + ";";
            else constraint += ") = " + lengthVar + ";";



            return distArray  + "\n" + constraint;
        }

        public override Color getColor()
        {
            return Color.DarkSlateGray;
        }

        private String getDistanceArrayName()
        {
            return "d_" + this.GetHashCode();
        }


    }
}
