using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coticula2.Tester
{
    public interface ITester
    {
        ITestingResult Test(ISolution solution);
    }
}
