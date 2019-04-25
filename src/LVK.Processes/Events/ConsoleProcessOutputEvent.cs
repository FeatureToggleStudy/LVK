using System;

using JetBrains.Annotations;

namespace LVK.Processes.Events
{
    [PublicAPI]
    public abstract class ConsoleProcessOutputEvent : ConsoleProcessEvent
    {
        [PublicAPI]
        protected ConsoleProcessOutputEvent(TimeSpan relativeTimestamp, DateTime timestamp, [NotNull] string line)
            : base(relativeTimestamp, timestamp)
        {
            Line = line ?? throw new ArgumentNullException(nameof(line));
        }

        [PublicAPI, NotNull]
        public string Line
        {
            get;
        }


        [PublicAPI]
        public override string ToString()
        {
            return $"{base.ToString()}: {Line}";
        }
    }
}