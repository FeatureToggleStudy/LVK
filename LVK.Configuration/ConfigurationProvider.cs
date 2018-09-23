using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using LVK.Configuration.Layers;
using LVK.Json;

using Newtonsoft.Json.Linq;

namespace LVK.Configuration
{
    internal class ConfigurationProvider : IConfigurationProvider
    {
        [NotNull, ItemNotNull]
        private readonly List<IConfigurationLayer> _Layers;

        [NotNull, ItemCanBeNull]
        private readonly List<JObject> _Configurations;

        private JObject _Configuration;

        [NotNull]
        private readonly object _Lock = new object();

        public ConfigurationProvider([NotNull, ItemNotNull] IEnumerable<IConfigurationLayer> layers)
        {
            if (layers == null)
                throw new ArgumentNullException(nameof(layers));

            _Layers = layers.ToList();
            _Configurations = _Layers.Select(_ => (JObject)null).ToList();
        }

        public JObject GetConfiguration()
        {
            lock (_Lock)
            {
                DetectChange();
                _Configuration = _Configuration ?? CalculateConfiguration();
                return _Configuration;
            }
        }

        private void DetectChange()
        {
            for (int index = 0; index < _Layers.Count; index++)
            {
                var layerConfiguration = _Layers[index].Configuration;
                if (ReferenceEquals(layerConfiguration, _Configurations[index]))
                    continue;

                _Configurations[index] = layerConfiguration;
                _Configuration = null;
            }
        }

        [NotNull]
        private JObject CalculateConfiguration()
        {
            var configuration = new JObject();
            foreach (var layerConfiguration in _Configurations)
                if (layerConfiguration != null)
                    JsonBuilder.Apply(layerConfiguration, configuration);

            return configuration;
        }
    }
}