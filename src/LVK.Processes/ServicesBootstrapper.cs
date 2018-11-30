using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;
using LVK.Processes.Monitors;

namespace LVK.Processes
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<LVK.Logging.ServicesBootstrapper>();
            container.Bootstrap<LVK.Configuration.ServicesBootstrapper>();
            
            container.Register<IConsoleProcessFactory, ConsoleProcessFactory>();
            container.Register<IConsoleProcessMonitor, LoggingConsoleProcessMonitor>();
        }
    }
}