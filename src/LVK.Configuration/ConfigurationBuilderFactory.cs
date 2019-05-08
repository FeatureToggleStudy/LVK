using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using LVK.Json;

using NodaTime;

namespace LVK.Configuration
{
    internal class ConfigurationBuilderFactory : IConfigurationBuilderFactory
    {
        [NotNull]
        private readonly IClock _Clock;

        [NotNull]
        private readonly List<IConfigurationConfigurator> _Configurators;

        [NotNull]
        private readonly IJsonSerializerFactory _JsonSerializerFactory;

        public ConfigurationBuilderFactory(
            [NotNull] IClock clock, [NotNull] IEnumerable<IConfigurationConfigurator> configurationConfigurators,
            [NotNull] IJsonSerializerFactory jsonSerializerFactory)
        {
            if (configurationConfigurators == null)
                throw new ArgumentNullException(nameof(configurationConfigurators));

            _Clock = clock ?? throw new ArgumentNullException(nameof(clock));
            _JsonSerializerFactory = jsonSerializerFactory ?? throw new ArgumentNullException(nameof(jsonSerializerFactory));

            _Configurators = configurationConfigurators.ToList();
        }

        public IConfigurationBuilder Create()
        {
            var builder = new ConfigurationBuilder(_Clock, _JsonSerializerFactory);
            foreach (var configurator in _Configurators)
                configurator.Configure(builder);

            return builder;
        }
    }
}