using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore.Windows.Service.Configuration;
using LVK.Core;
using LVK.Core.Services;
using LVK.Logging;

namespace LVK.AppCore.Windows.Service.Commands
{
    internal class RunCommand : IApplicationCommand
    {
        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly IWindowsServiceConfiguration _Configuration;

        [NotNull]
        private readonly IBackgroundServicesManager _BackgroundServicesManager;

        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        public RunCommand(
            [NotNull] ILogger logger, [NotNull] IWindowsServiceConfiguration configuration,
            [NotNull] IBackgroundServicesManager backgroundServicesManager,
            [NotNull] IApplicationLifetimeManager applicationLifetimeManager)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _BackgroundServicesManager = backgroundServicesManager ?? throw new ArgumentNullException(nameof(backgroundServicesManager));
            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));
        }

        public string[] CommandNames => new[] { "run" };

        public string Description => "Run the service code outside a windows service";

        public async Task<int> TryExecute()
        {
            _BackgroundServicesManager.StartBackgroundServices();
            await _ApplicationLifetimeManager.GracefulTerminationCancellationToken.AsTask();
            await _BackgroundServicesManager.WaitForBackgroundServicesToStop();
            return 0;
        }
    }
}