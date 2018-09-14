using System;

using DryIoc;

using JetBrains.Annotations;

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

            container.Bootstrap<LVK.Core.Services.ServicesBootstrapper>();

            container.Register<IBackgroundServicesManager, BackgroundServicesManager>(Reuse.Singleton);
        }
    }
}