using System;
using NUnit.Framework;

namespace Coticula2.Models.Tests
{
    [TestFixture]
    public class VerdictTests
    {
        [TestCase(Verdict.Waiting, 1)]
        [TestCase(Verdict.Accepted, 2)]
        [TestCase(Verdict.CopilationError, 3)]
        [TestCase(Verdict.WrongAnswer, 4)]
        [TestCase(Verdict.RunTimeError, 5)]
        [TestCase(Verdict.TimeLimit, 6)]
        [TestCase(Verdict.MemoryLimit, 7)]
        [TestCase(Verdict.InternalError, 8)]
        public void TestProgrammingLanguageWithId(Verdict language, int id)
        {
            Assert.AreEqual((int)language, id);
        }

        [TestCase(0)]
        [TestCase(9)]
        public void TestVerdictWithIncorrectId(int id)
        {
            Assert.That(Enum.IsDefined(typeof(Verdict), id), Is.False);
        }

        [TestCase("Accepted")]
        [TestCase("CopilationError")]
        [TestCase("WrongAnswer")]
        [TestCase("RunTimeError")]
        [TestCase("TimeLimit")]
        [TestCase("MemoryLimit")]
        [TestCase("InternalError")]
        public void TestVerdictWithName(string name)
        {
            Assert.That(Enum.IsDefined(typeof(Verdict), name), Is.True);
        }

        [Test]
        public void TestVerdictNumberOfElements()
        {
            Assert.AreEqual(8, Enum.GetNames(typeof(Verdict)).Length);
        }
    }
}
