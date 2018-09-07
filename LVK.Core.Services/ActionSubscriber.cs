using System;

using JetBrains.Annotations;

namespace LVK.Core.Services
{
    internal class ActionSubscriber<T> : ISubscriber<T>
    {
        [NotNull]
        private readonly Action<T> _Subscriber;

        public ActionSubscriber([NotNull] Action<T> subscriber)
        {
            _Subscriber = subscriber;
        }

        public void Notify(T message)
        {
            _Subscriber(message);
        }
    }
}