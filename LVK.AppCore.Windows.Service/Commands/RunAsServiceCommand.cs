using System;
using System.ServiceProcess;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core.Services;
using LVK.Logging;

namespace LVK.AppCore.Windows.Service.Commands
{
    internal class RunAsServiceCommand : IApplicationCommand
    {
        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly IWindowsServiceConfiguration _Configuration;

        [NotNull]
        private readonly IBackgroundServicesManager _BackgroundServicesManager;

        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        public RunAsServiceCommand(
            [NotNull] ILogger logger, [NotNull] IWindowsServiceConfiguration configuration,
            [NotNull] IBackgroundServicesManager backgroundServicesManager,
            [NotNull] IApplicationLifetimeManager applicationLifetimeManager)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _BackgroundServicesManager = backgroundServicesManager ?? throw new ArgumentNullException(nameof(backgroundServicesManager));
            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));
        }

        public string[] CommandNames => new[] { "runAsService" };

        public string Description => "Run as a Windows Service";

        public Task<int> TryExecute()
        {
            ServiceBase.Run(new WindowsService(_Configuration, _ApplicationLifetimeManager, _BackgroundServicesManager, _Logger));
            return Task.FromResult(0);
        }
    }
}