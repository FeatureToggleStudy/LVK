using System.IO;

using JetBrains.Annotations;

namespace LVK.Security.Cryptography
{
    [PublicAPI]
    public interface IHasher
    {
        [NotNull]
        string Hash([NotNull] byte[] content);

        [NotNull]
        string Hash([NotNull] Stream stream);
    }
}