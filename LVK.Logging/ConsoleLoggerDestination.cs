using System;

using JetBrains.Annotations;

namespace LVK.Logging
{
    internal class ConsoleLoggerDestination : ILoggerDestination
    {
        [NotNull]
        private readonly ITextLogFormatter _TextLogFormatter;

        [NotNull]
        private readonly object _Lock = new object();

        public ConsoleLoggerDestination([NotNull] ITextLogFormatter textLogFormatter)
        {
            _TextLogFormatter = textLogFormatter ?? throw new ArgumentNullException(nameof(textLogFormatter));
        }

        public void Log(LogLevel level, string message)
        {
            foreach (var output in _TextLogFormatter.Format(level, message))
                lock (_Lock)
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
    }
}