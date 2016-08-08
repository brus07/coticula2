using Coticula2.Jobs;
using Coticula2.Models;
using Coticula2.Test.Mocks;
using NUnit.Framework;

namespace Coticula2.Test.Jobs
{
    [TestFixture]
    public class ValidateOneTestJobTest
    {
        [Test]
        public void MainTestValidator([Values(1, 2, 3, 4)] int testId)
        {
            ValidateOneTestJob job = new ValidateOneTestJob(new RunnerMock(), 1, testId);
            job.Execute();
            var result = job.TestingResult;

            Assert.AreEqual(Verdict.Accepted, result.CompilationVerdict);
            Assert.AreEqual(result.TestVerdicts.Length, 1);
            Assert.That(List.Map(result.TestVerdicts).Property("Verdict"), Is.All.EqualTo(Verdict.Accepted));
        }

        [Test]
        public void TestValidatorWithDifferenIds([Values(44, 77, 12345, 987654321)] int testId)
        {
            ValidateOneTestJob job = new ValidateOneTestJob(new RunnerMock(), 8802, testId);
            job.Execute();
            var result = job.TestingResult;

            Assert.AreEqual(Verdict.Accepted, result.CompilationVerdict);
            Assert.AreEqual(result.TestVerdicts.Length, 1);
            Assert.That(List.Map(result.TestVerdicts).Property("Verdict"), Is.All.EqualTo(Verdict.Accepted));
        }

        [Test]
        public void IncorrectTestValidator([Values(1, 2, 3, 4, 5, 6)] int testId)
        {
            ValidateOneTestJob job = new ValidateOneTestJob(new RunnerMock(), 8801, testId);
            job.Execute();
            var result = job.TestingResult;

            Assert.AreEqual(Verdict.Accepted, result.CompilationVerdict);
            Assert.AreEqual(result.TestVerdicts.Length, 1);
            Assert.That(List.Map(result.TestVerdicts).Property("Verdict"), Is.All.EqualTo(Verdict.WrongAnswer));
        }

        [Test]
        [ExpectedException]
        public void IncorrectTestValidatorNoValidator()
        {
            ValidateOneTestJob job = new ValidateOneTestJob(new RunnerMock(), 8811, 1);
            job.Execute();
        }

        [Test]
        [ExpectedException]
        public void IncorrectTestValidatorIncorrectLanguage()
        {
            ValidateOneTestJob job = new ValidateOneTestJob(new RunnerMock(), 8812, 1);
            job.Execute();
        }

        [Test]
        public void TestSolutionJobWithIncorrectValidator()
        {
            Submit submit = new Submit()
            {
                Solution = "",
                ProblemID = 8801,
                SubmitType = SubmitType.Test
            };
            TestSubmitJob job = new TestSubmitJob(new RunnerMock(), submit);
            job.Execute();
            Assert.AreEqual(Verdict.WrongAnswer, job.SubmitResult.Verdict);
        }
    }
}
