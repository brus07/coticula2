using CommandLine;
using Coticula2.Jobs;
using Coticula2.Models;
using Protex;
using System;
using System.IO;

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
                ProgrammingLanguage language = ProgrammingLanguage.CSharp;
                string solution = File.ReadAllText(commandLineOptions.SolutionFile);
                IRunner runner = Protex.Windows.Creator.CreateRunner();
                TestSolutionJob job = new TestSolutionJob(runner, commandLineOptions.TaskId, solution, language);
                job.Execute();
                var testingResult = job.TestingResult;


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
