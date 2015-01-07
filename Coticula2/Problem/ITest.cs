using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coticula2.Problem
{
    public interface ITest
    {
        int Id { get; set; }
        string Input { get; set; }
        string Output { get; set; }
    }
}
