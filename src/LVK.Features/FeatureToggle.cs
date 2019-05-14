using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace LVK.Features
{
    internal class FeatureToggle : IFeatureToggle
    {
        [NotNull]
        private readonly List<IFeatureTogglesProvider> _Providers;

        [NotNull]
        private readonly string _Key;

        public FeatureToggle([NotNull] List<IFeatureTogglesProvider> providers, [NotNull] string key)
        {
            _Providers = providers;
            _Key = key;
        }

        public bool? IsEnabled => (
            from provider in _Providers
            let isEnabled = provider.IsEnabled(_Key)
            where isEnabled.HasValue
            select isEnabled).FirstOrDefault();
    }
}