using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Core.Services
{
    internal class Bus : IBus
    {
        [NotNull]
        private readonly object _Lock = new object();

        [NotNull]
        private readonly Dictionary<Type, HashSet<object>> _Subscribers = new Dictionary<Type, HashSet<object>>();

        public IDisposable Subscribe<T>(ISubscriber<T> subscriber)
        {
            lock (_Lock)
            {
                HashSet<object> subscribers;
                if (!_Subscribers.TryGetValue(typeof(T), out subscribers))
                {
                    subscribers = new HashSet<object>();
                    _Subscribers[typeof(T)] = subscribers;

                    subscribers.Add(subscriber);
                }
            }

            void unsubscribe()
            {
                lock (_Lock)
                {
                    _Subscribers.TryGetValue(typeof(T), out HashSet<object> subscribers);
                    assume(subscribers != null);
                    
                    subscribers.Remove(subscriber);
                    if (subscribers.Count == 0)
                        _Subscribers.Remove(typeof(T));
                }
            }

            return new ActionDisposable(unsubscribe);
        }

        public void Publish<T>(T message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            lock (_Lock)
            {
                if (_Subscribers.TryGetValue(typeof(T), out HashSet<object> subscribers))
                {
                    foreach (var subscriber in subscribers)
                        ((ISubscriber<T>)subscriber).Notify(message);
                }
            }
        }
    }
}