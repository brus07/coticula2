using System;
using Coticula2Face.Models;
using Coticula2Face.Models.Coticula;
using Coticula2Face.Tests.Helpers;
using NUnit.Framework;

namespace Coticula2Face.Tests.Models.Coticula
{
    [TestFixture]
    public class SubmitTest : DatabaseTestClass
    {
        #region Test Fields

        [Test]
        public void TestIdField()
        {
            const int Id = 1235679;
            Submit submit = new Submit();
            submit.Id = Id;
            Assert.AreEqual(Id, submit.Id);
        }

        [Test]
        public void TestTimeField()
        {
            DateTime Time = DateTime.Now;
            Submit submit = new Submit();
            submit.Time = Time;
            Assert.AreEqual(Time, submit.Time);
        }

        [Test]
        public void TestProblemIDField()
        {
            const int ProblemID = 1;
            Submit submit = new Submit();
            submit.ProblemID = ProblemID;
            Assert.AreEqual(ProblemID, submit.ProblemID);
        }

        [Test]
        public void TestVerdictIDField()
        {
            const int VerdictID = 2;
            Submit submit = new Submit();
            submit.VerdictID = VerdictID;
            Assert.AreEqual(VerdictID, submit.VerdictID);
        }

        [Test]
        public void TestLanguageIDField()
        {
            const int LanguageID = 1;
            Submit submit = new Submit();
            submit.LanguageID = LanguageID;
            Assert.AreEqual(LanguageID, submit.LanguageID);
        }

        [Test]
        public void TestSolutionField()
        {
            const string Solution = "begin end.";
            Submit submit = new Submit();
            submit.Solution = Solution;
            Assert.AreEqual(Solution, submit.Solution);
        }

        [Test]
        public void TestAllFields()
        {
            const int Id = 1235679;
            DateTime Time = DateTime.Now;
            const int ProblemID = 1;
            const int VerdictID = 2;
            const int LanguageID = 1;
            const string Solution = "begin end.";

            Submit submit = new Submit();
            submit.Id = Id;
            submit.Time = Time;
            submit.ProblemID = ProblemID;
            submit.VerdictID = VerdictID;
            submit.LanguageID = LanguageID;
            submit.Solution = Solution;

            Assert.AreEqual(Id, submit.Id);
            Assert.AreEqual(Time, submit.Time);
            Assert.AreEqual(ProblemID, submit.ProblemID);
            Assert.AreEqual(VerdictID, submit.VerdictID);
            Assert.AreEqual(LanguageID, submit.LanguageID);
            Assert.AreEqual(Solution, submit.Solution);
        }

        #endregion

        [Test]
        public void TestLoadSubmit1()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            const int id = 1;
            const string solutionCode = "begin end.";
            const int problemId = 1;
            const int verdictId = 1;
            const int languageId = 1;

            Submit submit = db.Submits.Find(id);

            Assert.NotNull(submit);
            Assert.AreEqual(id, submit.Id);
            Assert.AreEqual(solutionCode, submit.Solution);

            Assert.AreEqual(problemId, submit.ProblemID);
            Assert.NotNull(submit.Problem);
            Assert.AreEqual(problemId, submit.Problem.Id);
            Assert.AreEqual("Swap", submit.Problem.Name);

            Assert.AreEqual(verdictId, submit.VerdictID);
            Assert.NotNull(submit.Verdict);
            Assert.AreEqual(verdictId, submit.Verdict.Id);
            Assert.AreEqual("In queue", submit.Verdict.Name);

            Assert.AreEqual(languageId, submit.LanguageID);
            Assert.NotNull(submit.Language);
            Assert.AreEqual(languageId, submit.Language.Id);
            Assert.AreEqual("FPC", submit.Language.ShortName);
        }

        [Test]
        public void TestLoadSubmit2()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            const int id = 2;
            const string solutionCode = "begin end.";
            const int problemId = 1;
            const int verdictId = 2;
            const int languageId = 1;

            Submit submit = db.Submits.Find(id);

            Assert.NotNull(submit);
            Assert.AreEqual(id, submit.Id);
            Assert.AreEqual(solutionCode, submit.Solution);

            Assert.AreEqual(problemId, submit.ProblemID);
            Assert.NotNull(submit.Problem);
            Assert.AreEqual(problemId, submit.Problem.Id);
            Assert.AreEqual("Swap", submit.Problem.Name);

            Assert.AreEqual(verdictId, submit.VerdictID);
            Assert.NotNull(submit.Verdict);
            Assert.AreEqual(verdictId, submit.Verdict.Id);
            Assert.AreEqual("Accepted", submit.Verdict.Name);

            Assert.AreEqual(languageId, submit.LanguageID);
            Assert.NotNull(submit.Language);
            Assert.AreEqual(languageId, submit.Language.Id);
            Assert.AreEqual("FPC", submit.Language.ShortName);
        }
    }
}
