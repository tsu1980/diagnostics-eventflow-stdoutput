using System;
using System.Collections.Generic;
using System.Text;

namespace Mamemaki.EventFlow.HealthReporters.StdOutput.Configuration
{
    public class StdOutputHealthReporterConfiguration
    {
        public string MinReportLevel { get; set; }
        public bool SuppressEmptyMessage { get; set; }
        public bool OutputToStdError { get; set; }
        public string MessageFormat { get; set; }
    }
}
