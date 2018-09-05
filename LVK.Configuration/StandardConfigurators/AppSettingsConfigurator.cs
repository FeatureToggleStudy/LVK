namespace LVK.Configuration.StandardConfigurators
{
    internal class AppSettingsConfigurator : IConfigurationConfigurator
    {
        public void Configure(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.AddJsonFile("appsettings.json", isOptional: true);
            configurationBuilder.AddJsonFile("appsettings.debug.json", isOptional: true);
        }
    }
}