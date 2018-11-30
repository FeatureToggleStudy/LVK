using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Data.Caching
{
    internal class WeakCache<TKey, TValue> : IWeakCache<TKey, TValue>
    {
        [NotNull]
        private readonly Dictionary<TKey, WeakReference> _Cache = new Dictionary<TKey, WeakReference>();

        [NotNull]
        private readonly object _Lock = new object();
        
        public (bool success, TValue value) TryGetValue(TKey key)
        {
            lock (_Lock)
            {
                if (!_Cache.TryGetValue(key, out var weakReference))
                    return (false, default);

                var value = weakReference.Target;
                if (weakReference.IsAlive)
                    return (true, (TValue)value);

                _Cache.Remove(key);
                return (false, default);
            }
        }

        public TValue GetOrAddValue(TKey key, Func<TKey, TValue> factory)
        {
            lock (_Lock)
            {
                var (success, value) = TryGetValue(key);
                if (success)
                    return value;

                value = factory(key);
                assume(value != null);
                
                _Cache[key] = new WeakReference(value);
                return value;
            }
        }
    }
}