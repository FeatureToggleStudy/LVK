using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Newtonsoft.Json;

// ReSharper disable PossibleNullReferenceException

namespace LVK.Net.Http
{
    [PublicAPI]
    public static class HttpClientExtensions
    {
        [NotNull, ItemNotNull]
        public static Task<HttpResponseMessage> DeleteAsync(
            [NotNull] this IHttpClient httpClient, [NotNull] string requestUri, CancellationToken? cancellationToken = null)
        {
            if (httpClient == null)
                throw new ArgumentNullException(nameof(httpClient));

            if (requestUri == null)
                throw new ArgumentNullException(nameof(requestUri));

            return httpClient.SendRequestAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri), cancellationToken);
        }

        [NotNull]
        public static Task<string> GetStringAsync(
            [NotNull] this IHttpClient httpClient, [NotNull] string requestUri, [CanBeNull] CancellationToken? cancellationToken = null)
            => GetStringAsync(httpClient, new Uri(requestUri), cancellationToken);

        [NotNull]
        public static async Task<string> GetStringAsync(
            [NotNull] this IHttpClient httpClient, [NotNull] Uri requestUri, [CanBeNull] CancellationToken? cancellationToken = null)
        {
            if (httpClient is null)
                throw new ArgumentNullException(nameof(httpClient));

            if (requestUri is null)
                throw new ArgumentNullException(nameof(requestUri));

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(MediaTypes.Json));
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(MediaTypes.Html));

            var response = await httpClient.SendRequestAsync(request, cancellationToken ?? CancellationToken.None);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync() ?? string.Empty;
        }

        [NotNull]
        public static Task<T> GetFromJsonAsync<T>(
            [NotNull] this IHttpClient httpClient, [NotNull] string requestUri, [CanBeNull] CancellationToken? cancellationToken = null)
            => GetFromJsonAsync<T>(httpClient, new Uri(requestUri), cancellationToken);

        [NotNull]
        public static async Task<T> GetFromJsonAsync<T>(
            [NotNull] this IHttpClient httpClient, [NotNull] Uri requestUri, [CanBeNull] CancellationToken? cancellationToken = null)
        {
            if (httpClient is null)
                throw new ArgumentNullException(nameof(httpClient));

            if (requestUri is null)
                throw new ArgumentNullException(nameof(requestUri));

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(MediaTypes.Json));

            var response = await httpClient.SendRequestAsync(request, cancellationToken ?? CancellationToken.None);
            response.EnsureSuccessStatusCode();

            if (response.Content.Headers.ContentType.MediaType != MediaTypes.Json)
                throw new InvalidOperationException(
                    $"Invalid media-type, expected '{MediaTypes.Json}', got '{response.Content.Headers.ContentType.MediaType}'");

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        [NotNull]
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(
            [NotNull] this IHttpClient httpClient, [NotNull] string requestUri, [NotNull] T payload,
            [CanBeNull] CancellationToken? cancellationToken = null)
            => PostAsJsonAsync(httpClient, new Uri(requestUri), payload, cancellationToken);

        [NotNull]
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(
            [NotNull] this IHttpClient httpClient, [NotNull] Uri requestUri, [NotNull] T payload,
            [CanBeNull] CancellationToken? cancellationToken = null)
            => PostOrPutAsJsonAsync(httpClient, HttpMethod.Post, requestUri, payload, cancellationToken);

        [NotNull]
        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(
            [NotNull] this IHttpClient httpClient, [NotNull] string requestUri, [NotNull] T payload,
            [CanBeNull] CancellationToken? cancellationToken = null)
            => PutAsJsonAsync(httpClient, new Uri(requestUri), payload, cancellationToken);

        [NotNull]
        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(
            [NotNull] this IHttpClient httpClient, [NotNull] Uri requestUri, [NotNull] T payload,
            [CanBeNull] CancellationToken? cancellationToken = null)
            => PostOrPutAsJsonAsync(httpClient, HttpMethod.Put, requestUri, payload, cancellationToken);

        [NotNull]
        private static async Task<HttpResponseMessage> PostOrPutAsJsonAsync<T>(
            [NotNull] IHttpClient httpClient, HttpMethod method, [NotNull] Uri requestUri, [NotNull] T payload,
            [CanBeNull] CancellationToken? cancellationToken = null)
        {
            if (httpClient is null)
                throw new ArgumentNullException(nameof(httpClient));

            if (requestUri is null)
                throw new ArgumentNullException(nameof(requestUri));

            if (payload == null)
                throw new ArgumentNullException(nameof(payload));

            var json = JsonConvert.SerializeObject(payload);

            var request = new HttpRequestMessage(method, requestUri) { Content = new StringContent(json, Encoding.UTF8, MediaTypes.Json) };

            var response = await httpClient.SendRequestAsync(request, cancellationToken ?? CancellationToken.None);
            response.EnsureSuccessStatusCode();

            return response;
        }
    }
}