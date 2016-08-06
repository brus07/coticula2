using Coticula2.Jobs;
using Coticula2.Models;
using Coticula2.Test.Mocks;
using NUnit.Framework;
using System.IO;

namespace Coticula2.Test.Jobs
{
    [TestFixture]
    public class ValidatorTestJobTest
    {
        string PathToTestData = Path.Combine("..", "..", "..", "TestData", "Executables", "Validators");

        [Test]
        public void CorrectValidatorProblem1Test1()
        {
            string executableFilePath = Path.Combine(PathToTestData, "ValidatorForSwapProblem.exe");
            ValidatorTestJob job = new ValidatorTestJob(new RunnerMock(), executableFilePath, 1, 1);
            job.Execute();
            var result = job.TestResult;
            Assert.That(result.Verdict, Is.EqualTo(Verdict.Accepted));
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void CorrectValidatorProblem1TestId(int testId)
        {
            string executableFilePath = Path.Combine(PathToTestData, "ValidatorForSwapProblem.exe");
            ValidatorTestJob job = new ValidatorTestJob(new RunnerMock(), executableFilePath, 1, testId);
            job.Execute();
            var result = job.TestResult;
            Assert.That(result.Verdict, Is.EqualTo(Verdict.Accepted));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void InCorrectValidatorProblem8801Test1(int testId)
        {
            string executableFilePath = Path.Combine(PathToTestData, "ValidatorForSwapProblem.exe");
            ValidatorTestJob job = new ValidatorTestJob(new RunnerMock(), executableFilePath, 8801, testId);
            job.Execute();
            var result = job.TestResult;
            Assert.That(result.Verdict, Is.EqualTo(Verdict.WrongAnswer));
        }
    }
}
