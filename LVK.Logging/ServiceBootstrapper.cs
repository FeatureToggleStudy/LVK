using System.Diagnostics;

using DryIoc;

using LVK.DryIoc;

namespace LVK.Logging
{
    public class ServiceBootstrapper : IServiceBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Register<ILogger, Logger>(Reuse.Singleton);
            container.Register<ILogDestination, ConsoleLogDestination>(Reuse.Singleton);
            container.Register<ILoggingOptions, LoggingOptions>(Reuse.Singleton);
            container.Register<ITextLogFormatter, TextLogFormatter>();

            if (Debugger.IsAttached)
                container.Register<ILogDestination, DebugLogDestination>(Reuse.Singleton);
        }
    }
}