using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coticula2.Main.Console
{
    class ApplicationArguments
    {
        public bool IsWebMode { get; set; }
        public bool NeedShowHelp { get; set; }

        //Check time interval (in seconds). Default if 5.
        public int Time { get; set; }
    }
}
