using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Net.Http
{
    [PublicAPI]
    public class WebApiClient
    {
        [NotNull]
        private readonly HttpClient _HttpClient;

        [NotNull]
        private readonly Uri _BaseUri;

        public WebApiClient([NotNull] HttpClient httpClient, [NotNull] string baseUri)
            : this(httpClient, new Uri(baseUri))
        {
        }

        public WebApiClient([NotNull] HttpClient httpClient, [NotNull] Uri baseUri)
        {
            _HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _BaseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
        }

        [NotNull, ItemNotNull]
        protected Task<T> GetAsync<T>([NotNull] string query, CancellationToken? cancellationToken = null)
            => _HttpClient.GetFromJsonAsync<T>(_BaseUri.Append(query), cancellationToken);

        [NotNull, ItemNotNull]
        protected Task<string> GetStringAsync([NotNull] string query, CancellationToken? cancellationToken = null)
            => _HttpClient.GetStringAsync(_BaseUri.Append(query), cancellationToken);

        [NotNull, ItemNotNull]
        protected Task<HttpResponseMessage> PostAsync<T>([NotNull] string query, [NotNull] T payload,
                                                         CancellationToken? cancellationToken = null)
            => _HttpClient.PostAsJsonAsync(_BaseUri.Append(query), payload, cancellationToken);

        [NotNull, ItemNotNull]
        protected Task<HttpResponseMessage> PutAsync<T>([NotNull] string query, [NotNull] T payload,
                                                        CancellationToken? cancellationToken = null)
            => _HttpClient.PutAsJsonAsync(_BaseUri.Append(query), payload, cancellationToken);

        [NotNull, ItemNotNull]
        protected Task<HttpResponseMessage> DeleteAsync([NotNull] string query,
                                                        CancellationToken? cancellationToken = null)
            => _HttpClient.DeleteAsync(_BaseUri.Append(query), cancellationToken ?? CancellationToken.None);
    }
}