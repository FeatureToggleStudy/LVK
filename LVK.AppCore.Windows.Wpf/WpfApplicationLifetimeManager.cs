using System;
using System.Windows;

using JetBrains.Annotations;

using LVK.Core;

namespace LVK.AppCore.Windows.Wpf
{
    internal class WpfApplicationLifetimeManager : IWpfApplicationLifetimeManager
    {
        [NotNull]
        private readonly IBackgroundServicesManager _BackgroundServicesManager;

        public WpfApplicationLifetimeManager([NotNull] IBackgroundServicesManager backgroundServicesManager)
        {
            _BackgroundServicesManager = backgroundServicesManager ?? throw new ArgumentNullException(nameof(backgroundServicesManager));
        }

        public void Start()
        {
            _BackgroundServicesManager.StartBackgroundServices();

            Configure(Application.Current.NotNull());
        }

        private void Configure([NotNull] Application application)
        {
            application.Startup += ApplicationOnStartup;
            application.Exit += ApplicationOnExit;
        }

        private void ApplicationOnExit(object sender, ExitEventArgs e)
        {
            _BackgroundServicesManager.StartBackgroundServices();
        }

        private void ApplicationOnStartup(object sender, StartupEventArgs e)
        {
            _BackgroundServicesManager.WaitForBackgroundServicesToStop().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}