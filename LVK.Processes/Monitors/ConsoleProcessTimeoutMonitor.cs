using System;
using System.Threading;

using JetBrains.Annotations;

namespace LVK.Processes.Monitors
{
    [PublicAPI]
    public class ConsoleProcessTimeoutMonitor : IConsoleProcessMonitor, IDisposable
    {
        private static readonly TimeSpan _InfiniteTimeSpan = new TimeSpan(0, 0, 0, 0, -1);

        private readonly TimeSpan _Timeout;
        private readonly bool _ResetOnOutput;
        private IConsoleProcess _Process;
        private readonly object _Lock = new object();
        private Timer _Timer;

        [PublicAPI]
        public ConsoleProcessTimeoutMonitor(TimeSpan timeout, bool resetOnOutput = false)
        {
            _Timeout = timeout;
            _ResetOnOutput = resetOnOutput;
        }

        [PublicAPI]
        public ConsoleProcessTimeoutMonitor(bool resetOnOutput = false)
            : this(TimeSpan.FromSeconds(90), resetOnOutput)
        {
        }

        void IConsoleProcessMonitor.Started(IConsoleProcess process)
        {
            lock (_Lock)
            {
                _Timer = new Timer(TimerCallback, null, _Timeout, _InfiniteTimeSpan);
                _Process = process;
            }
        }

        private void TimerCallback(object state)
        {
            bool shouldTerminate;
            lock (_Lock)
            {
                shouldTerminate = _Timer != null;
                _Timer?.Dispose();
                _Timer = null;
            }

            if (shouldTerminate)
                _Process?.Terminate();

            _Process = null;
        }

        void IConsoleProcessMonitor.Exited(IConsoleProcess process, int exitCode)
        {
            lock (_Lock)
            {
                _Timer?.Dispose();
                _Timer = null;
                _Process = null;
            }
        }

        void IConsoleProcessMonitor.Error(IConsoleProcess process, string line)
        {
            if (_ResetOnOutput)
                lock (_Lock)
                    _Timer?.Change(_Timeout, _InfiniteTimeSpan);
        }

        void IConsoleProcessMonitor.Output(IConsoleProcess process, string line)
        {
            if (_ResetOnOutput)
                lock (_Lock)
                    _Timer?.Change(_Timeout, _InfiniteTimeSpan);
        }

        void IDisposable.Dispose()
        {
            lock (_Lock)
            {
                _Timer?.Dispose();
                _Timer = null;
                _Process = null;
            }
        }
    }
}