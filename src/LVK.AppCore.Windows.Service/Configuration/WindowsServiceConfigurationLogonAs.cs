using JetBrains.Annotations;

namespace LVK.AppCore.Windows.Service.Configuration
{
    internal enum WindowsServiceConfigurationLogonAs
    {
        LocalSystem,
        NetworkService,
        SpecificUser,
    
        [UsedImplicitly]
        User = SpecificUser
    }
}