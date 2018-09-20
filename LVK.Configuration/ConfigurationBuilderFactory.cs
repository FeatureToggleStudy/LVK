using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using NodaTime;

namespace LVK.Configuration
{
    internal class ConfigurationBuilderFactory : IConfigurationBuilderFactory
    {
        [NotNull]
        private readonly IClock _Clock;

        [NotNull]
        private readonly List<IConfigurationConfigurator> _Configurators;

        public ConfigurationBuilderFactory(
            [NotNull] IClock clock, [NotNull] IEnumerable<IConfigurationConfigurator> configurationConfigurators)
        {
            if (configurationConfigurators == null)
                throw new ArgumentNullException(nameof(configurationConfigurators));

            _Clock = clock ?? throw new ArgumentNullException(nameof(clock));

            _Configurators = configurationConfigurators.ToList();
        }

        public IConfigurationBuilder Create()
        {
            var builder = new ConfigurationBuilder(_Clock);
            foreach (var configurator in _Configurators)
                configurator.Configure(builder);

            return builder;
        }
    }
}