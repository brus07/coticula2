using Coticula2;
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
            var client = new RestClient("http://192.168.1.2:5000/");

            var request = new RestRequest();
            request.Resource = "api/SubmitsApi";
            IRestResponse<List<int>> response = client.Execute<List<int>>(request);
            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                List<int> untestedIds = response.Data;
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
                    string solution = submit.Solution;
                    var testingResult = tester.Test(submit.ProblemID, solution, language);
                    if (testingResult.CompilationVerdict == Verdict.CopilationError)
                    {
                        Console.WriteLine("Compilation output:{0}{1}", Environment.NewLine, testingResult.CompilationOutput);
                        submit.Status = 10;
                    }
                    else
                    {
                        submit.Status = 1;
                        for (int i = 0; i < testingResult.TestVerdicts.Length; i++)
                        {
                            if (testingResult.TestVerdicts[i] != Verdict.Accepted)
                            {
                                submit.Status = (i + 1) * 100;
                            }
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
                    }
                }
            }
        }
    }
}
