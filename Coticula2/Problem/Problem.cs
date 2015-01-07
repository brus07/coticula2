using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coticula2.Problem
{
    class Problem : IProblem
    {
        public int Id
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ITest[] Tests { get; set; }
    }
}
