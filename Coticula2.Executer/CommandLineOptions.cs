using CommandLine;
using System.Collections.Generic;

namespace Coticula2.Executer
{
    internal class CommandLineOptions
    {
        [Option('s', "solution", Required = true, HelpText = "The solution file.")]
        public string SolutionFile { get; set; }

        [Option('l', "language"/*, Required = true*/, HelpText = "The programming language id.")]
        public int Language { get; set; }

        [Option('t', "task", Required = true, HelpText = "The task id.")]
        public int TaskId { get; set; }
    }
}