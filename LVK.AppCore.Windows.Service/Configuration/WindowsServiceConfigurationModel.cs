using JetBrains.Annotations;

namespace LVK.AppCore.Windows.Service.Configuration
{
    [UsedImplicitly]
    internal class WindowsServiceConfigurationModel
    {
        [NotNull]
        public WindowsServiceConfigurationMetadataModel Metadata { get; } = new WindowsServiceConfigurationMetadataModel();

        [NotNull]
        public WindowsServiceConfigurationStartModel Start { get; } = new WindowsServiceConfigurationStartModel(); 

        [NotNull]
        public WindowsServiceConfigurationLogonModel Logon { get; } = new WindowsServiceConfigurationLogonModel();
    }
}