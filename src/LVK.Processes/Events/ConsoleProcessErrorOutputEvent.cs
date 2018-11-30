using System;

using JetBrains.Annotations;

namespace LVK.Processes.Events
{
    [PublicAPI]
    public class ConsoleProcessErrorOutputEvent : ConsoleProcessOutputEvent
    {
        [PublicAPI]
        public ConsoleProcessErrorOutputEvent(TimeSpan relativeTimestamp, DateTime timestamp, [NotNull] string line)
            : base(relativeTimestamp, timestamp, line)
        {
        }

        // ReSharper disable once AnnotationRedundancyInHierarchy
        [PublicAPI, NotNull]
        public override string ToString()
        {
            return $"{base.ToString()} <error>";
        }
    }
}