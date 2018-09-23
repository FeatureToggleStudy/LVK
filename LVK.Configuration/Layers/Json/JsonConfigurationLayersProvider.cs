using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using LVK.Configuration.Layers.Static;

using Newtonsoft.Json.Linq;

using static LVK.Core.JetBrainsHelpers;

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
            assume(configuration != null);
            yield return new StaticConfigurationLayer(configuration);
        }
    }
}