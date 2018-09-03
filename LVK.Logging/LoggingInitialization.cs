using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DryIoc;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.Core.Services;

using Newtonsoft.Json.Linq;

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

        public LoggingInitialization([NotNull] IContainer container, [NotNull] IConfiguration configuration,
                                     [NotNull] ITextLogFormatter textLogFormatter)
        {
            _Container = container ?? throw new ArgumentNullException(nameof(container));
            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _TextLogFormatter = textLogFormatter ?? throw new ArgumentNullException(nameof(textLogFormatter));
        }

        public Task Initialize(CancellationToken cancellationToken)
        {
            var destinationOptions = _Configuration["Logging/Destinations"].Value<Dictionary<string, JObject>>();
            var destinations = new List<ILoggerDestination>();

            if (destinationOptions != null)
            {
                foreach (KeyValuePair<string, JObject> kvp in destinationOptions)
                {
                    var options = kvp.Value?.ToObject<LoggerDestinationOptions>() ?? LoggerDestinationOptions.Default;
                    if (!options.Enabled)
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

                        case "File":
                            destination = new FileLoggerDestination(_TextLogFormatter,
                                kvp.Value?.ToObject<FileLoggerDestinationOptions>());

                            break;

                        default:
                            throw new InvalidOperationException("Unknown logging destination: {section.Key}");
                    }

                    destinations.Add(new LoggerDestinationFilter(destination, options.LogLevel));
                }
            }

            _Container.Register<ILoggerFactory>(Reuse.Singleton,
                Made.Of(() => new LoggerFactory(destinations, Arg.Of<IConfiguration>())));

            _Container.Register(typeof(ILogger<>),
                made: Made.Of(
                    typeof(ILoggerFactory).GetMethods().First(m => m.IsGenericMethod && m.Name == "CreateLogger"),
                    ServiceInfo.Of(typeof(ILoggerFactory))));

            return Task.CompletedTask;
        }
    }
}