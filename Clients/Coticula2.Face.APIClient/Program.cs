using Coticula2;
using Coticula2.Face.Models;
using Coticula2.Jobs;
using Protex;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coticula2.Face.APIClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Using {0}", typeof(Coticula2.Language).Assembly.GetName().FullName);

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

                        //test
                        IRunner runner = Protex.Windows.Creator.CreateRunner();
                        TestSolutionJob job = new TestSolutionJob(runner, submit.ProblemID, submit.Solution, language);
                        job.Execute();
                        var testingResult = job.TestingResult;
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
                                switch (testingResult.TestVerdicts[i].Verdict)
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
                                    case Verdict.TimeLimit:
                                        currentStatus = 5;
                                        break;
                                    case Verdict.MemoryLimit:
                                        currentStatus = 6;
                                        break;
                                    case Verdict.RunTimeError:
                                        currentStatus = 7;
                                        break;
                                    default:
                                        break;
                                }
                                submit.VerdictId = Math.Max(submit.VerdictId, currentStatus);
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
