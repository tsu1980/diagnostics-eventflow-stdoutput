using Mamemaki.EventFlow.HealthReporters.StdOutput.Configuration;
using Mamemaki.EventFlow.HealthReporters.StdOutput.Tokenization;
using Microsoft.Diagnostics.EventFlow;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Validation;

namespace Mamemaki.EventFlow.HealthReporters.StdOutput
{
    public class StdOutputHealthReporter : IHealthReporter
    {
        private static readonly string TraceTag = nameof(StdOutputHealthReporter);
        internal const string DefaultMessageFormat = "${Timestamp} ${Context} [${Level}] ${Message}";

        protected StdOutputHealthReporterConfiguration Configuration { get; private set; }

        private HealthReportLevel minReportLevel;
        private IEnumerable<FormatToken> formatTokens;

        /// <summary>
        /// Create a StdOutputHealthReporter with configuration.
        /// </summary>
        /// <param name="configuration">StdOutputHealthReporter configuration.</param>
        public StdOutputHealthReporter(StdOutputHealthReporterConfiguration configuration)
        {
            Initialize(configuration);
        }

        /// <summary>
        /// Create a StdOutputHealthReporter with configuration.
        /// </summary>
        /// <param name="configuration">StdOutputHealthReporter configuration.</param>
        public StdOutputHealthReporter(IConfiguration configuration)
            : this(configuration.ToStdOutputHealthReporterConfiguration())
        {
        }

        private void Initialize(StdOutputHealthReporterConfiguration configuration)
        {
            Requires.NotNull(configuration, nameof(configuration));

            // Prepare the configuration, set default values, handle invalid values.
            this.Configuration = configuration;

            this.formatTokens = new FormatTokenizer().Tokenize(Configuration.MessageFormat ?? DefaultMessageFormat);

            HealthReportLevel logLevel;
            string logLevelString = this.Configuration.MinReportLevel;
            if (string.IsNullOrWhiteSpace(logLevelString))
            {
                this.minReportLevel = HealthReportLevel.Problem;
            }
            else if (Enum.TryParse(logLevelString, out logLevel))
            {
                this.minReportLevel = logLevel;
            }
            else
            {
                this.minReportLevel = HealthReportLevel.Problem;
                // The severity has to be at least the same as the default level of problem.
                ReportProblem($"Failed to parse log level. Please check the value of: {logLevelString}. Falling back to default value: {this.minReportLevel.ToString()}", TraceTag);
            }
            this.Configuration.MinReportLevel = this.minReportLevel.ToString();
        }

        public void Dispose()
        {
        }

        public void ReportHealthy(string description = null, string context = null)
        {
            ReportText(HealthReportLevel.Healthy, description, context);
        }

        public void ReportProblem(string description, string context = null)
        {
            ReportText(HealthReportLevel.Problem, description, context);
        }

        public void ReportWarning(string description, string context = null)
        {
            ReportText(HealthReportLevel.Warning, description, context);
        }

        private void ReportText(HealthReportLevel level, string text, string context = null)
        {
            if (level < this.minReportLevel)
            {
                return;
            }

            if (Configuration.SuppressEmptyMessage && string.IsNullOrEmpty(text))
            {
                return;
            }

            string message = "";
            foreach (var token in formatTokens)
            {
                message += token.Writer(level, text, context);
            }

            if (Configuration.OutputToStdError)
            {
                Console.Error.WriteLine(message);
            }
            else
            {
                Console.WriteLine(message);
            }
        }
    }
}
