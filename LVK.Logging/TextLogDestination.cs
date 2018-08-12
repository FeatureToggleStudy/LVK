using System;

using JetBrains.Annotations;

namespace LVK.Logging
{
    public abstract class TextLogDestination
    {
        [NotNull]
        private readonly ITextLogFormatter _TextLogFormatter;

        [NotNull]
        private readonly ILoggingOptions _Options;

        protected TextLogDestination([NotNull] ITextLogFormatter textLogFormatter, [NotNull] ILoggingOptions options)
        {
            _TextLogFormatter = textLogFormatter ?? throw new ArgumentNullException(nameof(textLogFormatter));
            _Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void Log(LogLevel level, string message)
        {
            if (_Options.IsEnabledFor(level))
                foreach (var line in _TextLogFormatter.Format(level, message))
                    OutputLineToLog(line);
        }

        protected abstract void OutputLineToLog([NotNull] string line);
    }
}