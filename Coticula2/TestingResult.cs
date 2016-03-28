using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coticula2
{
    public class TestingResult
    {
        public Verdict CompilationVerdict { get; set; }
        public string CompilationOutput { get; set; }

        public TestResult[] TestVerdicts { get; set; }
    }
}
