using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using JetBrains.Annotations;

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
                    var ctx = new ApplicationContext();
                    _ApplicationLifetimeManager.GracefulTerminationCancellationToken.Register(
                        delegate
                        {
                            ctx.ExitThread();
                        });

                    Application.Run(ctx);

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
                await service.Start(cts.Token);
        }

        private async Task StopBackgroundServices()
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(40));
            foreach (var service in _BackgroundServices)
                await service.Stop(cts.Token);
        }
    }
}