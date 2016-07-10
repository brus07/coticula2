using NUnit.Framework;
using System;

namespace Coticula2.Models.Tests
{
    [TestFixture]
    public class SubmitTypeTests
    {
        [TestCase(1)]
        [TestCase(2)]
        public void TestSubmitTypeWithId(int id)
        {
            Assert.That(Enum.IsDefined(typeof(SubmitType), id), Is.True);
        }

        [TestCase(SubmitType.Solution, 1)]
        [TestCase(SubmitType.Test, 2)]
        public void TestSubmitTypeWithId(SubmitType language, int id)
        {
            Assert.AreEqual((int)language, id);
        }

        [TestCase(0)]
        [TestCase(3)]
        public void TestSubmitTypeWithIncorrectId(int id)
        {
            Assert.That(Enum.IsDefined(typeof(SubmitType), id), Is.False);
        }

        [TestCase("Solution")]
        [TestCase("Test")]
        public void TestSubmitTypeWithName(string name)
        {
            Assert.That(Enum.IsDefined(typeof(SubmitType), name), Is.True);
        }

        [Test]
        public void TestSubmitTypeNumberOfElements()
        {
            Assert.AreEqual(2, Enum.GetNames(typeof(SubmitType)).Length);
        }
    }
}
