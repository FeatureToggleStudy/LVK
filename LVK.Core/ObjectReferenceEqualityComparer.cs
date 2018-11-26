using System.Collections.Generic;
using System.Runtime.CompilerServices;

using JetBrains.Annotations;

namespace LVK.Core
{
    [PublicAPI]
    public class ObjectReferenceEqualityComparer : ObjectReferenceEqualityComparer<object>
    {
    }

    [PublicAPI]
    public class ObjectReferenceEqualityComparer<T> : IEqualityComparer<T>
        where T: class
    {
        public bool Equals(T x, T y) => ReferenceEquals(x, y);

        public int GetHashCode([CanBeNull] T obj) => obj is null ? 0 : RuntimeHelpers.GetHashCode(obj);
    }
}