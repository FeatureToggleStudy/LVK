using System;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.Logging;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Net.Smtp
{
    internal class SmtpClient : ISmtpClient
    {
        [NotNull]
        private readonly IConfigurationElement<SmtpClientConfiguration> _Configuration;

        [NotNull]
        private readonly ILogger _Logger;

        public SmtpClient([NotNull] IConfigurationElement<SmtpClientConfiguration> configuration, [NotNull] ILogger logger)
        {
            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ISmtpEmailMessageBuilder CreateEmailMessageBuilder() => new SmtpEmailMessageBuilder();

        public async Task SendEmailMessageAsync(ISmtpEmailMessage emailMessage)
        {
            var configuration = _Configuration.Value();
            if (configuration == null || !configuration.IsValid())
                throw new InvalidOperationException("Smtp configuration not present or valid");

            assume(configuration.Server?.Address != null);
            assume(configuration.Authentication != null);

            MailMessage mm = emailMessage.CreateFrameworkMailMessage();

            using (_Logger.LogScope(
                    LogLevel.Debug,
                    $"Sending email with subject '{mm.Subject}' from '{mm.From}' to '{string.Join(", ", mm.To)}' via '{configuration.Server.Address}:{configuration.Server.Port}'")
                )
            {
                using (var client = new System.Net.Mail.SmtpClient(configuration.Server.Address, configuration.Server.Port))
                {
                    client.Credentials = new NetworkCredential(
                        configuration.Authentication.Username, configuration.Authentication.Password);

                    var tcs = new TaskCompletionSource<bool>();

                    void onClientOnSendCompleted(object sender, AsyncCompletedEventArgs args)
                    {
                        assume(args != null);
                        if (args.Error != null)
                            tcs.SetException(args.Error);
                        else if (args.Cancelled)
                            tcs.SetCanceled();
                        else
                            tcs.SetResult(true);
                    }

                    client.SendCompleted += onClientOnSendCompleted;
                    client.SendAsync(mm, null);
                    await tcs.Task.NotNull();
                }
            }
        }
    }
}