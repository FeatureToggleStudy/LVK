using System.Net.Http;

using JetBrains.Annotations;

using LVK.Core;

namespace LVK.Net.Http
{
    internal class HttpClientDefaultRequestHeadersConfiguration
    {
        public void Configure([NotNull] HttpClient client)
        {
            var headers = client.DefaultRequestHeaders.NotNull();
            if (!string.IsNullOrWhiteSpace(Host))
                headers.Host = Host;

            if (!string.IsNullOrWhiteSpace(UserAgent))
                headers.UserAgent.NotNull().ParseAdd(UserAgent);
        }

        [UsedImplicitly]
        public string Host { get; set; }

        [UsedImplicitly]
        public string UserAgent { get; set; }
    }
}