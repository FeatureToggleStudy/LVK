using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Logging;
using LVK.Reflection;

namespace LVK.Notifications
{
    internal class NotificationDispatcher : INotificationDispatcher
    {
        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly ITypeHelper _TypeHelper;

        [NotNull]
        private readonly List<INotificationChannel> _NotificationChannels;

        public NotificationDispatcher([NotNull] IEnumerable<INotificationChannel> notificationChannels, [NotNull] ILogger logger, [NotNull] ITypeHelper typeHelper)
        {
            if (notificationChannels == null)
                throw new ArgumentNullException(nameof(notificationChannels));

            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _TypeHelper = typeHelper ?? throw new ArgumentNullException(nameof(typeHelper));

            _NotificationChannels = notificationChannels.ToList();
        }

        public async Task NotifyAsync(string subject, string body)
        {
            using (_Logger.LogScope(LogLevel.Information, $"Dispatching notification '{subject}'/'{body}'"))
            {
                var tasks = _NotificationChannels.Select(
                        async channel =>
                        {
                            JetBrainsHelpers.assume(channel != null);
                            using (_Logger.LogScope(
                                LogLevel.Debug,
                                $"Dispatching notification '{subject}'/'{body}' to channel '{_TypeHelper.NameOf(channel.GetType())}'"))
                            {
                                await channel.SendNotificationAsync(subject, body);
                            }
                        })
                   .ToList();

                await Task.WhenAll(tasks).NotNull();
            }
        }
    }
}