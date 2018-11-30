using System;

using JetBrains.Annotations;

using LVK.Logging;

namespace LVK.Net.Smtp
{
    internal class SmtpClientProvider : ISmtpClientProvider
    {
        [NotNull]
        private readonly ISmtpClientConfigurationProvider _SmtpClientConfigurationProvider;

        [NotNull]
        private readonly ILogger _Logger;

        public SmtpClientProvider([NotNull] ISmtpClientConfigurationProvider smtpClientConfigurationProvider, [NotNull] ILogger logger)
        {
            _SmtpClientConfigurationProvider = smtpClientConfigurationProvider
                                            ?? throw new ArgumentNullException(nameof(smtpClientConfigurationProvider));

            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ISmtpClient Provide(string configurationProfileName)
            => new SmtpClient(_SmtpClientConfigurationProvider.GetConfiguration(configurationProfileName), _Logger);
    }
}