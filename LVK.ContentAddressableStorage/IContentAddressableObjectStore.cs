using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.ContentAddressableStorage
{
    internal interface IContentAddressableObjectStore
    {
        [NotNull, ItemCanBeNull]
        Task<byte[]> TryGetObjectAsync(ContentAddressableKey key, CancellationToken cancellationToken);

        [NotNull]
        Task<ContentAddressableKey> StoreObjectAsync([NotNull] byte[] content, CancellationToken cancellationToken);

        [NotNull]
        IEnumerable<ContentAddressableKey> GetObjectKeys();

        [NotNull]
        Task DeleteObjectAsync(ContentAddressableKey key);
    }
}