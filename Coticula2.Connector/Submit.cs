using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coticula2.Connector
{
    public class Submit
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }

        public int VerdictID { get; set; }

        public int ProblemID { get; set; }

        public int LanguageID { get; set; }

        public string Solution { get; set; }
    }
}
