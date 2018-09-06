using System;

using JetBrains.Annotations;

using LVK.Configuration;

namespace LVK.Logging
{
    internal class ConsoleLoggerDestination : ILoggerDestination
    {
        [NotNull]
        private readonly ITextLogFormatter _TextLogFormatter;

        [NotNull]
        private readonly object _Lock = new object();

        [NotNull]
        private readonly LoggerDestinationOptions _Options;

        public ConsoleLoggerDestination([NotNull] ITextLogFormatter textLogFormatter, [NotNull] IConfiguration configuration)
        {
            _TextLogFormatter = textLogFormatter ?? throw new ArgumentNullException(nameof(textLogFormatter));
            _Options = configuration["Logging/Destinations/Console"].Value<LoggerDestinationOptions>()
                    ?? new LoggerDestinationOptions();
        }

        public void Log(LogLevel level, string message)
        {
            if (level < _Options.LogLevel || !_Options.Enabled)
                return;
            
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

        public void WriteLine(string line)
        {
            lock (_Lock)
                Console.Out.WriteLine(line);
        }
    }
}