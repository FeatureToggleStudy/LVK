using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.Core
{
    [PublicAPI]
    public static class DictionaryExtensions
    {
        [CanBeNull]
        public static TValue GetValueOrDefault<TKey, TValue>(
            [NotNull] this IDictionary<TKey, TValue> dictionary, [NotNull] TKey key, TValue defaultValue = default)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (dictionary.TryGetValue(key, out TValue value))
                return value;

            return defaultValue;
        }

        [CanBeNull]
        public static TValue GetOrAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
            => GetOrAdd(dictionary, key, () => defaultValue);

        [CanBeNull]
        public static TValue GetOrAdd<TKey, TValue>(
            [NotNull] this IDictionary<TKey, TValue> dictionary, [NotNull] TKey key, Func<TValue> getDefaultValue)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (dictionary.TryGetValue(key, out var value))
                return value;

            value = getDefaultValue();
            dictionary[key] = value;
            return value;
        }
    }
}