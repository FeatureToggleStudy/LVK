using System.Net.Http;

using JetBrains.Annotations;

namespace LVK.Net.Http
{
    [PublicAPI]
    public static class HttpClientProviderExtensions
    {
        [NotNull]
        public static HttpClient Provide([NotNull] this IHttpClientProvider httpClientProvider) => httpClientProvider.Provide("Default");
    }
}