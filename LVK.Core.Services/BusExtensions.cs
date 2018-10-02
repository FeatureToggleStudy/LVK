using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Core.Services
{
    [PublicAPI]
    public static class BusExtensions
    {
        [NotNull]
        public static IDisposable Subscribe<T>([NotNull] this IBus bus, [NotNull] Action<T> subscriber)
        {
            if (bus == null)
                throw new ArgumentNullException(nameof(bus));

            if (subscriber == null)
                throw new ArgumentNullException(nameof(subscriber));

            return bus.Subscribe(new ActionSubscriber<T>(subscriber));
        }

        [NotNull]
        public static IDisposable Subscribe<T>([NotNull] this IBus bus, [NotNull] Func<T, Task> subscriber)
        {
            if (bus == null)
                throw new ArgumentNullException(nameof(bus));

            if (subscriber == null)
                throw new ArgumentNullException(nameof(subscriber));

            return bus.Subscribe(new AsyncActionSubscriber<T>(subscriber));
        }
    }
}