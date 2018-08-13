using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace LVK.Net.Http
{
    internal class HttpClient : IHttpClient
    {
        [NotNull]
        private readonly ILogger<IHttpClient> _Logger;

        [NotNull]
        private readonly System.Net.Http.HttpClient _Client;

        public HttpClient([NotNull] IHttpClientOptions options, [NotNull] ILogger<IHttpClient> logger)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Options = options ?? throw new ArgumentNullException(nameof(options));
            _Client = new global::System.Net.Http.HttpClient();
            _Client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
        }

        public IHttpClientOptions Options { get; }

        public async Task<T> GetAsync<T>(string query, CancellationToken cancellationToken)
        {
            var json = await GetStringAsync(query, cancellationToken);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<string> GetStringAsync(string query, CancellationToken cancellationToken)
        {
            var fullUrl = GetFullUrl(query);
            var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);

            var response = await SendAsync(request, cancellationToken);
            var result = await response.Content.ReadAsStringAsync() ?? string.Empty;
            
            _Logger.LogTrace($"GET {fullUrl} --> {result.Length} characters");
            
            return result;
        }

        [NotNull, ItemNotNull]
        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _Logger.LogTrace($"{request.Method} {request.RequestUri}");
            Stopwatch stopwatch = Stopwatch.StartNew();

            var response = await _Client.SendAsync(request, cancellationToken);
            stopwatch.Stop();

            _Logger.LogTrace(
                $"{request.Method} {request.RequestUri} --> {response.StatusCode} in {stopwatch.ElapsedMilliseconds} ms");

            response.EnsureSuccessStatusCode();
            return response;
        }

        public Task PostAsync<T>(string query, T payload, CancellationToken cancellationToken)
            => PostAndPutAsync(query, HttpMethod.Post, payload, cancellationToken);

        public Task PutAsync<T>(string query, T payload, CancellationToken cancellationToken)
            => PostAndPutAsync(query, HttpMethod.Put, payload, cancellationToken);

        public async Task DeleteAsync(string query, CancellationToken cancellationToken)
        {
            var fullUrl = GetFullUrl(query);

            var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);

            await SendAsync(request, cancellationToken);
        }

        private async Task PostAndPutAsync<T>(string query, HttpMethod method, T payload, CancellationToken cancellationToken)
        {
            var fullUrl = GetFullUrl(query);
            var json = JsonConvert.SerializeObject(payload);

            var request = new HttpRequestMessage(method, fullUrl)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            await SendAsync(request, cancellationToken);
        }

        [NotNull]
        private string GetFullUrl([NotNull] string query)
        {
            if (string.IsNullOrWhiteSpace(Options.BaseUrl))
                return query;

            var baseUri = new Uri(Options.BaseUrl);
            var queryUri = new Uri(query, UriKind.Relative);
            return new Uri(baseUri, queryUri).ToString();
        }
    }
}