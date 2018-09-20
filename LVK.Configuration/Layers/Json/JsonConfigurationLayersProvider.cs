using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using LVK.Configuration.Layers.Static;

using Newtonsoft.Json.Linq;

using NodaTime;

namespace LVK.Configuration.Layers.Json
{
    internal class JsonConfigurationLayersProvider : IConfigurationLayersProvider
    {
        private readonly Instant _WhenCreated;
        
        [NotNull]
        private readonly string _Json;

        public JsonConfigurationLayersProvider(Instant whenCreated, [NotNull] string json)
        {
            _WhenCreated = whenCreated;
            _Json = json ?? throw new ArgumentNullException(nameof(json));
        }

        public IEnumerable<IConfigurationLayer> Provide()
        {
            var configuration = JObject.Parse(_Json);
            yield return new StaticConfigurationLayer(_WhenCreated, configuration);
        }
    }
}