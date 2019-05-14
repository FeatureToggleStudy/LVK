using JetBrains.Annotations;

namespace LVK.Features
{
    [PublicAPI]
    public static class FeatureTogglesExtensions
    {
        public static bool? IsEnabled([NotNull] this IFeatureToggles featureToggles, [NotNull] string key)
            => featureToggles.GetByKey(key).IsEnabled;

        public static bool IsEnabled([NotNull] this IFeatureToggles featureToggles, [NotNull] string key, bool defaultIfUnspecified)
            => featureToggles.IsEnabled(key) ?? defaultIfUnspecified;

        [NotNull]
        public static IFeatureToggleWithDefaultValue WithDefault(
            [NotNull] this IFeatureToggle featureToggle, bool defaultIfUnspecified)
            => new FeatureToggleWithDefaultValue(featureToggle, defaultIfUnspecified);
    }
}