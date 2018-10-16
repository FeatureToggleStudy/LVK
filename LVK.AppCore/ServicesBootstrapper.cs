using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.AppCore.Commands;
using LVK.Commands;
using LVK.Core.Services;
using LVK.DryIoc;

namespace LVK.AppCore
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<LVK.Configuration.ServicesBootstrapper>();
            container.Bootstrap<LVK.Logging.ServicesBootstrapper>();
            container.Bootstrap<LVK.Reflection.ServicesBootstrapper>();
            container.Bootstrap<LVK.Commands.ServicesBootstrapper>();
            container.Bootstrap<LVK.Core.Services.ServicesBootstrapper>();

            container.Register<IBackgroundServicesManager, BackgroundServicesManager>(Reuse.Singleton);
            container.Register<IBackgroundService, AssemblyLoadMonitorBackgroundService>();
            container.Register<ICommandHandler<StopApplicationCommand>, StopApplicationCommandHandler>();
            
            container.Register<IBackgroundService, PidFileBackgroundService>();
            container.Register<IBackgroundService, PidQuitFileMonitorBackgroundService>();
        }
    }
}