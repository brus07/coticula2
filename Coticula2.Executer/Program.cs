using CommandLine;
using Protex;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coticula2.Executer
{
    class Program
    {

        static void Main(string[] args)
        {
            CommandLineOptions commandLineOptions = null;

            var arguments = Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(options => commandLineOptions = options);

            if (arguments is NotParsed<CommandLineOptions>)
            {
                return;
            }

            if (arguments is Parsed<CommandLineOptions>)
            {
                IRunner runner = Protex.Windows.Creator.CreateRunner();
                Tester tester = new Tester(runner);
                Language language = Language.CSharp;
                string solution = File.ReadAllText(commandLineOptions.SolutionFile);
                var testingResult = tester.Test(commandLineOptions.TaskId, solution, language);


                Console.WriteLine("Compilation Verdict: {0}", testingResult.CompilationVerdict);
                Console.WriteLine("Compilation Output: {0}", testingResult.CompilationOutput);
                if (testingResult.TestVerdicts != null)
                    for (int i = 0; i < testingResult.TestVerdicts.Length; i++)
                        Console.WriteLine("Verdict {0}: {1}", i, testingResult.TestVerdicts[i]);

                //Console.WriteLine("Working time : {0}", result.WorkingTime);
                //Console.WriteLine("Peak memory used: {0}", result.PeakMemoryUsed);
                //Console.WriteLine("Exit code: {0}", result.ExitCode);
            }
        }
    }
}
