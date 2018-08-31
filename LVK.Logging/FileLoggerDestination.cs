using System;
using System.IO;

using JetBrains.Annotations;

namespace LVK.Logging
{
    internal class FileLoggerDestination : ILoggerDestination
    {
        [NotNull]
        private readonly ITextLogFormatter _TextLogFormatter;

        [NotNull]
        private readonly FileLoggerDestinationOptions _Options;

        [NotNull]
        private readonly object _Lock = new object();

        public FileLoggerDestination([NotNull] ITextLogFormatter textLogFormatter,
                                     [NotNull] FileLoggerDestinationOptions options)
        {
            _TextLogFormatter = textLogFormatter ?? throw new ArgumentNullException(nameof(textLogFormatter));
            _Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void Log(LogLevel level, string message)
        {
            lock (_Lock)
                File.AppendAllLines(GetLogFilename(), _TextLogFormatter.Format(level, message));
        }

        private string GetLogFilename() => Path.Combine(_Options.Path, string.Format(_Options.Filename, DateTime.Now));

        public void WriteLine(string line) => Log(LogLevel.Information, line);
    }
}