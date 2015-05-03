using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coticula2.Connector.Tests
{
    [TestFixture]
    public class ApiTest
    {
        [Test]
        [Ignore]
        public void TestSubmitApi()
        {
            int id = 3;
            Submit submit = Api.GetSubmit(id);
            Assert.AreEqual(id, submit.Id);
        }

        [Test]
        [Ignore]
        public void TestUntestedSubmits()
        {
            int[] submits = Api.UntestedSubmits();
            Assert.AreEqual(2, submits.Length);
        }
    }
}
