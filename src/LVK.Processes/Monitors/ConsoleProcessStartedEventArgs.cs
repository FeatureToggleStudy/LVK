using System;

using JetBrains.Annotations;

namespace LVK.Processes.Monitors
{
    [PublicAPI]
    public class ConsoleProcessStartedEventArgs : ConsoleProcessEventArgs
    {
        [PublicAPI]
        public ConsoleProcessStartedEventArgs(DateTime timestamp, TimeSpan relativeTimestamp, [NotNull] IConsoleProcess process)
            : base(timestamp, relativeTimestamp, process)
        {
        }
    }
}