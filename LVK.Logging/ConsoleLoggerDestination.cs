using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using LVK.Configuration;

namespace LVK.Logging
{
    internal class ConsoleLoggerDestination : LoggerDestinationBase<LoggerDestinationOptions>
    {
        public ConsoleLoggerDestination(
            [NotNull] ITextLogFormatter textLogFormatter, [NotNull] IConfiguration configuration)
            : base(textLogFormatter, configuration)
        {
        }

        protected override void OutputLinesToLog(LogLevel level, IEnumerable<string> lines)
        {
            foreach (var output in lines)
                switch (level)
                {
                    case LogLevel.Trace:
                    case LogLevel.Debug:
                    case LogLevel.Verbose:
                    case LogLevel.Information:
                        Console.Out.WriteLine(output);
                        break;

                    case LogLevel.Warning:
                    case LogLevel.Error:
                        Console.Error.WriteLine(output);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(level), level, null);
                }
        }
        
        public override void WriteLine(string line)
        {
            lock (Lock)
                Console.Out.WriteLine(line);
        }
    }
}