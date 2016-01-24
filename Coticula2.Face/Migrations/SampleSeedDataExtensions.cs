using Coticula2.Face.Models;
using Microsoft.Data.Entity.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coticula2.Face.Migrations
{
    public static class SampleSeedDataExtensions
    {
        public static void EnsureSeedData(this IEnumerable<ApplicationDbContext> contextList)
        //public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            //var context = serviceProvider.GetService<ApplicationDbContext>();

            var context = contextList.First();
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

            if (!context.Submits.Any())
            {
                context.Submits.AddRange(
                    new Submit()
                    {
                        Solution = @"
var a,b:integer;
begin
    read(a, b);
    write(b, ' ', a);
end.",
                        SubmitTime = DateTime.Now,
                        ProblemID = 1
                    },
                    new Submit()
                    {
                        Solution = @"
var a,b:integer;
begin
    read(a, b);
    write(a, ' ', b);
end.",
                        SubmitTime = DateTime.Now,
                        ProblemID = 1
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
