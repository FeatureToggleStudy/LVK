using JetBrains.Annotations;

namespace LVK.Notifications.Pushbullet
{
    [UsedImplicitly]
    internal class PushbulletNotificationChannelConfiguration
    {
        [UsedImplicitly]
        public string AccessToken { get; set; }

        public bool IsEnabled() => !string.IsNullOrWhiteSpace(AccessToken);
    }
}