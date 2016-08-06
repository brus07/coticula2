using Coticula2.Models;

namespace Coticula2
{
    public class TestingResult
    {
        public Verdict CompilationVerdict { get; set; }
        public string CompilationOutput { get; set; }

        public TestResult[] TestVerdicts { get; set; }
    }
}
