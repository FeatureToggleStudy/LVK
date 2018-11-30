using JetBrains.Annotations;

namespace LVK.Data.Protection
{
    [PublicAPI]
    public interface IDataEncryption
    {
        [NotNull]
        byte[] Protect([NotNull] byte[] unprotectedData, [NotNull] string password);

        [NotNull]
        byte[] Unprotect([NotNull] byte[] protectedData, [NotNull] string password);
    }
}