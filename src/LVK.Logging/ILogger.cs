using System;

using JetBrains.Annotations;

namespace LVK.Logging
{
    [PublicAPI]
    public interface ILogger
    {
        void Log(LogLevel level, [NotNull] string message);
        void Log(LogLevel level, [NotNull] Func<string> getMessage);
        
        void WriteLine([NotNull] string line);
    }
}