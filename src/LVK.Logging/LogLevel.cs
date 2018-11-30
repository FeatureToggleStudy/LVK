using JetBrains.Annotations;

namespace LVK.Logging
{
    [PublicAPI]
    public enum LogLevel
    {
        Trace,
        Debug,
        Verbose,
        Information,
        Warning,
        Error
    }
}