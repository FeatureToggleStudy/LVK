using System;

using JetBrains.Annotations;

using LVK.Configuration;

namespace LVK.Net.Smtp
{
    internal class SmtpClientConfigurationProvider : ISmtpClientConfigurationProvider
    {
        [NotNull]
        private readonly IConfiguration _Configuration;

        public SmtpClientConfigurationProvider([NotNull] IConfiguration configuration)
        {
            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IConfigurationElement<SmtpClientConfiguration> GetConfiguration(string configurationProfileName)
            => _Configuration[$"Email/Smtp/{configurationProfileName}"].Element<SmtpClientConfiguration>();
    }
}