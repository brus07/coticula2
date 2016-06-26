using Coticula2.Job;
using Protex;
using System;
using System.Collections.Generic;
using System.IO;

namespace Coticula2.Jobs
{
    /// <summary>
    /// Validate input files in tests.
    /// </summary>
    public class ValidatorProblemJob : IJob
    {
        private readonly IRunner Runner;
        private readonly int ProblemId;
        private string ValidatorCode;
        private Language Language;
        private string WorkingDirectoryPath;

        public ValidatorProblemJob(IRunner runner, int problemId)
        {
            Runner = runner;
            ProblemId = problemId;
        }

        public ValidatorProblemJob(IRunner runner, int problemId, string validatorCode, Language language)
        {
            Runner = runner;
            ProblemId = problemId;
            ValidatorCode = validatorCode;
            Language = language;
        }

        public TestingResult TestingResult { get; set; }

        public void Execute()
        {
            string fullPathToProblem = TestJob.FullPathToProblem(ProblemId);

            if (ValidatorCode == null)
            {
                //need get validator from problem
                var validatorFiles = Directory.GetFiles(fullPathToProblem, "validator*.*");
                if (validatorFiles.Length == 0)
                    throw new Exception(string.Format("Not validator for Problem {0}.", ProblemId));
                if (validatorFiles.Length > 1)
                {
                    //TODO: 
                }
                switch (Path.GetExtension(validatorFiles[0]))
                {
                    case ".cs":
                        Language = Language.CSharp;
                        break;
                    case ".cpp":
                        Language = Language.GPlusPlus;
                        break;
                    case ".pas":
                        Language = Language.Fpc;
                        break;
                    default:
                        throw new Exception("Validator can be only C#, C++ or Pascal.");
                }
                ValidatorCode = File.ReadAllText(Path.Combine(fullPathToProblem, validatorFiles[0]));
            }

            TestingResult = new TestingResult();
            WorkingDirectoryPath = CreateTemporaryDirectory();

            #region Compile
            CompileJob compileJob = new CompileJob(Runner, WorkingDirectoryPath, ValidatorCode, Language);
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

                ValidatorTestJob job = new ValidatorTestJob(Runner, executedFile, ProblemId, testId);
                job.Execute();
                var result = job.TestResult;

                testResults.Add(result);
            }
            TestingResult.TestVerdicts = testResults.ToArray();
            #endregion
        }

        static string CreateTemporaryDirectory()
        {
            if (!Directory.Exists("Temp"))
                Directory.CreateDirectory("Temp");
            string tempDirectory = Path.Combine("Temp", Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }
    }
}
