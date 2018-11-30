using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Logging;

namespace LVK.Net.Http
{
    [PublicAPI]
    public class LoggingHttpClientHandler : HttpClientHandler
    {
        [NotNull]
        private readonly ILogger _Logger;

        public LoggingHttpClientHandler([NotNull] ILogger logger)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (_Logger.LogScope(LogLevel.Debug, $"HttpClient.{request?.Method}: {request?.RequestUri}"))
            {
                return await base.SendAsync(request, cancellationToken).NotNull();
            }
        }
    }
}