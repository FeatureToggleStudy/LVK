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
            if (line == null)
                throw new ArgumentNullException(nameof(line));

            Line = line;
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