using JetBrains.Annotations;

namespace LVK.Logging
{
    [PublicAPI]
    public interface ILogger
    {
        void Log(LogLevel level, [NotNull] string message);
        
        void WriteLine([NotNull] string line);
    }

    [PublicAPI]
    public interface ILogger<[UsedImplicitly] T> : ILogger
    {
    }
}