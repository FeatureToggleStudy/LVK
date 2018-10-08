using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Logging;

namespace LVK.ContentAddressableStorage
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
            var filename = ComputeFilename(name);
            if (!File.Exists(filename))
                return default;

            try
            {
                var hash = File.ReadAllText(filename, Encoding.UTF8);
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
            var filename = ComputeFilename(name);
            if (!key.IsValid)
            {
                DeleteFile(filename);
                return;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(filename).NotNull());
            File.WriteAllText(filename, key.Hash, Encoding.UTF8);
        }

        public IEnumerable<ContentAddressableKey> GetStoredKeys()
        {
            string basePath = Path.GetFullPath(_BasePath);
            string[] files;
            try
            {
                files = Directory.GetFiles(basePath, "*.*", SearchOption.AllDirectories);
            }
            catch (DirectoryNotFoundException)
            {
                yield break;
            }

            foreach (var keyFilename in files)
            {
                var hash = File.ReadAllText(keyFilename, Encoding.UTF8);
                var key = ContentAddressableKey.TryParse(hash);
                if (key.IsValid)
                {
                    _Logger.LogDebug(
                        () =>
                        {
                            string name = keyFilename.Substring(basePath.Length).Replace(Path.DirectorySeparatorChar, '/').TrimStart(Path.DirectorySeparatorChar);
                            return $"discovered stored key '{key}' with name {name}";
                        });

                    yield return key;
                }
            }
        }

        private static void DeleteFile(string filename)
        {
            try
            {
                File.Delete(filename);
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

        private string ComputeFilename(string name) => Path.Combine(new[] { _BasePath }.Concat(name.Split('/')).ToArray());
    }
}