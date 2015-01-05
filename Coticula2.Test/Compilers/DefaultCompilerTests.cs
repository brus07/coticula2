using Coticula2.Compilers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Coticula2.Test.Compilers
{
    [TestFixture]
    public class DefaultCompilerTests
    {
        private const string FpcCompilerPath = @"..\..\..\TestData\Compilers\fpc_2.6.4\bin\i386-win32";

        [TearDown]
        public void Cleanup()
        {
            const string tempDirectory = "Temp";
            if (Directory.Exists(tempDirectory))
            {
                string[] directories = Directory.GetDirectories(tempDirectory);
                foreach (var dir in directories)
                {
                    Directory.Delete(dir, true);
                }
            }
        }

        [Test]
        public void SimpleCorrectCompileTest()
        {
            string pascalSourceCode = @"begin end. ";
            var compilerManager = new CompilerManager(FpcCompilerPath);
            ICompiler fpc = compilerManager.CreateCompiler("FPC");
            Assert.IsTrue(fpc.Compile(pascalSourceCode), "Compilation error.");
            Assert.IsTrue(File.Exists(fpc.OutputPath));
            Assert.AreEqual(-1, fpc.OutputString.IndexOf("Fatal"));
        }

        [Test]
        public void NotCorrectCompileTest()
        {
            string pascalSourceCode = @"be gin end. ";
            var compilerManager = new CompilerManager(FpcCompilerPath);
            ICompiler fpc = compilerManager.CreateCompiler("FPC");
            Assert.IsFalse(fpc.Compile(pascalSourceCode), "Compilation error.");
            Assert.IsFalse(File.Exists(fpc.OutputPath));
            Assert.AreNotEqual(-1, fpc.OutputString.IndexOf("Fatal"));
        }

        [Test]
        public void APlusBCorrectCompileTest()
        {
            string pascalSourceCode = @"var
   a, b: integer;
begin
   readln(a, b);
   writeln(a + b);
end.";
            var compilerManager = new CompilerManager(FpcCompilerPath);
            ICompiler fpc = compilerManager.CreateCompiler("FPC");
            Assert.IsTrue(fpc.Compile(pascalSourceCode), "Compilation error.");
            Assert.IsTrue(File.Exists(fpc.OutputPath));
            Assert.AreEqual(-1, fpc.OutputString.IndexOf("Fatal"));
        }

        [Test]
        public void BigProgramCorrectCompileTest()
        {
            string pascalSourceCode = @"Program CarHireSystem;
Var
StartMiles : Integer;
StopMiles  : Integer;
TotalMiles : Integer;
StandardCharge : Real;
Surcharge : Real;
TotalCharge : Real;
Begin { CarHireSystem }
Writeln ('Enter miles at start of hire: ');
Readln (StartMiles);
While (StartMiles <> 0) Do
Begin { While }
Writeln ('Enter miles at end of hire: ');
Readln (StopMiles);
TotalMiles := StopMiles - StartMiles;
StandardCharge := TotalMiles * 0.20;
Surcharge := (TotalMiles DIV 1000) * 25;
TotalCharge := StandardCharge + Surcharge;
Writeln ('The total cost of hire £', TotalCharge:4:2);
Writeln ('Enter miles at start of hire (0 to exit): ');
Readln (StartMiles);
End; { While }
End. { CarHireSystem }";
            var compilerManager = new CompilerManager(FpcCompilerPath);
            ICompiler fpc = compilerManager.CreateCompiler("FPC");
            Assert.IsTrue(fpc.Compile(pascalSourceCode), "Compilation error.");
            Assert.IsTrue(File.Exists(fpc.OutputPath));
            Assert.AreEqual(-1, fpc.OutputString.IndexOf("Fatal"));
        }
    }
}
