using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Storage.Addressable.PathBased
{
    [PublicAPI]
    public static class PathAddressableRepositoryExtensions
    {
        [NotNull]
        public static Task StoreObjectAsync<T>([NotNull] this IPathAddressableRepository repository, [NotNull] string path, [NotNull] T content)
            => repository.StoreObjectAsync(path, content, CancellationToken.None);

        [NotNull, ItemCanBeNull]
        public static Task<T> TryGetObjectAsync<T>([NotNull] this IPathAddressableRepository repository, [NotNull] string path)
            => repository.TryGetObjectAsync<T>(path, CancellationToken.None);
    }
}