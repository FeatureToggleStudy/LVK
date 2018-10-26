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

            return dataProtection.Protect(DefaultPasswordName, unprotectedData);
        }

        [NotNull]
        public static byte[] Unprotect([NotNull] this IDataProtection dataProtection, [NotNull] byte[] protectedData)
        {
            if (dataProtection == null)
                throw new ArgumentNullException(nameof(dataProtection));

            return dataProtection.Unprotect(DefaultPasswordName, protectedData);
        }

        [NotNull]
        public static string Protect([NotNull] this IDataProtection dataProtection, [NotNull] string passwordName, [NotNull] string unprotectedString)
        {
            if (dataProtection == null)
                throw new ArgumentNullException(nameof(dataProtection));

            if (unprotectedString == null)
                throw new ArgumentNullException(nameof(unprotectedString));

            var unprotectedBytes = Encoding.UTF8.GetBytes(unprotectedString);
            var protectedBytes = dataProtection.Protect(passwordName, unprotectedBytes);
            return Convert.ToBase64String(protectedBytes);
        }

        [NotNull]
        public static string Unprotect([NotNull] this IDataProtection dataProtection, [NotNull] string passwordName, [NotNull] string protectedString)
        {
            if (dataProtection == null)
                throw new ArgumentNullException(nameof(dataProtection));

            if (protectedString == null)
                throw new ArgumentNullException(nameof(protectedString));

            var protectedBytes = Convert.FromBase64String(protectedString);
            var unprotectedBytes = dataProtection.Unprotect(passwordName, protectedBytes);
            return Encoding.UTF8.GetString(unprotectedBytes);
        }

        [NotNull]
        public static string Protect([NotNull] this IDataProtection dataProtection, [NotNull] string unprotectedString)
        {
            if (dataProtection == null)
                throw new ArgumentNullException(nameof(dataProtection));

            if (unprotectedString == null)
                throw new ArgumentNullException(nameof(unprotectedString));

            return Protect(dataProtection, DefaultPasswordName, unprotectedString);
        }

        [NotNull]
        public static string Unprotect([NotNull] this IDataProtection dataProtection, [NotNull] string protectedString)
        {
            if (dataProtection == null)
                throw new ArgumentNullException(nameof(dataProtection));

            if (protectedString == null)
                throw new ArgumentNullException(nameof(protectedString));

            return Unprotect(dataProtection, DefaultPasswordName, protectedString);
        }
    }
}