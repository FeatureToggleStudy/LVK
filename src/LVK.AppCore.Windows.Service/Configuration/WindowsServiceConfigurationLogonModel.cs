using JetBrains.Annotations;

namespace LVK.AppCore.Windows.Service.Configuration
{
    internal class WindowsServiceConfigurationLogonModel
    {
        [UsedImplicitly]
        public WindowsServiceConfigurationLogonAs LogonAs { get; set; }

        [CanBeNull]
        [UsedImplicitly]
        public string Username { get; set; }

        [CanBeNull]
        [UsedImplicitly]
        public string Password { get; set; }
    }
}