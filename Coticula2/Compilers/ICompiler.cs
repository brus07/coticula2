using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coticula2.Compilers
{
    public interface ICompiler
    {
        string Language {get; set;}

        string OutputPath { get; set; }

        string OutputString { get; set; }

        bool Compile(string sourceCode);
    }
}
