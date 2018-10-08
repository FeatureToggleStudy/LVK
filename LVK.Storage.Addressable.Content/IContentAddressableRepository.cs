using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Storage.Addressable.Content
{
    [PublicAPI]
    public interface IContentAddressableRepository
    {
        [NotNull]
        Task<ContentAddressableKey> StoreObjectAsync<T>([NotNull] T content, CancellationToken cancellationToken);

        [NotNull, ItemCanBeNull]
        Task<T> GetObjectAsync<T>(ContentAddressableKey key, CancellationToken cancellationToken);

        ContentAddressableKey GetKey([NotNull] string name);
        void StoreKey([NotNull] string name, ContentAddressableKey key);

        Task CollectGarbage(CancellationToken cancellationToken);
    }
}