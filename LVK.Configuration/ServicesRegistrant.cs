using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.Configuration.StandardConfigurators;
using LVK.DryIoc;

namespace LVK.Configuration
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
            if (container is null)
                throw new ArgumentNullException(nameof(container));

            container.Register<IConfigurationBuilderFactory, ConfigurationBuilderFactory>();
            container.Register(Made.Of(r => ServiceInfo.Of<IConfigurationBuilderFactory>(), f => f.Create()));
            container.Register(Made.Of(r => ServiceInfo.Of<IConfigurationBuilder>(), f => f.Build()));
            
            container.Register<IConfigurationConfigurator, AppSettingsConfigurator>();
            container.Register<IConfigurationConfigurator, CommandLineArgumentsConfigurator>();
            container.Register<IConfigurationConfigurator, EnvironmentVariablesConfigurator>();
        }
    }
}