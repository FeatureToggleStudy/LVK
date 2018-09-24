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
        Task<KeyValuePair<TKey, TValue>[]> GetAllAsync(CancellationToken? cancellationToken = null);

        [NotNull]
        Task<TValue> GetAsync(TKey key, CancellationToken? cancellationToken = null);

        [NotNull]
        Task<bool> DeleteAsync(TKey key, CancellationToken? cancellationToken = null);

        [NotNull]
        Task<bool> PutAsync(TKey key, TValue payload, CancellationToken? cancellationToken = null);
        
        [NotNull]
        Task<bool> PostAsync(TValue payload, CancellationToken? cancellationToken = null);
    }
}