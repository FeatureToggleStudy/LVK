using System;

using JetBrains.Annotations;

using Newtonsoft.Json;

namespace LVK.Configuration
{
    internal class JsonConfigurationDecoderConverter : JsonConverter
    {
        [NotNull]
        private readonly IConfigurationDecoder _Decoder;

        public JsonConfigurationDecoderConverter([NotNull] IConfigurationDecoder decoder)
        {
            _Decoder = decoder ?? throw new ArgumentNullException(nameof(decoder));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override object ReadJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader?.Value is null)
                return null;

            return _Decoder.Decode(reader.Value);
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == null)
                return false;

            return _Decoder.CanDecode(objectType);
        }
    }
}