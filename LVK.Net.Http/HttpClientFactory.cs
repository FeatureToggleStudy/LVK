using System;

using JetBrains.Annotations;

using Microsoft.Extensions.Logging;

namespace LVK.Net.Http
{
    [UsedImplicitly]
    internal class HttpClientFactory : IHttpClientFactory
    {
        [NotNull]
        private readonly Func<IHttpClientOptions> _OptionsFactory;

        [NotNull]
        private readonly ILogger<IHttpClient> _Logger;

        public HttpClientFactory([NotNull] Func<IHttpClientOptions> optionsFactory, [NotNull] ILogger<IHttpClient> logger)
        {
            _OptionsFactory = optionsFactory ?? throw new ArgumentNullException(nameof(optionsFactory));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IHttpClient Create(IHttpClientOptions options) => new HttpClient(options, _Logger);
        public IHttpClient CreateDefault() => Create(_OptionsFactory());
    }
}