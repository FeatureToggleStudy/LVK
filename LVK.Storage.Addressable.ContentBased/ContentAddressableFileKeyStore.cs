using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Logging;

namespace LVK.Storage.Addressable.ContentBased
{
    internal class ContentAddressableFileKeyStore : IContentAddressableKeyStore
    {
        [NotNull]
        private readonly string _BasePath;

        [NotNull]
        private readonly ILogger _Logger;

        public ContentAddressableFileKeyStore([NotNull] string basePath, [NotNull] ILogger logger)
        {
            _BasePath = basePath ?? throw new ArgumentNullException(nameof(basePath));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ContentAddressableKey GetKey(string name)
        {
            var filePath = ComputeFilePath(name);
            if (!File.Exists(filePath))
                return default;

            try
            {
                var hash = File.ReadAllText(filePath, Encoding.UTF8);
                return ContentAddressableKey.TryParse(hash);
            }
            catch (FileNotFoundException)
            {
                return ContentAddressableKey.InvalidKey;
            }
            catch (DirectoryNotFoundException)
            {
                return ContentAddressableKey.InvalidKey;
            }
        }

        public void PutKey(string name, ContentAddressableKey key)
        {
            var filePath = ComputeFilePath(name);
            if (!key.IsValid)
            {
                DeleteFile(filePath);
                return;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(filePath).NotNull());
            File.WriteAllText(filePath, key.Hash, Encoding.UTF8);
        }

        public IEnumerable<ContentAddressableKey> GetStoredKeys()
        {
            string basePath = Path.GetFullPath(_BasePath);
            string[] keyFilePaths;
            try
            {
                keyFilePaths = Directory.GetFiles(basePath, "*.*", SearchOption.AllDirectories);
            }
            catch (DirectoryNotFoundException)
            {
                yield break;
            }

            foreach (var keyFilePath in keyFilePaths)
            {
                var hash = File.ReadAllText(keyFilePath, Encoding.UTF8);
                var key = ContentAddressableKey.TryParse(hash);
                if (key.IsValid)
                {
                    _Logger.LogDebug(
                        () =>
                        {
                            string name = keyFilePath.Substring(basePath.Length)
                               .Replace(Path.DirectorySeparatorChar, '/')
                               .TrimStart(Path.DirectorySeparatorChar);

                            return $"discovered stored key '{key}' with name {name}";
                        });

                    yield return key;
                }
            }
        }

        private static void DeleteFile([NotNull] string filePath)
        {
            try
            {
                File.Delete(filePath);
            }
            catch (DirectoryNotFoundException)
            {
                // ignore
            }
            catch (FileNotFoundException)
            {
                // ignore
            }
        }

        [NotNull]
        private string ComputeFilePath([NotNull] string name) => Path.Combine(new[] { _BasePath }.Concat(name.Split('/')).ToArray());
    }
}