using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using LVK.Core;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.DataStructures
{
    internal class TopologicalSorterScenario<T>
    {
        [NotNull]
        private readonly HashSet<T> _NoIncomingConnections;

        [NotNull]
        private readonly Dictionary<T, List<T>> _OutgoingConnections;

        [NotNull]
        private readonly Dictionary<T, int> _IncomingConnectionCount;

        public TopologicalSorterScenario([NotNull] IEnumerable<(T before, T after)> dependencies, [NotNull] IEqualityComparer<T> equalityComparer)
        {
            _NoIncomingConnections = new HashSet<T>(equalityComparer);
            _OutgoingConnections = new Dictionary<T, List<T>>(equalityComparer);
            _IncomingConnectionCount = new Dictionary<T, int>(equalityComparer);
            
            InitializeScenario(dependencies);
        }

        private void InitializeScenario([NotNull] IEnumerable<(T before, T after)> dependencies)
        {
            var dependencyList = dependencies.ToList();
            foreach (var dependency in dependencyList)
                _NoIncomingConnections.Add(dependency.before);

            foreach (var dependency in dependencyList)
                _NoIncomingConnections.Remove(dependency.after);

            foreach (var dependency in dependencyList)
            {
                _IncomingConnectionCount.TryGetValue(dependency.after, out int incomingConnectionCount);
                incomingConnectionCount++;
                _IncomingConnectionCount[dependency.after] = incomingConnectionCount;

                var outgoingConnections = _OutgoingConnections.GetOrAdd(dependency.before, () => new List<T>()).NotNull();
                outgoingConnections.Add(dependency.after);
            }
        }

        [NotNull]
        public IEnumerable<T> Sort()
        {
            while (_NoIncomingConnections.Any())
            {
                var batch = _NoIncomingConnections.ToList();
                _NoIncomingConnections.Clear();
                
                foreach (var element in batch.OrderBy(el => el))
                    yield return element;

                RemoveElementsFromScenario(batch);
            }

            if (_IncomingConnectionCount.Count > 0)
                throw new CyclicDependenciesException("Dependency graph contains a cycle, topological sort cannot continue");
        }

        private void RemoveElementsFromScenario([NotNull] IEnumerable<T> elements)
        {
            foreach (var element in elements)
            {
                assume(element != null);
                
                _OutgoingConnections.TryGetValue(element, out var outgoingConnections);
                if (outgoingConnections == null)
                    continue;
                
                foreach (var nextElement in outgoingConnections)
                {
                    if (--_IncomingConnectionCount[nextElement] > 0)
                        continue;

                    _NoIncomingConnections.Add(nextElement);
                    _IncomingConnectionCount.Remove(nextElement);
                }
            }
        }
    }
}