using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LVK.Storage.Addressable.ContentBased
{
    internal class ContentAddressableRepository : IContentAddressableRepository
    {
        [NotNull]
        private readonly IContentAddressableStore _ContentAddressableStore;

        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly JsonSerializerSettings _Settings;

        public ContentAddressableRepository([NotNull] IContentAddressableStore contentAddressableStore, [NotNull] ILogger logger)
        {
            _ContentAddressableStore = contentAddressableStore ?? throw new ArgumentNullException(nameof(contentAddressableStore));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            _Settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Culture = CultureInfo.InvariantCulture };
        }

        public Task<ContentAddressableKey> StoreObjectAsync<T>(T content, CancellationToken cancellationToken)
        {
            if (content is byte[] rawBytes)
                return StoreRawBytesAsync(rawBytes, cancellationToken);

            return StoreObjectAsJsonAsync(content, cancellationToken);
        }

        [NotNull]
        private Task<ContentAddressableKey> StoreObjectAsJsonAsync<T>(T content, CancellationToken cancellationToken)
        {
            var json = JsonConvert.SerializeObject(content, _Settings);
            var bytes = Encoding.UTF8.GetBytes("J" + json);

            return _ContentAddressableStore.StoreObjectAsync(bytes, cancellationToken);
        }

        [NotNull]
        private Task<ContentAddressableKey> StoreRawBytesAsync([NotNull] byte[] bytes, CancellationToken cancellationToken)
        {
            byte[] temp = new byte[bytes.Length + 1];
            temp[0] = (byte)'B';
            Buffer.BlockCopy(bytes, 0, temp, 1, bytes.Length);
            return _ContentAddressableStore.StoreObjectAsync(temp, cancellationToken);
        }

        public async Task<T> GetObjectAsync<T>(ContentAddressableKey key, CancellationToken cancellationToken)
        {
            var bytes = await _ContentAddressableStore.TryGetObjectAsync(key, cancellationToken);
            if (bytes == null)
                return default;

            switch (bytes[0])
            {
                case (byte)'B':
                    return (T)(object)GetRawBytes(bytes);
                
                case (byte)'J':
                    return GetObjectFromJson<T>(bytes);
                
                default:
                    throw new InvalidOperationException($"Invalid content type of object with key '{key}'");
            }
        }

        [NotNull]
        private T GetObjectFromJson<T>([NotNull] byte[] bytes)
        {
            var json = Encoding.UTF8.GetString(bytes, 1, bytes.Length - 1);
            _Logger.LogDebug($"json: {json}");
            return JsonConvert.DeserializeObject<T>(json, _Settings);
        }

        [NotNull]
        private byte[] GetRawBytes([NotNull] byte[] bytes)
        {
            var temp = new byte[bytes.Length - 1];
            Buffer.BlockCopy(bytes, 1, temp, 0, bytes.Length - 1);
            return temp;
        }

        public ContentAddressableKey GetKey(string name) => _ContentAddressableStore.GetKey(name);
        public void StoreKey(string name, ContentAddressableKey key) => _ContentAddressableStore.PutKey(name, key);

        public async Task CollectGarbage(CancellationToken cancellationToken)
        {
            using (_Logger.LogScope(LogLevel.Trace, $"{nameof(ContentAddressableRepository)}.{nameof(CollectGarbage)}"))
            {
                var contentKeys = _ContentAddressableStore.GetObjectKeys().ToHashSet();
                var storedKeys = _ContentAddressableStore.GetStoredKeys().ToList();

                var rootedKeys = await GetAllRootedKeys(storedKeys, cancellationToken);

                var danglingKeys = contentKeys.Except(rootedKeys).ToHashSet();
                foreach (ContentAddressableKey danglingKey in danglingKeys)
                {
                    _Logger.LogVerbose($"deleting content with dangling key '{danglingKey}'");
                    await _ContentAddressableStore.DeleteObjectAsync(danglingKey);
                }
            }
        }

        private async Task<HashSet<ContentAddressableKey>> GetAllRootedKeys([NotNull] IEnumerable<ContentAddressableKey> roots, CancellationToken cancellationToken)
        {
            var discoveredKeys = new HashSet<ContentAddressableKey>();
            var keysToProcess = roots.ToQueue();

            while (keysToProcess.Any())
            {
                ContentAddressableKey key = keysToProcess.Dequeue();
                if (!discoveredKeys.Add(key))
                    continue;

                _Logger.LogDebug($"object traversal discovered key '{key}");

                var keysInObject = await ParseObjectForKeys(key, cancellationToken);
                foreach (var newKey in keysInObject)
                    keysToProcess.Enqueue(newKey);
            }

            return discoveredKeys;
        }

        private async Task<HashSet<ContentAddressableKey>> ParseObjectForKeys(ContentAddressableKey key, CancellationToken cancellationToken)
        {
            var result = new HashSet<ContentAddressableKey>();
            byte[] bytes = await _ContentAddressableStore.TryGetObjectAsync(key, cancellationToken);
            if (bytes == null)
                return result;

            if (bytes[0] != (byte)'J')
                return result;

            string json;
            try
            {
                json  = Encoding.UTF8.GetString(bytes, 1, bytes.Length - 1);
            }
            catch (DecoderFallbackException)
            {
                return result;
            }

            try
            {
                var obj = JToken.Parse(json);

                Traverse(obj, result);

                return result;
            }
            catch (JsonReaderException)
            {
                return result;
            }
        }

        private void Traverse(JToken token, [NotNull] HashSet<ContentAddressableKey> discoveredKeys)
        {
            if (token is JProperty property)
                TraverseProperty(property, discoveredKeys);

            if (token is JContainer container)
                TraverseContainer(container, discoveredKeys);
        }

        private void TraverseContainer([NotNull] JContainer container, [NotNull] HashSet<ContentAddressableKey> discoveredKeys)
        {
            var child = container.First;
            while (child != null)
            {
                Traverse(child, discoveredKeys);
                child = child.Next;
            }
        }

        private void TraverseProperty([NotNull] JProperty property, [NotNull] HashSet<ContentAddressableKey> discoveredKeys)
        {
            if (property.Name == ContentAddressableKey.HashPropertyName)
            {
                var hash = property.Value.ToString();
                discoveredKeys.Add(new ContentAddressableKey(hash));
                return;
            }

            Traverse(property.Value, discoveredKeys);
        }
    }
}