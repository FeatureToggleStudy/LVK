using System;

using JetBrains.Annotations;

namespace LVK.Processes.Monitors
{
    [PublicAPI]
    public abstract class ConsoleProcessEventArgs : EventArgs
    {
        [PublicAPI]
        protected ConsoleProcessEventArgs(DateTime timestamp, TimeSpan relativeTimestamp, [NotNull] IConsoleProcess process)
        {
            Timestamp = timestamp;
            RelativeTimestamp = relativeTimestamp;
            Process = process ?? throw new ArgumentNullException(nameof(process));
        }

        [PublicAPI]
        public DateTime Timestamp
        {
            get;
        }

        [PublicAPI]
        public TimeSpan RelativeTimestamp
        {
            get;
        }

        [PublicAPI, NotNull]
        public IConsoleProcess Process
        {
            get;
        }
    }
}