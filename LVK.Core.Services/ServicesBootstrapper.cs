using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

using NodaTime;

namespace LVK.Core.Services
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.UseInstance<IClock>(SystemClock.Instance);
            container.UseInstance(DateTimeZoneProviders.Tzdb.GetSystemDefault());
            container.UseInstance<IApplicationLifetimeManager>(new ApplicationLifetimeManager());
        }
    }
}