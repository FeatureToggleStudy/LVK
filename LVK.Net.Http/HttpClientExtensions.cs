using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Net.Http
{
    [PublicAPI]
    public static class HttpClientExtensions
    {

        [NotNull, ItemNotNull]
        public static Task<T> GetAsync<T>([NotNull] this IHttpClient client, [NotNull] string query)
            => client.GetAsync<T>(query, CancellationToken.None);

        [NotNull, ItemNotNull]
        public static Task<string> GetStringAsync([NotNull] this IHttpClient client, [NotNull] string query)
            => client.GetStringAsync(query, CancellationToken.None);

        [NotNull]
        public static Task PostAsync<T>([NotNull] this IHttpClient client, [NotNull] string query, [NotNull] T payload)
            => client.PostAsync<T>(query, payload, CancellationToken.None);

        [NotNull]
        public static Task PutAsync<T>([NotNull] this IHttpClient client, [NotNull] string query, [NotNull] T payload)
            => client.PutAsync<T>(query, payload, CancellationToken.None);

        [NotNull]
        public static Task DeleteAsync([NotNull] this IHttpClient client, [NotNull] string query)
            => client.DeleteAsync(query, CancellationToken.None);
    }
}