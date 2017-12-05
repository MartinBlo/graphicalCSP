using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Management;

namespace GraphicalConstraintProgramming
{

    public enum SolverName
    {
        minizinc,
        gecode,
        or_tools,
        all,
        chuffed
    }

    public class MiniZincInterface
    {

        public static String mzinc_path = "";
        public static String mzn2fzn_path = "";
        public static String fzn_gecode_path = "";
        public static String fzn_chuffed_path = "";
        public static String mzinc_dir = "";
        public static String solns2out_path = "";
        public static String or_tools_path ="";

        public static String result;

        private static List<Process> processList;

        public static Boolean gotResult;

        public MiniZincInterface()
        {

            if (File.Exists(@"C:\Program Files\MiniZinc IDE (bundled)\minizinc.exe")){
                mzinc_path = @"C:\Program Files\MiniZinc IDE (bundled)\minizinc.exe";
                mzinc_dir = @"C:\Program Files\MiniZinc IDE (bundled)\";
                mzn2fzn_path = @"C:\Program Files\MiniZinc IDE (bundled)\mzn2fzn.exe";
                fzn_gecode_path = @"C:\Program Files\MiniZinc IDE (bundled)\fzn-gecode.exe";
                fzn_gecode_path = @"C:\Program Files\MiniZinc IDE (bundled)\fzn-gecode.exe";
                solns2out_path = @"C:\Program Files\MiniZinc IDE (bundled)\solns2out.exe";
                or_tools_path = @"C:\Program Files\MiniZinc IDE (bundled)\fzn-or-tools.exe";
                fzn_chuffed_path = @"C:\Program Files\MiniZinc IDE (bundled)\fzn-chuffed.exe";



            }
            else
            {

                throw new FileNotFoundException("Minizinc not found on this PC");

            }


        }

        internal string solve(CPInstance instance, SolverName solver)
        {
            String instance_path = instance.writeFile();

            result = "";

            gotResult = false;

            List<Solver> solvers = new List<Solver>();
            List<Thread> solverThreads = new List<Thread>();
            processList = new List<Process>();

            if (solver == SolverName.minizinc || solver == SolverName.all)
            {
                solvers.Add(new FlatZincSolver(instance_path));
            }
            if(solver == SolverName.gecode || solver == SolverName.all)
            {
                solvers.Add(new GecodeSolver(instance_path));
            }
            if(solver == SolverName.or_tools || solver == SolverName.all)
            {
                solvers.Add(new ORToolsSolver(instance_path));
            }
            if (solver == SolverName.chuffed || solver == SolverName.all)
            {
                solvers.Add(new ChuffedSolver(instance_path));
            }




            foreach (Solver s in solvers)
            {
                solverThreads.Add(new Thread(() =>
                {
                    try
                    {
                        result = s.solve();
                        Console.Out.WriteLine("Solver sucessfully finished: " + s);
                        gotResult = true;
                    }
                    catch(Exception e)
                    {
                        Console.Out.WriteLine("Solver crashed: " + s);
                    }
                }));
            }

            WaitingScreen wait = new WaitingScreen(solverThreads);

            wait.ShowDialog();

            var list = new List<Process>(processList);

            foreach (Process process in list)
            {
                try
                {
                    if (!process.HasExited)
                    {
                        Kill(process.Id.ToString());
                    }
                }
                catch(InvalidOperationException e)
                {
                    // do nothing, process already gone
                }
            }


            if (result != "")
            {

                if (result.Contains("UNSATISFIABLE"))
                {
                    instance.unsatisfiable = true;
                    Console.WriteLine("Instance unsatisfiable!");
                }

                return result;
            }
            else
            {
                return "";
            }
        }

        public static string ExecCommand(string filename, string arguments, string input)
        {
            Process process = new Process();
            processList.Add(process);
            ProcessStartInfo psi = new ProcessStartInfo(filename);
            psi.WorkingDirectory = mzinc_dir;
            psi.Arguments = arguments;
            // psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.RedirectStandardInput = true;
            psi.UseShellExecute = false;
            
            process.StartInfo = psi;

            StringBuilder output = new StringBuilder();
            process.OutputDataReceived += (sender, e) => { output.AppendLine(e.Data); };
            process.ErrorDataReceived += (sender, e) => { output.AppendLine(e.Data); };

            // run the process
            process.Start();
            

            using (StringReader reader = new StringReader(input))
            {
                string line = string.Empty;
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        process.StandardInput.WriteLine(line);
                    }

                } while (line != null);
            }



            // start reading output to events
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // Waiting Screen
            

            process.StandardInput.Close();


            // wait for process to exit
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                output.AppendLine("Command " + psi.FileName + " returned exit code " + process.ExitCode);

                throw new Exception("Process not sucessfully executed");
            }

            Console.WriteLine(output.ToString());
            return output.ToString();
        }

        public static string ExecCommand(string filename, string arguments)
        {
            Process process = new Process();
            processList.Add(process);
            ProcessStartInfo psi = new ProcessStartInfo(filename);
            psi.WorkingDirectory = mzinc_dir;
            psi.Arguments = arguments;
           // psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;
            process.StartInfo = psi;

            StringBuilder output = new StringBuilder();
            process.OutputDataReceived += (sender, e) => { output.AppendLine(e.Data); };
            process.ErrorDataReceived += (sender, e) => { output.AppendLine(e.Data); };

            // run the process
            process.Start();

            // start reading output to events
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

         
            // wait for process to exit
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                output.AppendLine("Command " + psi.FileName + " returned exit code " + process.ExitCode);

                throw new Exception("Process not sucessfully executed");
            }

            Console.WriteLine(output.ToString());
            return output.ToString();
        }

        public static void Kill(String PID)
        {
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo("taskkill", "/F /T /PID " + PID)
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WorkingDirectory = System.AppDomain.CurrentDomain.BaseDirectory,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                Process.Start(processStartInfo);
            }
            catch { }
        }

    }

    
}
