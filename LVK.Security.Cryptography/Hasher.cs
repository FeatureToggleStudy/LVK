using System;
using System.Security.Cryptography;

using LVK.Core;

namespace LVK.Security.Cryptography
{
    internal class Hasher : IHasher
    {
        public string Hash(byte[] content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            using (var sha = SHA1.Create().NotNull())
            {
                var hash = sha.ComputeHash(content);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}