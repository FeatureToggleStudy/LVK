using JetBrains.Annotations;

using LVK.Configuration;

namespace LVK.Data.Protection
{
    [PublicAPI]
    public static class ConfigurationElementExtensions
    {
        [NotNull]
        public static IConfigurationElement<string> WithEncryption(
            [NotNull] this IConfigurationElement<string> configurationElement, [NotNull] string passwordName, [NotNull] IDataProtection dataProtection)
            => new EncryptedConfigurationElement(configurationElement, passwordName, dataProtection);
    }
}