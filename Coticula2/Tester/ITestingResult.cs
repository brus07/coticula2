using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coticula2.Tester
{
    public interface ITestingResult
    {
        int Id { get; set; }

        Verdict Verdict { get; set; }

        int MaximumWorkingTime { get; set; }

        long PeakMemoryUsed { get; set; }

        string CompilationOutput { get; set; }

        //field for tests result
    }
}
