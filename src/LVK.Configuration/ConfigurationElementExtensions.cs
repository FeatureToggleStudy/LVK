using System;

using JetBrains.Annotations;

namespace LVK.Configuration
{
    [PublicAPI]
    public static class ConfigurationElementExtensions
    {
        [NotNull]
        public static IConfigurationElementWithDefault<T> WithDefault<T>(
            [NotNull] this IConfigurationElement<T> element, [NotNull] Func<T> getDefaultValue)
            => new ConfigurationElementWithDefault<T>(element, getDefaultValue);

        [NotNull]
        public static IConfigurationElementWithDefault<T> WithDefault<T>(
            [NotNull] this IConfigurationElement<T> element, [NotNull] T defaultValue)
            => new ConfigurationElementWithDefault<T>(element, () => defaultValue);

        [NotNull]
        public static T ValueOrDefault<T>(
            [NotNull] this IConfigurationElement<T> element, [NotNull] Func<T> getDefaultValue)
            => new ConfigurationElementWithDefault<T>(element, getDefaultValue).Value();

        [NotNull]
        public static T ValueOrDefault<T>([NotNull] this IConfigurationElement<T> element, [CanBeNull] T defaultValue = default)
            => new ConfigurationElementWithDefault<T>(element, () => defaultValue).Value();
    }
}