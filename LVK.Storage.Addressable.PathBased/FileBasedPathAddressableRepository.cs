using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;

using Newtonsoft.Json;

namespace LVK.Storage.Addressable.PathBased
{
    internal class FileBasedPathAddressableRepository : IPathAddressableRepository
    {
        [NotNull]
        private readonly string _BasePath;

        [NotNull]
        private readonly JsonSerializerSettings _Settings;

        public FileBasedPathAddressableRepository([NotNull] string basePath)
        {
            _BasePath = basePath ?? throw new ArgumentNullException(nameof(basePath));
            _Settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        }

        public Task StoreObjectAsync<T>(string path, T content, CancellationToken cancellationToken)
        {
            var filePath = ComputeFilePath(path);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath).NotNull());

            if (content is byte[] bytes)
                return StoreRawBytesAsync(filePath, (byte)'B', bytes, cancellationToken);

            var json = JsonConvert.SerializeObject(content, _Settings).NotNull();
            var jsonBytes = Encoding.UTF8.GetBytes(json);
            return StoreRawBytesAsync(filePath, (byte)'J', jsonBytes, cancellationToken);
        }

        [NotNull]
        private async Task StoreRawBytesAsync([NotNull] string filePath, byte type, [NotNull] byte[] bytes, CancellationToken cancellationToken)
        {
            using (var stream = File.Create(filePath))
            {
                stream.WriteByte(type);
                await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken).NotNull();
            }
        }

        public async Task<T> TryGetObjectAsync<T>(string path, CancellationToken cancellationToken)
        {
            var filePath = ComputeFilePath(path);
            if (!File.Exists(filePath))
                return default;

            try
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte type = (byte)stream.ReadByte();
                    var bytes = new byte[stream.Length - 1];
                    await stream.ReadAsync(bytes, 0, bytes.Length, cancellationToken).NotNull();

                    switch (type)
                    {
                        case (byte)'J':
                            var json = Encoding.UTF8.GetString(bytes);
                            return JsonConvert.DeserializeObject<T>(json);
                            
                        case (byte)'B':
                            return (T)(object)bytes;
                        
                        default:
                            throw new InvalidOperationException($"Corrupt blob in path based addressable stored element '{path}'");
                    }
                }
            }
            catch (FileNotFoundException)
            {
                return default;
            }
            catch (DirectoryNotFoundException)
            {
                return default;
            }
        }

        public void DeleteObject(string path)
        {
            var filePath = ComputeFilePath(path);
            try
            {
                File.Delete(filePath);
            }
            catch (DirectoryNotFoundException)
            {
                // Ignore
            }
            catch (FileNotFoundException)
            {
                // Ignore
            }
        }

        [NotNull]
        private string ComputeFilePath([NotNull] string path) => Path.Combine(new[] { _BasePath }.Concat(path.Split('/')).ToArray());
    }
}