using Coticula2.Jobs;
using Coticula2.Models;
using Coticula2.Test.Mocks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coticula2.Test.Jobs
{
    [TestFixture]
    public class TestJobTest
    {
        string PathToTestData = Path.Combine("..", "..", "..", "TestData", "Executables");

        [Test]
        public void CorrectSwapProblem1Test1()
        {
            string executableFilePath = Path.Combine(PathToTestData, "Swap.exe");
            TestJob job = new TestJob(new RunnerMock(), executableFilePath, 1, 1);
            job.Execute();
            var result = job.TestResult;
            Assert.That(result.Verdict, Is.EqualTo(Verdict.Accepted));
        }

        [Test]
        public void CorrectSwapProblem1Test2()
        {
            string executableFilePath = Path.Combine(PathToTestData, "Swap.exe");
            TestJob job = new TestJob(new RunnerMock(), executableFilePath, 1, 2);
            job.Execute();
            var result = job.TestResult;
            Assert.That(result.Verdict, Is.EqualTo(Verdict.Accepted));
        }

        [Test]
        public void CorrectEchoLineProblem1Test1()
        {
            string executableFilePath = Path.Combine(PathToTestData, "EchoLine.exe");
            TestJob job = new TestJob(new RunnerMock(), executableFilePath, 1, 1);
            job.Execute();
            var result = job.TestResult;
            Assert.That(result.Verdict, Is.EqualTo(Verdict.Accepted));
        }

        [Test]
        public void CorrectEchoLineProblem1Test4()
        {
            string executableFilePath = Path.Combine(PathToTestData, "EchoLine.exe");
            TestJob job = new TestJob(new RunnerMock(), executableFilePath, 1, 4);
            job.Execute();
            var result = job.TestResult;
            Assert.That(result.Verdict, Is.EqualTo(Verdict.WrongAnswer));
        }

        [Test]
        public void CorrectHelloWorldProblem1Test1()
        {
            string executableFilePath = Path.Combine(PathToTestData, "HelloWorld.exe");
            TestJob job = new TestJob(new RunnerMock(), executableFilePath, 1, 1);
            job.Execute();
            var result = job.TestResult;
            Assert.That(result.Verdict, Is.EqualTo(Verdict.WrongAnswer));
        }

        [Test]
        public void CorrectHelloWorldProblem1Test3()
        {
            string executableFilePath = Path.Combine(PathToTestData, "HelloWorld.exe");
            TestJob job = new TestJob(new RunnerMock(), executableFilePath, 1, 3);
            job.Execute();
            var result = job.TestResult;
            Assert.That(result.Verdict, Is.EqualTo(Verdict.WrongAnswer));
        }
    }
}
