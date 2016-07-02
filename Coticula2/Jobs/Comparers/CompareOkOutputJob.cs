using Coticula2.Job;

namespace Coticula2.Jobs.Comparers
{
    internal class CompareOkOutputJob : IJob
    {
        private string Output { get; set; }

        public CompareOkOutputJob(string output)
        {
            Output = output;
        }

        public bool EqualOutputs { get; private set; }

        public void Execute()
        {
            EqualOutputs = Output.TrimEnd().ToLower() == "ok";
        }
    }
}
