using System;

using JetBrains.Annotations;

namespace LVK.Configuration
{
    [PublicAPI]
    public static class ConfigurationElementExtensions
    {
        [NotNull]
        public static IConfigurationElement<T> WithDefault<T>(
            this IConfigurationElement<T> element, Func<T> getDefaultValue)
            => new ConfigurationElementWithDefaultValue<T>(element, getDefaultValue);

        [NotNull]
        public static IConfigurationElement<T> WithDefault<T>(
            this IConfigurationElement<T> element, T defaultValue)
            => new ConfigurationElementWithDefaultValue<T>(element, () => defaultValue);
        
        [NotNull]
        public static T ValueOrDefault<T>(this IConfigurationElement<T> element, [NotNull] Func<T> getDefaultValue)
            => new ConfigurationElementWithDefaultValue<T>(element, getDefaultValue).Value();

        [NotNull]
        public static T ValueOrDefault<T>(this IConfigurationElement<T> element, T defaultValue = default)
            => new ConfigurationElementWithDefaultValue<T>(element, () => defaultValue).Value();
    }
}