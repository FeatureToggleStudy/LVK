using System;

using DryIoc;

using LVK.DryIoc;

namespace LVK.AppCore.Console
{
    internal class DaemonServicesRegistrant<T> : IServicesRegistrant
        where T: class, IServicesRegistrant, new()
    {
        public void Register(IContainerBuilder containerBuilder)
        {
            if (containerBuilder is null)
                throw new ArgumentNullException(nameof(containerBuilder));

            containerBuilder.Register<T>();
        }

        public void Register(IContainer container)
        {
            if (container is null)
                throw new ArgumentNullException(nameof(container));

            container.Register<IApplicationEntryPoint, DaemonApplicationEntryPoint>();
        }
    }
}