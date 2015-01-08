using Coticula2.Tester;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coticula2.Test.Tester
{
    [TestFixture]
    public class MainTesterTests
    {
        const string PathToCompilers = @"..\..\..\TestData\Compilers\";
        const string PathToProblems = @"..\..\..\TestData\Problems";

        [Test]
        public void TestAccepted()
        {
            string solutionCode = @"var
   a, b: integer;
begin
   readln(a, b);
   writeln(b, ' ', a);
end.";
            ITestingResult testingResult = TestTestingProcess(solutionCode, Verdict.Accepted);
        }

        [Test]
        public void TestWrondAnswer()
        {
            string solutionCode = @"var
   a, b: integer;
begin
   readln(a, b);
   writeln(a, ' ', b);
end.";
            ITestingResult testingResult = TestTestingProcess(solutionCode, Verdict.WrongAnswer);
        }

        [Test]
        public void TestRunTimeError()
        {
            string solutionCode = @"var
   a, b: integer;
begin
   readln(a, b);
   writeln(b, ' ', a);
   a := a div b;
end.";
            ITestingResult testingResult = TestTestingProcess(solutionCode, Verdict.RunTimeError);
        }

        [Test]
        public void TestTimeLimit()
        {
            string solutionCode = @"begin
   while true do ;
end.";
            ITestingResult testingResult = TestTestingProcess(solutionCode, Verdict.TimeLimit);
            Assert.Less(1 * 1000, testingResult.MaximumWorkingTime);
        }

        [Test]
        public void TestTimeLimit2()
        {
            string solutionCode = @"var
   a, b: integer;
begin
   readln(a, b);
   writeln(b, ' ', a);
   if b > 400 then
      while true do ;
end.";
            ITestingResult testingResult = TestTestingProcess(solutionCode, Verdict.TimeLimit);
            Assert.Less(1 * 1000, testingResult.MaximumWorkingTime);
        }

        [Test]
        public void TestCompilationError()
        {
            string solutionCode = "b";
            ITestingResult testingResult = TestTestingProcess(solutionCode, Verdict.CompilationError);
            Assert.IsNotNullOrEmpty(testingResult.CompilationOutput);
        }

        private static ITestingResult TestTestingProcess(string sourceCode, Verdict expectedVerdict)
        {
            SimpleSolution solution = new SimpleSolution();
            solution.Id = 47;
            solution.SolutionCode = sourceCode;
            solution.Language = "FPC";
            solution.ProblemId = 1;

            ITester acmTester = TesterManager.CreateACMTester(PathToCompilers, PathToProblems);
            ITestingResult testingResult = acmTester.Test(solution);

            Assert.NotNull(testingResult);
            Assert.AreEqual(solution.Id, testingResult.Id);
            Assert.AreEqual(expectedVerdict, testingResult.Verdict);
            return testingResult;
        }
    }
}
