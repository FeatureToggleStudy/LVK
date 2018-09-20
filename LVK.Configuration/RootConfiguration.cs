using System;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

using NodaTime;

namespace LVK.Configuration
{
    internal class RootConfiguration : BaseConfiguration
    {
        [NotNull]
        private readonly IConfigurationProvider _ConfigurationProvider;

        [NotNull]
        private readonly string _Path;

        [CanBeNull]
        private JObject _RootElement;

        private Instant _ElementLastUpdatedAt;

        public RootConfiguration([NotNull] IConfigurationProvider configurationProvider, [NotNull] string path)
            : base(path)
        {
            _ConfigurationProvider =
                configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));

            _Path = path ?? throw new ArgumentNullException(nameof(path));
        }

        [NotNull]
        public JObject GetElement()
        {
            var providedConfigurationLastUpdatedAt = _ConfigurationProvider.LastUpdatedAt;
            if (_RootElement == null || _ElementLastUpdatedAt < providedConfigurationLastUpdatedAt)
            {
                _RootElement = _ConfigurationProvider.GetConfiguration();
                _ElementLastUpdatedAt = providedConfigurationLastUpdatedAt;
            }

            return _RootElement;
        }

        protected override RootConfiguration Root => this;
    }
}