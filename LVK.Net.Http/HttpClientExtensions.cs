using System;
using System.Net.Http;
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
        {
            if (client is null)
                throw new ArgumentNullException(nameof(client));

            return client.GetAsync<T>(query, CancellationToken.None);
        }

        [NotNull, ItemNotNull]
        public static Task<string> GetStringAsync([NotNull] this IHttpClient client, [NotNull] string query)
        {
            if (client is null)
                throw new ArgumentNullException(nameof(client));

            return client.GetStringAsync(query, CancellationToken.None);
        }

        [NotNull]
        public static Task PostAsync<T>([NotNull] this IHttpClient client, [NotNull] string query, [NotNull] T payload)
        {
            if (client is null)
                throw new ArgumentNullException(nameof(client));

            return client.PostAsync(query, payload, CancellationToken.None);
        }

        [NotNull]
        public static Task PutAsync<T>([NotNull] this IHttpClient client, [NotNull] string query, [NotNull] T payload)
        {
            if (client is null)
                throw new ArgumentNullException(nameof(client));

            return client.PutAsync(query, payload, CancellationToken.None);
        }

        [NotNull]
        public static Task DeleteAsync([NotNull] this IHttpClient client, [NotNull] string query)
        {
            if (client is null)
                throw new ArgumentNullException(nameof(client));

            return client.DeleteAsync(query, CancellationToken.None);
        }

        [NotNull, ItemNotNull]
        public static Task<HttpResponseMessage> SendAsync([NotNull] this IHttpClient client, [NotNull] HttpRequestMessage request)
        {
            if (client is null)
                throw new ArgumentNullException(nameof(client));

            return client.SendAsync(request, CancellationToken.None);
        }
    }
}