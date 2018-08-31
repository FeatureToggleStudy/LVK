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
        public static Task<string> GetStringAsync([NotNull] this HttpClient httpClient, [NotNull] string requestUri,
                                                  CancellationToken? cancellationToken = null)
            => GetStringAsync(httpClient, new Uri(requestUri), cancellationToken);

        public static async Task<string> GetStringAsync([NotNull] this HttpClient httpClient, [NotNull] Uri requestUri,
                                                        CancellationToken? cancellationToken = null)
        {
            if (httpClient is null)
                throw new ArgumentNullException(nameof(httpClient));

            if (requestUri is null)
                throw new ArgumentNullException(nameof(requestUri));

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

            var response = await httpClient.SendAsync(request, cancellationToken ?? CancellationToken.None);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync() ?? string.Empty;
        }

        public static Task<T> GetFromJsonAsync<T>([NotNull] this HttpClient httpClient, [NotNull] string requestUri,
                                                  CancellationToken? cancellationToken = null)
            => GetFromJsonAsync<T>(httpClient, new Uri(requestUri), cancellationToken);

        public static async Task<T> GetFromJsonAsync<T>([NotNull] this HttpClient httpClient, [NotNull] Uri requestUri,
                                                        CancellationToken? cancellationToken = null)
        {
            if (httpClient is null)
                throw new ArgumentNullException(nameof(httpClient));

            if (requestUri is null)
                throw new ArgumentNullException(nameof(requestUri));

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

            var response = await httpClient.SendAsync(request, cancellationToken ?? CancellationToken.None);
            response.EnsureSuccessStatusCode();

            if (response.Content.Headers.ContentType.MediaType != "application/json")
                throw new InvalidOperationException(
                    $"Invalid media-type, expected 'application/json', got '{response.Content.Headers.ContentType.MediaType}'");

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static Task<HttpResponseMessage> PostAsJsonAsync<T>([NotNull] this HttpClient httpClient,
                                                                   [NotNull] string requestUri, T payload,
                                                                   CancellationToken? cancellationToken = null)
            => PostAsJsonAsync(httpClient, new Uri(requestUri), payload, cancellationToken);

        [NotNull]
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>([NotNull] this HttpClient httpClient,
                                                                   [NotNull] Uri requestUri, T payload,
                                                                   CancellationToken? cancellationToken = null)
            => PostOrPutAsJsonAsync(httpClient, HttpMethod.Post, requestUri, payload, cancellationToken);

        [NotNull]
        public static Task<HttpResponseMessage> PutAsJsonAsync<T>([NotNull] this HttpClient httpClient,
                                                                  [NotNull] string requestUri, T payload,
                                                                  CancellationToken? cancellationToken = null)
            => PutAsJsonAsync(httpClient, new Uri(requestUri), payload, cancellationToken);

        [NotNull]
        public static Task<HttpResponseMessage> PutAsJsonAsync<T>([NotNull] this HttpClient httpClient,
                                                                  [NotNull] Uri requestUri, T payload,
                                                                  CancellationToken? cancellationToken = null)
            => PostOrPutAsJsonAsync(httpClient, HttpMethod.Put, requestUri, payload, cancellationToken);

        private static async Task<HttpResponseMessage> PostOrPutAsJsonAsync<T>(
            [NotNull] HttpClient httpClient, HttpMethod method, [NotNull] Uri requestUri, T payload,
            CancellationToken? cancellationToken = null)
        {
            if (httpClient is null)
                throw new ArgumentNullException(nameof(httpClient));

            if (requestUri is null)
                throw new ArgumentNullException(nameof(requestUri));

            if (payload == null)
                throw new ArgumentNullException(nameof(payload));

            var json = JsonConvert.SerializeObject(payload);

            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request, cancellationToken ?? CancellationToken.None);
            response.EnsureSuccessStatusCode();

            return response;
        }
    }
}