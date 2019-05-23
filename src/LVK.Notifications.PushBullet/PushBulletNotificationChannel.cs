using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.Core;
using LVK.Logging;

using PushBulletNet;

namespace LVK.Notifications.Pushbullet
{
    internal class PushbulletNotificationChannel : INotificationChannel
    {
        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly IConfigurationElementWithDefault<PushbulletNotificationChannelConfiguration> _Configuration;

        public PushbulletNotificationChannel([NotNull] IConfiguration configuration, [NotNull] ILogger logger)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _Configuration = configuration["Notifications/Pushbullet"]
               .Element<PushbulletNotificationChannelConfiguration>()
               .WithDefault(() => new PushbulletNotificationChannelConfiguration());
        }

        public async Task SendNotificationAsync(string subject, string body)
        {
            var configuration = _Configuration.Value();
            if (!configuration.IsEnabled())
            {
                _Logger.Log(LogLevel.Debug, "Pushbullet notification channel is not enabled");
                return;
            }

            var client = new PushBulletClient(configuration.AccessToken);
            await client.PushAsync(subject, body, null).NotNull();
        }
    }
}