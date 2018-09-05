using System;
using System.IO;
using System.Reflection;

using DryIoc;

using JetBrains.Annotations;

using LVK.Configuration.StandardConfigurators;
using LVK.Core;
using LVK.DryIoc;

using static LVK.Core.JetBrainsHelpers;

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

            container.Register<IConfigurationInitializer, ConfigurationInitializer>();
            container.Register<IConfigurationConfigurator, AppSettingsConfigurator>();
            container.Register<IConfigurationConfigurator, CommandLineArgumentsConfigurator>();
            container.Register<IConfigurationConfigurator, EnvironmentVariablesConfigurator>();
        }
    }
}