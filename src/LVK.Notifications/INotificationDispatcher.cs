using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Notifications
{
    [PublicAPI]
    public interface INotificationDispatcher
    {
        [NotNull]
        Task NotifyAsync([NotNull] string subject, [NotNull] string body);
    }
}