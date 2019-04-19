using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

using LVK.Configuration;

namespace LVK.Logging
{
    internal class DebugOutputLoggerDestination : LoggerDestinationBase<LoggerDestinationOptions>
    {
        public DebugOutputLoggerDestination(
            [NotNull] ITextLogFormatter textLogFormatter, [NotNull] IConfiguration configuration)
            : base(textLogFormatter, configuration, "Debug")
        {
        }

        protected override void OutputLinesToLog(LogLevel level, IEnumerable<string> lines)
        {
            foreach (var output in lines)
                Debug.WriteLine(output);
        }
    }
}