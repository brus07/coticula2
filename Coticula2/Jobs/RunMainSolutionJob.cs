using Coticula2.Job;
using Coticula2.Models;
using Protex;
using System;
using System.Collections.Generic;
using System.IO;

namespace Coticula2.Jobs
{
    public class RunMainSolutionJob : IJob
    {
        private const string SolutionFilePattern = "solution*.*";

        private readonly IRunner Runner;
        private readonly int ProblemId;
        private string SolutionCode;
        private ProgrammingLanguage Language;
        private string WorkingDirectoryPath;

        private readonly string fullPathToProblem;

        public TestingResult TestingResult { get; set; }

        public RunMainSolutionJob(IRunner runner, int problemId)
        {
            Runner = runner;
            ProblemId = problemId;
            fullPathToProblem = TestJob.FullPathToProblem(ProblemId);
        }

        public RunMainSolutionJob(IRunner runner, int problemId, string solutionCode, ProgrammingLanguage language) : this(runner, problemId)
        {
            SolutionCode = solutionCode;
            Language = language;
        }

        public bool HasMainSolution
        {
            get
            {
                var validatorFiles = Directory.GetFiles(fullPathToProblem, SolutionFilePattern);
                if (validatorFiles.Length == 0)
                    return false;
                return true;
            }
        }

        public void Execute()
        {
            Console.WriteLine("Generating outputs for problem {0}...", ProblemId);

            if (SolutionCode == null)
            {
                //need get validator from problem
                var validatorFiles = Directory.GetFiles(fullPathToProblem, SolutionFilePattern);
                if (validatorFiles.Length == 0)
                    throw new Exception(string.Format("No solution for Problem {0}.", ProblemId));
                if (validatorFiles.Length > 1)
                {
                    //TODO: 
                }
                switch (Path.GetExtension(validatorFiles[0]))
                {
                    case ".cs":
                        Language = ProgrammingLanguage.CSharp;
                        break;
                    case ".cpp":
                        Language = ProgrammingLanguage.GPlusPlus;
                        break;
                    case ".pas":
                        Language = ProgrammingLanguage.Fpc;
                        break;
                    default:
                        throw new Exception("Solution can be only C#, C++ or Pascal.");
                }
                SolutionCode = File.ReadAllText(Path.Combine(fullPathToProblem, validatorFiles[0]));
            }

            TestingResult = new TestingResult();
            WorkingDirectoryPath = TestSolutionJob.CreateTemporaryDirectory();

            #region Compile
            CompileJob compileJob = new CompileJob(Runner, WorkingDirectoryPath, SolutionCode, Language);
            compileJob.Execute();

            var executedResult = compileJob.TestExecutedResult;
            TestingResult.CompilationOutput = string.Concat(executedResult.ErrorOutputString, executedResult.OutputString);
            if (executedResult.ExitCode != 0)
            {
                TestingResult.CompilationVerdict = Verdict.CopilationError;
                Console.WriteLine("Verdict: {0}.", TestingResult.CompilationVerdict);
                //TODO: Return compile error result
                return;
            }
            TestingResult.CompilationVerdict = Verdict.Accepted;
            Console.WriteLine("Verdict: {0}.", TestingResult.CompilationVerdict);
            #endregion

            #region Test solution
            TestingResult testingResult = new TestingResult();
            string executedFile = Path.Combine(WorkingDirectoryPath, "source.exe");

            var testDirectories = Directory.GetDirectories(fullPathToProblem, "test*");
            List<TestResult> testResults = new List<TestResult>();
            int testId = 0;
            foreach (var testDirectory in testDirectories)
            {
                testId++;
                Console.WriteLine("Testing {0}/{1} ...", testId, testDirectories.Length);

                RunMainSolutionOnTestJob job = new RunMainSolutionOnTestJob(Runner, executedFile, ProblemId, testId);
                job.Execute();
                var result = job.TestResult;

                testResults.Add(result);
            }
            TestingResult.TestVerdicts = testResults.ToArray();
            #endregion
        }
    }
}
