using JetBrains.Annotations;

namespace LVK.AppCore.Windows.Service.Configuration
{
    internal class WindowsServiceConfigurationLogonModel
    {
        public WindowsServiceConfigurationLogonAs LogonAs { get; set; }

        [CanBeNull]
        public string Username { get; set; }

        [CanBeNull]
        public string Password { get; set; }
    }
}