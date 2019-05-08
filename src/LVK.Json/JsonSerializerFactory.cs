using System;

using JetBrains.Annotations;

using Newtonsoft.Json;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Json
{
    internal class JsonSerializerFactory : IJsonSerializerFactory
    {
        [NotNull]
        private readonly IJsonSerializerSettingsFactory _JsonSerializerSettingsFactory;

        public JsonSerializerFactory([NotNull] IJsonSerializerSettingsFactory jsonSerializerSettingsFactory)
        {
            _JsonSerializerSettingsFactory = jsonSerializerSettingsFactory ?? throw new ArgumentNullException(nameof(jsonSerializerSettingsFactory));
        }

        public JsonSerializer Create()
        {
            var serializer = JsonSerializer.Create(_JsonSerializerSettingsFactory.Create());
            assume(serializer != null);
            return serializer;
        }
    }
}