using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Core.Services;
using LVK.Logging;

using Microsoft.Win32;

namespace LVK.AppCore.Windows.Tray
{
    internal class ApplicationShutdownBackgroundService : IBackgroundService
    {
        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        public ApplicationShutdownBackgroundService([NotNull] ILogger logger, [NotNull] IApplicationLifetimeManager applicationLifetimeManager)
        {
            _Logger = logger;
            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));
        }

        private void SessionEnding(object sender, [NotNull] SessionEndingEventArgs e)
        {
            _Logger.Log(LogLevel.Information, $"Session is ending with reason '{e.Reason}', application gracefully shutting down");
            _ApplicationLifetimeManager.SignalGracefulTermination();
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            SystemEvents.SessionEnding += SessionEnding;
            try
            {
                await cancellationToken.AsTask();
            }
            finally
            {
                SystemEvents.SessionEnding -= SessionEnding;
            }
        }
    }
}