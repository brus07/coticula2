﻿using System;
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

        /// <summary>
        /// Worked time in milliseconds.
        /// </summary>
        public int WorkingTime { get; set; }

        /// <summary>
        /// Maximum used memory in KiB.
        /// </summary>
        public long PeakMemoryUsed { get; set; }
    }
}
