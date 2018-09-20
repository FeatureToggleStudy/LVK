using System;
using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

using LVK.Configuration.Layers.Static;
using LVK.Json;

using Newtonsoft.Json.Linq;

using NodaTime;

namespace LVK.Configuration.Layers.EnvironmentVariables
{
    internal class EnvironmentVariablesConfigurationLayerProvider : IConfigurationLayersProvider
    {
        private readonly Instant _WhenCreated;

        [NotNull]
        private readonly string _Prefix;

        public EnvironmentVariablesConfigurationLayerProvider(Instant whenCreated, [NotNull] string prefix)
        {
            _WhenCreated = whenCreated;
            _Prefix = prefix ?? throw new ArgumentNullException(nameof(prefix));
        }

        public IEnumerable<IConfigurationLayer> Provide()
        {
            IDictionary environmentVariables = Environment.GetEnvironmentVariables();

            var configuration = new JObject();
            foreach (string key in environmentVariables.Keys)
            {
                if (!key.StartsWith(_Prefix, StringComparison.InvariantCultureIgnoreCase))
                    continue;

                var path = key.Substring(_Prefix.Length);
                var value = JsonBuilder.ValueFromString(environmentVariables[key]?.ToString() ?? string.Empty);

                var variableConfiguration = JsonBuilder.Construct(path.Split('/'), value);
                JsonBuilder.Apply(variableConfiguration, configuration);
            }

            yield return new StaticConfigurationLayer(_WhenCreated, configuration);
        }
    }
}