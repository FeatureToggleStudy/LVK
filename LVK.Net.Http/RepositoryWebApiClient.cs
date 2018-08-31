using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Net.Http
{
    public class RepositoryWebApiClient<TKey, TPayload> : WebApiClient, IRepositoryWebApiClient<TKey, TPayload>
    {
        public RepositoryWebApiClient([NotNull] HttpClient httpClient, [NotNull] string uri)
            : base(httpClient, uri)
        {
        }

        public RepositoryWebApiClient([NotNull] HttpClient httpClient, [NotNull] Uri uri)
            : base(httpClient, uri)
        {
        }

        public Task<KeyValuePair<TKey, TPayload>[]> GetAllAsync(CancellationToken? cancellationToken = null)
            => GetAsync<KeyValuePair<TKey, TPayload>[]>("", cancellationToken);

        public Task<TPayload> GetAsync(TKey key, CancellationToken? cancellationToken = null)
            => GetAsync<TPayload>($"{key}", cancellationToken);

        public async Task<bool> DeleteAsync(TKey key, CancellationToken? cancellationToken = null)
            => (await DeleteAsync($"{key}", cancellationToken)).IsSuccessStatusCode;

        public async Task<bool> PutAsync(TKey key, [NotNull] TPayload payload, CancellationToken? cancellationToken = null)
            => (await PutAsync($"{key}", payload, cancellationToken)).IsSuccessStatusCode;

        public async Task<bool> PostAsync([NotNull] TPayload payload, CancellationToken? cancellationToken = null)
            => (await PostAsync("", payload, cancellationToken)).IsSuccessStatusCode;
    }
}