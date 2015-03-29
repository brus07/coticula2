namespace Coticula2Face.Migrations
{
    using Coticula2Face.Models.Coticula;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Coticula2Face.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Coticula2Face.Models.ApplicationDbContext";
        }

        protected override void Seed(Coticula2Face.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            context.Languages.AddOrUpdate(
                p => p.ShortName,
                new Language { Id=1, ShortName = "FPC", Name = "Free Pascal"}
            );

            context.Verdicts.AddOrUpdate(
                p => p.Id,
                new Verdict { Id = 1, Name = "In queue" },
                new Verdict { Id = 2, Name = "Accepted" },
                new Verdict { Id = 3, Name = "Compilation error" },
                new Verdict { Id = 4, Name = "Wrong answer" }
            );

            context.Problems.AddOrUpdate(
                p => p.Name,
                new Problem
                {
                    Id = 1,
                    Name = "Swap",
                    Description = @"Swap problem

Input
A and B

Output
B and A

Sample
Input
4 7
Output
7 4"
                }
            );

            context.Submits.AddOrUpdate(
                new Submit
                {
                    Time = DateTime.Now,
                    LanguageID = 1,
                    Solution = "begin end.",
                    VerdictID = 1,
                    ProblemID = 1
                },
                new Submit
                {
                    Time = DateTime.Now,
                    LanguageID = 1,
                    Solution = "begin end.",
                    VerdictID = 2,
                    ProblemID = 1
                }
             );

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
