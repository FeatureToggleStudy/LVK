using System;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

namespace LVK.Configuration
{
    internal class RootConfiguration : BaseConfiguration
    {
        [NotNull]
        private readonly IConfigurationProvider _ConfigurationProvider;

        public RootConfiguration([NotNull] IConfigurationProvider configurationProvider, [NotNull] string path)
            : base(path)
        {
            _ConfigurationProvider =
                configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
        }

        protected override RootConfiguration Root => this;

        public JObject Element => _ConfigurationProvider.GetConfiguration();
    }
}