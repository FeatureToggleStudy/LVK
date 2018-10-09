using System;

using JetBrains.Annotations;

using LVK.Logging;

namespace LVK.Storage.Addressable.ContentBased
{
    internal class ContentAddressableRepositoryFactory : IContentAddressableRepositoryFactory
    {
        [NotNull]
        private readonly IContentAddressableStoreFactory _ContentAddressableStoreFactory;

        [NotNull]
        private readonly ILogger _Logger;

        public ContentAddressableRepositoryFactory([NotNull] IContentAddressableStoreFactory contentAddressableStoreFactory, [NotNull] ILogger logger)
        {
            _ContentAddressableStoreFactory = contentAddressableStoreFactory ?? throw new ArgumentNullException(nameof(contentAddressableStoreFactory));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IContentAddressableRepository TryCreate(string name)
        {
            var store = _ContentAddressableStoreFactory.Create(name);
            if (store == null)
                return null;

            return new ContentAddressableRepository(store, _Logger);
        }
    }
}