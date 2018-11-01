using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Humanizer;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Logging;
using LVK.Security.Cryptography;

namespace LVK.Storage.Addressable.ContentBased
{
    internal class ContentAddressableFileObjectStore : IContentAddressableObjectStore
    {
        [NotNull]
        private readonly string _BasePath;

        [NotNull]
        private readonly IHasher _Hasher;

        [NotNull]
        private readonly ILogger _Logger;

        public ContentAddressableFileObjectStore([NotNull] string basePath, [NotNull] IHasher hasher, [NotNull] ILogger logger)
        {
            _BasePath = basePath ?? throw new ArgumentNullException(nameof(basePath));
            _Hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<byte[]> TryGetObjectAsync(ContentAddressableKey key, CancellationToken cancellationToken)
        {
            using (_Logger.LogScope(LogLevel.Trace, $"{nameof(ContentAddressableFileObjectStore)}.{nameof(TryGetObjectAsync)}"))
            {
                var filePath = ComputeFilePath(key);
                if (!File.Exists(filePath))
                {
                    _Logger.LogDebug($"file '{filePath}' not found, returning empty content for '{key}'");
                    return null;
                }

                try
                {
                    using (var stream = File.OpenRead(filePath))
                    {
                        byte[] result = new byte[stream.Length];
                        int inResult = await stream.ReadAsync(result, 0, result.Length, cancellationToken).NotNull();

                        if (inResult != result.Length)
                            throw new InvalidOperationException("Unable to read entire content");

                        _Logger.LogDebug($"retrieved {result.Length.Bytes()} for key '{key}'");

                        return result;
                    }
                }
                catch (FileNotFoundException)
                {
                    _Logger.LogDebug($"file '{filePath}' was not found, returning empty content for key '{key}'");
                    return null;
                }
                catch (DirectoryNotFoundException)
                {
                    _Logger.LogDebug($"directory containing '{filePath}' was not found, returning empty content for key '{key}'");
                    return null;
                }
            }
        }

        public async Task<ContentAddressableKey> StoreObjectAsync(byte[] content, CancellationToken cancellationToken)
        {
            var key = new ContentAddressableKey(_Hasher.Hash(content));
            var filePath = ComputeFilePath(key);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath).NotNull());
            using (FileStream stream = File.Create(filePath))
            {
                await stream.WriteAsync(content, 0, content.Length, cancellationToken).NotNull();
            }

            return key;
        }

        public IEnumerable<ContentAddressableKey> GetObjectKeys()
        {
            var objectsDirectory = new DirectoryInfo(_BasePath);
            DirectoryInfo[] contentDirectories;
            try
            {
                contentDirectories = objectsDirectory.GetDirectories();
            }
            catch (DirectoryNotFoundException)
            {
                yield break;
            }

            foreach (DirectoryInfo contentDirectory in contentDirectories)
            {
                foreach (FileInfo contentFile in contentDirectory.GetFiles())
                {
                    string hash = contentDirectory.Name + contentFile.Name;
                    var key = ContentAddressableKey.TryParse(hash);
                    if (key.IsValid)
                    {
                        _Logger.LogDebug($"discovered content key '{key}'");
                        yield return key;
                    }
                }
            }
        }

        public Task DeleteObjectAsync(ContentAddressableKey key)
        {
            var filePath = ComputeFilePath(key);
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (FileNotFoundException)
                {
                }
                catch (DirectoryNotFoundException)
                {
                }
            }

            return Task.CompletedTask;
        }

        [NotNull]
        private string ComputeFilePath(ContentAddressableKey key) => Path.Combine(_BasePath, key.Hash.Substring(0, 2), key.Hash.Substring(2));
    }
}