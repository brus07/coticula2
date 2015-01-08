using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coticula2.Tester
{
    public class SimpleSolution : ISolution
    {
        public int Id { get; set; }

        public string SolutionCode { get; set; }

        public string Language { get; set; }

        public int ProblemId { get; set; }
    }
}
