using System;
using System.Diagnostics;

using JetBrains.Annotations;

namespace LVK.Logging
{
    internal class DebugLoggerDestination : ILoggerDestination
    {
        [NotNull]
        private readonly ITextLogFormatter _TextLogFormatter;

        [NotNull]
        private readonly object _Lock = new object();

        public DebugLoggerDestination([NotNull] ITextLogFormatter textLogFormatter)
        {
            _TextLogFormatter = textLogFormatter ?? throw new ArgumentNullException(nameof(textLogFormatter));
        }

        public void Log(LogLevel level, string message)
        {
            foreach (var output in _TextLogFormatter.Format(level, message))
                lock (_Lock)
                    Debug.WriteLine(output);
        }
    }
}