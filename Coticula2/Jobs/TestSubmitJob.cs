using Coticula2.Job;
using Coticula2.Models;
using Protex;
using System;

namespace Coticula2.Jobs
{
    public class TestSubmitJob : IJob
    {
        private readonly IRunner Runner;
        private readonly Submit Submit;

        public Submit SubmitResult { get; set; }

        public TestSubmitJob(IRunner runner, Submit submit)
        {
            Runner = runner;
            Submit = submit;
        }

        public void Execute()
        {
            SubmitResult = Submit;
            if (Submit.SubmitType == SubmitType.Solution)
            {
                TestSolutionJob job = new TestSolutionJob(Runner, Submit.ProblemID, Submit.Solution, Submit.ProgrammingLanguage);
                job.Execute();
                var testingResult = job.TestingResult;
                if (testingResult.CompilationVerdict != Verdict.Accepted)
                {
                    if (testingResult.CompilationVerdict == Verdict.CopilationError)
                    {
                        Console.WriteLine("Compilation output:{0}{1}", Environment.NewLine, testingResult.CompilationOutput);
                    }
                    switch (testingResult.CompilationVerdict)
                    {
                        case Verdict.CopilationError:
                            SubmitResult.Verdict = Verdict.CopilationError;
                            break;
                        case Verdict.InternalError:
                            SubmitResult.Verdict = Verdict.InternalError;
                            break;
                    }
                }
                else
                {
                    SubmitResult.Verdict = Verdict.Accepted;
                    for (int i = 0; i < testingResult.TestVerdicts.Length; i++)
                    {
                        if (SubmitResult.Verdict < testingResult.TestVerdicts[i].Verdict)
                            SubmitResult.Verdict = testingResult.TestVerdicts[i].Verdict;
                        SubmitResult.WorkingTime = Math.Max(SubmitResult.WorkingTime, testingResult.TestVerdicts[i].WorkingTime);
                        SubmitResult.PeakMemoryUsed = Math.Max(SubmitResult.PeakMemoryUsed, testingResult.TestVerdicts[i].PeakMemoryUsed);
                    }
                }
                return;
            }
            SubmitResult.Verdict = Verdict.InternalError;
        }
    }
}
