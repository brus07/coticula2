using Coticula2.Problem;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Coticula2.Test.Problem
{
    [TestFixture]
    public class ProblemManagerTests
    {
        [Test]
        public void TestCreateProblem()
        {
            IProblem problem = ProblemManager.CreateProblem(@"..\..\..\TestData\Problems\Problem1");
            Assert.NotNull(problem);
            Assert.AreEqual(4, problem.Tests.Length);
            foreach (var test in problem.Tests)
            {
                Assert.IsNotNullOrEmpty(test.Input);
                Assert.IsNotNullOrEmpty(test.Output);
            }
            Assert.AreEqual("1 1", problem.Tests[0].Input.Trim());
            Assert.AreEqual("1 1", problem.Tests[0].Output.Trim());
        }

        [Test]
        [ExpectedException(typeof(InvalidDataException))]
        public void TestCreateProblemFromIncorrectFileStructure()
        {
            IProblem problem = ProblemManager.CreateProblem(@"..\..\..\TestData\Problems\ProblemBadTest");
        }
    }
}
