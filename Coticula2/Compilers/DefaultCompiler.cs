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

        public const string WorkingFolderName = "Temp";

        public bool Compile(string sourceCode)
        {
            string randomPathName = Path.GetRandomFileName();
            string fullRelativePath = Path.Combine(WorkingFolderName, randomPathName);
            Directory.CreateDirectory(fullRelativePath);
            string sourceFilePath = Path.Combine(fullRelativePath, "source.scr");
            File.WriteAllText(sourceFilePath, sourceCode);

            var startInfo = WindowsCreator.CreateRunnerStartInfo();
            startInfo.ExecutableFile = InvokeParameters.Split()[0];
            startInfo.Arguments = InvokeParameters.Remove(0, startInfo.ExecutableFile.Length);
            startInfo.Arguments = startInfo.Arguments.Replace("%1", sourceFilePath);

            var runner = WindowsCreator.CreateRunner();
            var result = runner.Run(startInfo);
            OutputString = OptimizeOutput(result.OutputString).Replace(fullRelativePath,"");
            OutputPath = Path.Combine(fullRelativePath, "source.exe");
            return result.ExitCode == 0;
        }

        private string OptimizeOutput(string output)
        {
            string[] lines = output.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string result = "";
            const int SkipFirstXLines = 3;
            for (int i = SkipFirstXLines; i < lines.Length; i++)
            {
                result += lines[i];
                result += Environment.NewLine;
            }
            return result;
        }
    }
}
