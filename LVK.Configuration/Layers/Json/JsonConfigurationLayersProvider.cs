using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using LVK.Configuration.Layers.Static;

using Newtonsoft.Json.Linq;

namespace LVK.Configuration.Layers.Json
{
    internal class JsonConfigurationLayersProvider : IConfigurationLayersProvider
    {
        [NotNull]
        private readonly string _Json;

        public JsonConfigurationLayersProvider([NotNull] string json)
        {
            _Json = json ?? throw new ArgumentNullException(nameof(json));
        }

        public IEnumerable<IConfigurationLayer> Provide()
        {
            var configuration = JObject.Parse(_Json);
            yield return new StaticConfigurationLayer(configuration);
        }
    }
}