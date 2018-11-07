using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.Notifications;

namespace ConsoleSandbox
{
    internal class ApplicationEntryPoint : IApplicationEntryPoint
    {
        [NotNull]
        private readonly INotificationDispatcher _NotificationDispatcher;

        public ApplicationEntryPoint([NotNull] INotificationDispatcher notificationDispatcher)
        {
            _NotificationDispatcher = notificationDispatcher ?? throw new ArgumentNullException(nameof(notificationDispatcher));
        }

        public async Task<int> Execute(CancellationToken cancellationToken)
        {
            await _NotificationDispatcher.NotifyAsync("Some subject", "Some body");
            return 0;
        }
    }
}