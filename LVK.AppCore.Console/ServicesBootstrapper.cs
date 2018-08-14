using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

using Microsoft.Extensions.Logging;

namespace LVK.AppCore.Console
{
    [UsedImplicitly]
    internal class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<LVK.AppCore.ServicesBootstrapper>();
            
            container.Register<IConsoleApplicationEntryPoint, ConsoleApplicationEntryPoint>();
            container.UseInstance<ILoggerFactory>(new LoggerFactory());
            container.Register<ILogger, Logger<ConsoleApplicationEntryPoint>>(Reuse.Singleton);
            container.Register(typeof(ILogger<>), typeof(Logger<>), Reuse.Singleton);

            var minLevel = LogLevel.Warning;

            foreach (var arg in Environment.GetCommandLineArgs())
            {
                switch (arg)
                {
                    case "--trace":
                        minLevel = LogLevel.Trace;
                        break;
                    
                    case "--debug":
                        minLevel = LogLevel.Debug;
                        break;
                    
                    case "--info":
                        minLevel = LogLevel.Information;
                        break;
                }
            }

            container.Resolve<ILoggerFactory>().AddConsole(minLevel, true).AddDebug(LogLevel.Debug);
        }
    }
}