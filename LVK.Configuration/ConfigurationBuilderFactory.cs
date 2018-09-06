using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace LVK.Configuration
{
    internal class ConfigurationBuilderFactory : IConfigurationBuilderFactory
    {
        [NotNull]
        private readonly List<IConfigurationConfigurator> _Configurators;

        public ConfigurationBuilderFactory([NotNull] IEnumerable<IConfigurationConfigurator> configurationConfigurators)
        {
            if (configurationConfigurators == null)
                throw new ArgumentNullException(nameof(configurationConfigurators));

            _Configurators = configurationConfigurators.ToList();
        }

        public IConfigurationBuilder Create()
        {
            var builder = new ConfigurationBuilder();
            foreach (var configurator in _Configurators)
                configurator.Configure(builder);

            return builder;
        }
    }
}