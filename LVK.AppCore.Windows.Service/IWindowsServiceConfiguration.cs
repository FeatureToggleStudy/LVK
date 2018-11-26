using JetBrains.Annotations;

using LVK.Configuration;

namespace LVK.AppCore.Windows.Service
{
    internal interface IWindowsServiceConfiguration
    {
        [NotNull]
        string ServiceName { get; }
    }

    internal class WindowsServiceConfiguration : IWindowsServiceConfiguration
    {
        [NotNull]
        private readonly IConfigurationElementWithDefault<WindowsServiceConfigurationModel> _Configuration;

        public WindowsServiceConfiguration([NotNull] IConfiguration configuration)
        {
            _Configuration = configuration["WindowsService"]
               .Element<WindowsServiceConfigurationModel>()
               .WithDefault(() => new WindowsServiceConfigurationModel());
        }

        public string ServiceName => _Configuration.Value().Name;
    }
}