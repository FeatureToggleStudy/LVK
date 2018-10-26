using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace LVK.Data.Protection
{
    internal class DataProtection : IDataProtection
    {
        [NotNull]
        private readonly IDataEncryption _DataEncryption;

        [NotNull, ItemNotNull]
        private readonly List<IDataProtectionPasswordProvider> _DataProtectionPasswordProviders;

        public DataProtection([NotNull, ItemNotNull] IEnumerable<IDataProtectionPasswordProvider> dataProtectionPasswordProviders, [NotNull] IDataEncryption dataEncryption)
        {
            if (dataProtectionPasswordProviders == null)
                throw new ArgumentNullException(nameof(dataProtectionPasswordProviders));

            _DataEncryption = dataEncryption ?? throw new ArgumentNullException(nameof(dataEncryption));

            _DataProtectionPasswordProviders = dataProtectionPasswordProviders.ToList();
        }

        public byte[] Protect(string passwordName, byte[] unprotectedData)
        {
            var password = TryGetPassword(passwordName);
            if (password == null)
                throw new InvalidOperationException($"No data protection scope defined for '{passwordName}'");

            return _DataEncryption.Protect(unprotectedData, password);
        }

        public byte[] Unprotect(string passwordName, byte[] protectedData)
        {
            var password = TryGetPassword(passwordName);
            if (password == null)
                throw new InvalidOperationException($"No data protection scope defined for '{passwordName}'");

            return _DataEncryption.Unprotect(protectedData, password);
        }

        [CanBeNull]
        private string TryGetPassword([NotNull] string passwordName)
            => (
                from scope in _DataProtectionPasswordProviders
                let password = scope.TryGetPassword(passwordName)
                where password != null
                select password).FirstOrDefault();
    }
}