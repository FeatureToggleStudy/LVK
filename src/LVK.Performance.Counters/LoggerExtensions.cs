using JetBrains.Annotations;

using LVK.Logging;

namespace LVK.Performance.Counters
{
    [PublicAPI]
    public static class LoggerExtensions
    {
        public static void Log([NotNull] this ILogger logger, LogLevel level, [NotNull] IPerformanceCounter performanceCounter)
            => logger.Log(level, $"@{performanceCounter.Key} = {performanceCounter.Value}");
    }
}