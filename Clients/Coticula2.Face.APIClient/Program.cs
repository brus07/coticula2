﻿using Coticula2.Jobs;
using Coticula2.Models;
using Protex;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Coticula2.Face.APIClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Using {0}", typeof(TestSolutionJob).Assembly.GetName().FullName);

            bool needNextTest = true;
            while (needNextTest)
            {
                needNextTest = false;
                var client = new RestClient(ConfigurationManager.AppSettings["FaceBaseUrl"]);

                var request = new RestRequest();
                request.Resource = "api/Submits";
                IRestResponse<List<int>> response = client.Execute<List<int>>(request);
                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    List<int> untestedIds = response.Data;
                    Console.WriteLine(DateTime.Now);
                    Console.Write("{0}: ", untestedIds.Count);
                    for (int i = 0; i < untestedIds.Count; i++)
                        Console.Write("{0}, ", untestedIds[i]);
                    Console.WriteLine();

                    if (untestedIds.Count > 0)
                    {
                        request.Resource = "api/Submits/{id}";
                        request.AddUrlSegment("id", untestedIds[0].ToString());
                        IRestResponse<Submit> submitResponse = client.Execute<Submit>(request);
                        Submit submit = submitResponse.Data;
                        Console.WriteLine("{0}:", untestedIds[0]);
                        Console.WriteLine("Submit time: {0}", submit.SubmitTime);
                        Console.WriteLine("Problem ID: {0}", submit.ProblemID);

                        var language = submit.ProgrammingLanguage;

                        //test
                        IRunner runner = Protex.Windows.Creator.CreateRunner();
                        TestSolutionJob job = new TestSolutionJob(runner, submit.ProblemID, submit.Solution, language);
                        job.Execute();
                        var testingResult = job.TestingResult;
                        if (testingResult.CompilationVerdict != Verdict.Accepted)
                        {
                            if (testingResult.CompilationVerdict == Verdict.CopilationError)
                            {
                                Console.WriteLine("Compilation output:{0}{1}", Environment.NewLine, testingResult.CompilationOutput);
                            }
                            switch(testingResult.CompilationVerdict)
                            {
                                case Verdict.CopilationError:
                                    submit.Verdict = Verdict.CopilationError;
                                    break;
                                case Verdict.InternalError:
                                    submit.Verdict = Verdict.InternalError;
                                    break;
                            }
                        }
                        else
                        {
                            submit.Verdict = Verdict.Accepted;
                            for (int i = 0; i < testingResult.TestVerdicts.Length; i++)
                            {
                                if (submit.Verdict < testingResult.TestVerdicts[i].Verdict)
                                    submit.Verdict = testingResult.TestVerdicts[i].Verdict;
                                submit.WorkingTime = Math.Max(submit.WorkingTime, testingResult.TestVerdicts[i].WorkingTime);
                                submit.PeakMemoryUsed = Math.Max(submit.PeakMemoryUsed, testingResult.TestVerdicts[i].PeakMemoryUsed);
                            }
                        }
                        //submit.Status = 1;
                        //
                        request.Method = Method.PUT;
                        request.AddJsonBody(submit);
                        IRestResponse putSubmitResponse = client.Execute(request);
                        if (putSubmitResponse.ResponseStatus == ResponseStatus.Completed)
                        {
                            Console.WriteLine("Put result correct!");
                            needNextTest = true;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect response status. ResponseStatus:{0}, ErrorMessage:{1}", response.ResponseStatus, response.ErrorMessage);
                    const string message = "Error retrieving response.  Check inner details for more info.";
                    var appException = new ApplicationException(message, response.ErrorException);
                    throw appException;
                }
            }
        }
    }
}
