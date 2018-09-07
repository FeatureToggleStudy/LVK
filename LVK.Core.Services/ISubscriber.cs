using JetBrains.Annotations;

namespace LVK.Core.Services
{
    [PublicAPI]
    public interface ISubscriber<in T>
    {
        void Notify([NotNull] T message);
    }
}