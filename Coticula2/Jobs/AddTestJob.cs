using System;
using Coticula2.Job;
using Protex;
using System.IO;
using Coticula2.Models;

namespace Coticula2.Jobs
{
    public class AddTestJob : IJob
    {
        private readonly IRunner Runner;
        private readonly int ProblemId;
        private readonly string TestContent;
        private readonly int TestId;

        private readonly string fullPathToProblem;

        public TestingResult TestingResult { get; private set; }

        public AddTestJob(IRunner runner, int problemId, int testId, string testContent)
        {
            Runner = runner;
            ProblemId = problemId;
            TestId = testId;
            TestContent = testContent;
            fullPathToProblem = TestJob.FullPathToProblem(ProblemId);
        }

        public void Execute()
        {
            if (!Directory.Exists(fullPathToProblem))
                throw new FileNotFoundException("Can't find problem folder.", fullPathToProblem);
            var testDirectories = Directory.GetDirectories(fullPathToProblem, string.Format("test{0}", TestId));
            if (testDirectories.Length == 0)
            {
                Directory.CreateDirectory(Path.Combine(fullPathToProblem, string.Format("test{0}", TestId)));
                testDirectories = Directory.GetDirectories(fullPathToProblem, string.Format("test{0}", TestId));
            }
            if (testDirectories.Length != 1)
                throw new InvalidOperationException();

            var testDirectory = testDirectories[0];

            string testFilePath = Path.Combine(testDirectory, "in.txt");
            File.WriteAllText(testFilePath, TestContent);

            ValidateOneTestJob job = new ValidateOneTestJob(Runner, ProblemId, TestId);
            job.Execute();
            TestingResult = job.TestingResult;
            if (!(TestingResult.CompilationVerdict == Verdict.Accepted && TestingResult.TestVerdicts != null && TestingResult.TestVerdicts.Length == 1 && TestingResult.TestVerdicts[0].Verdict == Verdict.Accepted))
            {
                foreach (FileInfo file in (new DirectoryInfo(testDirectory)).GetFiles())
                {
                    file.Delete();
                }
                Directory.Delete(testDirectory);
            }
        }
    }
}
