using Coticula2.Connector;
using Coticula2.Tester;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Coticula2.Main.Console
{
    class Runner
    {
        ApplicationArguments Arguments;

        public Runner(ApplicationArguments arguments)
        {
            Arguments = arguments;
        }

        public bool Run()
        {
            while (true)
            {
                int[] submits = Api.UntestedSubmits();
                System.Console.WriteLine("Untested {0} submits.", submits.Length);

                if (submits.Length > 0)
                {
                    Submit submit = Api.GetSubmit(submits[0]);
                    ReturnSubmit result = Testing(submit);

                    //Api.PostTestingResult(result);
                }

                //need create better logic
                //for contol app with ReadKey (or key press)
                Thread.Sleep(Arguments.Time * 1000);
            }
            return false;
        }


        const string PathToCompilers = @"TestData\Compilers\";
        const string PathToProblems = @"TestData\Problems";

        public ReturnSubmit Testing(Submit submit)
        {
            SimpleSolution solution = new SimpleSolution();
            solution.Id = submit.Id;
            solution.SolutionCode = submit.Solution;
            solution.Language = "FPC";
            solution.ProblemId = submit.ProblemID;

            ITester acmTester = TesterManager.CreateACMTester(PathToCompilers, PathToProblems);
            ITestingResult testingResult = acmTester.Test(solution);

            ReturnSubmit result = new ReturnSubmit();
            result.Id = submit.Id;
            result.VerdictID = VerdictToId(testingResult.Verdict);
            return result;
        }

        private static int VerdictToId(Verdict verdict)
        {
            switch (verdict)
            {
                case Verdict.Accepted:
                    return 2;
                case Verdict.CompilationError:
                    return 3;
                case Verdict.WrongAnswer:
                    return 4;
                case Verdict.RunTimeError:
                    return 47;
                case Verdict.TimeLimit:
                    return 47;
            }
            return 47;
        }
    }
}
