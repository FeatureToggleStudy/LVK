using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.Configuration.StandardConfigurators;
using LVK.DryIoc;

namespace LVK.Configuration
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container is null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<LVK.Json.ServicesBootstrapper>();
            container.Bootstrap<LVK.NodaTime.ServicesBootstrapper>();

            container.Register<IConfigurationBuilderFactory, ConfigurationBuilderFactory>();
            container.Register(Made.Of(r => ServiceInfo.Of<IConfigurationBuilderFactory>(), f => f.Create()));
            container.Register(Made.Of(r => ServiceInfo.Of<IConfigurationBuilder>(), f => f.Build()), Reuse.Singleton);
            
            container.Register<IConfigurationConfigurator, AppSettingsConfigurator>();
            container.Register<IConfigurationConfigurator, CommandLineArgumentsConfigurator>();
            container.Register<IConfigurationConfigurator, EnvironmentVariablesConfigurator>();
        }
    }
}