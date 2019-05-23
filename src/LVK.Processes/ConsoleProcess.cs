using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Processes.Monitors;

namespace LVK.Processes
{
    internal class ConsoleProcess : IDisposable, IConsoleProcess
    {
        [NotNull]
        private readonly Process _Process;

        [NotNull]
        private readonly IConsoleProcessMonitor[] _Monitors;

        private Stopwatch _Stopwatch;
        
        [NotNull]
        private readonly TaskCompletionSource<bool> _ProcessCompletedTaskCompletionSource;

        public ConsoleProcess([NotNull] ProcessStartInfo processStartInfo, [NotNull] IConsoleProcessMonitor[] monitors)
        {
            _Monitors = monitors;

            Encoding standardOutputEncoding = Encoding.GetEncoding(850);

            _Process = new Process { StartInfo = processStartInfo };
            _Process.StartInfo.UseShellExecute = false;
            _Process.StartInfo.RedirectStandardError = true;
            _Process.StartInfo.StandardErrorEncoding = standardOutputEncoding;
            _Process.StartInfo.RedirectStandardInput = true;
            _Process.StartInfo.RedirectStandardOutput = true;
            _Process.StartInfo.StandardOutputEncoding = standardOutputEncoding;
            _Process.StartInfo.CreateNoWindow = true;
            _Process.EnableRaisingEvents = true;
            _Process.OutputDataReceived += ProcessOnOutputDataReceived;
            _Process.ErrorDataReceived += ProcessOnErrorDataReceived;

            _ProcessCompletedTaskCompletionSource = new TaskCompletionSource<bool>();
            _Process.Exited += (s, e) => _ProcessCompletedTaskCompletionSource.SetResult(true);
        }

        private void ProcessOnErrorDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            if (dataReceivedEventArgs?.Data != null)
            {
                foreach (var monitor in _Monitors)
                    monitor?.Error(this, dataReceivedEventArgs.Data);
            }
        }

        private void ProcessOnOutputDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            if (dataReceivedEventArgs?.Data != null)
            {
                foreach (var monitor in _Monitors)
                    monitor?.Output(this, dataReceivedEventArgs.Data);
            }
        }

        void IDisposable.Dispose()
        {
            _Process.Dispose();
        }

        void IConsoleProcess.Write(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
        
            _Process.StandardInput.Write(text);
        }
        
        void IConsoleProcess.WriteLine(string line)
        {
            if (line == null)
                throw new ArgumentNullException(nameof(line));
        
            _Process.StandardInput.WriteLine(line);
        }

        public void Terminate()
        {
            _Process.Kill();
        }

        public TimeSpan ExecutionDuration => _Stopwatch?.Elapsed ?? TimeSpan.Zero;

        public int Id { get; private set; }

        public async Task<int> StartAsync()
        {
            _Stopwatch = Stopwatch.StartNew();

            _Process.Start();
            Id = _Process.Id;
            foreach (var monitor in _Monitors)
                monitor?.Started(this);

            _Process.BeginErrorReadLine();
            _Process.BeginOutputReadLine();
            
            await _ProcessCompletedTaskCompletionSource.Task.NotNull();

            _Process.WaitForExit();
            foreach (var monitor in _Monitors)
                monitor?.Exited(this, _Process.ExitCode);

            return _Process.ExitCode;
        }
    }
}