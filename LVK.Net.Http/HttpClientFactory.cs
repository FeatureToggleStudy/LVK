using System;
using System.Net.Http;

using JetBrains.Annotations;

using LVK.Logging;

namespace LVK.Net.Http
{
    internal static class HttpClientFactory
    {
        [NotNull]
        public static HttpClient Create(HttpMessageHandler httpMessageHandler, [NotNull] ILogger logger)
        {
            if (logger is null)
                throw new ArgumentNullException(nameof(logger));

            HttpClientLoggingHandler handler = httpMessageHandler != null
                ? new HttpClientLoggingHandler(httpMessageHandler, logger)
                : new HttpClientLoggingHandler(logger);

            return new HttpClient(handler);
        }
    }
}