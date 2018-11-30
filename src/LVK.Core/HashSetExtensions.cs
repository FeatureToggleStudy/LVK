using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.Core
{
    [PublicAPI]
    public static class HashSetExtensions
    {
        [NotNull]
        public static HashSet<T> ToHashSet<T>([NotNull] this IEnumerable<T> collection, IEqualityComparer<T> equalityComparer = null)
            => new HashSet<T>(collection, equalityComparer ?? EqualityComparer<T>.Default);

        public static int AddRange<T>([NotNull] this HashSet<T> hashSet, [NotNull, ItemNotNull] IEnumerable<T> values)
        {
            if (hashSet == null)
                throw new ArgumentNullException(nameof(hashSet));

            if (values == null)
                throw new ArgumentNullException(nameof(values));

            int counter = 0;
            foreach (var value in values)
                if (hashSet.Add(value))
                    counter++;

            return counter;
        }
    }
}