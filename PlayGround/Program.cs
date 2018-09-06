using Microsoft.Diagnostics.EventFlow;
using System;
using System.Diagnostics;

namespace PlayGround
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var pipeline = DiagnosticPipelineFactory.CreatePipeline("eventFlowConfig.json"))
            {
                PlayWithTrace(pipeline);

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey(intercept: true);
            }
        }

        static void PlayWithTrace(DiagnosticPipeline pipeline)
        {
            Trace.TraceWarning("EventFlow is working!");
        }
    }
}
