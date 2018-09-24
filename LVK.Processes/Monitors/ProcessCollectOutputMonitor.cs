using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using LVK.Processes.Events;

namespace LVK.Processes.Monitors
{
    [PublicAPI]
    public class ProcessCollectOutputMonitor : IConsoleProcessMonitor
    {
        [NotNull]
        private readonly List<ConsoleProcessEvent> _Events = new List<ConsoleProcessEvent>();

        [PublicAPI]
        public ProcessCollectOutputMonitor()
        {
        }

        void IConsoleProcessMonitor.Started(IConsoleProcess process)
        {
            _Events.Add(new ConsoleProcessStartedEvent(process.ExecutionDuration, DateTime.Now));
        }

        void IConsoleProcessMonitor.Exited(IConsoleProcess process, int exitCode)
        {
            _Events.Add(new ConsoleProcessExitedEvent(process.ExecutionDuration, DateTime.Now, exitCode));
        }

        void IConsoleProcessMonitor.Error(IConsoleProcess process, string line)
        {
            if (line == null)
                throw new ArgumentNullException(nameof(line));

            _Events.Add(new ConsoleProcessErrorOutputEvent(process.ExecutionDuration, DateTime.Now, line));
        }

        void IConsoleProcessMonitor.Output(IConsoleProcess process, string line)
        {
            if (line == null)
                throw new ArgumentNullException(nameof(line));

            _Events.Add(new ConsoleProcessStandardOutputEvent(process.ExecutionDuration, DateTime.Now, line));
        }

        [PublicAPI, NotNull, ItemNotNull]
        public List<ConsoleProcessEvent> Events
        {
            get
            {
                return _Events;
            }
        }
    }}