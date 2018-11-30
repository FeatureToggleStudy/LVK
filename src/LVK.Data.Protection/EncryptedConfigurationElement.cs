using System;

using JetBrains.Annotations;

using LVK.Configuration;

namespace LVK.Data.Protection
{
    internal class EncryptedConfigurationElement : IConfigurationElement<string>
    {
        [NotNull]
        private readonly IConfigurationElement<string> _ConfigurationElement;

        [NotNull]
        private readonly string _PasswordName;

        [NotNull]
        private readonly IDataProtection _DataProtection;

        public EncryptedConfigurationElement(
            [NotNull] IConfigurationElement<string> configurationElement, [NotNull] string passwordName, [NotNull] IDataProtection dataProtection)
        {
            _ConfigurationElement = configurationElement ?? throw new ArgumentNullException(nameof(configurationElement));
            _PasswordName = passwordName ?? throw new ArgumentNullException(nameof(passwordName));
            _DataProtection = dataProtection ?? throw new ArgumentNullException(nameof(dataProtection));
        }

        public string Value()
        {
            var encryptedValue = _ConfigurationElement.Value();
            if (string.IsNullOrWhiteSpace(encryptedValue))
                return encryptedValue;
            
            try
            {
                return _DataProtection.Unprotect(encryptedValue, _PasswordName);
            }
            catch (DataProtectionException)
            {
                return encryptedValue;
            }
        }
    }
}