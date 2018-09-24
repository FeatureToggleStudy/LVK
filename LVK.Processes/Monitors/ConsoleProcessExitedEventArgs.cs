using System;

using JetBrains.Annotations;

namespace LVK.Processes.Monitors
{
    [PublicAPI]
    public class ConsoleProcessExitedEventArgs : ConsoleProcessEventArgs
    {
        [PublicAPI]
        public ConsoleProcessExitedEventArgs(DateTime timestamp, TimeSpan relativeTimestamp, [NotNull] IConsoleProcess process, int exitCode)
            : base(timestamp, relativeTimestamp, process)
        {
            ExitCode = exitCode;
        }

        [PublicAPI]
        public int ExitCode
        {
            get;
        }
    }
}