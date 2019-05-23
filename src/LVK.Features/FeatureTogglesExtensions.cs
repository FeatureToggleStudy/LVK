using JetBrains.Annotations;

namespace LVK.Features
{
    [PublicAPI]
    public static class FeatureTogglesExtensions
    {
        [CanBeNull]
        public static bool? IsEnabled([NotNull] this IFeatureToggles featureToggles, [NotNull] string key)
            => featureToggles.GetByKey(key).IsEnabled;

        public static bool IsEnabled([NotNull] this IFeatureToggles featureToggles, [NotNull] string key, bool defaultIfUnspecified)
            => featureToggles.IsEnabled(key) ?? defaultIfUnspecified;

        [NotNull]
        public static IFeatureToggleWithDefault WithDefault(
            [NotNull] this IFeatureToggle featureToggle, bool defaultIfUnspecified)
            => new FeatureToggleWithDefault(featureToggle, defaultIfUnspecified);
    }
}