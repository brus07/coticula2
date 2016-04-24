using Coticula2.Jobs;
using Coticula2.Test.Mocks;
using NUnit.Framework;
using Protex.Windows;
using System.IO;

namespace Coticula2.Test.Jobs
{
    [TestFixture]
    public class ExecuteFileJobTest
    {
        string PathToTestData = Path.Combine("..", "..", "..", "TestData", "Executables");

        [Test]
        public void ExecuteHelloWorld()
        {
            string executableFilePath = Path.Combine(PathToTestData, "HelloWorld.exe");
            string inputFilePath = Path.Combine(PathToTestData, "input_HelloWorld.txt");
            ExecuteFileJob job = new ExecuteFileJob(new RunnerMock(), executableFilePath, inputFilePath);
            job.Execute();
            var result = job.TestExecutedResult;
            Assert.That(result.ExitCode, Is.EqualTo(0));
            Assert.That(result.OutputString, Is.EqualTo("Hello world!\r\n"));
        }

        [Test]
        public void ExecuteHelloWorld2()
        {
            string executableFilePath = Path.Combine(PathToTestData, "HelloWorld.exe");
            string inputFilePath = Path.Combine(PathToTestData, "input_SpecialForEcho.txt");
            ExecuteFileJob job = new ExecuteFileJob(new RunnerMock(), executableFilePath, inputFilePath);
            job.Execute();
            var result = job.TestExecutedResult;
            Assert.That(result.ExitCode, Is.EqualTo(0));
            Assert.That(result.OutputString, Is.EqualTo("Hello world!\r\n"));
        }

        [Test]
        public void ExecuteEchoLine()
        {
            string executableFilePath = Path.Combine(PathToTestData, "EchoLine.exe");
            string inputFilePath = Path.Combine(PathToTestData, "input_HelloWorld.txt");
            ExecuteFileJob job = new ExecuteFileJob(new RunnerMock(), executableFilePath, inputFilePath);
            job.Execute();
            var result = job.TestExecutedResult;
            Assert.That(result.ExitCode, Is.EqualTo(0));
            Assert.That(result.OutputString, Is.EqualTo("Hello world!\r\n"));
        }

        [Test]
        public void ExecuteEchoLine2()
        {
            string executableFilePath = Path.Combine(PathToTestData, "EchoLine.exe");
            string inputFilePath = Path.Combine(PathToTestData, "input_SpecialForEcho.txt");
            ExecuteFileJob job = new ExecuteFileJob(new RunnerMock(), executableFilePath, inputFilePath);
            job.Execute();
            var result = job.TestExecutedResult;
            Assert.That(result.ExitCode, Is.EqualTo(0));
            Assert.That(result.OutputString, Is.EqualTo(File.ReadAllText(inputFilePath)));
        }



        [Test]
        public void ExecuteHelloWorldStartInfo()
        {
            var startInfo = Creator.CreateRunnerStartInfo();
            startInfo.ExecutableFile = Path.Combine(PathToTestData, "HelloWorld.exe");
            startInfo.InputString = File.ReadAllText(Path.Combine(PathToTestData, "input_HelloWorld.txt"));
            ExecuteFileJob job = new ExecuteFileJob(new RunnerMock(), startInfo);
            job.Execute();
            var result = job.TestExecutedResult;
            Assert.That(result.ExitCode, Is.EqualTo(0));
            Assert.That(result.OutputString, Is.EqualTo("Hello world!\r\n"));
        }

        [Test]
        public void ExecuteHelloWorld2StartInfo()
        {
            var startInfo = Creator.CreateRunnerStartInfo();
            startInfo.ExecutableFile = Path.Combine(PathToTestData, "HelloWorld.exe");
            startInfo.InputString = File.ReadAllText(Path.Combine(PathToTestData, "input_SpecialForEcho.txt"));
            ExecuteFileJob job = new ExecuteFileJob(new RunnerMock(), startInfo);
            job.Execute();
            var result = job.TestExecutedResult;
            Assert.That(result.ExitCode, Is.EqualTo(0));
            Assert.That(result.OutputString, Is.EqualTo("Hello world!\r\n"));
        }

        [Test]
        public void ExecuteEchoLineStartInfo()
        {
            var startInfo = Creator.CreateRunnerStartInfo();
            startInfo.ExecutableFile = Path.Combine(PathToTestData, "EchoLine.exe");
            startInfo.InputString = File.ReadAllText(Path.Combine(PathToTestData, "input_HelloWorld.txt"));
            ExecuteFileJob job = new ExecuteFileJob(new RunnerMock(), startInfo);
            job.Execute();
            var result = job.TestExecutedResult;
            Assert.That(result.ExitCode, Is.EqualTo(0));
            Assert.That(result.OutputString, Is.EqualTo("Hello world!\r\n"));
        }

        [Test]
        public void ExecuteEchoLine2StartInfo()
        {
            string inputFilePath = Path.Combine(PathToTestData, "input_SpecialForEcho.txt");
            var startInfo = Creator.CreateRunnerStartInfo();
            startInfo.ExecutableFile = Path.Combine(PathToTestData, "EchoLine.exe");
            startInfo.InputString = File.ReadAllText(inputFilePath);
            ExecuteFileJob job = new ExecuteFileJob(new RunnerMock(), startInfo);
            job.Execute();
            var result = job.TestExecutedResult;
            Assert.That(result.ExitCode, Is.EqualTo(0));
            Assert.That(result.OutputString, Is.EqualTo(File.ReadAllText(inputFilePath)));
        }
    }
}
