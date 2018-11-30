using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Core.Services
{
    internal class AsyncActionSubscriber<T> : ISubscriber<T>
    {
        [NotNull]
        private readonly Func<T, Task> _Subscriber;

        public AsyncActionSubscriber([NotNull] Func<T, Task> subscriber) => _Subscriber = subscriber;
        public Task Notify(T message) => _Subscriber(message).NotNull();
    }
}