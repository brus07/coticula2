using Coticula2.Job;

namespace Coticula2.Jobs
{
    internal class CompareStringOutputsJob : IJob
    {
        private string FirstOutput { get; set; }
        private string SecondOutput { get; set; }

        public CompareStringOutputsJob(string firstOutput, string secondOutput)
        {
            FirstOutput = firstOutput;
            SecondOutput = secondOutput;
        }

        public bool EqualOutputs { get; private set; }

        public void Execute()
        {
            EqualOutputs = FirstOutput.TrimEnd() == SecondOutput.TrimEnd();
        }
    }
}
