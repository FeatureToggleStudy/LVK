using JetBrains.Annotations;

namespace LVK.AppCore.Windows.Service.Configuration
{
    internal class WindowsServiceConfigurationStartModel
    {
        [UsedImplicitly]
        public WindowsServiceConfigurationStartType Type { get; set; } = WindowsServiceConfigurationStartType.Manual;

        [UsedImplicitly]
        public bool Delayed { get; set; }
    }
}