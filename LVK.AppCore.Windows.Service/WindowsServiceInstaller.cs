using System;
using System.Configuration.Install;
using System.ServiceProcess;

using JetBrains.Annotations;

using LVK.AppCore.Windows.Service.Configuration;

namespace LVK.AppCore.Windows.Service
{
    internal class WindowsServiceInstaller : Installer
    {
        public WindowsServiceInstaller([NotNull] IWindowsServiceConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            AddProcessInstaller(configuration);
            AddServiceInstaller(configuration);
        }

        private void AddServiceInstaller([NotNull] IWindowsServiceConfiguration configuration)
        {
            var serviceInstaller = new ServiceInstaller
            {
                ServiceName = configuration.ServiceName,
                Description = configuration.Description,
                DisplayName = configuration.DisplayName
            };

            switch (configuration.StartType)
            {
                case WindowsServiceConfigurationStartType.Automatic:
                    serviceInstaller.StartType = ServiceStartMode.Automatic;
                    serviceInstaller.DelayedAutoStart = configuration.DelayedStart;
                    break;
                
                case WindowsServiceConfigurationStartType.Disabled:
                    serviceInstaller.StartType = ServiceStartMode.Disabled;
                    break;
                case WindowsServiceConfigurationStartType.Manual:
                    serviceInstaller.StartType = ServiceStartMode.Manual;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Installers.Add(serviceInstaller);
        }

        private void AddProcessInstaller([NotNull] IWindowsServiceConfiguration configuration)
        {
            var processInstaller = new ServiceProcessInstaller();
            switch (configuration.LogonAs)
            {
                case WindowsServiceConfigurationLogonAs.LocalSystem:
                    processInstaller.Account = ServiceAccount.LocalSystem;
                    break;
                
                case WindowsServiceConfigurationLogonAs.NetworkService:
                    processInstaller.Account = ServiceAccount.NetworkService;
                    break;

                case WindowsServiceConfigurationLogonAs.SpecificUser:
                    processInstaller.Account = ServiceAccount.User;
                    processInstaller.Username = configuration.LogonAsUsername;
                    processInstaller.Password = configuration.LogonAsPassword;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Installers.Add(processInstaller);
        }
    }
}