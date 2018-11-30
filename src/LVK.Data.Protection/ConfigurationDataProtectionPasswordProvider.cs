using System.Collections.Generic;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.Core;

namespace LVK.Data.Protection
{
    internal class ConfigurationDataProtectionPasswordProvider : IDataProtectionPasswordProvider
    {
        [NotNull]
        private IConfigurationElementWithDefault<Dictionary<string, string>> _Configuration;

        public ConfigurationDataProtectionPasswordProvider([NotNull] IConfiguration configuration)
        {
            _Configuration = configuration["DataProtection/Passwords"]
               .Element<Dictionary<string, string>>()
               .WithDefault(() => new Dictionary<string, string>());
        }

        public string TryGetPassword(string passwordName) => _Configuration.Value().GetValueOrDefault(passwordName);
    }
}