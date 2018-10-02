using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Core.Services
{
    [PublicAPI]
    public interface ISubscriber<in T>
    {
        [NotNull]
        Task Notify([NotNull] T message);
    }
}