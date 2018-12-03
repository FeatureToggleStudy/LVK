using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore.Windows.Service.Configuration;
using LVK.Core;

namespace LVK.AppCore.Windows.Service.Commands
{
    internal class UninstallWindowsServiceCommand : IApplicationCommand
    {
        [NotNull]
        private readonly IWindowsServiceConfiguration _Configuration;

        [NotNull]
        private readonly IInstallContextProvider _InstallContextProvider;

        [NotNull]
        private readonly IPersistentInstallState _PersistentInstallState;

        public UninstallWindowsServiceCommand(
            [NotNull] IWindowsServiceConfiguration configuration, [NotNull] IInstallContextProvider installContextProvider,
            [NotNull] IPersistentInstallState persistentInstallState)
        {
            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _InstallContextProvider = installContextProvider ?? throw new ArgumentNullException(nameof(installContextProvider));
            _PersistentInstallState = persistentInstallState ?? throw new ArgumentNullException(nameof(persistentInstallState));
        }

        public string[] CommandNames => new[] { "uninstall" };

        public string Description => "uninstall windows service";

        public Task<int> TryExecute(string[] arguments)
        {
            var state = _PersistentInstallState.Load();
            var serviceName = (string)state[StateConstants.InstalledServiceName].NotNull();
            var installer = new WindowsServiceInstaller(new UninstallConfiguration(serviceName)) { Context = _InstallContextProvider.GetContext() };

            try
            {
                installer.Uninstall(state);
            }
            catch
            {
                try
                {
                    installer.Rollback(state);
                }
                catch
                {
                    // Do nothing here
                }

                _PersistentInstallState.Delete();

                throw;
            }

            return Task.FromResult(0);
        }
    }
}