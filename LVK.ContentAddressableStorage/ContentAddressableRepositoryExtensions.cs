using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.ContentAddressableStorage
{
    [PublicAPI]
    public static class ContentAddressableRepositoryExtensions
    {
        [NotNull]
        public static Task<ContentAddressableKey> StoreObjectAsync<T>([NotNull] this IContentAddressableRepository repository, [NotNull] T content)
            => repository.StoreObjectAsync<T>(content, CancellationToken.None);

        [NotNull, ItemCanBeNull]
        public static Task<T> GetObjectAsync<T>([NotNull] this IContentAddressableRepository repository, ContentAddressableKey key)
            => repository.GetObjectAsync<T>(key, CancellationToken.None);

        public static Task CollectGarbage([NotNull] this IContentAddressableRepository repository) => repository.CollectGarbage(CancellationToken.None);

        public static void DeleteStoredKey([NotNull] this IContentAddressableRepository repository, [NotNull] string name)
            => repository.StoreKey(name, new ContentAddressableKey());
    }
}