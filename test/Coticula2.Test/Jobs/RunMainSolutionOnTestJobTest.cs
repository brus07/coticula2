using Coticula2.Jobs;
using Coticula2.Models;
using Coticula2.Test.Mocks;
using NUnit.Framework;
using System.IO;

namespace Coticula2.Test.Jobs
{
    [TestFixture]
    public class RunMainSolutionOnTestJobTest
    {
        string PathToTestData = Path.Combine("..", "..", "..", "TestData", "Executables");

        [TestCase(8901, 1)]
        [TestCase(8901, 2)]
        [TestCase(8901, 3)]
        [TestCase(8901, 4)]
        [TestCase(8902, 1)]
        [TestCase(8902, 2)]
        [TestCase(8902, 5)]
        [TestCase(8902, 6)]
        public void CorrectRunMainSolutionOnTestJob(int problemId, int testId)
        {
            var fullPathToProblem = TestJob.FullPathToProblem(problemId);
            var testDirectory = Directory.GetDirectories(fullPathToProblem, string.Format("test{0}", testId))[0];
            var outputFiles = Directory.GetFiles(testDirectory, "out.txt");
            foreach (var outputfile in outputFiles)
            {
                if (File.Exists(outputfile))
                    File.Delete(outputfile);
            }

            string executableFilePath = Path.Combine(PathToTestData, "Swap.exe");
            RunMainSolutionOnTestJob job = new RunMainSolutionOnTestJob(new RunnerMock(), executableFilePath, problemId, testId);
            job.Execute();
            var result = job.TestResult;
            Assert.That(result.Verdict, Is.EqualTo(Verdict.Accepted));
            string outputFile = Path.Combine(testDirectory, "out.txt");
            Assert.IsTrue(File.Exists(outputFile));
        }

        [TestCase(8902, 3)]
        [TestCase(8902, 4)]
        public void IncorrectRunMainSolutionOnTestJob(int problemId, int testId)
        {
            var fullPathToProblem = TestJob.FullPathToProblem(problemId);
            var testDirectory = Directory.GetDirectories(fullPathToProblem, string.Format("test{0}", testId))[0];
            var outputFiles = Directory.GetFiles(testDirectory, "out.txt");
            foreach (var outputfile in outputFiles)
            {
                if (File.Exists(outputfile))
                    File.Delete(outputfile);
            }

            string executableFilePath = Path.Combine(PathToTestData, "Swap.exe");
            RunMainSolutionOnTestJob job = new RunMainSolutionOnTestJob(new RunnerMock(), executableFilePath, problemId, testId);
            job.Execute();
            var result = job.TestResult;
            Assert.That(result.Verdict, Is.Not.EqualTo(Verdict.Accepted));
            string outputFile = Path.Combine(testDirectory, "out.txt");
            Assert.IsFalse(File.Exists(outputFile));
        }
    }
}
