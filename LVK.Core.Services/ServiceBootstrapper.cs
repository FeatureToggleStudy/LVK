using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

using NodaTime;

namespace LVK.Core.Services
{
    [PublicAPI]
    public class ServiceBootstrapper : IServiceBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.UseInstance<IClock>(SystemClock.Instance);
        }
    }
}