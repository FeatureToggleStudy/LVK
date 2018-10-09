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
            var filename = ComputeFilename(path);
            Directory.CreateDirectory(Path.GetDirectoryName(filename).NotNull());

            if (content is byte[] bytes)
                return StoreRawBytesAsync(filename, (byte)'B', bytes, cancellationToken);

            var json = JsonConvert.SerializeObject(content, _Settings).NotNull();
            var jsonBytes = Encoding.UTF8.GetBytes(json);
            return StoreRawBytesAsync(filename, (byte)'J', jsonBytes, cancellationToken);
        }

        [NotNull]
        private async Task StoreRawBytesAsync([NotNull] string filename, byte type, [NotNull] byte[] bytes, CancellationToken cancellationToken)
        {
            using (var stream = File.Create(filename))
            {
                stream.WriteByte(type);
                await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken).NotNull();
            }
        }

        public async Task<T> TryGetObjectAsync<T>(string path, CancellationToken cancellationToken)
        {
            var filename = ComputeFilename(path);
            if (!File.Exists(filename))
                return default;

            try
            {
                using (var stream = File.OpenRead(filename))
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
            var filename = ComputeFilename(path);
            try
            {
                File.Delete(filename);
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
        private string ComputeFilename([NotNull] string path) => Path.Combine(new[] { _BasePath }.Concat(path.Split('/')).ToArray());
    }
}