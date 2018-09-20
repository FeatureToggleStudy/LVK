using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using LVK.Configuration.Layers;
using LVK.Json;

using Newtonsoft.Json.Linq;

using NodaTime;

namespace LVK.Configuration
{
    internal class ConfigurationProvider : IConfigurationProvider
    {
        [NotNull, ItemNotNull]
        private readonly List<IConfigurationLayer> _Layers;

        private JObject _Configuration;
        private Instant _LastUpdatedAt;

        [NotNull]
        private readonly object _Lock = new object();

        public ConfigurationProvider(
            [NotNull, ItemNotNull] IEnumerable<IConfigurationLayer> layers)
        {
            if (layers == null)
                throw new ArgumentNullException(nameof(layers));

            _Layers = layers.ToList();
        }

        public JObject GetConfiguration()
        {
            lock (_Lock)
            {
                DetectChange();
                if (_Configuration == null)
                    CalculateConfiguration();

                return _Configuration;
            }
        }

        public Instant LastUpdatedAt
        {
            get
            {
                lock (_Lock)
                {
                    DetectChange();
                    return _LastUpdatedAt;
                }
            }
        }

        private void DetectChange()
        {
            var lastUpdatedAt = _Layers.Max(layer => layer.LastChangedAt);
            if (lastUpdatedAt > _LastUpdatedAt)
            {
                _Configuration = null;
                _LastUpdatedAt = lastUpdatedAt;
            }
        }

        private void CalculateConfiguration()
        {
            var lastUpdatedAt = Instant.MinValue;

            var configuration = new JObject();
            foreach (var layer in _Layers)
            {
                JObject layerConfiguration = layer.GetConfiguration();
                JsonBuilder.Apply(layerConfiguration, configuration);

                lastUpdatedAt = Instant.Max(lastUpdatedAt, layer.LastChangedAt);
            }

            _LastUpdatedAt = lastUpdatedAt;
            _Configuration = configuration;
        }
    }
}