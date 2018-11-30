using System;

using JetBrains.Annotations;

using LVK.AppCore.Windows.Service.Configuration;

namespace LVK.AppCore.Windows.Service.Commands
{
    internal class UninstallConfiguration : IWindowsServiceConfiguration
    {
        public UninstallConfiguration([NotNull] string serviceName)
        {
            ServiceName = serviceName ?? throw new ArgumentNullException(nameof(serviceName));
        }

        public string ServiceName { get; }

        public string DisplayName => string.Empty;

        public string Description => string.Empty;

        public WindowsServiceConfigurationLogonAs LogonAs => WindowsServiceConfigurationLogonAs.LocalSystem;

        public string LogonAsUsername => null;

        public string LogonAsPassword => null;

        public WindowsServiceConfigurationStartType StartType => WindowsServiceConfigurationStartType.Automatic;

        public bool DelayedStart => false;
    }
}