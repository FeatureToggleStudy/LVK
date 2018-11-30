using System;

using JetBrains.Annotations;

using LVK.Logging;

namespace LVK.Processes.Monitors
{
    internal class LoggingConsoleProcessMonitor : IConsoleProcessMonitor
    {
        [NotNull]
        private readonly ILogger _Logger;

        public LoggingConsoleProcessMonitor([NotNull] ILogger logger)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Started(IConsoleProcess process)
        {
            _Logger.LogDebug($"process #{process.Id} started");
        }

        public void Exited(IConsoleProcess process, int exitCode)
        {
            _Logger.LogDebug($"process #{process.Id} terminated with exit code {exitCode}");
        }

        public void Error(IConsoleProcess process, string line)
        {
            _Logger.LogDebug($"process #{process.Id} error: {line}");
        }

        public void Output(IConsoleProcess process, string line)
        {
            _Logger.LogDebug($"process #{process.Id}: {line}");
        }
    }
}