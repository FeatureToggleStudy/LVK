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

            container.Bootstrap<LVK.Configuration.ServicesBootstrapper>();
            container.Bootstrap<LVK.NodaTime.ServicesBootstrapper>();
            container.Bootstrap<LVK.Core.Services.ServicesBootstrapper>();

            container.Register<ITextLogFormatter, TextLogFormatter>();
            container.Register<IOptionsHelpTextProvider, LoggingOptionsHelpTextProvider>();

            container.Register<ILogger, Logger>(Reuse.Singleton);
            container.Register(typeof(ILogger<>), typeof(Logger<>));

            container.Register<ILoggerDestination, ConsoleLoggerDestination>();
            container.Register<ILoggerDestination, FileLoggerDestination>();
        }
    }
}