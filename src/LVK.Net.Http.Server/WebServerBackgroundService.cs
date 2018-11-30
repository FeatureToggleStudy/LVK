using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Core.Services;

namespace LVK.Net.Http.Server
{
    [PublicAPI]
    public class WebServerBackgroundService : IBackgroundService
    {
        [NotNull]
        private readonly IWebServer _WebServer;

        public WebServerBackgroundService([NotNull] IWebServer webServer)
        {
            _WebServer = webServer ?? throw new ArgumentNullException(nameof(webServer));
        }

        async Task IBackgroundService.Execute(CancellationToken cancellationToken)
        {
            await _WebServer.StartAsync();
            await cancellationToken.AsTask();
            await _WebServer.StopAsync();
        }
    }
}