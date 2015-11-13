using Protex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coticula2.Test.Mocks
{
    class RunnerMockResult : IResult
    {
        public string ErrorOutputString
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

        public int ExitCode
        {
            get;
            set;
        }

        public string OutputString
        {
            get;
            set;
        }

        public long PeakMemoryUsed
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

        public int WorkingTime
        {
            get;
            set;
        }
    }
}
