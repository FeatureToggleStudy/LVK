using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using LVK.Configuration;

namespace LVK.Logging
{
    internal abstract class LoggerDestinationBase<TOptions> : ILoggerDestination
        where TOptions: LoggerDestinationOptions, new()
    {
        [NotNull]
        private readonly ITextLogFormatter _TextLogFormatter;

        protected LoggerDestinationBase(
            [NotNull] ITextLogFormatter textLogFormatter, [NotNull] IConfiguration configuration)
        {
            _TextLogFormatter = textLogFormatter ?? throw new ArgumentNullException(nameof(textLogFormatter));
            Options = configuration["Logging/Destinations/File"].Value<TOptions>() ?? new TOptions();
        }

        [NotNull]
        protected TOptions Options { get; }

        [NotNull]
        protected object Lock { get; } = new object();

        public void Log(LogLevel level, string message)
        {
            if (!IsEnabled(level))
                return;

            OutputToLog(level, message);
        }

        public void Log(LogLevel level, Func<string> getMessage)
        {
            if (!IsEnabled(level))
                return;

            OutputToLog(level, getMessage());
        }

        private void OutputToLog(LogLevel level, string message)
        {
            lock (Lock)
                OutputLinesToLog(level, _TextLogFormatter.Format(level, message));
        }

        protected virtual bool IsEnabled(LogLevel level) => level >= Options.LogLevel && Options.Enabled;

        protected abstract void OutputLinesToLog(LogLevel level, [NotNull] IEnumerable<string> lines);

        public virtual void WriteLine(string line) => Log(LogLevel.Information, line);
    }
}