using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.Json;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Data.Protection
{
    internal class DataProtectionJsonStringDecoder : IJsonStringDecoder
    {
        [NotNull]
        private readonly IContainer _Container;

        public DataProtectionJsonStringDecoder([NotNull] IContainer container)
        {
            _Container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public string Decode(string value)
        {
            var dataProtection = _Container.Resolve<IDataProtection>();
            assume(dataProtection != null);
            try
            {
                return dataProtection.Unprotect(value);
            }
            catch (DataProtectionException)
            {
                return value;
            }
        }
    }
}