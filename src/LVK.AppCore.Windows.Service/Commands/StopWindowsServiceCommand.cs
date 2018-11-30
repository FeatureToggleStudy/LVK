using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Core.Services;
using LVK.Logging;

namespace LVK.AppCore.Windows.Service.Commands
{
    internal class StopWindowsServiceCommand : IApplicationCommand
    {
        [NotNull]
        private readonly IWindowsServiceController _WindowsServiceController;

        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        public StopWindowsServiceCommand(
            [NotNull] IWindowsServiceController windowsServiceController, [NotNull] ILogger logger,
            [NotNull] IApplicationLifetimeManager applicationLifetimeManager)
        {
            _WindowsServiceController = windowsServiceController ?? throw new ArgumentNullException(nameof(windowsServiceController));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));
        }

        public string[] CommandNames => new[] { "stop" };

        public string Description => "Stop Windows Service";

        public async Task<int> TryExecute()
        {
            _WindowsServiceController.Stop();

            _Logger.LogInformation("Waiting for service to stop");
            while (_WindowsServiceController.IsRunning() && !_ApplicationLifetimeManager.GracefulTerminationCancellationToken.IsCancellationRequested)
            {
                await Task.Delay(5000, _ApplicationLifetimeManager.GracefulTerminationCancellationToken).NotNull();
                if (_ApplicationLifetimeManager.GracefulTerminationCancellationToken.IsCancellationRequested)
                    break;
                
                _WindowsServiceController.QueryStatus();
            }

            _Logger.LogInformation("Service successfully stopped");

            return 0;
        }
    }
}