using Coticula2.Jobs;
using Coticula2.Models;
using Coticula2.Test.Mocks;
using NUnit.Framework;
using System;

namespace Coticula2.Test.Jobs
{
    [TestFixture]
    public class TestSubmitJobTest
    {
        [Test]
        public void SolutionMainTestAccepted()
        {
            Submit submit = new Submit()
            {
                Solution = @"using System;

public class Swap
{
    private static void Main()
    {
        string[] tokens = Console.ReadLine().Split();
        Console.WriteLine(""{0} {1}"", int.Parse(tokens[1]), int.Parse(tokens[0]));
    }
}",
                ProblemID = 1,
                ProgrammingLanguage = ProgrammingLanguage.CSharp,
                SubmitType = SubmitType.Solution
            };
            TestSubmitJob job = new TestSubmitJob(new RunnerMock(), submit);
            job.Execute();
            var result = job.SubmitResult;

            Assert.AreEqual(Verdict.Accepted, result.Verdict);
            AsserSubmitWithoutVerdict(submit, result);
        }

        [Test]
        public void SolutionCompilationError()
        {
            Submit submit = new Submit()
            {
                Solution = @"using System;

public class Swap
{
    private static void Main()
    {
        string[] tokens = Console.ReadLine().Split();
        Console.WriteLine(""{0} {1}"", int.Parse(toke ns[1]), int.Parse(tokens[0]));
    }
}",
                ProblemID = 1,
                ProgrammingLanguage = ProgrammingLanguage.CSharp,
                SubmitType = SubmitType.Solution
            };
            TestSubmitJob job = new TestSubmitJob(new RunnerMock(), submit);
            job.Execute();
            var result = job.SubmitResult;

            Assert.AreEqual(Verdict.CopilationError, result.Verdict);
            AsserSubmitWithoutVerdict(submit, result);
        }

        [Test]
        public void SolutionInternalErrorIncorrectProblem()
        {
            Submit submit = new Submit()
            {
                ProblemID = 176763567,
                ProgrammingLanguage = ProgrammingLanguage.CSharp,
                SubmitType = SubmitType.Solution
            };
            TestSubmitJob job = new TestSubmitJob(new RunnerMock(), submit);
            job.Execute();
            var result = job.SubmitResult;

            Assert.AreEqual(Verdict.InternalError, result.Verdict);
            AsserSubmitWithoutVerdict(submit, result);
        }

        [Test]
        public void SolutionRunTimeTest()
        {
            Submit submit = new Submit()
            {
                Solution = @"using System;

public class Swap
{
    private static void Main()
    {
        string[] tokens = Console.ReadLine().Split();
        Console.WriteLine(""{0} {1}"", int.Parse(tokens[1])/int.Parse(tokens[1]), int.Parse(tokens[0]));
    }
}",
                ProblemID = 1,
                ProgrammingLanguage = ProgrammingLanguage.CSharp,
                SubmitType = SubmitType.Solution
            };
            TestSubmitJob job = new TestSubmitJob(new RunnerMock(), submit);
            job.Execute();
            var result = job.SubmitResult;

            Assert.AreEqual(Verdict.RunTimeError, result.Verdict);
            AsserSubmitWithoutVerdict(submit, result);
        }

        [Test]
        public void SolutionWrongAnswerTest()
        {
            Submit submit = new Submit()
            {
                Solution = @"using System;

public class Swap
{
    private static void Main()
    {
        Console.WriteLine(""bad output"");
    }
}",
                ProblemID = 1,
                ProgrammingLanguage = ProgrammingLanguage.CSharp,
                SubmitType = SubmitType.Solution
            };
            TestSubmitJob job = new TestSubmitJob(new RunnerMock(), submit);
            job.Execute();
            var result = job.SubmitResult;

            Assert.AreEqual(Verdict.WrongAnswer, result.Verdict);
            AsserSubmitWithoutVerdict(submit, result);
        }

        [Test]
        public void SolutionWrongAnswer2Test()
        {
            Submit submit = new Submit()
            {
                Solution = @"using System;

public class Swap
{
    private static void Main()
    {
        string[] tokens = Console.ReadLine().Split();
        Console.WriteLine(""{0} {1}"", int.Parse(tokens[0]), int.Parse(tokens[1]));
    }
}",
                ProblemID = 1,
                ProgrammingLanguage = ProgrammingLanguage.CSharp,
                SubmitType = SubmitType.Solution
            };
            TestSubmitJob job = new TestSubmitJob(new RunnerMock(), submit);
            job.Execute();
            var result = job.SubmitResult;

            Assert.AreEqual(Verdict.WrongAnswer, result.Verdict);
            AsserSubmitWithoutVerdict(submit, result);
        }

        [Test]
        [Timeout(60 * 1000)]
        public void SolutionTestTimeLimitTest()
        {
            Submit submit = new Submit()
            {
                Solution = @"using System;

public class Swap
{
    private static void Main()
    {
        while (true) {}
    }
}",
                ProblemID = 1,
                ProgrammingLanguage = ProgrammingLanguage.CSharp,
                SubmitType = SubmitType.Solution
            };
            TestSubmitJob job = new TestSubmitJob(new RunnerMock(), submit);
            job.Execute();
            var result = job.SubmitResult;

            Assert.AreEqual(Verdict.TimeLimit, result.Verdict);
            AsserSubmitWithoutVerdict(submit, result);
        }

        [Test]
        public void TestSubmitTypeTestAccepted()
        {
            Submit submit = new Submit()
            {
                ProblemID = 8831,
                Solution = "0 0",
                SubmitType = SubmitType.Test
            };
            TestSubmitJob job = new TestSubmitJob(new RunnerMock(), submit);
            job.Execute();
            var result = job.SubmitResult;

            Assert.AreEqual(Verdict.Accepted, result.Verdict);
            AsserSubmitWithoutVerdict(submit, result);
        }

        [Test]
        public void TestSubmitTypeTestWrongAnswer()
        {
            Submit submit = new Submit()
            {
                ProblemID = 8831,
                Solution = "0",
                SubmitType = SubmitType.Test
            };
            TestSubmitJob job = new TestSubmitJob(new RunnerMock(), submit);
            job.Execute();
            var result = job.SubmitResult;

            Assert.AreEqual(Verdict.WrongAnswer, result.Verdict);
            AsserSubmitWithoutVerdict(submit, result);
        }

        [Test]
        public void TestSubmitBigLogic()
        {
            Submit submitTest = new Submit()
            {
                SubmitID = 10,
                ProblemID = 8841,
                Solution = "0 0",
                SubmitType = SubmitType.Test
            };
            TestSubmitJob job = new TestSubmitJob(new RunnerMock(), submitTest);
            job.Execute();
            var result = job.SubmitResult;
            Assert.AreEqual(Verdict.Accepted, result.Verdict);

            submitTest.Solution = "4 7";
            submitTest.SubmitID = 12;
            job = new TestSubmitJob(new RunnerMock(), submitTest);
            job.Execute();
            result = job.SubmitResult;
            Assert.AreEqual(Verdict.Accepted, result.Verdict);

            Submit submit = new Submit()
            {
                SubmitID = 1,
                ProblemID = 8841,
                Solution = @"using System;

public class Swap
{
    private static void Main()
    {
        string[] tokens = Console.ReadLine().Split();
        Console.WriteLine(""{0} {1}"", int.Parse(tokens[1]), int.Parse(tokens[0]));
    }
}",
                ProgrammingLanguage = ProgrammingLanguage.CSharp,
                SubmitType = SubmitType.Solution
            };
            job = new TestSubmitJob(new RunnerMock(), submit);
            job.Execute();
            result = job.SubmitResult;

            Assert.AreEqual(Verdict.Accepted, result.Verdict);
            AsserSubmitWithoutVerdict(submit, result);
        }

        private static void AsserSubmitWithoutVerdict(Submit expected, Submit actual)
        {
            Assert.AreEqual(expected.SubmitID, actual.SubmitID);
            Assert.AreEqual(expected.ProblemID, actual.ProblemID);
            Assert.AreEqual(expected.ProgrammingLanguage, actual.ProgrammingLanguage);
            Assert.AreEqual(expected.Solution, actual.Solution);
            Assert.AreEqual(expected.SubmitType, actual.SubmitType);

            Assert.AreEqual(expected.PeakMemoryUsed, actual.PeakMemoryUsed);
            Assert.AreEqual(expected.WorkingTime, actual.WorkingTime);
        }
    }
}
