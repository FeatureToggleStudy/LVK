using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Net.Http
{
    public interface IHttpClient
    {
        [NotNull, ItemNotNull]
        Task<T> GetAsync<T>([NotNull] string query, CancellationToken cancellationToken);

        [NotNull, ItemNotNull]
        Task<string> GetStringAsync([NotNull] string query, CancellationToken cancellationToken);

        [NotNull]
        Task PostAsync<T>([NotNull] string query, [NotNull] T payload, CancellationToken cancellationToken);

        [NotNull]
        Task PutAsync<T>([NotNull] string query, [NotNull] T payload, CancellationToken cancellationToken);

        [NotNull]
        Task DeleteAsync([NotNull] string query, CancellationToken cancellationToken);

        [NotNull, ItemNotNull]
        Task<HttpResponseMessage> SendAsync([NotNull] HttpRequestMessage request, CancellationToken cancellationToken);
    }
}