using System;
using System.Text;

using JetBrains.Annotations;

namespace LVK.Data.Protection
{
    [PublicAPI]
    public static class DataProtectionExtensions
    {
        public const string DefaultPasswordName = "Default";

        [NotNull]
        public static byte[] Protect([NotNull] this IDataProtection dataProtection, [NotNull] byte[] unprotectedData)
        {
            if (dataProtection == null)
                throw new ArgumentNullException(nameof(dataProtection));

            return dataProtection.Protect(unprotectedData, DefaultPasswordName);
        }

        [NotNull]
        public static byte[] Unprotect([NotNull] this IDataProtection dataProtection, [NotNull] byte[] protectedData)
        {
            if (dataProtection == null)
                throw new ArgumentNullException(nameof(dataProtection));

            return dataProtection.Unprotect(protectedData, DefaultPasswordName);
        }

        [NotNull]
        public static string Protect([NotNull] this IDataProtection dataProtection, [NotNull] string unprotectedString, [NotNull] string passwordName)
        {
            if (dataProtection == null)
                throw new ArgumentNullException(nameof(dataProtection));

            if (unprotectedString == null)
                throw new ArgumentNullException(nameof(unprotectedString));

            var unprotectedBytes = Encoding.UTF8.GetBytes(unprotectedString);
            var protectedBytes = dataProtection.Protect(unprotectedBytes, passwordName);
            return Convert.ToBase64String(protectedBytes);
        }

        [NotNull]
        public static string Unprotect([NotNull] this IDataProtection dataProtection, [NotNull] string protectedString, [NotNull] string passwordName)
        {
            if (dataProtection == null)
                throw new ArgumentNullException(nameof(dataProtection));

            if (protectedString == null)
                throw new ArgumentNullException(nameof(protectedString));

            byte[] protectedBytes;
            try
            {
                protectedBytes = Convert.FromBase64String(protectedString);
            }
            catch (FormatException ex)
            {
                throw new DataProtectionException($"Unable to unprotect data: {ex.Message}", ex);
            }

            var unprotectedBytes = dataProtection.Unprotect(protectedBytes, passwordName);
            return Encoding.UTF8.GetString(unprotectedBytes);
        }

        [NotNull]
        public static string Protect([NotNull] this IDataProtection dataProtection, [NotNull] string unprotectedString)
        {
            if (dataProtection == null)
                throw new ArgumentNullException(nameof(dataProtection));

            if (unprotectedString == null)
                throw new ArgumentNullException(nameof(unprotectedString));

            return Protect(dataProtection, unprotectedString, DefaultPasswordName);
        }

        [NotNull]
        public static string Unprotect([NotNull] this IDataProtection dataProtection, [NotNull] string protectedString)
        {
            if (dataProtection == null)
                throw new ArgumentNullException(nameof(dataProtection));

            if (protectedString == null)
                throw new ArgumentNullException(nameof(protectedString));

            return Unprotect(dataProtection, protectedString, DefaultPasswordName);
        }
    }
}