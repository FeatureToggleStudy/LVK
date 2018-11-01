using System;

using JetBrains.Annotations;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LVK.Configuration
{
    internal class RootConfiguration : BaseConfiguration
    {
        [NotNull]
        private readonly IConfigurationProvider _ConfigurationProvider;

        public RootConfiguration(
            [NotNull] IConfigurationProvider configurationProvider, [NotNull] string path,
            [NotNull] JsonSerializer serializer)
            : base(path, serializer)
        {
            _ConfigurationProvider =
                configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
        }

        protected override RootConfiguration Root => this;

        [NotNull]
        public JObject Element => _ConfigurationProvider.GetConfiguration();
    }
}