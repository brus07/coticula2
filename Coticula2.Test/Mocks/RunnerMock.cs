using Protex;
using System;
using System.Diagnostics;
using System.Threading;

namespace Coticula2.Test.Mocks
{
    class RunnerMock : IRunner
    {
        public IResult Run(IRunnerStartInfo runnerStartInfo)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = runnerStartInfo.ExecutableFile;
            startInfo.Arguments = runnerStartInfo.Arguments;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardError = true;

            IResult result = new RunnerMockResult();

            using (Process process = Process.Start(startInfo))
            {
                if (runnerStartInfo.InputString != null)
                    process.StandardInput.WriteLine(runnerStartInfo.InputString);
                if (!process.WaitForExit(runnerStartInfo.WorkingTimeLimit * 5))
                {
                    process.Kill();
                    result.ExitCode = -1;
                    result.WorkingTime = runnerStartInfo.WorkingTimeLimit;
                }
                //process.WaitForExit();

                Thread.Sleep(50);

                if (result.ExitCode == 0)
                {
                    string outputString = process.StandardOutput.ReadToEnd();

                    result.ExitCode = process.ExitCode;
                    result.OutputString = outputString;
                }
            }
            return result;
        }
    }
}
