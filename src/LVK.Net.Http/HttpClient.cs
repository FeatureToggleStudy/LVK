using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Logging;

namespace LVK.Net.Http
{
    internal class HttpClient : IHttpClient
    {
        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly System.Net.Http.HttpClient _Client = new System.Net.Http.HttpClient();

        public HttpClient([NotNull] ILogger logger)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, CancellationToken? cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            using (_Logger.LogScope(LogLevel.Debug, $"HttpClient.{request.Method}: {request.RequestUri}"))
            {
                return await _Client.SendAsync(
                        request, HttpCompletionOption.ResponseHeadersRead, cancellationToken ?? CancellationToken.None)
                   .NotNull();
            }
        }
    }
}