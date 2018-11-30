using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using JetBrains.Annotations;

using NodaTime;

namespace LVK.Logging
{
    [UsedImplicitly]
    internal class TextLogFormatter : ITextLogFormatter
    {
        [NotNull]
        private readonly IClock _Clock;

        [NotNull]
        private readonly DateTimeZone _SystemTimeZone;

        [NotNull]
        private readonly IFormatProvider _InvariantCulture = CultureInfo.InvariantCulture;

        [NotNull]
        private readonly Dictionary<LogLevel, string> _LogLevelNames = new Dictionary<LogLevel, string>
        {
            [LogLevel.Trace] = "TRACE",
            [LogLevel.Debug] = "DEBUG",
            [LogLevel.Verbose] = "INFO",
            [LogLevel.Information] = "INFO",
            [LogLevel.Warning] = "WARN",
            [LogLevel.Error] = "ERROR"
        };

        public TextLogFormatter([NotNull] IClock clock, [NotNull] DateTimeZone systemTimeZone)
        {
            _Clock = clock ?? throw new ArgumentNullException(nameof(clock));
            _SystemTimeZone = systemTimeZone ?? throw new ArgumentNullException(nameof(systemTimeZone));
        }

        public IEnumerable<string> Format(LogLevel level, string message)
        {
            using (var reader =
                new StringReader(message))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    ZonedDateTime now = _Clock.GetCurrentInstant().InZone(_SystemTimeZone);
                    yield return $"{now.ToString("yyyy-MM-dd HH:mm:ss.fff", _InvariantCulture)} {_LogLevelNames[level],-5}: {line}";
                }
            }
        }
    }
}