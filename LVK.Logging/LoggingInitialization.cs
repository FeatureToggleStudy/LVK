using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DryIoc;

using JetBrains.Annotations;

using LVK.Core.Services;

using Microsoft.Extensions.Configuration;

namespace LVK.Logging
{
    [UsedImplicitly]
    internal class LoggingInitialization : IApplicationInitialization
    {
        [NotNull]
        private readonly IContainer _Container;

        [NotNull]
        private readonly IConfiguration _Configuration;

        [NotNull]
        private readonly ITextLogFormatter _TextLogFormatter;

        public LoggingInitialization([NotNull] IContainer container, [NotNull] IConfiguration configuration, [NotNull] ITextLogFormatter textLogFormatter)
        {
            _Container = container ?? throw new ArgumentNullException(nameof(container));
            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _TextLogFormatter = textLogFormatter ?? throw new ArgumentNullException(nameof(textLogFormatter));
        }

        public Task Initialize(CancellationToken cancellationToken)
        {
            IConfigurationSection loggingSection = _Configuration.GetSection("Logging:Destinations");
            var destinations = new List<ILoggerDestination>();

            if (loggingSection != null)
            {
                foreach (IConfigurationSection section in loggingSection.GetChildren()
                                                       ?? Enumerable.Empty<IConfigurationSection>())
                {
                    LogLevel? minLogLevel = ParseLogLevel(section["LogLevel"]);
                    if (minLogLevel is null)
                        continue;

                    var isEnabled = ParseEnabled(section["Enabled"]) ?? true;
                    if (!isEnabled)
                        continue;

                    ILoggerDestination destination;
                    switch (section.Key)
                    {
                        case "Console":
                            destination = new ConsoleLoggerDestination(_TextLogFormatter);
                            break;

                        case "Debug":
                            destination = new DebugLoggerDestination(_TextLogFormatter);
                            break;

                        default:
                            throw new InvalidOperationException("Unknown logging destionation: {section.Key}");
                    }

                    destinations.Add(new LoggerDestinationFilter(destination, minLogLevel.Value));
                }
            }

            _Container.Register<ILoggerFactory>(Reuse.Singleton,
                Made.Of(() => new LoggerFactory(destinations, Arg.Of<IConfiguration>())));

            return Task.CompletedTask;
        }

        private bool? ParseEnabled(string value)
        {
            if (value is null)
                return null;

            if (value.ToUpper() == "YES" || value.ToUpper() == "TRUE")
                return true;

            return false;
        }

        private LogLevel? ParseLogLevel(string value)
        {
            if (value == null)
                return null;

            if (Enum.TryParse(value, out LogLevel level))
                return level;

            return null;
        }
    }
}