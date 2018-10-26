using System;

using JetBrains.Annotations;

namespace LVK.Data.Caching
{
    [PublicAPI]
    public interface IWeakCache<TKey, TValue>
    {
        (bool success, TValue value) TryGetValue([NotNull] TKey key);

        [NotNull]
        TValue GetOrAddValue([NotNull] TKey key, [NotNull] Func<TKey, TValue> factory);
    }
}