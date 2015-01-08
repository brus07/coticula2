using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coticula2.Tester
{
    public static class TesterManager
    {
        public static ITester CreateACMTester(string pathToCompilers, string pathToProblems)
        {
            ITester tester = new ACMTester(pathToCompilers, pathToProblems);
            return tester;
        }
    }
}
