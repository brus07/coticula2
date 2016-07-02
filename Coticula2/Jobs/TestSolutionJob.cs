using Coticula2.Job;
using Protex;
using System;
using System.Collections.Generic;
using System.IO;

namespace Coticula2.Jobs
{
    public class TestSolutionJob : IJob
    {
        private readonly IRunner Runner;
        private readonly int ProblemId;
        private readonly string SourceCode;
        private readonly Language Language;
        private string WorkingDirectoryPath;

        public TestSolutionJob(IRunner runner, int problemId, string sourceCode, Language language)
        {
            Runner = runner;
            ProblemId = problemId;
            SourceCode = sourceCode;
            Language = language;
        }

        public TestingResult TestingResult { get; set; }

        public void Execute()
        {
            TestingResult = new TestingResult();

            try
            {

                #region Test validating
                ValidatorProblemJob testsValidatorJob = new ValidatorProblemJob(Runner, ProblemId);
                if (testsValidatorJob.HasValidator)
                {
                    testsValidatorJob.Execute();
                    var validatorResult = testsValidatorJob.TestingResult;
                    if (validatorResult.CompilationVerdict == Verdict.Accepted)
                    {
                        bool correctTests = true;
                        for (int i = 0; i < validatorResult.TestVerdicts.Length; i++)
                        {
                            if (validatorResult.TestVerdicts[i].Verdict != Verdict.Accepted)
                            {
                                correctTests = false;
                                break;
                            }
                        }

                        if (!correctTests)
                        {
                            List<TestResult> testsResults = new List<TestResult>();
                            testsResults.Add(new TestResult() { TestId = 1, Verdict = Verdict.InternalError });
                            TestingResult.TestVerdicts = testsResults.ToArray();
                            return;
                        }
                    }
                }
                #endregion

                #region Run Main Solution
                RunMainSolutionJob runMainSolutionJob = new RunMainSolutionJob(Runner, ProblemId);
                if (runMainSolutionJob.HasMainSolution)
                {
                    runMainSolutionJob.Execute();
                    var runMainSolutionResult = runMainSolutionJob.TestingResult;
                    if (runMainSolutionResult.CompilationVerdict == Verdict.Accepted)
                    {
                        bool correctTests = true;
                        for (int i = 0; i < runMainSolutionResult.TestVerdicts.Length; i++)
                        {
                            if (runMainSolutionResult.TestVerdicts[i].Verdict != Verdict.Accepted)
                            {
                                correctTests = false;
                                break;
                            }
                        }

                        if (!correctTests)
                        {
                            List<TestResult> testsResults = new List<TestResult>();
                            testsResults.Add(new TestResult() { TestId = 1, Verdict = Verdict.InternalError });
                            TestingResult.TestVerdicts = testsResults.ToArray();
                            return;
                        }
                    }
                }
                #endregion

                WorkingDirectoryPath = CreateTemporaryDirectory();

                #region Compile
                CompileJob compileJob = new CompileJob(Runner, WorkingDirectoryPath, SourceCode, Language);
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

                string fullPathToProblem = TestJob.FullPathToProblem(ProblemId);
                var testDirectories = Directory.GetDirectories(fullPathToProblem, "test*");
                List<TestResult> testResults = new List<TestResult>();
                int testId = 0;
                foreach (var testDirectory in testDirectories)
                {
                    testId++;
                    Console.WriteLine("Testing {0}/{1} ...", testId, testDirectories.Length);
                
                    TestJob job = new TestJob(Runner, executedFile, ProblemId, testId);
                    job.Execute();
                    var result = job.TestResult;

                    testResults.Add(result);
                }
                TestingResult.TestVerdicts = testResults.ToArray();
                #endregion

            }
            catch (FileNotFoundException ex)
            {
                TestingResult.CompilationVerdict = Verdict.InternalError;
                Console.WriteLine("Verdict: {0}.", TestingResult.CompilationVerdict);
                Console.WriteLine(ex);
            }
        }

        public static string CreateTemporaryDirectory()
        {
            if (!Directory.Exists("Temp"))
                Directory.CreateDirectory("Temp");
            string tempDirectory = Path.Combine("Temp", Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }
    }
}
