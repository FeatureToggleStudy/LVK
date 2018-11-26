using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using JetBrains.Annotations;

using LVK.Configuration;

namespace LVK.Logging
{
    internal class FileLoggerDestination : LoggerDestinationBase<FileLoggerDestinationOptions>
    {
        private const int _FileAccessDeniedErrorCode = unchecked((int)0x80070020);
        
        public FileLoggerDestination(
            [NotNull] ITextLogFormatter textLogFormatter, [NotNull] IConfiguration configuration)
            : base(textLogFormatter, configuration, "File")
        {

        }

        protected override void OutputLinesToLog(LogLevel level, IEnumerable<string> lines)
        {
            int attempts = 0;
            while (true)
            {
                try
                {
                    File.AppendAllLines(GetLogFilePath(), lines);
                }
                catch (IOException ex) when (ex.HResult == _FileAccessDeniedErrorCode)
                {
                    attempts++;
                    if (attempts > 50)
                        throw;

                    Thread.Yield();
                }

                return;
            }
        }

        protected override bool IsEnabled(LogLevel level)
            => base.IsEnabled(level) && !string.IsNullOrWhiteSpace(Options.Filename);

        [NotNull]
        private string GetLogFilePath() => Path.Combine(Options.DirectoryPath, string.Format(Options.Filename, DateTime.Now));
    }
}