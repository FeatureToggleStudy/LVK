using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Core.Services
{
    [PublicAPI]
    public interface IBus
    {
        [NotNull]
        IDisposable Subscribe<T>([NotNull] ISubscriber<T> subscriber);

        [NotNull]
        Task PublishAsync<T>([NotNull] T message);

        [NotNull]
        Task PublishAsync<T>([NotNull] Func<T> getMessage);

        void Publish<T>([NotNull] T message);
        void Publish<T>([NotNull] Func<T> getMessage);
    }
}