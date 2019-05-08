using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using Newtonsoft.Json;

namespace LVK.Json
{
    internal class JsonSerializerSettingsFactory : IJsonSerializerSettingsFactory
    {
        [NotNull, ItemNotNull]
        private readonly List<IJsonSerializerSettingsConfigurator> _JsonSerializerSettingsConfigurators;

        public JsonSerializerSettingsFactory([NotNull] IEnumerable<IJsonSerializerSettingsConfigurator> jsonSerializerSettingsConfigurators)
        {
            if (jsonSerializerSettingsConfigurators == null)
                throw new ArgumentNullException(nameof(jsonSerializerSettingsConfigurators));

            _JsonSerializerSettingsConfigurators = jsonSerializerSettingsConfigurators.ToList();
        }

        public JsonSerializerSettings Create()
        {
            var settings = new JsonSerializerSettings();

            foreach (var configurator in _JsonSerializerSettingsConfigurators)
                configurator.Configure(settings);

            return settings;
        }
    }
}