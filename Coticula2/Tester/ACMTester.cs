using Coticula2.Compilers;
using Coticula2.Problem;
using Protex.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coticula2.Tester
{
    class ACMTester: ITester
    {
        private string pathToCompilers;
        private string pathToProblems;

        public ACMTester(string pathToCompilers, string pathToProblems)
        {
            this.pathToCompilers = pathToCompilers;
            this.pathToProblems = pathToProblems;
        }
        public ITestingResult Test(ISolution solution)
        {
            ITestingResult result = new TestingResult();
            result.Id = solution.Id;

            IProblem problem = ProblemManager.CreateProblem(pathToProblems, solution.ProblemId);

            var compilerManager = new CompilerManager(pathToCompilers);
            ICompiler fpc = compilerManager.CreateCompiler("FPC");
            if (!fpc.Compile(solution.SolutionCode))
            {
                result.Verdict = Verdict.CompilationError;
                result.CompilationOutput = fpc.OutputString;
                return result;
            }

            string executableFile = fpc.OutputPath;

            result.Verdict = Verdict.Accepted;
            foreach (var test in problem.Tests)
            {
                var startInfo = WindowsCreator.CreateRunnerStartInfo();
                startInfo.ExecutableFile = fpc.OutputPath;
                startInfo.InputString = test.Input;

                var runner = WindowsCreator.CreateRunner();
                var runnerResult = runner.Run(startInfo);
                result.MaximumWorkingTime = Math.Max(result.MaximumWorkingTime, runnerResult.WorkingTime);
                result.PeakMemoryUsed = Math.Max(result.PeakMemoryUsed, runnerResult.PeakMemoryUsed);
                if (runnerResult.ExitCode > 0)
                {
                    result.Verdict = Verdict.RunTimeError;
                    break;
                }
                if (runnerResult.ExitCode == -1)
                {
                    if (runnerResult.WorkingTime >= startInfo.WorkingTimeLimit)
                    {
                        result.Verdict = Verdict.TimeLimit;
                    }
                    break;
                }
                if (!ValidateOutputTrimming(test.Output, runnerResult.OutputString))
                {
                    result.Verdict = Verdict.WrongAnswer;
                    break;
                }
            }

            return result;
        }

        private static bool ValidateOutputTrimming(string expectedOutput, string realOutput)
        {
            return expectedOutput.Trim() == realOutput.Trim();
        }
    }
}
