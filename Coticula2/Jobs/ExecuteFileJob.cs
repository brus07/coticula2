using Coticula2.Job;
using Protex;
using Protex.Windows;
using System.IO;

namespace Coticula2.Jobs
{
    internal class ExecuteFileJob : IJob
    {
        private readonly IRunner Runner;
        private readonly IRunnerStartInfo StartInfo;

        public ExecuteFileJob(IRunner runner, string executableFilePath, string inputFilePath)
        {
            Runner = runner;
            StartInfo = Creator.CreateRunnerStartInfo();
            StartInfo.ExecutableFile = executableFilePath;
            StartInfo.InputString = File.ReadAllText(inputFilePath);
        }

        public ExecuteFileJob(IRunner runner, IRunnerStartInfo startInfo)
        {
            Runner = runner;
            StartInfo = startInfo;
        }

        public IResult TestExecutedResult { get; private set; }

        public void Execute()
        {
            TestExecutedResult = Runner.Run(StartInfo);
        }
    }
}
