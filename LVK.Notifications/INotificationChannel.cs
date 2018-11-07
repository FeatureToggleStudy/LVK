using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Notifications
{
    [PublicAPI]
    public interface INotificationChannel
    {
        [NotNull]
        Task SendNotificationAsync([NotNull] string subject, [NotNull] string body);
    }
}