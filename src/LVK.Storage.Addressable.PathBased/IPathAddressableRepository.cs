using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Storage.Addressable.PathBased
{
    [PublicAPI]
    public interface IPathAddressableRepository
    {
        [NotNull]
        Task StoreObjectAsync<T>([NotNull] string path, [NotNull] T content, CancellationToken cancellationToken);

        [NotNull, ItemCanBeNull]
        Task<T> TryGetObjectAsync<T>([NotNull] string path, CancellationToken cancellationToken);

        void DeleteObject([NotNull] string path);
    }
}