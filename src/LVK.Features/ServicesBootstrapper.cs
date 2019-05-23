using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.Features
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<LVK.Configuration.ServicesBootstrapper>();

            container.Register<IFeatureToggles, FeatureToggles>(Reuse.Singleton);
            container.Register<IFeatureTogglesProvider, ConfigurationFeatureTogglesProvider>(Reuse.Singleton);
        }
    }
}