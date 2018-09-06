using System;
using System.IO;

using JetBrains.Annotations;

using LVK.Configuration;

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
                                     [NotNull] IConfiguration configuration)
        {
            _TextLogFormatter = textLogFormatter ?? throw new ArgumentNullException(nameof(textLogFormatter));
            _Options = configuration["Logging/Destinations/File"].Value<FileLoggerDestinationOptions>()
                    ?? new FileLoggerDestinationOptions();

        }

        public void Log(LogLevel level, string message)
        {
            if (level < _Options.LogLevel || !_Options.Enabled || string.IsNullOrWhiteSpace(_Options.Filename))
                return;
            
            lock (_Lock)
                File.AppendAllLines(GetLogFilename(), _TextLogFormatter.Format(level, message));
        }

        [NotNull] private string GetLogFilename() => Path.Combine(_Options.Path, string.Format(_Options.Filename, DateTime.Now));

        public void WriteLine(string line) => Log(LogLevel.Information, line);
    }
}