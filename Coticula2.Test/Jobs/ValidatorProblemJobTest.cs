using Coticula2.Jobs;
using Coticula2.Test.Mocks;
using NUnit.Framework;
using System.IO;

namespace Coticula2.Test.Jobs
{
    [TestFixture]
    public class ValidatorProblemJobTest
    {
        const string ValidatorForSwapProblem = @"using System;

public class ValidatorForSwapProblem
{
    private static void Main()
    {
        string[] tokens = Console.ReadLine().Split();
        if (tokens.Length != 2)
        {
                Console.WriteLine(""the number of parameters should be 2"");
                return;
        }
        int a = int.Parse(tokens[0]);
        int b = int.Parse(tokens[1]);
        if (a < 0 || a > 1000)
        {
                Console.WriteLine(""a is bad"");
                return;
        }
        if (b < 0 || b > 1000)
        {
                Console.WriteLine(""b is bad"");
                return;
        }
        Console.WriteLine(""ok"");
    }
}";

        [Test]
        public void MainTestValidator()
        {
            ValidatorProblemJob job = new ValidatorProblemJob(new RunnerMock(), 1, ValidatorForSwapProblem, Language.CSharp);
            job.Execute();
            var result = job.TestingResult;

            Assert.AreEqual(Verdict.Accepted, result.CompilationVerdict);
            Assert.Greater(result.TestVerdicts.Length, 0);
            Assert.That(List.Map(result.TestVerdicts).Property("Verdict"), Is.All.EqualTo(Verdict.Accepted));
        }

        [Test]
        public void MainTestValidatorFromProblem()
        {
            ValidatorProblemJob job = new ValidatorProblemJob(new RunnerMock(), 1);
            job.Execute();
            var result = job.TestingResult;

            Assert.AreEqual(Verdict.Accepted, result.CompilationVerdict);
            Assert.Greater(result.TestVerdicts.Length, 0);
            Assert.That(List.Map(result.TestVerdicts).Property("Verdict"), Is.All.EqualTo(Verdict.Accepted));
        }

        [Test]
        public void IncorrectTestValidator()
        {
            ValidatorProblemJob job = new ValidatorProblemJob(new RunnerMock(), 8801, ValidatorForSwapProblem, Language.CSharp);
            job.Execute();
            var result = job.TestingResult;

            Assert.AreEqual(Verdict.Accepted, result.CompilationVerdict);
            Assert.AreEqual(6, result.TestVerdicts.Length);
            Assert.That(List.Map(result.TestVerdicts).Property("Verdict"), Is.All.EqualTo(Verdict.WrongAnswer));
        }

        [Test]
        public void IncorrectTestValidatorFromProblem()
        {
            ValidatorProblemJob job = new ValidatorProblemJob(new RunnerMock(), 8801);
            job.Execute();
            var result = job.TestingResult;

            Assert.AreEqual(Verdict.Accepted, result.CompilationVerdict);
            Assert.AreEqual(6, result.TestVerdicts.Length);
            Assert.That(List.Map(result.TestVerdicts).Property("Verdict"), Is.All.EqualTo(Verdict.WrongAnswer));
        }

        [Test]
        [ExpectedException]
        public void IncorrectTestValidatorNoValidator()
        {
            ValidatorProblemJob job = new ValidatorProblemJob(new RunnerMock(), 8811);
            job.Execute();
        }

        [Test]
        public void TestHasValidator()
        {
            ValidatorProblemJob job = new ValidatorProblemJob(new RunnerMock(), 1);
            Assert.IsTrue(job.HasValidator);
        }

        [Test]
        public void TestNoValidator()
        {
            ValidatorProblemJob job = new ValidatorProblemJob(new RunnerMock(), 8811);
            Assert.IsFalse(job.HasValidator);
        }

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TestValidatorWithIncorrectProblem()
        {
            ValidatorProblemJob job = new ValidatorProblemJob(new RunnerMock(), 14534534);
            var hasValidator = job.HasValidator;
        }

        [Test]
        [ExpectedException]
        public void IncorrectTestValidatorIncorrectLanguage()
        {
            ValidatorProblemJob job = new ValidatorProblemJob(new RunnerMock(), 8812);
            job.Execute();
        }
        
        [Test]
        public void TestSolutionJobWithIncorrectValidator()
        {
            TestSolutionJob job = new TestSolutionJob(new RunnerMock(), 8801, "", Language.CSharp);
            job.Execute();
            var result = job.TestingResult;

            Assert.AreEqual(1, result.TestVerdicts.Length);
            Assert.AreEqual(Verdict.InternalError, result.TestVerdicts[0].Verdict);
        }
    }
}
