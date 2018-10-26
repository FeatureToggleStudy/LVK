using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.Core;
using LVK.Logging;

namespace LVK.Net.Http
{
    [PublicAPI]
    public interface IHttpClientProvider
    {
        [NotNull]
        HttpClient Provide([NotNull] string name);
    }

    internal class HttpClientProvider : IHttpClientProvider
    {
        [NotNull]
        private readonly IConfiguration _Configuration;

        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly Dictionary<string, HttpClient> _Clients = new Dictionary<string, HttpClient>();

        [NotNull]
        private readonly object _Lock = new object();

        public HttpClientProvider([NotNull] IConfiguration configuration, [NotNull] ILogger logger)
        {
            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public HttpClient Provide(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            lock (_Lock)
            {
                return _Clients.GetOrAdd(name, () => Createclient(name)).NotNull();
            }
        }

        [NotNull]
        private HttpClient Createclient([NotNull] string name)
        {
            var clientHandler = new LoggingHttpClientHandler(_Logger);
            var client = new HttpClient(clientHandler);

            var configuration = _Configuration[$"HttpClients/{name}"].Element<HttpClientConfiguration>().ValueOrDefault(() => new HttpClientConfiguration());
            configuration.Configure(clientHandler, client);

            return client;
        }
    }
}