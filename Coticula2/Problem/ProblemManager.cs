using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Coticula2.Problem
{
    public static class ProblemManager
    {
        private const string TestFolderPrefix = "test";
        private const string InputFile = "in.txt";
        private const string OutputFile = "out.txt";

        public static IProblem CreateProblem(string path)
        {
            string[] testDirectories = Directory.GetDirectories(path, TestFolderPrefix+"*");
            List<ITest> testsList = new List<ITest>();
            foreach (var testDirectory in testDirectories)
            {
                string inputFilePath = Path.Combine(testDirectory, InputFile);
                string outputFilePath = Path.Combine(testDirectory, OutputFile);
                if (File.Exists(inputFilePath) && File.Exists(outputFilePath))
                {
                    ITest test = new Test();
                    test.Input = File.ReadAllText(inputFilePath);
                    test.Output = File.ReadAllText(outputFilePath);
                    testsList.Add(test);
                }
                else
                {
                    throw new InvalidDataException(string.Format("Thera now input or output file in \"{0}\" folder", testDirectories));
                }
            }
            IProblem problem = new Problem();
            problem.Tests = testsList.ToArray();
            return problem;
        }
    }
}
