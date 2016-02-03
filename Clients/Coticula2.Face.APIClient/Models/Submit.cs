using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coticula2.Face.Models
{
    public class Submit
    {
        public int SubmitID { get; set; }

        public string Solution { get; set; }

        public DateTime SubmitTime { get; set; }

        public int Status { get; set; }

        public int ProblemID { get; set; }
        public Problem Problem { get; set; }

        public int ProgrammingLanguageID { get; set; }
        public ProgrammingLanguage ProgrammingLanguage { get; set; }
    }
}
