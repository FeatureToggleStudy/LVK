using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using LVK.Configuration.Layers;
using LVK.Configuration.Layers.CommandLine;
using LVK.Configuration.Layers.EnvironmentVariables;
using LVK.Configuration.Layers.Json;
using LVK.Configuration.Layers.JsonFile;

using NodaTime;

namespace LVK.Configuration
{
    internal class ConfigurationBuilder : IConfigurationBuilder
    {
        [NotNull, ItemNotNull]
        private readonly List<IConfigurationLayersProvider> _LayerProviders = new List<IConfigurationLayersProvider>();

        [NotNull]
        private readonly IClock _Clock;

        public ConfigurationBuilder([NotNull] IClock clock)
        {
            _Clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        public void AddJsonFile(string filename, Encoding encoding = null, bool isOptional = false)
            => _LayerProviders.Add(new JsonFileConfigurationLayersProvider(_Clock, filename, encoding, isOptional));

        public void AddJson(string json)
            => _LayerProviders.Add(new JsonConfigurationLayersProvider(_Clock.GetCurrentInstant(), json));

        public void AddCommandLine(string[] arguments)
            => _LayerProviders.Add(new CommandLineConfigurationLayersProvider(_Clock.GetCurrentInstant(), arguments));

        public void AddEnvironmentVariables(string prefix)
            => _LayerProviders.Add(
                new EnvironmentVariablesConfigurationLayerProvider(_Clock.GetCurrentInstant(), prefix));

        public IConfiguration Build()
        {
            var layers = from layerProvider in _LayerProviders from layer in layerProvider.Provide() select layer;
            var configurationProvider = new ConfigurationProvider(layers);
            var throttlingConfigurationProvider = new ThrottlingConfigurationProvider(
                _Clock, Duration.FromSeconds(10), configurationProvider);

            return new RootConfiguration(throttlingConfigurationProvider, string.Empty);
        }
    }
}