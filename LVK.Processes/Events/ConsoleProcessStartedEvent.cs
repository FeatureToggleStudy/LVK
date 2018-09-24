using System;

using JetBrains.Annotations;

namespace LVK.Processes.Events
{
    [PublicAPI]
    public class ConsoleProcessStartedEvent : ConsoleProcessEvent
    {
        [PublicAPI]
        public ConsoleProcessStartedEvent(TimeSpan relativeTimestamp, DateTime timestamp)
            : base(relativeTimestamp, timestamp)
        {
        }

        [PublicAPI]
        public override string ToString()
        {
            return $"{base.ToString()}: <started>";
        }
    }
}