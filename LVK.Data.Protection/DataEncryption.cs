using System;
using System.IO;
using System.Security.Cryptography;

using JetBrains.Annotations;

using LVK.Core.Services;

namespace LVK.Data.Protection
{
    internal class DataEncryption : IDataEncryption
    {
        [NotNull]
        private readonly IDataEncoder _DataEncoder;
        private const int _SaltSize = 32; // 256 bits of salt

        public DataEncryption([NotNull] IDataEncoder dataEncoder)
        {
            _DataEncoder = dataEncoder ?? throw new ArgumentNullException(nameof(dataEncoder));
        }

        public byte[] Protect(byte[] unprotectedData, string password)
        {
            byte[] salt = CreateSalt();

            using (RijndaelManaged algorithm = CreateAlgorithm(password, salt))
            using (var targetStream = new MemoryStream())
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor())
            using (var encryptionStream = new CryptoStream(targetStream, encryptor, CryptoStreamMode.Write))
            {
                _DataEncoder.WriteInt32(targetStream, salt.Length);
                targetStream.Write(salt, 0, salt.Length);

                encryptionStream.Write(unprotectedData, 0, unprotectedData.Length);
                encryptionStream.FlushFinalBlock();

                return targetStream.ToArray();
            }
        }

        public byte[] Unprotect(byte[] protectedData, string password)
        {
            using (var sourceStream = new MemoryStream(protectedData))
            {
                var saltLength = _DataEncoder.ReadInt32(sourceStream);
                if (saltLength > 1024)
                    throw new InvalidOperationException("Invalid salt length");

                var salt = new byte[saltLength];
                sourceStream.Read(salt, 0, salt.Length);

                using (var algorithm = CreateAlgorithm(password, salt))
                using (var decryptor = algorithm.CreateDecryptor())
                using (var decryptionStream = new CryptoStream(sourceStream, decryptor, CryptoStreamMode.Read))
                using (var targetStream = new MemoryStream())
                {
                    decryptionStream.CopyTo(targetStream);
                    return targetStream.ToArray();
                }
            }
        }

        [NotNull]
        private byte[] CreateSalt()
        {
            using (var x = new RNGCryptoServiceProvider())
            {
                var salt = new byte[_SaltSize];
                x.GetBytes(salt);
                return salt;
            }
        }

        [NotNull]
        private RijndaelManaged CreateAlgorithm([NotNull] string password, [NotNull] byte[] salt)
        {
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt);

            var algorithm = new RijndaelManaged { BlockSize = 128, Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7 };
            algorithm.Key = rfc2898DeriveBytes.GetBytes(algorithm.KeySize / 8);
            algorithm.IV = rfc2898DeriveBytes.GetBytes(algorithm.BlockSize / 8);

            return algorithm;
        }
    }
}