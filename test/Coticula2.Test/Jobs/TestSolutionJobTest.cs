using Coticula2.Jobs;
using Coticula2.Test.Mocks;
using NUnit.Framework;

namespace Coticula2.Test.Jobs
{
    [TestFixture]
    public class TestSolutionJobTest
    {
        [Test]
        public void MainTestAccepted()
        {
            const string solutions = @"using System;

public class Swap
{
    private static void Main()
    {
        string[] tokens = Console.ReadLine().Split();
        Console.WriteLine(""{0} {1}"", int.Parse(tokens[1]), int.Parse(tokens[0]));
    }
}";
            TestSolutionJob job = new TestSolutionJob(new RunnerMock(), 1, solutions, Language.CSharp);
            job.Execute();
            var result = job.TestingResult;

            Assert.AreEqual(Verdict.Accepted, result.CompilationVerdict);
            Assert.Greater(result.TestVerdicts.Length, 0);
            Assert.That(List.Map(result.TestVerdicts).Property("Verdict"), Is.All.EqualTo(Verdict.Accepted));
        }

        [Test]
        public void CompilationErrorTest()
        {
            const string solutions = @"using System;

public class Swap
{
    private static void Main()
    {
        string[] tokens = Console.ReadLine().Split();
        Console.WriteLine(""{0} {1}"", int.Parse(toke ns[1]), int.Parse(tokens[0]));
    }
}";
            TestSolutionJob job = new TestSolutionJob(new RunnerMock(), 1, solutions, Language.CSharp);
            job.Execute();
            var result = job.TestingResult;

            Assert.AreEqual(Verdict.CopilationError, result.CompilationVerdict);
        }

        [Test]
        public void InternalErrorIncorrectProblemTest()
        {
            const string solutions = @"";
            TestSolutionJob job = new TestSolutionJob(new RunnerMock(), 176763567, solutions, Language.CSharp);
            job.Execute();
            var result = job.TestingResult;

            Assert.AreEqual(Verdict.InternalError, result.CompilationVerdict);
        }

        [Test]
        public void RunTimeTest()
        {
            const string solutions = @"using System;

public class Swap
{
    private static void Main()
    {
        string[] tokens = Console.ReadLine().Split();
        Console.WriteLine(""{0} {1}"", int.Parse(tokens[1])/int.Parse(tokens[1]), int.Parse(tokens[0]));
    }
}";
            TestSolutionJob job = new TestSolutionJob(new RunnerMock(), 1, solutions, Language.CSharp);
            job.Execute();
            var result = job.TestingResult;

            Assert.AreEqual(Verdict.Accepted, result.CompilationVerdict);
            Assert.Greater(result.TestVerdicts.Length, 0);
            Assert.That(List.Map(result.TestVerdicts).Property("Verdict"), Has.Some.EqualTo(Verdict.RunTimeError));
        }

        [Test]
        public void WrongAnswerTest()
        {
            const string solutions = @"using System;

public class Swap
{
    private static void Main()
    {
        Console.WriteLine(""bad output"");
    }
}";
            TestSolutionJob job = new TestSolutionJob(new RunnerMock(), 1, solutions, Language.CSharp);
            job.Execute();
            var result = job.TestingResult;

            Assert.AreEqual(Verdict.Accepted, result.CompilationVerdict);
            Assert.Greater(result.TestVerdicts.Length, 0);
            Assert.That(List.Map(result.TestVerdicts).Property("Verdict"), Is.All.EqualTo(Verdict.WrongAnswer));
        }

        [Test]
        public void WrongAnswer2Test()
        {
            const string solutions = @"using System;

public class Swap
{
    private static void Main()
    {
        string[] tokens = Console.ReadLine().Split();
        Console.WriteLine(""{0} {1}"", int.Parse(tokens[0]), int.Parse(tokens[1]));
    }
}";
            TestSolutionJob job = new TestSolutionJob(new RunnerMock(), 1, solutions, Language.CSharp);
            job.Execute();
            var result = job.TestingResult;

            Assert.AreEqual(Verdict.Accepted, result.CompilationVerdict);
            Assert.Greater(result.TestVerdicts.Length, 0);
            Assert.That(List.Map(result.TestVerdicts).Property("Verdict"), Has.Some.EqualTo(Verdict.Accepted));
            Assert.That(List.Map(result.TestVerdicts).Property("Verdict"), Has.Some.EqualTo(Verdict.WrongAnswer));
        }

        [Test]
        [Timeout(60 * 1000)]
        public void TestTimeLimitTest()
        {
            const string solutions = @"using System;

public class Swap
{
    private static void Main()
    {
        while (true) {}
    }
}";
            TestSolutionJob job = new TestSolutionJob(new RunnerMock(), 1, solutions, Language.CSharp);
            job.Execute();
            var result = job.TestingResult;

            Assert.AreEqual(Verdict.Accepted, result.CompilationVerdict);
            Assert.Greater(result.TestVerdicts.Length, 0);
            Assert.That(List.Map(result.TestVerdicts).Property("Verdict"), Is.All.EqualTo(Verdict.TimeLimit));
        }
    }
}
