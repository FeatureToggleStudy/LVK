using System;

using DryIoc;

using LVK.DryIoc;

namespace LVK.AppCore.Console.Daemons
{
    internal class DaemonServicesBootstrapper<T> : IServicesBootstrapper
        where T: class, IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container is null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<T>();

            container.Register<IApplicationEntryPoint, DaemonApplicationEntryPoint>();
        }
    }
}