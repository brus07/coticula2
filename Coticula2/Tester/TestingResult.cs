using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coticula2.Tester
{
    class TestingResult: ITestingResult
    {
        public int Id { get; set; }

        public Verdict Verdict { get; set; }

        public int MaximumWorkingTime { get; set; }

        public long PeakMemoryUsed { get; set; }

        public string CompilationOutput { get; set; }
    }
}
