using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.Core
{
    [PublicAPI]
    public static class QueueExtensions
    {
        [NotNull]
        public static Queue<T> ToQueue<T>([NotNull] this IEnumerable<T> collection) => new Queue<T>(collection);
    }
}