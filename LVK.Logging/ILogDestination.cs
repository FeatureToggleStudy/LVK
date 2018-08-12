using System;

using JetBrains.Annotations;

namespace LVK.Logging
{
    public interface ILogDestination
    {
        void Log(LogLevel level, [NotNull] string message);
    }
}