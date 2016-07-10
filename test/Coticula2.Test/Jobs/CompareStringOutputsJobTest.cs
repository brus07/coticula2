using Coticula2.Jobs;
using NUnit.Framework;

namespace Coticula2.Test.Jobs
{
    [TestFixture]
    public class CompareStringOutputsJobTest
    {
        [TestCase("123")]
        [TestCase("1234567890")]
        [TestCase("ABC")]
        [TestCase("sdfj lsdkfjlk 2j5j2l5t2 34uj024jrfsd nushtih23 21 pjrjwpiorf j")]
        public void BaseLogic(string s)
        {
            CompareStringOutputsJob job = new CompareStringOutputsJob(s, s);
            job.Execute();
            Assert.That(job.EqualOutputs, Is.True);
        }

        [TestCase("123", "123")]
        [TestCase("123 ", "123")]
        [TestCase("123", "123 ")]
        [TestCase("123 ", "123  ")]
        [TestCase("123 ", "123                ")]
        [TestCase("123 ", "123\n")]
        [TestCase("  123", "  123")]
        public void LogicWithTrimEnd(string s1, string s2)
        {
            CompareStringOutputsJob job = new CompareStringOutputsJob(s1, s2);
            job.Execute();
            Assert.That(job.EqualOutputs, Is.True);
        }

        [TestCase("321", "123")]
        [TestCase("124", "123")]
        [TestCase("asdasjldjalsd adj aslkdjda s", "asd kjasld jaksl djalksd jalsdj asldkj")]
        public void NotEqual(string s1, string s2)
        {
            CompareStringOutputsJob job = new CompareStringOutputsJob(s1, s2);
            job.Execute();
            Assert.That(job.EqualOutputs, Is.False);
        }

        [TestCase("321 ", "123")]
        public void NotEqualWithTrimEnd(string s1, string s2)
        {
            CompareStringOutputsJob job = new CompareStringOutputsJob(s1, s2);
            job.Execute();
            Assert.That(job.EqualOutputs, Is.False);
        }
    }
}
