using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mamemaki.EventFlow.HealthReporters.StdOutput.Configuration
{
    internal static class StdOutputHeathReporterExtensions
    {
        public static StdOutputHealthReporterConfiguration ToStdOutputHealthReporterConfiguration(this IConfiguration configuration)
        {
            Validation.Requires.NotNull(configuration, nameof(configuration));
            StdOutputHealthReporterConfiguration boundConfiguration = new StdOutputHealthReporterConfiguration();
            configuration.Bind(boundConfiguration);
            return boundConfiguration;
        }
    }
}
