using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Logging;

namespace LVK.Net.Http
{
    public class HttpClientLoggingHandler : DelegatingHandler
    {
        [NotNull]
        private readonly ILogger _Logger;

        public HttpClientLoggingHandler([NotNull] ILogger logger)
            : this(new HttpClientHandler(), logger)
        {
        }

        public HttpClientLoggingHandler([NotNull] HttpMessageHandler underlyingHttpMessageHandler, [NotNull] ILogger logger)
            : base(underlyingHttpMessageHandler)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            _Logger.LogTrace($"HttpClient.{request?.Method}: {request?.RequestUri}");
            
            Stopwatch stopwatch = Stopwatch.StartNew();
            var response = await base.SendAsync(request, cancellationToken).NotNull();
            stopwatch.Stop();
            
            _Logger.LogTrace($"HttpClient.{request?.Method}: {request?.RequestUri} --> {response.StatusCode} in {stopwatch.ElapsedMilliseconds} ms");

            return response;
        }
    }
}