using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Storage.Addressable.Content
{
    internal class ContentAddressableStore : IContentAddressableStore
    {
        [NotNull]
        private readonly IContentAddressableObjectStore _ObjectStore;

        [NotNull]
        private readonly IContentAddressableKeyStore _KeyStore;

        public ContentAddressableStore([NotNull] IContentAddressableObjectStore objectStore, [NotNull] IContentAddressableKeyStore keyStore)
        {
            _ObjectStore = objectStore ?? throw new ArgumentNullException(nameof(objectStore));
            _KeyStore = keyStore ?? throw new ArgumentNullException(nameof(keyStore));
        }

        public Task<byte[]> TryGetObjectAsync(ContentAddressableKey key, CancellationToken cancellationToken)
            => _ObjectStore.TryGetObjectAsync(key, cancellationToken);

        public Task<ContentAddressableKey> StoreObjectAsync(byte[] content, CancellationToken cancellationToken)
            => _ObjectStore.StoreObjectAsync(content, cancellationToken);

        public IEnumerable<ContentAddressableKey> GetObjectKeys() => _ObjectStore.GetObjectKeys();
        public Task DeleteObjectAsync(ContentAddressableKey key) => _ObjectStore.DeleteObjectAsync(key);

        public ContentAddressableKey GetKey(string name) => _KeyStore.GetKey(name);

        public void PutKey(string name, ContentAddressableKey key) => _KeyStore.PutKey(name, key);
        public IEnumerable<ContentAddressableKey> GetStoredKeys() => _KeyStore.GetStoredKeys();
    }
}