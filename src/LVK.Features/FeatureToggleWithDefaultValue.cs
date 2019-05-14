using System;

using JetBrains.Annotations;

namespace LVK.Features
{
    internal class FeatureToggleWithDefaultValue : IFeatureToggleWithDefaultValue
    {
        [NotNull]
        private readonly IFeatureToggle _FeatureToggle;

        private readonly bool _DefaultIfUnspecified;

        public FeatureToggleWithDefaultValue([NotNull] IFeatureToggle featureToggle, bool defaultIfUnspecified)
        {
            _FeatureToggle = featureToggle ?? throw new ArgumentNullException(nameof(featureToggle));
            _DefaultIfUnspecified = defaultIfUnspecified;
        }

        public bool IsEnabled => _FeatureToggle.IsEnabled ?? _DefaultIfUnspecified;
    }
}