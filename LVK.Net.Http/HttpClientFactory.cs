using System;

using JetBrains.Annotations;

using Microsoft.Extensions.Logging;

namespace LVK.Net.Http
{
    [UsedImplicitly]
    internal class HttpClientFactory : IHttpClientFactory
    {
        [NotNull]
        private readonly IHttpClientOptions _DefaultOptions;

        [NotNull]
        private readonly ILogger<IHttpClient> _Logger;

        public HttpClientFactory([NotNull] IHttpClientOptions defaultOptions, [NotNull] ILogger<IHttpClient> logger)
        {
            _DefaultOptions = defaultOptions ?? throw new ArgumentNullException(nameof(defaultOptions));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IHttpClient Create(IHttpClientOptions options) => new HttpClient(options.Clone(), _Logger);
        public IHttpClient CreateDefault() => Create(_DefaultOptions);
    }
}