using System;

using JetBrains.Annotations;

namespace LVK.Processes.Monitors
{
    [PublicAPI]
    public class ConsoleProcessEventMonitor : IConsoleProcessMonitor
    {
        [PublicAPI]
        public ConsoleProcessEventMonitor()
        {
        }

        [PublicAPI]
        public EventHandler<ConsoleProcessStartedEventArgs> Started;

        [PublicAPI]
        public EventHandler<ConsoleProcessExitedEventArgs> Exited;

        [PublicAPI]
        public EventHandler<ConsoleProcessOutputEventArgs> StandardOutput;

        [PublicAPI]
        public EventHandler<ConsoleProcessOutputEventArgs> ErrorOutput;

        void IConsoleProcessMonitor.Started(IConsoleProcess process)
        {
            Started?.Invoke(this, new ConsoleProcessStartedEventArgs(DateTime.Now, process.ExecutionDuration, process));
        }

        void IConsoleProcessMonitor.Exited(IConsoleProcess process, int exitCode)
        {
            Exited?.Invoke(this, new ConsoleProcessExitedEventArgs(DateTime.Now, process.ExecutionDuration, process, exitCode));
        }

        void IConsoleProcessMonitor.Error(IConsoleProcess process, string line)
        {
            ErrorOutput?.Invoke(this, new ConsoleProcessOutputEventArgs(DateTime.Now, process.ExecutionDuration, process, line));
        }

        void IConsoleProcessMonitor.Output(IConsoleProcess process, string line)
        {
            StandardOutput?.Invoke(this, new ConsoleProcessOutputEventArgs(DateTime.Now, process.ExecutionDuration, process, line));
        }
    }
}