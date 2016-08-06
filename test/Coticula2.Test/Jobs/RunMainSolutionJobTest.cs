using Coticula2.Jobs;
using Coticula2.Test.Mocks;
using NUnit.Framework;
using System.IO;
using Coticula2.Models;

namespace Coticula2.Test.Jobs
{
    [TestFixture]
    public class RunMainSolutionJobTest
    {
        const string SolutionForSwapProblem = @"using System;

public class Swap
{
    private static void Main()
    {
        string[] tokens = Console.ReadLine().Split();
        Console.WriteLine(""{0} {1}"", int.Parse(tokens[1]), int.Parse(tokens[0]));
    }
}";

        [TestCase(1)]
        [TestCase(8901)]
        public void TestHasSolution(int problemId)
        {
            RunMainSolutionJob job = new RunMainSolutionJob(new RunnerMock(), problemId);
            Assert.IsTrue(job.HasMainSolution);
        }

        [Test]
        public void TestNoSolution()
        {
            RunMainSolutionJob job = new RunMainSolutionJob(new RunnerMock(), 8902);
            Assert.IsFalse(job.HasMainSolution);
        }

        [Test]
        public void MainTestMainSolution()
        {
            const int problemId = 8901;

            RemoveOutputFiles(problemId);

            RunMainSolutionJob job = new RunMainSolutionJob(new RunnerMock(), problemId, SolutionForSwapProblem, ProgrammingLanguage.CSharp);
            job.Execute();
            var result = job.TestingResult;

            Assert.AreEqual(Verdict.Accepted, result.CompilationVerdict);
            Assert.Greater(result.TestVerdicts.Length, 0);
            Assert.That(List.Map(result.TestVerdicts).Property("Verdict"), Is.All.EqualTo(Verdict.Accepted));
            CheckOutputFilesExists(problemId);
        }

        [Test]
        public void MainTestMainSolutionFromProblem()
        {
            const int problemId = 8901;

            RemoveOutputFiles(problemId);

            RunMainSolutionJob job = new RunMainSolutionJob(new RunnerMock(), problemId);
            job.Execute();
            var result = job.TestingResult;

            Assert.AreEqual(Verdict.Accepted, result.CompilationVerdict);
            Assert.Greater(result.TestVerdicts.Length, 0);
            Assert.That(List.Map(result.TestVerdicts).Property("Verdict"), Is.All.EqualTo(Verdict.Accepted));
            CheckOutputFilesExists(problemId);
        }

        [Test]
        public void IncorrectTestMainSolution()
        {
            const int problemId = 8902;

            RemoveOutputFiles(problemId);

            RunMainSolutionJob job = new RunMainSolutionJob(new RunnerMock(), problemId, SolutionForSwapProblem, ProgrammingLanguage.CSharp);
            job.Execute();
            var result = job.TestingResult;

            Assert.AreEqual(Verdict.Accepted, result.CompilationVerdict);
            Assert.AreEqual(6, result.TestVerdicts.Length);
            Assert.That(List.Map(result.TestVerdicts).Property("Verdict"), Is.Not.All.EqualTo(Verdict.Accepted));
        }

        [Test]
        public void IncorrectTestMainSolutionFromProblem()
        {
            const int problemId = 8903;

            RemoveOutputFiles(problemId);

            RunMainSolutionJob job = new RunMainSolutionJob(new RunnerMock(), problemId);
            job.Execute();
            var result = job.TestingResult;

            Assert.AreEqual(Verdict.Accepted, result.CompilationVerdict);
            Assert.AreEqual(6, result.TestVerdicts.Length);
            Assert.That(List.Map(result.TestVerdicts).Property("Verdict"), Is.Not.All.EqualTo(Verdict.Accepted));
        }

        [Test]
        [ExpectedException]
        public void IncorrectTestMainSolutionNoSolution()
        {
            RunMainSolutionJob job = new RunMainSolutionJob(new RunnerMock(), 8902);
            job.Execute();
        }

        [Test]
        [ExpectedException]
        public void IncorrectTestMainSolutionIncorrectLanguage()
        {
            RunMainSolutionJob job = new RunMainSolutionJob(new RunnerMock(), 8812);
            job.Execute();
        }

        [Test]
        public void TestSolutionJobWithIncorrectValidator()
        {
            TestSolutionJob job = new TestSolutionJob(new RunnerMock(), 8903, "", ProgrammingLanguage.CSharp);
            job.Execute();
            var result = job.TestingResult;

            Assert.AreEqual(1, result.TestVerdicts.Length);
            Assert.AreEqual(Verdict.InternalError, result.TestVerdicts[0].Verdict);
        }

        private void RemoveOutputFiles(int problemId)
        {
            var fullPathToProblem = TestJob.FullPathToProblem(problemId);
            var testDirectories = Directory.GetDirectories(fullPathToProblem, "test*");
            foreach (var testDirectory in testDirectories)
            {
                string filePath = Path.Combine(testDirectory, "out.txt");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        private void CheckOutputFilesExists(int problemId)
        {
            var fullPathToProblem = TestJob.FullPathToProblem(problemId);
            var testDirectories = Directory.GetDirectories(fullPathToProblem, "test*");
            foreach (var testDirectory in testDirectories)
            {
                string filePath = Path.Combine(testDirectory, "out.txt");
                Assert.IsTrue(File.Exists(filePath));
            }
        }
    }
}
