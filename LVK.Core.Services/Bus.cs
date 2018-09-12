using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Core.Services
{
    internal class Bus : IBus
    {
        [NotNull]
        private readonly object _Lock = new object();

        [NotNull]
        private readonly Dictionary<Type, HashSet<WeakReference>> _Subscribers = new Dictionary<Type, HashSet<WeakReference>>();

        public IDisposable Subscribe<T>(ISubscriber<T> subscriber)
        {
            lock (_Lock)
            {
                if (!_Subscribers.TryGetValue(typeof(T), out HashSet<WeakReference> subscribers))
                {
                    subscribers = new HashSet<WeakReference>();
                    _Subscribers[typeof(T)] = subscribers;
                }
                subscribers.Add(new WeakReference(subscriber));
            }

            void unsubscribe()
            {
                lock (_Lock)
                {
                    _Subscribers.TryGetValue(typeof(T), out HashSet<WeakReference> subscribers);
                    assume(subscribers != null);

                    Cleanup(subscribers, subscriber);
                    
                    if (subscribers.Count == 0)
                        _Subscribers.Remove(typeof(T));
                }
            }

            return new ActionDisposable(unsubscribe);
        }

        private void Cleanup([NotNull] HashSet<WeakReference> subscribers, [CanBeNull] object subscriber)
        {
            var matches = subscribers.Where(wr => wr?.Target == subscriber || wr?.Target is null).ToList();
            foreach (var match in matches)
                subscribers.Remove(match);
        }

        public void Publish<T>(T message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            lock (_Lock)
            {
                if (_Subscribers.TryGetValue(typeof(T), out HashSet<WeakReference> subscribers))
                {
                    Cleanup(subscribers, null);

                    foreach (var subscriberReference in subscribers)
                        (subscriberReference.Target as ISubscriber<T>)?.Notify(message);
                }
            }
        }
    }
}