using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coticula2.Problem
{
    public interface IProblem
    {
        int Id { get; set; }
        string Name { get; set; }
        ITest[] Tests { get; set; }
    }
}
