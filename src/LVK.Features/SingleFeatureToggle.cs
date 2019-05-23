using JetBrains.Annotations;

namespace LVK.Features
{
    internal class SingleFeatureToggle : IFeatureToggle
    {
        [NotNull]
        private readonly IFeatureTogglesProvider _Provider;

        [NotNull]
        private readonly string _Key;

        public SingleFeatureToggle([NotNull] IFeatureTogglesProvider provider, [NotNull] string key)
        {
            _Provider = provider;
            _Key = key;
        }

        public bool? IsEnabled => _Provider.IsEnabled(_Key);
    }
}