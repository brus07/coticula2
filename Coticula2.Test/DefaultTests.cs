using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coticula2.Test
{
    [TestFixture]
    public class DefaultTests
    {
        [Test]
        public void EmptyTest()
        {
            Assert.AreEqual(5, Math.Max(3, 5));
        }
    }
}
