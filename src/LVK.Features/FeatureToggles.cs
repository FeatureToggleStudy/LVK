using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace LVK.Features
{
    internal class FeatureToggles : IFeatureToggles
    {
        [NotNull, ItemNotNull]
        private List<IFeatureTogglesProvider> _Providers;

        public FeatureToggles([NotNull] IEnumerable<IFeatureTogglesProvider> providers)
        {
            if (providers == null)
                throw new ArgumentNullException(nameof(providers));

            _Providers = providers.ToList();
        }

        public IFeatureToggle GetByKey(string key)
        {
            if (_Providers.Count == 1)
                return new SingleFeatureToggle(_Providers[0], key);

            return new MultiFeatureToggle(_Providers, key);
        }
    }
}