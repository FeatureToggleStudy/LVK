using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

using NodaTime;

namespace LVK.Core.Services
{
    [PublicAPI]
    public class ServicesRegistrant : IServicesRegistrant
    {
        public void Register(IContainerBuilder containerBuilder)
        {
            if (containerBuilder is null)
                throw new ArgumentNullException(nameof(containerBuilder));
        }

        public void Register(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.UseInstance<IClock>(SystemClock.Instance);
            container.UseInstance(DateTimeZoneProviders.Tzdb.GetSystemDefault());
            container.UseInstance<IApplicationLifetimeManager>(new ApplicationLifetimeManager());
        }
    }
}