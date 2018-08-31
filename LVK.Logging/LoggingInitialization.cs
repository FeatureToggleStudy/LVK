using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DryIoc;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.Core.Services;

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
            var destinationOptions = _Configuration["Logging/Destinations"].Value<Dictionary<string, LoggerDestinationOptions>>();
            var destinations = new List<ILoggerDestination>();
            
            if (destinationOptions != null)
            {
                foreach (var kvp in destinationOptions)
                {
                    if (!kvp.Value?.Enabled ?? true)
                        continue;
                    
                    ILoggerDestination destination;
                    switch (kvp.Key)
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

                    destinations.Add(new LoggerDestinationFilter(destination, kvp.Value.LogLevel));
                }
            }

            _Container.Register<ILoggerFactory>(Reuse.Singleton,
                Made.Of(() => new LoggerFactory(destinations, Arg.Of<IConfiguration>())));

            return Task.CompletedTask;
        }
    }
}