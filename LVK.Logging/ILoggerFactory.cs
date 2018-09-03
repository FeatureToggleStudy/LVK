using JetBrains.Annotations;

namespace LVK.Logging
{
    [PublicAPI]
    public interface ILoggerFactory
    {
        [NotNull]
        ILogger CreateLogger([NotNull] string systemName);

        [NotNull]
        ILogger<T> CreateLogger<T>();
    }
}