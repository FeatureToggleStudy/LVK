using System;

using JetBrains.Annotations;

namespace LVK.Configuration
{
    [PublicAPI]
    public static class ConfigurationElementExtensions
    {
        [NotNull]
        public static IConfigurationElement<T> WithDefault<T>(
            [NotNull] this IConfigurationElement<T> element, [NotNull] Func<T> getDefaultValue)
            => new ConfigurationElementWithDefaultValue<T>(element, getDefaultValue);

        [NotNull]
        public static IConfigurationElement<T> WithDefault<T>(
            [NotNull] this IConfigurationElement<T> element, [NotNull] T defaultValue)
            => new ConfigurationElementWithDefaultValue<T>(element, () => defaultValue);

        [NotNull]
        public static T ValueOrDefault<T>(
            [NotNull] this IConfigurationElement<T> element, [NotNull] Func<T> getDefaultValue)
            => new ConfigurationElementWithDefaultValue<T>(element, getDefaultValue).Value();

        [NotNull]
        public static T ValueOrDefault<T>([NotNull] this IConfigurationElement<T> element, T defaultValue = default)
            => new ConfigurationElementWithDefaultValue<T>(element, () => defaultValue).Value();
    }
}