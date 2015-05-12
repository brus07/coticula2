using Fclp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coticula2.Main.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new FluentCommandLineParser<ApplicationArguments>();
            parser.IsCaseSensitive = false;

            /*
            p.Setup<int>('r')
             .Callback(record => RecordID = record)
             .Required();

            p.Setup<string>('v')
             .Callback(value => NewValue = value)
             .Required();
            
            p.Setup<bool>('s', "silent")
             .Callback(silent => InSilentMode = silent)
             .SetDefault(false)
             .WithDescription("Execute operation in silent mode without feedback");
            */

            parser.Setup(arg => arg.IsWebMode)
                .As('w', "web")
                //.Callback(webmode => isWebMode = true)
                //.Required()
                .SetDefault(false);

            parser.Setup(arg => arg.Time)
                .As('t', "time")
                .SetDefault(5)
                .WithDescription("Set check time interval (in seconds). Default value is 5 seconds.");

            parser.Setup(arg => arg.NeedShowHelp)
                .As('h', "help")
                .WithDescription("Show help screen.");
            parser.Setup(arg => arg.NeedShowHelp)
                .As('?')
                .WithDescription("Show help screen.");

            parser.SetupHelp("?", "h", "help")
                .Callback(text => System.Console.WriteLine(text))
                .WithHeader("Coticula2 program. This is console ranner.");

            var result = parser.Parse(args);

            if (result.HasErrors == false)
            {
                Runner runner = new Runner(parser.Object);
                if (runner.Run())
                {

                }
                // use the instantiated ApplicationArguments object from the Object property on the parser.
                //application.Run(p.Object);
            }
            else
            {
                parser.HelpOption.ShowHelp(parser.Options);
            }
        }
    }
}
