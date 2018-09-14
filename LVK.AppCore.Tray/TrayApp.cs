using System;
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
        private readonly IBackgroundServicesManager _BackgroundServicesManager;

        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        [NotNull]
        private readonly ILogger _Logger;

        public TrayApp(
            [NotNull] IBackgroundServicesManager backgroundServicesManager,
            [NotNull] IApplicationLifetimeManager applicationLifetimeManager, [NotNull] ILogger logger)
        {
            _BackgroundServicesManager = backgroundServicesManager ?? throw new ArgumentNullException(nameof(backgroundServicesManager));
            _ApplicationLifetimeManager = applicationLifetimeManager
                                       ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));

            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [NotNull]
        public async Task<int> RunAsync()
        {
            using (_Logger.LogScope(LogLevel.Trace, "TrayApp.RunAsync"))
            {
                _BackgroundServicesManager.StartBackgroundServices();
                try
                {
                    await _ApplicationLifetimeManager.GracefulTerminationCancellationToken.AsTask();
                    return 0;
                }
                catch (Exception ex)
                {
                    _Logger.LogException(ex);
                    return 1;
                }
                finally
                {
                    await _BackgroundServicesManager.WaitForBackgroundServicesToStop();
                }
            }
        }
    }
}