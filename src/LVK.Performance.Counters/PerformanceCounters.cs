using System;
using System.Collections.Concurrent;

namespace LVK.Performance.Counters
{
    internal class PerformanceCounters : IPerformanceCounters
    {
        private readonly ConcurrentDictionary<string, IPerformanceCounter> _Counters =
            new ConcurrentDictionary<string, IPerformanceCounter>(StringComparer.InvariantCultureIgnoreCase);

        public IPerformanceCounter GetByKey(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return _Counters.GetOrAdd(key, k => new PerformanceCounter(k));
        }
    }
}