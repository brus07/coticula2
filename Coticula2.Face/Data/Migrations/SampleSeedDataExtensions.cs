using Coticula2.Face.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coticula2.Face.Data.Migrations
{
    public static class SampleSeedDataExtensions
    {
        public static void EnsureSeedData(this IEnumerable<ApplicationDbContext> contextList)
        //public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            //var context = serviceProvider.GetService<ApplicationDbContext>();

            var context = contextList.First();
            if (!context.Verdicts.Any())
            {
                context.Verdicts.AddRange(
                    new Verdict()
                    {
                        Name = "Waiting"
                    },
                    new Verdict()
                    {
                        Name = "Accepted"
                    },
                    new Verdict()
                    {
                        Name = "Compilation Error"
                    },
                    new Verdict()
                    {
                        Name = "Wrong Answer"
                    },
                    new Verdict()
                    {
                        Name = "Time Limit"
                    },
                    new Verdict()
                    {
                        Name = "Memory Limit"
                    },
                    new Verdict()
                    {
                        Name = "Run-time Error"
                    },
                    new Verdict()
                    {
                        Name = "Internal Error"
                    }
                );
                context.SaveChanges();
            }
            if (!context.SubmitTypes.Any())
            {
                context.SubmitTypes.AddRange(
                    new SubmitType()
                    {
                        Name = "Solution"
                    },
                    new SubmitType()
                    {
                        Name = "Test"
                    }
                );
                context.SaveChanges();
            }
            if (!context.Problems.Any())
            {
                context.Problems.AddRange(
                    new Problem()
                    {
                        Title = "Swap problem",
                        Description = "Swap two integers."
                    }
                );
                context.SaveChanges();
            }


            if (!context.ProgrammingLanguages.Any())
            {
                context.ProgrammingLanguages.AddRange(
                    new ProgrammingLanguage()
                    {
                        Name = "C#",
                    },
                    new ProgrammingLanguage()
                    {
                        Name = "FPC",
                    },
                    new ProgrammingLanguage()
                    {
                        Name = "G++",
                    },
                    new ProgrammingLanguage()
                    {
                        Name = "Text",
                    }
                );
                context.SaveChanges();
            }

            if (!context.Submits.Any())
            {
                context.Submits.AddRange(
                    new Submit()
                    {
                        //compilation error
                        Solution = @"var",
                        SubmitTime = DateTime.Now,
                        ProblemID = 1,
                        ProgrammingLanguageID = 2, //C# 
                        VerdictId = 1,
                        SubmitTypeId = 1
                    },
                    new Submit()
                    {
                        //wrong answer
                        Solution = @"
var a,b:integer;
begin
    read(a, b);
    write(a, ' ', b);
end.",
                        SubmitTime = DateTime.Now,
                        ProblemID = 1,
                        ProgrammingLanguageID = 2, //FPC
                        VerdictId = 1,
                        SubmitTypeId = 1
                    },
                    new Submit()
                    {
                        Solution = @"
var a,b:integer;
begin
    read(a, b);
    write(b, ' ', a);
end.",
                        SubmitTime = DateTime.Now,
                        ProblemID = 1,
                        ProgrammingLanguageID = 2, //FPC
                        VerdictId = 1,
                        SubmitTypeId = 1
                    },
                    new Submit()
                    {
                        Solution = @"
using System;

public class Swap
{
    private static void Main()
    {
        string[] tokens = Console.ReadLine().Split();
        Console.WriteLine(""{0} {1}"", int.Parse(tokens[1]), int.Parse(tokens[0]));
                    }
}",
                        SubmitTime = DateTime.Now,
                        ProblemID = 1,
                        ProgrammingLanguageID = 1, //C#
                        VerdictId = 1,
                        SubmitTypeId = 1
                    },
                    new Submit()
                    {
                        Solution = @"
#include <cstdio>

int main()
{
	int a,b;
	scanf(""%d%d"", &a, &b);
	printf(""%d %d"", b, a);
	return 0;
}",
                        SubmitTime = DateTime.Now,
                        ProblemID = 1,
                        ProgrammingLanguageID = 3, //G++
                        VerdictId = 1,
                        SubmitTypeId = 1
                    },
                    new Submit()
                    {
                        //wrong answer
                        Solution = @"
#include <cstdio>

int main()
{
	int a,b;
	scanf(""%d%d"", &a, &b);
	printf(""%d %d"", b+1, a+1);
	return 0;
}",
                        SubmitTime = DateTime.Now,
                        ProblemID = 1,
                        ProgrammingLanguageID = 3, //G++
                        VerdictId = 1,
                        SubmitTypeId = 1
                    },
                    new Submit()
                    {
                        //time limit
                        Solution = @"
#include <cstdio>

int main()
{
	int a,b;
	scanf(""%d%d"", &a, &b);
    while(true);
	return 0;
}",
                        SubmitTime = DateTime.Now,
                        ProblemID = 1,
                        ProgrammingLanguageID = 3, //G++
                        VerdictId = 1,
                        SubmitTypeId = 1
                    },
                    new Submit()
                    {
                        //run-time error
                        Solution = @"
#include <cstdio>

int main()
{
	return 47;
}",
                        SubmitTime = DateTime.Now,
                        ProblemID = 1,
                        ProgrammingLanguageID = 3, //G++
                        VerdictId = 1,
                        SubmitTypeId = 1
                    }
                    //TODO: add Submits for Test type
                );
                context.SaveChanges();
            }
        }
    }
}
