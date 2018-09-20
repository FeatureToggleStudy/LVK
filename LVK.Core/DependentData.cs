using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace LVK.Core
{
    [PublicAPI]
    public class DependentData
    {
        [NotNull]
        private readonly Dictionary<DependentDataKey, DependentDataMember> _MembersByKey =
            new Dictionary<DependentDataKey, DependentDataMember>();

        [NotNull]
        private readonly DependentDataDependencyTracker _DependencyTracker;

        public DependentData() => _DependencyTracker = new DependentDataDependencyTracker(this);

        private DependentDataMember GetMember(DependentDataKey key)
            => _MembersByKey.GetOrAdd(key, () => new DependentDataMember(this, key, _DependencyTracker));

        internal void Invalidate(DependentDataKey key) => GetMember(key).Invalidate();

        [CanBeNull]
        public T Get<T>([NotNull] string name) => GetMember(new DependentDataKey(typeof(T), name)).Get<T>();

        public void Set<T>([NotNull] string name, [CanBeNull] T value)
            => GetMember(new DependentDataKey(typeof(T), name)).Set(value);

        public void Set<T>([NotNull] string name, [NotNull] Func<T> valueGetter)
            => GetMember(new DependentDataKey(typeof(T), name)).Set(valueGetter);
    }
}