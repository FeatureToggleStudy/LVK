using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Core.Services;
using LVK.Logging;

namespace LVK.AppCore.Tray
{
    [UsedImplicitly]
    internal class TrayApp
    {
        [NotNull]
        private readonly IEnumerable<IBackgroundService> _BackgroundServices;

        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        [NotNull]
        private readonly ILogger _Logger;

        public TrayApp(
            [NotNull] IEnumerable<IBackgroundService> backgroundServices,
            [NotNull] IApplicationLifetimeManager applicationLifetimeManager, [NotNull] ILogger logger)
        {
            _BackgroundServices = backgroundServices ?? throw new ArgumentNullException(nameof(backgroundServices));
            _ApplicationLifetimeManager = applicationLifetimeManager
                                       ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));

            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [NotNull]
        public async Task<int> RunAsync()
        {
            using (_Logger.LogScope(LogLevel.Trace, "TrayApp.RunAsync"))
            {
                await StartBackgroundServices();
                try
                {
                    var tcs = new TaskCompletionSource<bool>();
                    _ApplicationLifetimeManager.GracefulTerminationCancellationToken.Register(() => tcs.SetResult(true));
                    await tcs.Task.NotNull();
                    return 0;
                }
                catch (Exception ex)
                {
                    _Logger.LogException(ex);
                    return 1;
                }
                finally
                {
                    await StopBackgroundServices();
                }
            }
        }

        private async Task StartBackgroundServices()
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(40));
            foreach (var service in _BackgroundServices)
                using (_Logger.LogScope(
                    LogLevel.Debug, $"awaiting background service startup of {service.GetType().Name}"))
                    await service.Start(cts.Token);
        }

        private async Task StopBackgroundServices()
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(40));
            foreach (var service in _BackgroundServices)
            {
                using (_Logger.LogScope(
                    LogLevel.Debug, $"awaiting background service stop of {service.GetType().Name}"))
                {
                    try
                    {
                        await service.Stop(cts.Token);
                    }
                    catch (TaskCanceledException)
                    {
                    }
                }
            }
        }
    }
}