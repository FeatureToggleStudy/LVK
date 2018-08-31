using System;

using JetBrains.Annotations;

namespace LVK.Logging
{
    internal class LoggerDestinationFilter : ILoggerDestination
    {
        [NotNull]
        private readonly ILoggerDestination _Destination;

        private readonly LogLevel _MinLogLevel;

        public LoggerDestinationFilter([NotNull] ILoggerDestination destination, LogLevel minLogLevel)
        {
            _Destination = destination ?? throw new ArgumentNullException(nameof(destination));
            _MinLogLevel = minLogLevel;
        }

        public void Log(LogLevel level, string message)
        {
            if (level < _MinLogLevel)
                return;

            _Destination.Log(level, message);
        }
    }
}
