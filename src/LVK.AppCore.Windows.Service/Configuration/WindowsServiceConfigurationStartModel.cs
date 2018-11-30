namespace LVK.AppCore.Windows.Service.Configuration
{
    internal class WindowsServiceConfigurationStartModel
    {
        public WindowsServiceConfigurationStartType Type { get; set; } = WindowsServiceConfigurationStartType.Manual;

        public bool Delayed { get; set; }
    }
}