using System;
using System.IO;
using System.Security.Cryptography;

using JetBrains.Annotations;

using LVK.Core;

namespace LVK.Security.Cryptography
{
    internal class Hasher : IHasher
    {
        public string Hash(byte[] content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            using (SHA1 sha = SHA1.Create().NotNull())
            {
                byte[] hash = sha.ComputeHash(content);
                return CreateHashString(hash);
            }
        }

        [NotNull]
        private static string CreateHashString([NotNull] byte[] hash)
            => BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();

        public string Hash(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            using (SHA1 sha = SHA1.Create().NotNull())
            {
                byte[] hash = sha.ComputeHash(stream);
                return CreateHashString(hash);
            }
        }
    }
}