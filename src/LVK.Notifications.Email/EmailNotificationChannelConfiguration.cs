using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace LVK.Notifications.Email
{
    [UsedImplicitly]
    internal class EmailNotificationChannelConfiguration
    {
        [CanBeNull]
        public string Profile { get; set; }

        [CanBeNull]
        public string From { get; set; }

        [NotNull, ItemNotNull]
        public List<string> To { get; } = new List<string>();

        public bool IsEnabled() => !string.IsNullOrWhiteSpace(Profile) && !string.IsNullOrWhiteSpace(From) && To.Any();
    }
}