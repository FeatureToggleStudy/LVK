using System;
using System.Net;
using System.Net.Http;

using JetBrains.Annotations;

namespace LVK.Net.Http
{
    internal class HttpClientConfiguration
    {
        public void Configure([NotNull] HttpClientHandler clientHandler, [NotNull] HttpClient client)
        {
            if (Timeout.HasValue)
                client.Timeout = TimeSpan.FromSeconds(Timeout.GetValueOrDefault());

            if (!string.IsNullOrWhiteSpace(BaseAddress))
                client.BaseAddress = new Uri(BaseAddress);

            if (MaxResponseContentBufferSize.HasValue)
                client.MaxResponseContentBufferSize = MaxResponseContentBufferSize.GetValueOrDefault();

            DefaultRequestHeaders.Configure(client);
            
            clientHandler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            clientHandler.AllowAutoRedirect = true;
        }

        [UsedImplicitly]
        public int? Timeout { get; set; }

        [UsedImplicitly]
        public string BaseAddress { get; set; }

        [UsedImplicitly]
        public long? MaxResponseContentBufferSize { get; set; }

        [UsedImplicitly, NotNull]
        public HttpClientDefaultRequestHeadersConfiguration DefaultRequestHeaders { get; } = new HttpClientDefaultRequestHeadersConfiguration();
    }
}