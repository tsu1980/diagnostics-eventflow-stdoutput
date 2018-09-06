using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Mamemaki.EventFlow.HealthReporters.StdOutput.Tokenization
{
    internal class FormatToken
    {
        public FormatTokenType Type { get; set; }

        public string MatchedText { get; set; }

        public Func<HealthReportLevel, string, string, string> Writer { get; set; }

        public FormatToken(FormatTokenType type, string matchedText)
        {
            this.Type = type;
            this.MatchedText = matchedText;
            this.Writer = GetWriter();
        }

        public override string ToString()
        {
            return $"[{Type}] \"{MatchedText}\"";
        }

        private Func<HealthReportLevel, string, string, string> GetWriter()
        {
            switch (this.Type)
            {
                case FormatTokenType.Timestamp:
                    return (level, text, context) => DateTime.UtcNow.ToString(CultureInfo.CurrentCulture.DateTimeFormat.UniversalSortableDateTimePattern);
                case FormatTokenType.Context:
                    return (level, text, context) => context ?? string.Empty;
                case FormatTokenType.Level:
                    return (level, text, context) => level.ToString();
                case FormatTokenType.Message:
                    return (level, text, context) => text;
                case FormatTokenType.String:
                    return (level, text, context) => this.MatchedText;
                default:
                    Debug.Fail($"Unknown format type({this.Type})");
                    return null;
            }
        }
    }
}
