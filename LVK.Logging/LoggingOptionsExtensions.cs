using System;

using JetBrains.Annotations;

namespace LVK.Logging
{
    public static class LoggingOptionsExtensions
    {
        public static bool IsEnabledFor([NotNull] this ILoggingOptions options, LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return options.DebugEnabled;

                case LogLevel.Verbose:
                    return options.VerboseEnabled;

                case LogLevel.Information:
                case LogLevel.Warning:
                case LogLevel.Error:
                    return true;

                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }
    }
}