using Coticula2.Compilers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coticula2.Test.Compilers
{
    [TestFixture]
    public class DefaultCompilerTests
    {
        [Test]
        public void DemoTest()
        {
            string pascalSourceCode = @"begin end. ";
            ICompiler fpc = CompilerManager.CreateCompiler("FPC");
            Assert.IsTrue(fpc.Compile(pascalSourceCode), "Compilation error.");
        }
    }
}
