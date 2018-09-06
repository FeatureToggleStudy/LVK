using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using LVK.Configuration;

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

        public void WriteLine(string line)
        {
            foreach (ILoggerDestination destination in _LoggerDestinations)
                destination.WriteLine(line);
        }

    }

    internal class Logger<T> : ILogger<T>
    {
        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly LoggerSystemOptions _Options;

        public Logger([NotNull] ILogger logger, [NotNull] IConfiguration configuration)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _Options = configuration[$"Logging/Systems/{typeof(T).Name}"].Value<LoggerSystemOptions>()
                    ?? new LoggerSystemOptions();
        }

        public void Log(LogLevel level, string message)
        {
            if (_Options.Enabled)
                _Logger.Log(level, message);
        }

        public void WriteLine(string line)
        {
            if (_Options.Enabled)
                _Logger.WriteLine(line);
        }
    }
}