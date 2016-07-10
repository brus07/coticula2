using Coticula2.Job;
using Protex;
using Protex.Windows;
using System;
using System.IO;

namespace Coticula2.Jobs
{
    internal class TestJob : IJob
    {
        private readonly IRunner Runner;
        private readonly string ExecuteFilePath;
        private readonly int ProblemId;
        private readonly int TestId;

        public TestJob(IRunner runner, string executeFilePath, int problemId, int testId)
        {
            Runner = runner;
            ExecuteFilePath = executeFilePath;
            ProblemId = problemId;
            TestId = testId;
        }

        public TestResult TestResult { get; private set; }

        public void Execute()
        {
            string fullPathToProblem = FullPathToProblem(ProblemId);
            var testDirectories = Directory.GetDirectories(fullPathToProblem, string.Format("test{0}", TestId));
            if (testDirectories.Length != 1)
                throw new InvalidOperationException();

            var testDirectory = testDirectories[0];

            var inputFiles = Directory.GetFiles(testDirectory, "in.txt");
            if (inputFiles.Length != 1)
            {
                throw new FileNotFoundException(Path.Combine(testDirectory, "in.txt"));
            }
            string inputFile = inputFiles[0];

            var outputFiles = Directory.GetFiles(testDirectory, "out.txt");
            if (outputFiles.Length != 1)
            {
                throw new FileNotFoundException(Path.Combine(testDirectory, "out.txt"));
            }
            string outputFile = outputFiles[0];

            var testStartInfo = Creator.CreateRunnerStartInfo();
            testStartInfo.ExecutableFile = ExecuteFilePath;
            testStartInfo.InputString = File.ReadAllText(inputFile);

            var testExecutedResult = Runner.Run(testStartInfo);
            Verdict currentVerdict = Verdict.Accepted;
            if (testExecutedResult.ExitCode != 0)
                currentVerdict = Verdict.RunTimeError;

            //TODO: check for limits
            if (testExecutedResult.ExitCode == -1)
            {
                if (testExecutedResult.WorkingTime >= testStartInfo.WorkingTimeLimit)
                    currentVerdict = Verdict.TimeLimit;
                if (testExecutedResult.PeakMemoryUsed >= testStartInfo.MemoryLimit)
                    currentVerdict = Verdict.MemoryLimit;
            }

            //compare outputs
            if (currentVerdict == Verdict.Accepted)
            {
                string expectedOutput = File.ReadAllText(outputFile);

                CompareStringOutputsJob job = new CompareStringOutputsJob(expectedOutput, testExecutedResult.OutputString);
                job.Execute();
                if (!job.EqualOutputs)
                    currentVerdict = Verdict.WrongAnswer;
            }

            TestResult = new TestResult() { TestId = TestId, Verdict = currentVerdict, WorkingTime = testExecutedResult.WorkingTime, PeakMemoryUsed = testExecutedResult.PeakMemoryUsed };
            Console.WriteLine("Verdict: {0}; Time used: {1}ms; Memory used: {2}KiB;", currentVerdict, testExecutedResult.WorkingTime, testExecutedResult.PeakMemoryUsed);
        }

        public static string FullPathToProblem(int id)
        {
            string res = Path.Combine("Problems", "Problem" + id);
            string toProblemsFolder = Path.Combine("..", "..", "..", "TestData");
            res = Path.Combine(toProblemsFolder, res);
            Directory.Exists(res);
            return res;
        }
    }
}
