using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coticula2.Connector.Tests
{
    [TestFixture]
    public class SubmitTests
    {
        [Test]
        public void TestSerialization()
        {
            const string Serialized = "{\"Id\":3,\"Time\":\"2015-04-30T23:56:26.347\",\"VerdictID\":1,\"ProblemID\":1,\"LanguageID\":1,\"Solution\":\"7878787\"}";

            Submit submit = new Submit();
            submit.Id = 3;
            submit.Time = DateTime.Parse("2015-04-30T23:56:26.347");
            submit.VerdictID = 1;
            submit.ProblemID = 1;
            submit.LanguageID = 1;
            submit.Solution = "7878787";
            string json = JsonConvert.SerializeObject(submit);

            Assert.AreEqual(Serialized, json);
        }

        [Test]
        public void TestDeserialization()
        {
            const string Serialized = "{\"Id\":3,\"Time\":\"2015-04-30T23:56:26.347\",\"VerdictID\":1,\"ProblemID\":1,\"LanguageID\":1,\"Solution\":\"7878787\"}";

            Submit submit = new Submit();
            submit.Id = 3;
            submit.Time = DateTime.Parse("2015-04-30T23:56:26.347");
            submit.VerdictID = 1;
            submit.ProblemID = 1;
            submit.LanguageID = 1;
            submit.Solution = "7878787";
            Submit desSubmit = JsonConvert.DeserializeObject<Submit>(Serialized);

            Assert.AreEqual(desSubmit.Id, submit.Id);
            Assert.AreEqual(desSubmit.Time, submit.Time);
            Assert.AreEqual(desSubmit.VerdictID, submit.VerdictID);
            Assert.AreEqual(desSubmit.ProblemID, submit.ProblemID);
            Assert.AreEqual(desSubmit.LanguageID, submit.LanguageID);
            Assert.AreEqual(desSubmit.Solution, submit.Solution);
        }

        [Test]
        public void TestSerializationAndDeserialization()
        {
            Submit submit = new Submit();
            submit.Id = 3;
            submit.Time = DateTime.Parse("2015-04-30T23:56:26.347");
            submit.VerdictID = 1;
            submit.ProblemID = 1;
            submit.LanguageID = 1;
            submit.Solution = "7878787";
            string json = JsonConvert.SerializeObject(submit);

            Submit desSubmit = JsonConvert.DeserializeObject<Submit>(json);

            Assert.AreEqual(desSubmit.Id, submit.Id);
            Assert.AreEqual(desSubmit.Time, submit.Time);
            Assert.AreEqual(desSubmit.VerdictID, submit.VerdictID);
            Assert.AreEqual(desSubmit.ProblemID, submit.ProblemID);
            Assert.AreEqual(desSubmit.LanguageID, submit.LanguageID);
            Assert.AreEqual(desSubmit.Solution, submit.Solution);
        }
    }
}
