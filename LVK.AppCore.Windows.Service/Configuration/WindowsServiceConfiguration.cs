using System.Diagnostics;
using System.Text;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.Core;

namespace LVK.AppCore.Windows.Service.Configuration
{
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

        public WindowsServiceConfigurationLogonAs LogonAs => _Configuration.Value().Logon.LogonAs;

        public string LogonAsUsername => _Configuration.Value().Logon.Username;
        
        public string LogonAsPassword => _Configuration.Value().Logon.Password;

        public WindowsServiceConfigurationStartType StartType => _Configuration.Value().Start.Type;

        public bool DelayedStart => _Configuration.Value().Start.Delayed;

        public string ServiceName
        {
            get
            {
                WindowsServiceConfigurationModel configuration = _Configuration.Value();
                return string.IsNullOrWhiteSpace(configuration.Metadata.Name)
                    ? GetDefaultServiceName()
                    : configuration.Metadata.Name.NotNull();
            }
        }

        [NotNull]
        private string GetDefaultServiceName()
        {
            using (Process currentProcess = Process.GetCurrentProcess())
                return currentProcess.ProcessName;
        }

        public string DisplayName
        {
            get
            {
                WindowsServiceConfigurationModel configuration = _Configuration.Value();
                return string.IsNullOrWhiteSpace(configuration.Metadata.DisplayName)
                    ? GetDefaultDisplayName()
                    : configuration.Metadata.DisplayName.NotNull();
            }
        }

        [NotNull]
        private string GetDefaultDisplayName() => ServiceName;

        public string Description
        {
            get
            {
                WindowsServiceConfigurationModel configuration = _Configuration.Value();
                return string.IsNullOrWhiteSpace(configuration.Metadata.Description)
                    ? GetDefaultDescription()
                    : configuration.Metadata.Description.NotNull();
            }
        }

        [NotNull]
        private string GetDefaultDescription() => UntitleCase(DisplayName);

        [NotNull]
        private string UntitleCase([NotNull] string text)
        {
            var result = new StringBuilder();
            foreach (var c in text.ToCharArray())
            {
                if (char.IsUpper(c) && result.Length > 0)
                    result.Append(' ');
                result.Append(c);
            }
            
            return result.ToString();
        }
    }
}