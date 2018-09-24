using System;

using JetBrains.Annotations;

namespace LVK.Processes.Monitors
{
    [PublicAPI]
    public class ConsoleProcessOutputEventArgs : ConsoleProcessEventArgs
    {
        [PublicAPI]
        public ConsoleProcessOutputEventArgs(DateTime timestamp, TimeSpan relativeTimestamp, [NotNull] IConsoleProcess process, [NotNull] string line)
            : base(timestamp, relativeTimestamp, process)
        {
            Line = line;
            if (line == null)
                throw new ArgumentNullException(nameof(line));
        }

        [PublicAPI, NotNull]
        public string Line
        {
            get;
        }
    }
}