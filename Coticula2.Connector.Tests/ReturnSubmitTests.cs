using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coticula2.Connector.Tests
{
    [TestFixture]
    public class ReturnSubmitTests
    {
        [Test]
        public void TestSerialization()
        {
            const string Serialized = "{\"Id\":3,\"VerdictID\":1}";

            ReturnSubmit returnSubmit = new ReturnSubmit();
            returnSubmit.Id = 3;
            returnSubmit.VerdictID = 1;
            string json = JsonConvert.SerializeObject(returnSubmit);

            Assert.AreEqual(Serialized, json);
        }

        [Test]
        public void TestDeserialization()
        {
            const string Serialized = "{\"Id\":3,\"VerdictID\":1}";

            ReturnSubmit submit = new ReturnSubmit();
            submit.Id = 3;
            submit.VerdictID = 1;
            ReturnSubmit desSubmit = JsonConvert.DeserializeObject<ReturnSubmit>(Serialized);

            Assert.AreEqual(desSubmit.Id, submit.Id);
            Assert.AreEqual(desSubmit.VerdictID, submit.VerdictID);
        }

        [Test]
        public void TestSerializationAndDeserialization()
        {
            ReturnSubmit submit = new ReturnSubmit();
            submit.Id = 3;
            submit.VerdictID = 1;
            string json = JsonConvert.SerializeObject(submit);

            ReturnSubmit desSubmit = JsonConvert.DeserializeObject<ReturnSubmit>(json);

            Assert.AreEqual(desSubmit.Id, submit.Id);
            Assert.AreEqual(desSubmit.VerdictID, submit.VerdictID);
        }
    }
}
