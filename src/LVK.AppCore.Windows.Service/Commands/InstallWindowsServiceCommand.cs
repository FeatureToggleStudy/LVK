using System;
using System.Collections;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore.Windows.Service.Configuration;

namespace LVK.AppCore.Windows.Service.Commands
{
    internal class InstallWindowsServiceCommand : IApplicationCommand
    {
        [NotNull]
        private readonly IWindowsServiceConfiguration _Configuration;

        [NotNull]
        private readonly IInstallContextProvider _InstallContextProvider;

        [NotNull]
        private readonly IPersistentInstallState _PersistentInstallState;

        public InstallWindowsServiceCommand(
            [NotNull] IWindowsServiceConfiguration configuration, [NotNull] IInstallContextProvider installContextProvider,
            [NotNull] IPersistentInstallState persistentInstallState)
        {
            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _InstallContextProvider = installContextProvider ?? throw new ArgumentNullException(nameof(installContextProvider));
            _PersistentInstallState = persistentInstallState ?? throw new ArgumentNullException(nameof(persistentInstallState));
        }

        public string[] CommandNames => new[] { "install" };

        public string Description => "install windows service";

        public Task<int> TryExecute()
        {
            var installer = new WindowsServiceInstaller(_Configuration) { Context = _InstallContextProvider.GetContext() };

            var state = new Hashtable();
            state[StateConstants.InstalledServiceName] = _Configuration.ServiceName;
            try
            {
                installer.Install(state);
                installer.Commit(state);

                _PersistentInstallState.Save(state);
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

                throw;
            }

            return Task.FromResult(0);
        }
    }
}