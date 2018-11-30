using System;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

namespace LVK.Configuration.Layers.Dynamic
{
    internal class DynamicConfigurationLayer : IConfigurationLayer
    {
        [NotNull]
        private readonly Func<JObject> _GetConfiguration;

        public DynamicConfigurationLayer([NotNull] Func<JObject> getConfiguration)
        {
            _GetConfiguration = getConfiguration ?? throw new ArgumentNullException(nameof(getConfiguration));
        }

        public JObject Configuration => _GetConfiguration();
    }
}