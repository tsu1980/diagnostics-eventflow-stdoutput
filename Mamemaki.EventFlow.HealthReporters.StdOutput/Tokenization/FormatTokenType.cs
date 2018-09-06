using System;
using System.Collections.Generic;
using System.Text;

namespace Mamemaki.EventFlow.HealthReporters.StdOutput.Tokenization
{
    internal enum FormatTokenType
    {
        Timestamp,
        Context,
        Level,
        Message,
        String
    }
}
