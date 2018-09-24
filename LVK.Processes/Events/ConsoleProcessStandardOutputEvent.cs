using System;

using JetBrains.Annotations;

namespace LVK.Processes.Events
{
    [PublicAPI]
    public class ConsoleProcessStandardOutputEvent : ConsoleProcessOutputEvent
    {
        [PublicAPI]
        public ConsoleProcessStandardOutputEvent(TimeSpan relativeTimestamp, DateTime timestamp, [NotNull] string line)
            : base(relativeTimestamp, timestamp, line)
        {
        }
    }
}