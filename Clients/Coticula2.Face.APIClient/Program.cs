﻿using Coticula2;
using Coticula2.Face.Models;
using Protex;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coticula2.Face.APIClient
{
    class Program
    {
        static void Main(string[] args)
        {
            bool needNextTest = true;
            while (needNextTest)
            {
                needNextTest = false;
                var client = new RestClient("http://192.168.1.2:5000/");

                var request = new RestRequest();
                request.Resource = "api/SubmitsApi";
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
                        request.Resource = "api/SubmitsApi/{id}";
                        request.AddUrlSegment("id", untestedIds[0].ToString());
                        IRestResponse<Submit> submitResponse = client.Execute<Submit>(request);
                        Submit submit = submitResponse.Data;
                        Console.WriteLine("{0}:", untestedIds[0]);
                        Console.WriteLine("Submit time: {0}", submit.SubmitTime);
                        Console.WriteLine("Problem ID: {0}", submit.ProblemID);

                        //test
                        IRunner runner = Protex.Windows.Creator.CreateRunner();
                        Tester tester = new Tester(runner);
                        Language language = Language.CSharp;
                        switch (submit.ProgrammingLanguageID)
                        {
                            case 1:
                                language = Language.CSharp;
                                break;
                            case 2:
                                language = Language.Fpc;
                                break;
                            case 3:
                                language = Language.GPlusPlus;
                                break;
                            default:
                                break;
                        }
                        string solution = submit.Solution;
                        var testingResult = tester.Test(submit.ProblemID, solution, language);
                        if (testingResult.CompilationVerdict == Verdict.CopilationError)
                        {
                            Console.WriteLine("Compilation output:{0}{1}", Environment.NewLine, testingResult.CompilationOutput);
                            submit.VerdictId = 3;
                        }
                        else
                        {
                            submit.VerdictId = 2;
                            for (int i = 0; i < testingResult.TestVerdicts.Length; i++)
                            {
                                int currentStatus = 1;
                                switch (testingResult.TestVerdicts[i])
                                {
                                    case Verdict.Accepted:
                                        currentStatus = 2;
                                        break;
                                    case Verdict.CopilationError:
                                        currentStatus = 3;
                                        break;
                                    case Verdict.WrongAnswer:
                                        currentStatus = 4;
                                        break;
                                    case Verdict.RunTimeError:
                                        currentStatus = 7;
                                        break;
                                    case Verdict.TimeLimit:
                                        currentStatus = 5;
                                        break;
                                    default:
                                        break;
                                }
                                submit.VerdictId = Math.Max(submit.VerdictId, currentStatus);
                                Console.WriteLine("Verdict {0}: {1}", i, testingResult.TestVerdicts[i]);
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
            }
        }
    }
}