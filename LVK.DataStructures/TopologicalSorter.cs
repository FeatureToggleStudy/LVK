using System;
using System.Collections.Generic;

namespace LVK.DataStructures
{
    internal class TopologicalSorter : ITopologicalSorter
    {
        public IEnumerable<T> Sort<T>(IEnumerable<(T before, T after)> dependencies, IEqualityComparer<T> equalityComparer = null)
        {
            if (dependencies == null)
                throw new ArgumentNullException(nameof(dependencies));

            equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
            var scenario = new TopologicalSorterScenario<T>(dependencies, equalityComparer);
            return scenario.Sort();
        }
    }
}