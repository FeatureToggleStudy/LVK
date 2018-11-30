using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using Newtonsoft.Json;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Configuration
{
    internal class JsonConfigurationDecoderConverter : JsonConverter
    {
        [NotNull, ItemNotNull]
        private readonly List<IConfigurationDecoder> _Decoders;

        public JsonConfigurationDecoderConverter([NotNull] IEnumerable<IConfigurationDecoder> decoders)
        {
            if (decoders == null)
                throw new ArgumentNullException(nameof(decoders));

            _Decoders = decoders.ToList();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader?.Value is null)
                return null;

            assume(objectType != null);

            var value = reader.Value;
            foreach (var decoder in _Decoders)
                value = decoder.Decode(value);

            return value;
        }

        public override bool CanConvert(Type objectType) => objectType == _Decoders[0].SupportedType;
    }
}