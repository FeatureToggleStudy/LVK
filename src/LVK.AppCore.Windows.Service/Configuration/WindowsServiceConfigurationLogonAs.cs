namespace LVK.AppCore.Windows.Service.Configuration
{
    internal enum WindowsServiceConfigurationLogonAs
    {
        LocalSystem,
        NetworkService,
        SpecificUser,
        User = SpecificUser
    }
}