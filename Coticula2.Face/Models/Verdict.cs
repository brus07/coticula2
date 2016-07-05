using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coticula2.Face.Models
{
    public class Verdict
    {
        public const int Waiting = 1;

        public int VerdictID { get; set; }

        public string Name { get; set; }
    }
}
