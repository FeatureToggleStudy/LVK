using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.Core.Services;
using LVK.DryIoc;

namespace LVK.Logging
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container is null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<Configuration.ServicesBootstrapper>();
            container.Bootstrap<NodaTime.ServicesBootstrapper>();
            container.Bootstrap<Core.Services.ServicesBootstrapper>();

            container.Register<ITextLogFormatter, TextLogFormatter>();
            container.Register<IOptionsHelpTextProvider, LoggingOptionsHelpTextProvider>();

            container.Register<ILogger, Logger>(Reuse.Singleton);

            container.Register<ILoggerDestination, ConsoleLoggerDestination>();
            container.Register<ILoggerDestination, FileLoggerDestination>();
            container.Register<ILoggerDestination, DebugOutputLoggerDestination>();
        }
    }
}