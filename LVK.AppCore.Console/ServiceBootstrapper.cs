using System;
using System.Linq;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

using Microsoft.Extensions.Logging;

namespace LVK.AppCore.Console
{
    [UsedImplicitly]
    internal class ServiceBootstrapper : IServiceBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.Register<IConsoleApplicationEntryPoint, ConsoleApplicationEntryPoint>();
            container.UseInstance<ILoggerFactory>(new LoggerFactory());
            container.Register(typeof(ILogger<>), typeof(Logger<>), Reuse.Singleton);

            var minLevel = LogLevel.Information;
            if (Environment.GetCommandLineArgs().Contains("--debug"))
                minLevel = LogLevel.Debug;
            else if (Environment.GetCommandLineArgs().Contains("--trace"))
                minLevel = LogLevel.Trace;
            
            container.Resolve<ILoggerFactory>().AddConsole(minLevel, true).AddDebug(LogLevel.Debug);
        }
    }
}