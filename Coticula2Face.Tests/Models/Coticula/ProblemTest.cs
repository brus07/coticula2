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
    public class ProblemTest
    {
        [Test]
        public void TestIdField()
        {
            const int Id = 1235679;
            Problem verdict = new Problem();
            verdict.Id = Id;
            Assert.AreEqual(Id, verdict.Id);
        }

        [Test]
        public void TestNameField()
        {
            const string Name = "Test Name";
            Problem verdict = new Problem();
            verdict.Name = Name;
            Assert.AreEqual(Name, verdict.Name);
        }

        [Test]
        public void TestDescriptionField()
        {
            const string Description = "Test Description";
            Problem verdict = new Problem();
            verdict.Description = Description;
            Assert.AreEqual(Description, verdict.Description);
        }

        [Test]
        public void TestAllFields()
        {
            const string Name = "Test Name";
            const int Id = 1235679;
            const string Description = "Test Description";
            Problem verdict = new Problem();
            verdict.Id = Id;
            verdict.Name = Name;
            verdict.Description = Description;
            Assert.AreEqual(Id, verdict.Id);
            Assert.AreEqual(Name, verdict.Name);
            Assert.AreEqual(Description, verdict.Description);
        }
    }
}
