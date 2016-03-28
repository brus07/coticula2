using Coticula2.Test.Mocks;
using NUnit.Framework;

namespace Coticula2.Test
{
    [TestFixture]
    public class TesterTest
    {
        [Test]
        public void MainTest()
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
            Tester tester = new Tester(new RunnerMock());
            var result = tester.Test(1, solutions, Language.CSharp);

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
            Tester tester = new Tester(new RunnerMock());
            var result = tester.Test(1, solutions, Language.CSharp);

            Assert.AreEqual(Verdict.CopilationError, result.CompilationVerdict);
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
        Console.WriteLine(""{0} {1}"", int.Parse(tokens[1]), int.Parse(tokens[2]));
    }
}";
            Tester tester = new Tester(new RunnerMock());
            var result = tester.Test(1, solutions, Language.CSharp);

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
            Tester tester = new Tester(new RunnerMock());
            var result = tester.Test(1, solutions, Language.CSharp);

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
            Tester tester = new Tester(new RunnerMock());
            var result = tester.Test(1, solutions, Language.CSharp);

            Assert.AreEqual(Verdict.Accepted, result.CompilationVerdict);
            Assert.Greater(result.TestVerdicts.Length, 0);
            Assert.That(List.Map(result.TestVerdicts).Property("Verdict"), Has.Some.EqualTo(Verdict.Accepted));
            Assert.That(List.Map(result.TestVerdicts).Property("Verdict"), Has.Some.EqualTo(Verdict.WrongAnswer));
        }

        [Test]
        [Timeout(60*1000)]
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
            Tester tester = new Tester(new RunnerMock());
            var result = tester.Test(1, solutions, Language.CSharp);

            Assert.AreEqual(Verdict.Accepted, result.CompilationVerdict);
            Assert.Greater(result.TestVerdicts.Length, 0);
            Assert.That(List.Map(result.TestVerdicts).Property("Verdict"), Is.All.EqualTo(Verdict.TimeLimit));
        }
    }
}
