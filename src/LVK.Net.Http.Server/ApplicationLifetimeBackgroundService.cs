using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Core.Services;
using LVK.Logging;

using Microsoft.Extensions.Hosting;

namespace LVK.Net.Http.Server
{
    internal class ApplicationLifetimeBackgroundService : IBackgroundService
    {
        [NotNull]
        private readonly IApplicationLifetime _ApplicationLifetime;

        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        [NotNull]
        private readonly ILogger _Logger;

        public ApplicationLifetimeBackgroundService([NotNull] IApplicationLifetime applicationLifetime, [NotNull] IApplicationLifetimeManager applicationLifetimeManager, [NotNull] ILogger logger)
        {
            _ApplicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            using (_Logger.LogScope(LogLevel.Trace, $"{nameof(ApplicationLifetimeBackgroundService)}.{nameof(Execute)}"))
            {
                await _ApplicationLifetimeManager.GracefulTerminationCancellationToken.AsTask();
                
                _Logger.LogVerbose("application has requested shutdown, shutting down web host");
                _ApplicationLifetime.StopApplication();
            }
        }
    }
}