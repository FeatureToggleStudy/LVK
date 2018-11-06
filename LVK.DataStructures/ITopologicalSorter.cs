using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.DataStructures
{
    [PublicAPI]
    public interface ITopologicalSorter
    {
        [NotNull]
        IEnumerable<T> Sort<T>([NotNull] IEnumerable<(T before, T after)> dependencies, [CanBeNull] IEqualityComparer<T> equalityComparer = null);
    }
}