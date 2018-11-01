using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using Newtonsoft.Json;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Configuration
{
    internal class JsonSerializerFactory : IJsonSerializerFactory
    {
        [NotNull]
        private readonly List<IConfigurationDecoder> _ConfigurationDecoders;

        public JsonSerializerFactory([NotNull] IEnumerable<IConfigurationDecoder> configurationDecoders)
        {
            if (configurationDecoders == null)
                throw new ArgumentNullException(nameof(configurationDecoders));

            _ConfigurationDecoders = configurationDecoders.ToList();
        }

        public JsonSerializer Create()
        {
            var serializer = JsonSerializer.CreateDefault();
            assume(serializer?.Converters != null);

            foreach (var decoder in _ConfigurationDecoders)
                serializer.Converters.Add(new JsonConfigurationDecoderConverter(decoder));

            return serializer;
        }
    }
}