using JetBrains.Annotations;

namespace LVK.AppCore.Windows.Service.Configuration
{
    internal interface IWindowsServiceConfiguration
    {
        [NotNull]
        string ServiceName { get; }

        [NotNull]
        string DisplayName { get; }

        [NotNull]
        string Description { get; }

        WindowsServiceConfigurationLogonAs LogonAs { get; }

        [CanBeNull]
        string LogonAsUsername { get; }

        [CanBeNull]
        string LogonAsPassword { get; }

        WindowsServiceConfigurationStartType StartType { get; }

        bool DelayedStart { get; }
    }
}