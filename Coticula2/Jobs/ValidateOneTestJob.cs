using System;
using Coticula2.Job;
using Protex;
using System.IO;
using Coticula2.Models;
using System.Collections.Generic;

namespace Coticula2.Jobs
{
    public class ValidateOneTestJob : IJob
    {
        private const string ValidatorFilePattern = "validator*.*";

        private string fullPathToProblem;
        private string ValidatorCode;
        private ProgrammingLanguage Language;
        private readonly int ProblemId;
        private readonly int TestId;
        private IRunner Runner { get; set; }
        private string WorkingDirectoryPath;

        public TestingResult TestingResult { get; set; }

        public ValidateOneTestJob(IRunner runner, int problemId, int testId)
        {
            Runner = runner;
            ProblemId = problemId;
            TestId = testId;
            fullPathToProblem = TestJob.FullPathToProblem(ProblemId);
        }

        public void Execute()
        {
            Console.WriteLine("Validating test {0} for problem {1}...", TestId, ProblemId);

            //need get validator from problem
            var validatorFiles = Directory.GetFiles(fullPathToProblem, ValidatorFilePattern);
            if (validatorFiles.Length == 0)
                throw new Exception(string.Format("Not validator for Problem {0}.", ProblemId));
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
                    throw new Exception("Validator can be only C#, C++ or Pascal.");
            }
            ValidatorCode = File.ReadAllText(Path.Combine(fullPathToProblem, validatorFiles[0]));

            TestingResult = new TestingResult();
            WorkingDirectoryPath = TestSolutionJob.CreateTemporaryDirectory();

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

            #region Validate test
            string executedFile = Path.Combine(WorkingDirectoryPath, "source.exe");
            ValidatorTestJob job = new ValidatorTestJob(Runner, executedFile, ProblemId, TestId);
            job.Execute();

            List<TestResult> testResults = new List<TestResult>();
            testResults.Add(job.TestResult);

            TestingResult testingResult = new TestingResult();
            TestingResult.TestVerdicts = testResults.ToArray();
            #endregion
        }
    }
}
