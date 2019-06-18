using JetBrains.Annotations;

namespace LVK.Performance.Counters
{
    [PublicAPI]
    public interface IPerformanceCounters
    {
        [NotNull]
        IPerformanceCounter GetByKey([NotNull] string key);
    }
}