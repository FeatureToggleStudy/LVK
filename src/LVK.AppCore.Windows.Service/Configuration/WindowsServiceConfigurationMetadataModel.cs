using JetBrains.Annotations;

namespace LVK.AppCore.Windows.Service.Configuration
{
    internal class WindowsServiceConfigurationMetadataModel
    {
        [CanBeNull]
        [UsedImplicitly]
        public string Name { get; set; }

        [CanBeNull]
        [UsedImplicitly]
        public string Description { get; set; }
        
        [CanBeNull]
        [UsedImplicitly]
        public string DisplayName { get; set; }
    }
}