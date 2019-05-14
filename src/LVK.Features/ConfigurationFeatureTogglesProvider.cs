using System;

using JetBrains.Annotations;

using LVK.Configuration;

using Newtonsoft.Json.Linq;

namespace LVK.Features
{
    internal class ConfigurationFeatureTogglesProvider : IFeatureTogglesProvider
    {
        [NotNull]
        private readonly IConfigurationElementWithDefault<FeatureTogglesConfiguration> _Configuration;

        public ConfigurationFeatureTogglesProvider([NotNull] IConfiguration configuration)
        {
            _Configuration = configuration.Element<FeatureTogglesConfiguration>("Features")
               .WithDefault(() => new FeatureTogglesConfiguration());
        }

        public bool? IsEnabled(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            var configuration = _Configuration.Value();

            if (configuration.Root is null)
                return null;

            string[] keyParts = key.Split('/', '.');

            JToken current = configuration.Root;
            foreach (string part in keyParts)
            {
                JToken wildcard = current["*"];
                if (wildcard != null)
                {
                    return wildcard.Value<bool>();
                }

                JToken child;
                if (current is JObject obj)
                    child = obj.GetValue(part, StringComparison.InvariantCultureIgnoreCase);
                else
                    child = current[part];

                if (child == null)
                    return null;

                current = child;
            }

            return current.Value<bool>();
        }
    }
}