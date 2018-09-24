using System;

using JetBrains.Annotations;

namespace LVK.Processes.Monitors
{
    [PublicAPI]
    public class ConsoleProcessStartedEventArgs : ConsoleProcessEventArgs
    {
        [PublicAPI]
        public ConsoleProcessStartedEventArgs(DateTime timestamp, TimeSpan relativeTimestamp, IConsoleProcess process)
            : base(timestamp, relativeTimestamp, process)
        {
        }
    }
}