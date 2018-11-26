using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core.Services;
using LVK.Logging;

namespace LVK.AppCore.Windows.Service
{
    [UsedImplicitly]
    internal class WindowsService : ServiceBase
    {
        [NotNull]
        private readonly IBackgroundServicesManager _BackgroundServicesManager;

        [NotNull]
        private readonly ILogger _Logger;

        private int _StopAlreadyRequested;

        [NotNull]
        private readonly object _Lock = new object();

        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        [CanBeNull]
        private CancellationTokenRegistration? _CancellationTokenRegistration;

        public WindowsService(
            [NotNull] IWindowsServiceConfiguration configuration, [NotNull] IApplicationLifetimeManager applicationLifetimeManager,
            [NotNull] IBackgroundServicesManager backgroundServicesManager, [NotNull] ILogger logger)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            _BackgroundServicesManager = backgroundServicesManager ?? throw new ArgumentNullException(nameof(backgroundServicesManager));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));

            ServiceName = configuration.ServiceName;
            CanStop = true;
        }

        private async void GracefulTerminationRequested()
        {
            if (Interlocked.Exchange(ref _StopAlreadyRequested, 1) == 1)
                return;

            await Task.Yield();

            lock (_Lock)
            {
                _CancellationTokenRegistration?.Dispose();
                _CancellationTokenRegistration = null;
            }

            Stop();
        }

        protected override void OnStart(string[] args)
        {
            using (_Logger.LogScope(LogLevel.Debug, $"Starting background services in '{ServiceName}'"))
            {
                _Logger.LogInformation($"Starting background services in '{ServiceName}'");
                lock (_Lock)
                {
                    _CancellationTokenRegistration =
                        _ApplicationLifetimeManager.GracefulTerminationCancellationToken.Register(GracefulTerminationRequested);
                }

                _BackgroundServicesManager.StartBackgroundServices();
                _Logger.LogInformation($"Background services in '{ServiceName}' started successfully");
            }

            base.OnStart(args);
        }

        protected override void OnStop()
        {
            using (_Logger.LogScope(LogLevel.Debug, $"Stopping background services in '{ServiceName}'"))
            {
                _Logger.LogInformation($"Stopping background services in '{ServiceName}'");

                lock (_Lock)
                {
                    _CancellationTokenRegistration?.Dispose();
                    _CancellationTokenRegistration = null;
                }

                _ApplicationLifetimeManager.SignalGracefulTermination();
                _BackgroundServicesManager.WaitForBackgroundServicesToStop().ConfigureAwait(false).GetAwaiter().GetResult();
                
                _Logger.LogInformation($"Background services in '{ServiceName}' stopped successfully");
            }

            base.OnStop();
        }
    }
}