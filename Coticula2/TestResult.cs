using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coticula2
{
    public class TestResult
    {
        public int TestId { get; set; }
        public Verdict Verdict { get; set; }
        public int WorkingTime { get; set; }
        public long PeakMemoryUsed { get; set; }
    }
}
