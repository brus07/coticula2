using Protex;
using Protex.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Coticula2
{
    public class Tester
    {
        private IRunner runner;

        public Tester(IRunner runner)
        {
            this.runner = runner;
        }

        public TestingResult Test(int problemId, string solution, Language language)
        {
            TestingResult testingResult = new TestingResult();

            var workingTestDirectory = CreateTemporaryDirectory();
            //HACK: only for CSharp (1)
            var sourceFile = Path.Combine(workingTestDirectory, "source.cs");
            File.WriteAllText(sourceFile, solution);

            var startInfo = Creator.CreateRunnerStartInfo();
            startInfo.ExecutableFile = Compiler;
            //HACK: only for CSharp (2)
            if (IsUnix)
            {
                startInfo.Arguments = string.Format(" -o+ -out:{0} {1}", Path.Combine(workingTestDirectory, "source.exe"), Path.Combine(workingTestDirectory, "source.cs"));
                switch (language)
                {
                    case Language.CSharp:
                        break;
                    case Language.Fpc:
                        startInfo.ExecutableFile = "fpc";
                        //fpc -O2 -Xs -Sgic -viwn -Cs67107839 -Mdelphi -XS source.pas -osource.exe
                        startInfo.Arguments = string.Format(" -O2 -Xs -Sgic -viwn -Cs67107839 -Mdelphi -XS {1} -o{0}", Path.Combine(workingTestDirectory, "source.exe"), Path.Combine(workingTestDirectory, "source.cs"));
                        break;
                    case Language.GPlusPlus:
                        startInfo.ExecutableFile = "g++";
                        //g++ -static -lm -s -x c++ -O2 -o {filename}.exe {file}
                        startInfo.Arguments = string.Format(" -static -lm -s -x c++ -O2 -o {0} {1}", Path.Combine(workingTestDirectory, "source.exe"), Path.Combine(workingTestDirectory, "source.cs"));
                        break;
                    default:
                        break;
                }
            }
            else
                startInfo.Arguments = string.Format(" /nologo /out:{0} {1}", Path.Combine(workingTestDirectory, "source.exe"), Path.Combine(workingTestDirectory, "source.cs"));
            startInfo.WorkingTimeLimit = 30000;

            Console.WriteLine("Compiling {0} ...", language);
            var executedResult = runner.Run(startInfo);
            testingResult.CompilationOutput = string.Concat(executedResult.ErrorOutputString, executedResult.OutputString);
            if (executedResult.ExitCode != 0)
            {
                testingResult.CompilationVerdict = Verdict.CopilationError;
                Console.WriteLine("Verdict: {0}.", testingResult.CompilationVerdict);
                //TODO: Return compile error result
                return testingResult;
            }
            testingResult.CompilationVerdict = Verdict.Accepted;
            Console.WriteLine("Verdict: {0}.", testingResult.CompilationVerdict);

            string executedFile = Path.Combine(workingTestDirectory, "source.exe");

            string fullPathToProblem = FullPathToProblem(problemId);
            var testDirectories = Directory.GetDirectories(fullPathToProblem, "test*");
            List<TestResult> testResults = new List<TestResult>();
            int testId = 0;
            foreach (var testDirectory in testDirectories)
            {
                testId++;
                Console.WriteLine("Testing {0}/{1} ...", testId, testDirectories.Length);

                var inputFiles = Directory.GetFiles(testDirectory, "in.txt");
                if (inputFiles.Length != 1)
                {
                    throw new FileNotFoundException(Path.Combine(testDirectory, "in.txt"));
                }
                string inputFile = inputFiles[0];

                var outputFiles = Directory.GetFiles(testDirectory, "out.txt");
                if (outputFiles.Length != 1)
                {
                    throw new FileNotFoundException(Path.Combine(testDirectory, "out.txt"));
                }
                string outputFile = outputFiles[0];

                var testStartInfo = Creator.CreateRunnerStartInfo();
                //HACK: need make universal command (for different languages)
                testStartInfo.ExecutableFile = Path.Combine(workingTestDirectory, "source.exe");
                testStartInfo.InputString = File.ReadAllText(inputFile);

                var testExecutedResult = runner.Run(testStartInfo);
                Verdict currentVerdict = Verdict.Accepted;
                if (testExecutedResult.ExitCode != 0)
                    currentVerdict = Verdict.RunTimeError;

                //TODO: check for limits
                if (testExecutedResult.ExitCode == -1)
                {
                    if (testExecutedResult.WorkingTime >= testStartInfo.WorkingTimeLimit)
                        currentVerdict = Verdict.TimeLimit;
                    if (testExecutedResult.PeakMemoryUsed >= testStartInfo.MemoryLimit)
                        currentVerdict = Verdict.MemoryLimit;
                }

                //compare outputs
                if (currentVerdict == Verdict.Accepted)
                {
                    string expectedOutput = File.ReadAllText(outputFile);
                    if (expectedOutput.TrimEnd() != testExecutedResult.OutputString.TrimEnd())
                        currentVerdict = Verdict.WrongAnswer;
                }

                testResults.Add(new TestResult() { TestId = testId, Verdict = currentVerdict, WorkingTime = testExecutedResult.WorkingTime, PeakMemoryUsed = testExecutedResult.PeakMemoryUsed });
                Console.WriteLine("Verdict: {0}; Time used: {1}ms; Memory used: {2}MiB;", currentVerdict, testExecutedResult.WorkingTime, testExecutedResult.PeakMemoryUsed);
            }

            testingResult.TestVerdicts = testResults.ToArray();
            return testingResult;
        }

        private string FullPathToProblem(int id)
        {
            string res = Path.Combine("Problems", "Problem" + id);
            string toProblemsFolder = Path.Combine("..", "..", "..", "TestData");
            res = Path.Combine(toProblemsFolder, res);
            Directory.Exists(res);
            return res;
        }

        #region  Helpers

        static string CreateTemporaryDirectory()
        {
            if (!Directory.Exists("Temp"))
                Directory.CreateDirectory("Temp");
            string tempDirectory = Path.Combine("Temp", Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        static string Compiler
        {
            get
            {
                ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
                configFile.ExeConfigFilename = Path.Combine(Environment.CurrentDirectory, "Coticula2.dll.config");
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);

                AppSettingsSection section = (AppSettingsSection)config.GetSection("appSettings");
                string MySetting = section.Settings["CscCompiler"].Value;
                
                string currentCompiler = section.Settings["CscCompiler"].Value;

                //for Unix with Mono (mcs)
                if (IsUnix)
                    currentCompiler = section.Settings["DmcsCompiler"].Value;

                return currentCompiler;
            }
        }

        static bool IsUnix
        {
            get
            {
                return Environment.OSVersion.Platform == PlatformID.Unix;
            }
        }

        #endregion
    }
}
