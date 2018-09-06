using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using DryIoc;

using JetBrains.Annotations;

using LVK.Core;
using LVK.DryIoc;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Configuration
{
    internal class ConfigurationInitializer : IConfigurationInitializer
    {
        [NotNull]
        private readonly IEnumerable<IConfigurationConfigurator> _ConfigurationConfigurators;

        public ConfigurationInitializer([NotNull] IEnumerable<IConfigurationConfigurator> configurationConfigurators)
        {
            _ConfigurationConfigurators = configurationConfigurators ?? throw new ArgumentNullException(nameof(configurationConfigurators));
        }

        public void Initialize(IContainer container)
        {
            var configurationBuilder = new ConfigurationBuilder();

            Assembly assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();

            var entryAssemblyLocation = assembly.NotNull().Location.NotNull();
            var entryAssemblyDirectory = Path.GetDirectoryName(entryAssemblyLocation).NotNull();
            
            configurationBuilder.SetBasePath(entryAssemblyDirectory);

            foreach (var configurator in _ConfigurationConfigurators)
                configurator.Configure(configurationBuilder);
            
            container.UseInstance(configurationBuilder.Build());
        }
    }
}