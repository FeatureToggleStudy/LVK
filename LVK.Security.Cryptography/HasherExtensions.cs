using System;
using System.Text;

using JetBrains.Annotations;

namespace LVK.Security.Cryptography
{
    [PublicAPI]
    public static class HasherExtensions
    {
        public static string Hash([NotNull] this IHasher hasher, [NotNull] string content)
        {
            if (hasher == null)
                throw new ArgumentNullException(nameof(hasher));

            if (content == null)
                throw new ArgumentNullException(nameof(content));
            
            return hasher.Hash(Encoding.UTF8.GetBytes(content.Normalize()));
        }
    }
}