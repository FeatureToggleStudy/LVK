using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using Microsoft.Extensions.Configuration;

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

        private bool? IsSystemEnabled(string systemName)
        {
            IConfigurationSection systemsSection = _Configuration.GetSection("Logging:Systems");
            IConfigurationSection systemSection = systemsSection?.GetSection(systemName);
            var value = systemSection?["Enabled"];
            if (string.IsNullOrWhiteSpace(value))
                return null;

            switch (value.ToUpperInvariant())
            {
                case "YES":
                    return true;

                case "NO":
                    return false;

                default:
                    return null;
            }
        }
    }
}