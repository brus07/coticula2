using Protex.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Coticula2.Compilers
{
    class DefaultCompiler: ICompiler
    {
        public string Language { get; set; }

        public string OutputPath { get; set; }

        public string OutputString { get; set; }

        private string InvokeParameters;

        public DefaultCompiler(string invokeParameters)
        {
            InvokeParameters = invokeParameters;
        }

        public bool Compile(string sourceCode)
        {
            string randomFileName = Path.GetRandomFileName();
            File.WriteAllText(randomFileName, sourceCode);

            var startInfo = WindowsCreator.CreateRunnerStartInfo();
            startInfo.ExecutableFile = InvokeParameters.Split()[0];
            startInfo.Arguments = InvokeParameters.Remove(0, startInfo.ExecutableFile.Length);
            startInfo.Arguments = startInfo.Arguments.Replace("%1", randomFileName);

            var runner = WindowsCreator.CreateRunner();
            var result = runner.Run(startInfo);
            return result.ExitCode == 0;
        }
    }
}
