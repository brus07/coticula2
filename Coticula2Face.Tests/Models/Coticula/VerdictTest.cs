using Coticula2Face.Models.Coticula;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coticula2Face.Tests.Models.Coticula
{
    [TestFixture]
    public class VerdictTest
    {
        [Test]
        public void TestIdField()
        {
            const int Id = 1235679;
            Verdict verdict = new Verdict();
            verdict.Id = Id;
            Assert.AreEqual(Id, verdict.Id);
        }

        [Test]
        public void TestNameField()
        {
            const string Name = "Test Name";
            Verdict verdict = new Verdict();
            verdict.Name = Name;
            Assert.AreEqual(Name, verdict.Name);
        }

        [Test]
        public void TestAllFields()
        {
            const string Name = "Test Name";
            const int Id = 1235679;
            Verdict verdict = new Verdict();
            verdict.Id = Id;
            verdict.Name = Name;
            Assert.AreEqual(Id, verdict.Id);
            Assert.AreEqual(Name, verdict.Name);
        }
    }
}
