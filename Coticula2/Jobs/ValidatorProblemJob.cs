using Coticula2.Job;
using Coticula2.Models;
using Protex;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Coticula2.Jobs
{
    /// <summary>
    /// Validate input files in tests.
    /// </summary>
    public class ValidatorProblemJob : IJob
    {
        private const string ValidatorFilePattern = "validator*.*";

        private readonly IRunner Runner;
        private readonly int ProblemId;
        private string ValidatorCode;
        private ProgrammingLanguage Language;
        private string WorkingDirectoryPath;

        private readonly string fullPathToProblem;

        public ValidatorProblemJob(IRunner runner, int problemId)
        {
            Runner = runner;
            ProblemId = problemId;
            fullPathToProblem = TestJob.FullPathToProblem(ProblemId);
        }

        public ValidatorProblemJob(IRunner runner, int problemId, string validatorCode, ProgrammingLanguage language) : this(runner, problemId)
        {
            ValidatorCode = validatorCode;
            Language = language;
        }

        public TestingResult TestingResult { get; set; }

        public bool HasValidator
        {
            get
            {
                if (!Directory.Exists(fullPathToProblem))
                    throw new FileNotFoundException("Can't find problem folder.", fullPathToProblem);
                var validatorFiles = Directory.GetFiles(fullPathToProblem, ValidatorFilePattern);
                if (validatorFiles.Length == 0)
                    return false;
                return true;
            }
        }

        public void Execute()
        {
            Console.WriteLine("Validating tests for problem {0}...", ProblemId);

            if (ValidatorCode == null)
            {
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
            }

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

            #region Test solution
            TestingResult testingResult = new TestingResult();
            string executedFile = Path.Combine(WorkingDirectoryPath, "source.exe");

            var testDirectories = Directory.GetDirectories(fullPathToProblem, "test*");
            List<TestResult> testResults = new List<TestResult>();
            foreach (var testDirectory in testDirectories)
            {
                Regex regex = new Regex(@"(\d+)(?!.*\d)");
                Match match = regex.Match(testDirectory);
                int testId = int.Parse(match.Value);
                Console.WriteLine("Testing {0}/{1} ...", testId, testDirectories.Length);

                ValidatorTestJob job = new ValidatorTestJob(Runner, executedFile, ProblemId, testId);
                job.Execute();
                var result = job.TestResult;

                testResults.Add(result);
            }
            TestingResult.TestVerdicts = testResults.ToArray();
            #endregion
        }
    }
}
