using JetBrains.Annotations;

namespace LVK.Data.Protection
{
    [PublicAPI]
    public interface IDataProtection
    {
        [NotNull]
        byte[] Protect([NotNull] byte[] unprotectedData, [NotNull] string passwordName);

        [NotNull]
        byte[] Unprotect([NotNull] byte[] protectedData, [NotNull] string passwordName);
    }
}