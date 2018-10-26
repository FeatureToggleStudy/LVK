using JetBrains.Annotations;

namespace LVK.Data.Protection
{
    [PublicAPI]
    public interface IDataProtection
    {
        [NotNull]
        byte[] Protect([NotNull] string passwordName, [NotNull] byte[] unprotectedData);

        [NotNull]
        byte[] Unprotect([NotNull] string passwordName, [NotNull] byte[] protectedData);
    }
}