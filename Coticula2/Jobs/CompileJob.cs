using System;
using Coticula2.Job;
using Protex;
using Protex.Windows;
using System.IO;
using Coticula2.Models;

namespace Coticula2.Jobs
{
    internal class CompileJob : IJob
    {
        private readonly IRunner Runner;
        private readonly string WorkingDirectoryPath;
        private readonly string SourceCode;
        private readonly ProgrammingLanguage Language;

        public CompileJob(IRunner runner, string workingDirectoryPath, string sourceCode, ProgrammingLanguage language)
        {
            Runner = runner;
            WorkingDirectoryPath = workingDirectoryPath;
            SourceCode = sourceCode;
            Language = language;
        }

        public IResult TestExecutedResult { get; private set; }

        public void Execute()
        {
            string pathToExeFile = Path.Combine(WorkingDirectoryPath, "source.exe");
            string pathToSourceCode = Path.Combine(WorkingDirectoryPath, "source.cs");
            File.WriteAllText(pathToSourceCode, SourceCode);

            var startInfo = Creator.CreateRunnerStartInfo();
            //HACH: need remove Tester.Compiler
            startInfo.ExecutableFile = CompilerHelper.Compiler;
            //HACK: need remove Tester.IsUnix
            if (CompilerHelper.IsUnix)
            {
                startInfo.Arguments = string.Format(" -o+ -out:{0} {1}", pathToExeFile, pathToSourceCode);
                switch (Language)
                {
                    case ProgrammingLanguage.CSharp:
                        break;
                    case ProgrammingLanguage.Fpc:
                        startInfo.ExecutableFile = "fpc";
                        //fpc -O2 -Xs -Sgic -viwn -Cs67107839 -Mdelphi -XS source.pas -osource.exe
                        startInfo.Arguments = string.Format(" -O2 -Xs -Sgic -viwn -Cs67107839 -Mdelphi -XS {1} -o{0}", pathToExeFile, pathToSourceCode);
                        break;
                    case ProgrammingLanguage.GPlusPlus:
                        startInfo.ExecutableFile = "g++";
                        //g++ -static -lm -s -x c++ -O2 -o {filename}.exe {file}
                        startInfo.Arguments = string.Format(" -static -lm -s -x c++ -O2 -o {0} {1}", pathToExeFile, pathToSourceCode);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch(Language)
                {
                    case ProgrammingLanguage.CSharp:
                        startInfo.Arguments = string.Format(" /nologo /out:{0} {1}", pathToExeFile, pathToSourceCode);
                        break;
                    default:
                        throw new ArgumentException("Allows only CSharp for Windows.");
                }
            }
            startInfo.WorkingTimeLimit = 30000;

            ExecuteFileJob job = new ExecuteFileJob(Runner, startInfo);
            job.Execute();
            TestExecutedResult = job.TestExecutedResult;
        }
    }
}
