using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace LVK.Notifications.Email
{
    [UsedImplicitly]
    internal class EmailNotificationChannelConfiguration
    {
        [CanBeNull]
        [UsedImplicitly]
        public string Profile { get; set; }

        [CanBeNull]
        [UsedImplicitly]
        public string From { get; set; }

        [NotNull, ItemNotNull]
        [UsedImplicitly]
        public List<string> To { get; } = new List<string>();

        public bool IsEnabled() => !string.IsNullOrWhiteSpace(Profile) && !string.IsNullOrWhiteSpace(From) && To.Any();
    }
}