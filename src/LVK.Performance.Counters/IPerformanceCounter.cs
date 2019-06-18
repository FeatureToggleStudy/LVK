using JetBrains.Annotations;

namespace LVK.Performance.Counters
{
    [PublicAPI]
    public interface IPerformanceCounter
    {
        [NotNull]
        string Key { get; }

        long Reset();
        long Increment();

        long Value { get; }
    }
}