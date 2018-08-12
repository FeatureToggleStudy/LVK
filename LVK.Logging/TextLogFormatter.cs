using System;
using System.Collections.Generic;
using System.IO;

using JetBrains.Annotations;

using NodaTime;

namespace LVK.Logging
{
    internal class TextLogFormatter : ITextLogFormatter
    {
        [NotNull]
        private readonly IClock _Clock;

        [NotNull]
        private readonly DateTimeZone _DateTimeZone;

        public TextLogFormatter([NotNull] IClock clock)
        {
            _Clock = clock ?? throw new ArgumentNullException(nameof(clock));
            _DateTimeZone = DateTimeZoneProviders.Tzdb.GetSystemDefault(); // TODO: Inject this? Or change services
        }

        public IEnumerable<string> Format(LogLevel level, [NotNull] string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            ZonedDateTime time = _Clock.GetCurrentInstant().InZone(_DateTimeZone);

            using (var reader = new StringReader(message))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    yield return $"{time:yyyy-MM-dd HH:mm:ss.fff} {LogLevelToString(level),-5}: {line}";
            }
        }

        private string LogLevelToString(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return "DEBUG";

                case LogLevel.Verbose:
                case LogLevel.Information:
                    return "INFO";

                case LogLevel.Warning:
                    return "WARN";

                case LogLevel.Error:
                    return "ERROR";

                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }
    }
}