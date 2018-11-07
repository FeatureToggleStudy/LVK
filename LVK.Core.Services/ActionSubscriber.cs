using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Core.Services
{
    internal class ActionSubscriber<T> : ISubscriber<T>
    {
        [NotNull]
        private readonly Action<T> _Subscriber;

        public ActionSubscriber([NotNull] Action<T> subscriber) => _Subscriber = subscriber;

        public Task Notify(T message)
        {
            _Subscriber(message);
            return Task.CompletedTask;
        }
    }
}