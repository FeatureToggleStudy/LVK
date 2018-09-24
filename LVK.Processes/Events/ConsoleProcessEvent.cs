using System;

using JetBrains.Annotations;

namespace LVK.Processes.Events
{
    [PublicAPI]
    public abstract class ConsoleProcessEvent
    {
        [PublicAPI]
        protected ConsoleProcessEvent(TimeSpan relativeTimestamp, DateTime timestamp)
        {
            RelativeTimestamp = relativeTimestamp;
            Timestamp = timestamp;
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

        [PublicAPI]
        public override string ToString()
        {
            return $"{Timestamp} (+{RelativeTimestamp})";
        }
    }
}