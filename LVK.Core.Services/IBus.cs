using System;

using JetBrains.Annotations;

namespace LVK.Core.Services
{
    [PublicAPI]
    public interface IBus
    {
        [NotNull]
        IDisposable Subscribe<T>([NotNull] ISubscriber<T> subscriber);

        void Publish<T>([NotNull] T message);
    }
}