using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Net.Http
{
    [PublicAPI]
    public interface IRepositoryWebApiClient<TKey, TValue>
    {
        [NotNull, ItemNotNull]
        Task<KeyValuePair<TKey, TValue>[]> GetAllAsync([CanBeNull] CancellationToken? cancellationToken = null);

        [NotNull, ItemNotNull]
        Task<TValue> GetAsync([NotNull] TKey key, [CanBeNull] CancellationToken? cancellationToken = null);

        [NotNull]
        Task<bool> DeleteAsync([NotNull] TKey key, [CanBeNull] CancellationToken? cancellationToken = null);

        [NotNull]
        Task<bool> PutAsync([NotNull] TKey key, [NotNull] TValue payload, [CanBeNull] CancellationToken? cancellationToken = null);
        
        [NotNull]
        Task<bool> PostAsync([NotNull] TValue payload, [CanBeNull] CancellationToken? cancellationToken = null);
    }
}