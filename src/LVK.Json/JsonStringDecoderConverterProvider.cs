using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using Newtonsoft.Json;

namespace LVK.Json
{
    internal class JsonStringDecoderConverterProvider : IJsonConvertersProvider
    {

        [NotNull, ItemNotNull]
        private readonly List<IJsonStringDecoder> _JsonStringDecoders;

        public JsonStringDecoderConverterProvider([NotNull, ItemNotNull] IEnumerable<IJsonStringDecoder> jsonStringDecoders)
        {
            if (jsonStringDecoders == null)
                throw new ArgumentNullException(nameof(jsonStringDecoders));

            _JsonStringDecoders = jsonStringDecoders.ToList();
        }
        
        public IEnumerable<JsonConverter> GetJsonConverters()
        {
            yield return new JsonStringDecoderConverter(_JsonStringDecoders);
        }
    }
}