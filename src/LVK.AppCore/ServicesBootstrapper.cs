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

            container.Bootstrap<Configuration.ServicesBootstrapper>();
            container.Bootstrap<Logging.ServicesBootstrapper>();
            container.Bootstrap<Reflection.ServicesBootstrapper>();
            container.Bootstrap<LVK.Commands.ServicesBootstrapper>();
            container.Bootstrap<Core.Services.ServicesBootstrapper>();
            container.Bootstrap<Features.ServicesBootstrapper>();

            container.Register<IBackgroundServicesManager, BackgroundServicesManager>(Reuse.Singleton);
            container.Register<ICommandHandler<StopApplicationCommand>, StopApplicationCommandHandler>();

            container.Register<IBackgroundService, PidFileBackgroundService>();
            container.Register<IBackgroundService, PidQuitFileMonitorBackgroundService>();

            container.Register<IApplicationDataFolder, ApplicationDataFolder>(Reuse.Singleton);
        }
    }
}