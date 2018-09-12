using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.Core.Services
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.Register<IApplicationLifetimeManager, ApplicationLifetimeManager>(Reuse.Singleton);
            container.Register<IBus, Bus>(Reuse.Singleton);
        }
    }
}