using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.Logging
{
    internal class Logger : ILogger
    {
        [NotNull, ItemNotNull]
        private readonly IEnumerable<ILoggerDestination> _LoggerDestinations;

        public Logger([NotNull, ItemNotNull] IEnumerable<ILoggerDestination> loggerDestinations)
        {
            _LoggerDestinations = loggerDestinations ?? throw new ArgumentNullException(nameof(loggerDestinations));
        }

        public void Log(LogLevel level, string message)
        {
            foreach (ILoggerDestination destination in _LoggerDestinations)
                destination.Log(level, message);
        }
    }
}