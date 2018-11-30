using System;

using JetBrains.Annotations;

namespace LVK.Processes.Events
{
    [PublicAPI]
    public class ConsoleProcessExitedEvent : ConsoleProcessEvent
    {
        [PublicAPI]
        public ConsoleProcessExitedEvent(TimeSpan executionDuration, DateTime timestamp, int exitCode)
            : base(executionDuration, timestamp)
        {
            ExitCode = exitCode;
        }

        [PublicAPI]
        public int ExitCode
        {
            get;
        }

        [PublicAPI]
        public override string ToString()
        {
            return $"{base.ToString()}: <exited: {ExitCode}>";
        }
    }
}