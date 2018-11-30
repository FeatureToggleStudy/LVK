using System;

using JetBrains.Annotations;

namespace LVK.Data.Protection
{
    [PublicAPI]
    public class DataProtectionException : InvalidOperationException
    {
        public DataProtectionException([NotNull] string message)
            : base(message)
        {
        }

        public DataProtectionException([NotNull] string message, [CanBeNull] Exception innerException)
            : base(message, innerException)
        {
        }
    }
}