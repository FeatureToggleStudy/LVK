using System;
using System.Threading;

using JetBrains.Annotations;

namespace LVK.Performance.Counters
{
    internal class PerformanceCounter : IPerformanceCounter
    {
        [NotNull]
        public string Key { get; }

        private long _Value;

        public long Reset() => Interlocked.Exchange(ref _Value, 0);

        public long Increment() => Interlocked.Increment(ref _Value);

        public long Value => _Value;

        public PerformanceCounter([NotNull] string key)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }
    }
}