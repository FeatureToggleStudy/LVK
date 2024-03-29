using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using LVK.Configuration.Layers;
using LVK.Configuration.Layers.CommandLine;
using LVK.Configuration.Layers.Dynamic;
using LVK.Configuration.Layers.EnvironmentVariables;
using LVK.Configuration.Layers.Json;
using LVK.Configuration.Layers.JsonFile;
using LVK.Json;

using Newtonsoft.Json.Linq;

using NodaTime;

namespace LVK.Configuration
{
    internal class ConfigurationBuilder : IConfigurationBuilder
    {
        [NotNull, ItemNotNull]
        private readonly List<IConfigurationLayersProvider> _LayerProviders = new List<IConfigurationLayersProvider>();

        [NotNull]
        private readonly IClock _Clock;

        [NotNull]
        private readonly IJsonSerializerFactory _JsonSerializerFactory;

        public ConfigurationBuilder([NotNull] IClock clock, [NotNull] IJsonSerializerFactory jsonSerializerFactory)
        {
            _Clock = clock ?? throw new ArgumentNullException(nameof(clock));
            _JsonSerializerFactory =
                jsonSerializerFactory ?? throw new ArgumentNullException(nameof(jsonSerializerFactory));
        }

        public void AddJsonFile(string filename, Encoding encoding = null, bool isOptional = false)
            => _LayerProviders.Add(new JsonFileConfigurationLayersProvider(filename, encoding, isOptional));

        public void AddJson(string json) => _LayerProviders.Add(new JsonConfigurationLayersProvider(json));

        public void AddDynamic(Func<JObject> getConfiguration)
            => _LayerProviders.Add(new DynamicConfigurationLayersProvider(getConfiguration));

        public void AddCommandLine(string[] arguments)
            => _LayerProviders.Add(new CommandLineConfigurationLayersProvider(arguments));

        public void AddEnvironmentVariables(string prefix)
            => _LayerProviders.Add(new EnvironmentVariablesConfigurationLayersProvider(prefix));

        public IConfiguration Build()
        {
            var layers =
                from layerProvider in _LayerProviders
                from layer in layerProvider.Provide()
                select layer;

            var configurationProvider = new ConfigurationProvider(layers);
            var throttlingConfigurationProvider = new ThrottlingConfigurationProvider(
                _Clock, Duration.FromSeconds(1), configurationProvider);

            return new RootConfiguration(
                throttlingConfigurationProvider, string.Empty, _JsonSerializerFactory.Create());
        }
    }
}