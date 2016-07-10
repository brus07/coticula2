using System;
using NUnit.Framework;

namespace Coticula2.Models.Tests
{
    [TestFixture]
    public class ProgrammingLanguageTests
    {
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void TestProgrammingLanguageWithId(int id)
        {
            Assert.That(Enum.IsDefined(typeof(ProgrammingLanguage), id), Is.True);
        }

        [TestCase(ProgrammingLanguage.CSharp, 1)]
        [TestCase(ProgrammingLanguage.Fpc, 2)]
        [TestCase(ProgrammingLanguage.GPlusPlus, 3)]
        [TestCase(ProgrammingLanguage.Text, 4)]
        public void TestProgrammingLanguageWithId(ProgrammingLanguage language, int id)
        {
            Assert.AreEqual((int)language, id);
        }

        [TestCase(0)]
        [TestCase(5)]
        public void TestProgrammingLanguageWithIncorrectId(int id)
        {
            Assert.That(Enum.IsDefined(typeof(ProgrammingLanguage), id), Is.False);
        }

        [TestCase("CSharp")]
        [TestCase("Fpc")]
        [TestCase("GPlusPlus")]
        [TestCase("Text")]
        public void TestProgrammingLanguageWithName(string name)
        {
            Assert.That(Enum.IsDefined(typeof(ProgrammingLanguage), name), Is.True);
        }

        [Test]
        public void TestProgrammingLanguageNumberOfElements()
        {
            Assert.AreEqual(4, Enum.GetNames(typeof(ProgrammingLanguage)).Length);
        }
    }
}
