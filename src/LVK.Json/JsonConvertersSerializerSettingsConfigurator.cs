using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using Newtonsoft.Json;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Json
{
    internal class JsonConvertersSerializerSettingsConfigurator : IJsonSerializerSettingsConfigurator
    {
        [NotNull, ItemNotNull]
        private readonly List<IJsonConvertersProvider> _JsonConvertersProviders;

        public JsonConvertersSerializerSettingsConfigurator(
            [NotNull, ItemNotNull] IEnumerable<IJsonConvertersProvider> jsonConvertersProviders)
        {
            _JsonConvertersProviders = jsonConvertersProviders.ToList();
        }

        public void Configure(JsonSerializerSettings settings)
        {
            assume(settings.Converters != null);
            
            foreach (var provider in _JsonConvertersProviders)
                foreach (var converter in provider.GetJsonConverters())
                    settings.Converters.Add(converter);
        }
    }
}