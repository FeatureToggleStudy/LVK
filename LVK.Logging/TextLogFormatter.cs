using System;
using System.Collections.Generic;
using System.IO;

using JetBrains.Annotations;

namespace LVK.Logging
{
    [UsedImplicitly]
    internal class TextLogFormatter : ITextLogFormatter
    {
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

        public IEnumerable<string> Format(LogLevel level, string message)
        {
            using (var reader =
                new StringReader(message))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    yield return $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {_LogLevelNames[level],-5}: {line}";
            }
        }
    }
}