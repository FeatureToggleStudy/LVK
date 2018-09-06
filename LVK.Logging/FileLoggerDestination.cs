using System;
using System.Collections.Generic;
using System.IO;

using JetBrains.Annotations;

using LVK.Configuration;

namespace LVK.Logging
{
    internal class FileLoggerDestination : LoggerDestinationBase<FileLoggerDestinationOptions>
    {
        public FileLoggerDestination(
            [NotNull] ITextLogFormatter textLogFormatter, [NotNull] IConfiguration configuration)
            : base(textLogFormatter, configuration)
        {

        }

        protected override void OutputLinesToLog(LogLevel level, IEnumerable<string> lines)
        {
            File.AppendAllLines(GetLogFilename(), lines);
        }

        protected override bool IsEnabled(LogLevel level)
            => base.IsEnabled(level) && !string.IsNullOrWhiteSpace(Options.Filename);

        [NotNull]
        private string GetLogFilename() => Path.Combine(Options.Path, string.Format(Options.Filename, DateTime.Now));
    }
}