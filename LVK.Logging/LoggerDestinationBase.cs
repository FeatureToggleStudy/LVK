using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using LVK.Configuration;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Logging
{
    internal abstract class LoggerDestinationBase<TOptions> : ILoggerDestination
        where TOptions: LoggerDestinationOptions, new()
    {
        [NotNull]
        private readonly ITextLogFormatter _TextLogFormatter;

        protected LoggerDestinationBase(
            [NotNull] ITextLogFormatter textLogFormatter, [NotNull] IConfiguration configuration,
            [NotNull] string identifier)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));

            _TextLogFormatter = textLogFormatter ?? throw new ArgumentNullException(nameof(textLogFormatter));
            Options = configuration[$"Logging/Destinations/{identifier}"].Value<TOptions>() ?? new TOptions();
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

            var message = getMessage();
            assume(message != null);
            
            OutputToLog(level, message);
        }

        private void OutputToLog(LogLevel level, [NotNull] string message)
        {
            lock (Lock)
                OutputLinesToLog(level, _TextLogFormatter.Format(level, message));
        }

        protected virtual bool IsEnabled(LogLevel level) => level >= Options.LogLevel && Options.Enabled;

        protected abstract void OutputLinesToLog(LogLevel level, [NotNull] IEnumerable<string> lines);

        public virtual void WriteLine(string line) => Log(LogLevel.Information, line);
    }
}