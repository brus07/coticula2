using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coticula2.Tester
{
    public interface ISolution
    {
        int Id { get; set; }

        /// <summary>
        /// Solution code which must be tested.
        /// </summary>
        string SolutionCode { get; set; }

        /// <summary>
        /// Short name of programming language (FPC etc.).
        /// </summary>
        string Language { get; set; }

        int ProblemId { get; set; }
    }
}
