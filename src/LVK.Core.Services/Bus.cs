using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DryIoc;

using JetBrains.Annotations;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Core.Services
{
    internal class Bus : IBus
    {
        [NotNull]
        private readonly IContainer _Container;

        [NotNull]
        private readonly object _Lock = new object();

        [NotNull]
        private readonly Dictionary<Type, HashSet<WeakReference>> _Subscribers = new Dictionary<Type, HashSet<WeakReference>>();

        public Bus([NotNull] IContainer container)
        {
            _Container = container ?? throw new ArgumentNullException(nameof(container));
        }

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

        public Task PublishAsync<T>(T message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            return PublishToSubscribersAsync(message, GetSubscribers<T>());
        }

        public Task PublishAsync<T>(Func<T> getMessage)
        {
            if (getMessage == null)
                throw new ArgumentNullException(nameof(getMessage));

            var subscribers = GetSubscribers<T>();
            if (!subscribers.Any())
                return Task.CompletedTask;

            var message = getMessage();
            if (message == null)
                throw new InvalidOperationException("null message constructed");

            return PublishToSubscribersAsync(message, subscribers);
        }

        [NotNull]
        private Task PublishToSubscribersAsync<T>([NotNull] T message, [NotNull, ItemNotNull] List<ISubscriber<T>> subscribers)
            => Task.WhenAll(subscribers.Select(subscriber => subscriber.Notify(message))).NotNull();

        private void Cleanup([NotNull] HashSet<WeakReference> subscribers, [CanBeNull] object subscriber)
        {
            var matches = subscribers.Where(wr => wr?.Target == subscriber || wr?.Target is null).ToList();
            foreach (var match in matches)
                subscribers.Remove(match);
        }

        [NotNull, ItemNotNull]
        private List<ISubscriber<T>> GetSubscribers<T>()
        {
            var result = new List<ISubscriber<T>>();

            lock (_Lock)
            {
                if (_Subscribers.TryGetValue(typeof(T), out HashSet<WeakReference> subscribers))
                {
                    Cleanup(subscribers, null);

                    foreach (var subscriberReference in subscribers)
                        if (subscriberReference.Target is ISubscriber<T> subscriber)
                            result.Add(subscriber);
                }
            }

            result.AddRange(_Container.Resolve<IEnumerable<ISubscriber<T>>>().NotNull());

            return result;
        }
    }
}