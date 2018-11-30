using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

namespace LVK.Configuration.Layers.Dynamic
{
    internal class DynamicConfigurationLayersProvider : IConfigurationLayersProvider
    {
        [NotNull, ItemNotNull]
        private readonly IConfigurationLayer[] _Layers;

        public DynamicConfigurationLayersProvider([NotNull] Func<JObject> getConfiguration)
        {
            _Layers = new IConfigurationLayer[] { new DynamicConfigurationLayer(getConfiguration) };
        }

        public IEnumerable<IConfigurationLayer> Provide() => _Layers;
    }
}