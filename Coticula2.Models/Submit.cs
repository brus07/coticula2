using System;

namespace Coticula2.Models
{
    public class Submit
    {
        public int SubmitID { get; set; }

        public string Solution { get; set; }

        public DateTime SubmitTime { get; set; }

        public Verdict Verdict { get; set; }

        public int ProblemID { get; set; }

        public ProgrammingLanguage ProgrammingLanguage { get; set; }

        public int WorkingTime { get; set; }

        public long PeakMemoryUsed { get; set; }

        public SubmitType SubmitType { get; set; }
    }
}
