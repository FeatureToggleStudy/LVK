using JetBrains.Annotations;

namespace LVK.AppCore.Windows.Service.Configuration
{
    internal class WindowsServiceConfigurationMetadataModel
    {
        [CanBeNull]
        public string Name { get; set; }

        [CanBeNull]
        public string Description { get; set; }
        
        [CanBeNull]
        public string DisplayName { get; set; }
    }
}