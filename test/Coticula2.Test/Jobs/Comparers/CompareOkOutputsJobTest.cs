using Coticula2.Jobs.Comparers;
using NUnit.Framework;

namespace Coticula2.Test.Jobs.Comparers
{
    [TestFixture]
    public class CompareOkOutputsJobTest
    {
        [TestCase("ok")]
        [TestCase("OK")]
        [TestCase("Ok")]
        [TestCase("oK")]
        public void BaseLogic(string s)
        {
            CompareOkOutputJob job = new CompareOkOutputJob(s);
            job.Execute();
            Assert.That(job.EqualOutputs, Is.True);
        }

        [TestCase("ok ")]
        [TestCase("OK ")]
        [TestCase("Ok         ")]
        [TestCase("oK  ")]
        [TestCase("ok                ")]
        [TestCase("ok\n")]
        [TestCase("ok")]
        public void LogicWithTrimEnd(string s)
        {
            CompareOkOutputJob job = new CompareOkOutputJob(s);
            job.Execute();
            Assert.That(job.EqualOutputs, Is.True);
        }

        [TestCase("bad")]
        [TestCase("not ok")]
        [TestCase(" ok")]
        [TestCase("ok.")]
        [TestCase("ok!")]
        [TestCase("ook")]
        [TestCase("ookk")]
        public void NotEqual(string s)
        {
            CompareOkOutputJob job = new CompareOkOutputJob(s);
            job.Execute();
            Assert.That(job.EqualOutputs, Is.False);
        }

        [TestCase("bad ")]
        public void NotEqualWithTrimEnd(string s)
        {
            CompareOkOutputJob job = new CompareOkOutputJob(s);
            job.Execute();
            Assert.That(job.EqualOutputs, Is.False);
        }
    }
}
