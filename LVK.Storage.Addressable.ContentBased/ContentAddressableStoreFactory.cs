using System;
using System.IO;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.Logging;
using LVK.Security.Cryptography;

namespace LVK.Storage.Addressable.ContentBased
{
    internal class ContentAddressableStoreFactory : IContentAddressableStoreFactory
    {
        [NotNull]
        private readonly IConfiguration _Configuration;

        [NotNull]
        private readonly IHasher _Hasher;

        [NotNull]
        private readonly ILogger _Logger;

        public ContentAddressableStoreFactory([NotNull] IConfiguration configuration, [NotNull] IHasher hasher, [NotNull] ILogger logger)
        {
            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _Hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IContentAddressableStore Create(string name)
        {
            var configuration = _Configuration[$"ContentAddressable/Stores/{name}"]
               .Element<ContentAddressableStoreConfiguration>()
               .ValueOrDefault(() => new ContentAddressableStoreConfiguration());

            if (string.IsNullOrWhiteSpace(configuration.BasePath))
                return null;

            var objectStore = new ContentAddressableFileObjectStore(Path.Combine(configuration.BasePath, "objects"), _Hasher, _Logger);
            var keyStore = new ContentAddressableFileKeyStore(Path.Combine(configuration.BasePath, "refs"), _Logger);
            return new ContentAddressableStore(objectStore, keyStore);
        }
    }
}