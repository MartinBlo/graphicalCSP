using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicalConstraintProgramming.HelperTools
{
    public class SaveLoadTools
    {
        public static Boolean save(CPInstance instance)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Constraint Programming Instance files (*.cpi)|*.cpi";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(saveFileDialog1.FileName, sv(instance));
            }

            return false;
        }

        private static String sv(CPInstance instance)
        {
            String o = "CONSTRAINT PROGRAMMING INSTANCE\n";

            o += "--VARIABLES--\n";

            foreach(Variable v in instance.variables.Values) {

                o += v.getSerialization();
                o += "\n";
             }

            o += "--CONSTRAINTS--\n";



            foreach (Constraint c in instance.constraints)
            {
                o += c.getSerialization();
                o += "\n";

            }


            o += "--OPTIMIZATION GOALS--\n";

            foreach (OptimizationGoal g in instance.optimizationGoals)
            {
                o += g.getSerialization();
                o += "\n";

            }



            return o;
        }



        public static CPInstance load(DataGridView grid)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                String[] data = System.IO.File.ReadAllLines(file.FileName);
                int index = 2;


                CPInstance i = new CPInstance(200,200);


                // Vars

                while(index < data.Length && !data[index].StartsWith("--CONSTRAINTS--")){

                    i.addVariable(Variable.loadVar(data[index], grid));


                    index++;
                }

                index++;



                // Constraints

                while (index < data.Length && !data[index].StartsWith("--OPTIMIZATION GOALS--"))
                {

                    try
                    {
                        String[] values = data[index].Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        Type elementType = Type.GetType(values[0]);
                        MethodInfo f = elementType.GetMethod("fromSerialization");
                        Constraint c = (Constraint)f.Invoke(null, new Object[] { data[index], i });

                        if (c != null) i.addConstraint(c);
                    }
                    catch(Exception e)
                    {
                        throw e;
                    }



                    




                    index++;
                }

                index++;




                // Opt Goals

                while (index < data.Length)
                {

                    Variable v = OptimizationGoal.loadVar(i, data[index]);
                    String value = OptimizationGoal.loadValue(data[index]);

                    i.setOptTo(v, value);


                    index++;
                }

                return i;
            }

            return null;

        }

    }
}
