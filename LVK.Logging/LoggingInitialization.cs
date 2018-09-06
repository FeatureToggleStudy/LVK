using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DryIoc;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.Core.Services;
using LVK.DryIoc;

using Newtonsoft.Json.Linq;

namespace LVK.Logging
{
    [UsedImplicitly]
    internal class LoggingContainerInitializer : IContainerInitializer
    {
        [NotNull]
        private readonly IContainer _Container;

        [NotNull]
        private readonly IConfiguration _Configuration;

        [NotNull]
        private readonly ITextLogFormatter _TextLogFormatter;

        public LoggingContainerInitializer(
            [NotNull] IContainer container, [NotNull] IConfiguration configuration,
            [NotNull] ITextLogFormatter textLogFormatter)
        {
            _Container = container ?? throw new ArgumentNullException(nameof(container));
            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _TextLogFormatter = textLogFormatter ?? throw new ArgumentNullException(nameof(textLogFormatter));
        }

        public void Initialize()
        {
            var destinationOptions = _Configuration["Logging/Destinations"].Value<Dictionary<string, JObject>>();
            var destinations = new List<ILoggerDestination>();

            bool debugAdded = false;
            bool consoleAdded = false;

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
                            consoleAdded = true;
                            break;

                        case "Debug":
                            destination = new DebugLoggerDestination(_TextLogFormatter);
                            debugAdded = true;
                            break;

                        case "File":
                            destination = new FileLoggerDestination(
                                _TextLogFormatter,
                                kvp.Value?.ToObject<FileLoggerDestinationOptions>()
                             ?? new FileLoggerDestinationOptions());

                            break;

                        default:
                            throw new InvalidOperationException("Unknown logging destination: {section.Key}");
                    }

                    destinations.Add(new LoggerDestinationFilter(destination, options.LogLevel));
                }
            }

            if (!consoleAdded)
                destinations.Add(
                    new LoggerDestinationFilter(new ConsoleLoggerDestination(_TextLogFormatter), LogLevel.Information));

            if (!debugAdded)
                destinations.Add(
                    new LoggerDestinationFilter(new DebugLoggerDestination(_TextLogFormatter), LogLevel.Debug));

            _Container.Register<ILoggerFactory>(
                Reuse.Singleton, Made.Of(() => new LoggerFactory(destinations, Arg.Of<IConfiguration>())));

            _Container.Register(
                typeof(ILogger<>),
                made: Made.Of(
                    typeof(ILoggerFactory).GetMethods()
                       .First(m => (m?.IsGenericMethod ?? false) && m.Name == "CreateLogger"),
                    ServiceInfo.Of(typeof(ILoggerFactory))));
        }
    }
}