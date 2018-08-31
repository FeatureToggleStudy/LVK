using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using LVK.Configuration;

namespace LVK.Logging
{
    internal class LoggerFactory : ILoggerFactory
    {
        [NotNull]
        private readonly IEnumerable<ILoggerDestination> _LoggerDestinations;

        [NotNull]
        private readonly IConfiguration _Configuration;

        public LoggerFactory([NotNull, ItemNotNull] IEnumerable<ILoggerDestination> loggerDestinations, [NotNull] IConfiguration configuration)
        {
            _LoggerDestinations = loggerDestinations ?? throw new ArgumentNullException(nameof(loggerDestinations));
            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public ILogger CreateLogger(string systemName)
        {
            var isEnabled = IsSystemEnabled(systemName) ?? IsSystemEnabled("Default") ?? true;
            if (isEnabled)
                return new Logger(_LoggerDestinations);

            return new DummyLogger();
        }

        private bool? IsSystemEnabled(string systemName) => _Configuration[$"Logging/Systems/{systemName}/Enabled"].Value<bool?>();
    }
}