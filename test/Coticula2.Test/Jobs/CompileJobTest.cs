using Coticula2.Jobs;
using Coticula2.Test.Mocks;
using NUnit.Framework;
using System;
using System.IO;

namespace Coticula2.Test.Jobs
{
    [TestFixture]
    public class CompileJobTest
    {
        [Test]
        public void CorrectCompilationJob()
        {
            var sourceCode = @"using System;

public class Swap
{
    private static void Main()
    {
        string[] tokens = Console.ReadLine().Split();
        Console.WriteLine(""{0} {1}"", int.Parse(tokens[1]), int.Parse(tokens[0]));
    }
}";
            var temporaryDirectory = CreateTemporaryDirectory();
            Assert.That(Directory.GetFiles(temporaryDirectory, "*.exe").Length == 0);
            var compileJob = new CompileJob(new RunnerMock(), temporaryDirectory, sourceCode, Language.CSharp);
            compileJob.Execute();
            Assert.AreEqual(0, compileJob.TestExecutedResult.ExitCode);
            Assert.That(Directory.GetFiles(temporaryDirectory, "*.exe").Length > 0);
        }

        [Test]
        public void InCorrectCompilationJob()
        {
            var sourceCode = @"begin";
            var temporaryDirectory = CreateTemporaryDirectory();
            var compileJob = new CompileJob(new RunnerMock(), temporaryDirectory, sourceCode, Language.CSharp);
            compileJob.Execute();
            Assert.AreEqual(1, compileJob.TestExecutedResult.ExitCode);
            Assert.That(Directory.GetFiles(temporaryDirectory, "*.exe").Length == 0);
        }

        [Test]
        public void InCorrectCompilerFpc()
        {
            var compileJob = new CompileJob(new RunnerMock(), CreateTemporaryDirectory(), "", Language.Fpc);
            Assert.Throws<ArgumentException>(compileJob.Execute);
        }

        [Test]
        public void InCorrectCompilerGPlusPlus()
        {
            var compileJob = new CompileJob(new RunnerMock(), CreateTemporaryDirectory(), "", Language.GPlusPlus);
            Assert.Throws<ArgumentException>(compileJob.Execute);
        }

        static string CreateTemporaryDirectory()
        {
            if (!Directory.Exists("Temp"))
                Directory.CreateDirectory("Temp");
            string tempDirectory = Path.Combine("Temp", Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }
    }
}
