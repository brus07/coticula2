using Coticula2.Jobs;
using Coticula2.Models;
using Coticula2.Test.Mocks;
using NUnit.Framework;

namespace Coticula2.Test.Jobs
{
    [TestFixture]
    public class AddTestJobTest
    {
        [Test]
        public void MainTestAddTestJob()
        {
            AddTestJob job = new AddTestJob(new RunnerMock(), 8831, 4, "0 0");
            job.Execute();
            var result = job.TestingResult;

            Assert.AreEqual(Verdict.Accepted, result.CompilationVerdict);
            Assert.AreEqual(result.TestVerdicts.Length, 1);
            Assert.That(List.Map(result.TestVerdicts).Property("Verdict"), Is.All.EqualTo(Verdict.Accepted));
        }

        [TestCase(51, "1 1")]
        [TestCase(52, "0 0")]
        [TestCase(53, "123 444")]
        [TestCase(54, "4 7")]
        public void TestAddTestJobCorrectTest(int testId, string testContent)
        {
            AddTestJob job = new AddTestJob(new RunnerMock(), 8831, testId, testContent);
            job.Execute();
            var result = job.TestingResult;

            Assert.AreEqual(Verdict.Accepted, result.CompilationVerdict);
            Assert.AreEqual(result.TestVerdicts.Length, 1);
            Assert.That(List.Map(result.TestVerdicts).Property("Verdict"), Is.All.EqualTo(Verdict.Accepted));
        }

        [TestCase(61, "")]
        [TestCase(62, "0 -1")]
        [TestCase(63, "-1 0")]
        [TestCase(64, "0")]
        [TestCase(65, "1000 10001")]
        [TestCase(66, "5 100000      ")]
        [TestCase(67, "0 0 0")]
        public void TestAddTestJobIncorrectTest(int testId, string testContent)
        {
            AddTestJob job = new AddTestJob(new RunnerMock(), 8831, testId, testContent);
            job.Execute();
            var result = job.TestingResult;

            Assert.AreEqual(Verdict.Accepted, result.CompilationVerdict);
            Assert.AreEqual(result.TestVerdicts.Length, 1);
            Assert.That(List.Map(result.TestVerdicts).Property("Verdict"), Is.All.EqualTo(Verdict.WrongAnswer));
        }

        [Test]
        [ExpectedException]
        public void IncorrectAddTestJobrNoValidator()
        {
            ValidateOneTestJob job = new ValidateOneTestJob(new RunnerMock(), 8811, 1);
            job.Execute();
        }
    }
}
