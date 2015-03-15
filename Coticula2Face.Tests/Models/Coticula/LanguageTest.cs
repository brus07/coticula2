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
    public class LanguageTest
    {
        [Test]
        public void TestIdField()
        {
            const int Id = 1235679;
            Language verdict = new Language();
            verdict.Id = Id;
            Assert.AreEqual(Id, verdict.Id);
        }

        [Test]
        public void TestShortNameField()
        {
            const string ShortName = "Test Short Name";
            Language verdict = new Language();
            verdict.ShortName = ShortName;
            Assert.AreEqual(ShortName, verdict.ShortName);
        }

        [Test]
        public void TestNameField()
        {
            const string Name = "Test Name";
            Language verdict = new Language();
            verdict.Name = Name;
            Assert.AreEqual(Name, verdict.Name);
        }

        [Test]
        public void TestAllFields()
        {
            const string ShortName = "Test Short Name";
            const string Name = "Test Name";
            const int Id = 1235679;
            Language verdict = new Language();
            verdict.Id = Id;
            verdict.ShortName = ShortName;
            verdict.Name = Name;
            Assert.AreEqual(Id, verdict.Id);
            Assert.AreEqual(ShortName, verdict.ShortName);
            Assert.AreEqual(Name, verdict.Name);
        }
    }
}
