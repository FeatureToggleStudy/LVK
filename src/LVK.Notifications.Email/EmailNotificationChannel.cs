using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.Logging;
using LVK.Net.Smtp;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Notifications.Email
{
    internal class EmailNotificationChannel : INotificationChannel
    {
        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly ISmtpClientProvider _SmtpClientProvider;

        [NotNull]
        private readonly IConfigurationElementWithDefault<EmailNotificationChannelConfiguration> _Configuration;

        public EmailNotificationChannel([NotNull] IConfiguration configuration, [NotNull] ILogger logger, [NotNull] ISmtpClientProvider smtpClientProvider)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _SmtpClientProvider = smtpClientProvider ?? throw new ArgumentNullException(nameof(smtpClientProvider));
            _Configuration = configuration["Notifications/Email"]
               .Element<EmailNotificationChannelConfiguration>()
               .WithDefault(() => new EmailNotificationChannelConfiguration());
        }

        public async Task SendNotificationAsync(string subject, string body)
        {
            var configuration = _Configuration.Value();
            if (!configuration.IsEnabled())
            {
                _Logger.Log(LogLevel.Debug, "Email notification channel is not enabled");
                return;
            }

            assume(configuration.Profile != null);
            assume(configuration.From != null);

            var client = _SmtpClientProvider.Provide(configuration.Profile);
            var messageBuilder = client.CreateEmailMessageBuilder();
            messageBuilder.From = configuration.From;
            messageBuilder.To.AddRange(configuration.To);
            messageBuilder.Subject = subject;
            messageBuilder.Body = body;

            await client.SendEmailMessageAsync(messageBuilder.Build());
        }
    }
}