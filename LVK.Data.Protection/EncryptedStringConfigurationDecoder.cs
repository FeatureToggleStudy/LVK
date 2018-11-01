using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.Configuration;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Data.Protection
{
    internal class EncryptedStringConfigurationDecoder : IConfigurationDecoder
    {
        [NotNull]
        private readonly IContainer _Container;

        public EncryptedStringConfigurationDecoder([NotNull] IContainer container)
        {
            _Container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public object Decode(object value)
        {
            if (!(value is string protectedString))
                return value;

            var dataProtection = _Container.Resolve<IDataProtection>();
            assume(dataProtection != null);

            int index = protectedString.IndexOf(':');
            if (index > 0)
            {
                string passwordName = protectedString.Substring(0, index);
                var protectedSubString = protectedString.Substring(index + 1);
                try
                {
                    return dataProtection.Unprotect(protectedSubString, passwordName);
                }
                catch (DataProtectionException)
                {
                    // Ignore this
                }
            }

            try
            {
                return dataProtection.Unprotect(protectedString);
            }
            catch (DataProtectionException)
            {
                return value;
            }
        }

        public bool CanDecode(Type type) => type == typeof(string);
    }
}