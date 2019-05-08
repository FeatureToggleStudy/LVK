using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using Newtonsoft.Json;

namespace LVK.Json
{
    internal class JsonStringDecoderConverter : JsonConverter
    {
        [NotNull, ItemNotNull]
        private readonly List<IJsonStringDecoder> _JsonStringDecoders;

        public JsonStringDecoderConverter([NotNull, ItemNotNull] List<IJsonStringDecoder> jsonStringDecoders)
        {
            _JsonStringDecoders = jsonStringDecoders ?? throw new ArgumentNullException(nameof(jsonStringDecoders));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader?.Value == null)
                return null;

            string value = (string)reader.Value;
            if (!_JsonStringDecoders.Any())
                return value;

            for (int index = 0; index < _JsonStringDecoders.Count; index++)
            {
                string original = value;

                foreach (var decoder in _JsonStringDecoders)
                    value = decoder.Decode(value);

                if (value == original)
                    break;
            }

            return value;
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(string);
    }
}