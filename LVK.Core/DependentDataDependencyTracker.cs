using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Core
{
    internal class DependentDataDependencyTracker
    {
        [NotNull]
        private readonly DependentData _DependentData;

        [NotNull, ItemNotNull]
        private readonly Stack<HashSet<DependentDataKey>> _ReadScopes = new Stack<HashSet<DependentDataKey>>();

        [NotNull]
        private readonly Dictionary<DependentDataKey, HashSet<DependentDataKey>> _Dependencies =
            new Dictionary<DependentDataKey, HashSet<DependentDataKey>>();

        [NotNull]
        private readonly Dictionary<DependentDataKey, HashSet<DependentDataKey>> _Dependents =
            new Dictionary<DependentDataKey, HashSet<DependentDataKey>>();

        public DependentDataDependencyTracker([NotNull] DependentData dependentData)
        {
            _DependentData = dependentData ?? throw new ArgumentNullException(nameof(dependentData));
        }

        internal void BeginReadScope() => _ReadScopes.Push(new HashSet<DependentDataKey>());

        internal void EndReadScopeFor(DependentDataKey key)
        {
            var dependenciesForKey = _ReadScopes.Pop();

            var existingDependenciesForKey = _Dependencies.GetOrAdd(key, () => new HashSet<DependentDataKey>());
            assume(existingDependenciesForKey != null);

            foreach (var newDependency in dependenciesForKey)
                if (!existingDependenciesForKey.Contains(newDependency))
                {
                    existingDependenciesForKey.Add(newDependency);
                    KeyNowDependsOn(key, newDependency);
                }

            foreach (var oldDependency in existingDependenciesForKey.ToList())
                if (!dependenciesForKey.Contains(oldDependency))
                {
                    existingDependenciesForKey.Remove(oldDependency);
                    KeyNoLongerDependsOn(key, oldDependency);
                }
        }

        private void KeyNowDependsOn(DependentDataKey key, DependentDataKey dependency)
        {
            var dependents = _Dependents.GetOrAdd(dependency, () => new HashSet<DependentDataKey>());
            assume(dependents != null);
            dependents.Add(key);
        }

        private void KeyNoLongerDependsOn(DependentDataKey key, DependentDataKey dependency)
        {
            var dependents = _Dependents.GetOrAdd(dependency, () => new HashSet<DependentDataKey>());
            assume(dependents != null);
            dependents.Remove(key);
        }

        public void RegisterRead(DependentDataKey key)
        {
            _ReadScopes.PeekOrDefault()?.Add(key);
        }

        public void RegisterWrite(DependentDataKey key)
        {
            var dependents = _Dependents.GetValueOrDefault(key);
            if (dependents == null)
                return;

            foreach (var dependent in dependents)
                _DependentData.Invalidate(dependent);
        }
    }
}