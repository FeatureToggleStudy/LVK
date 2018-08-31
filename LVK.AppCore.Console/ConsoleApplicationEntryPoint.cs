using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Logging;

namespace LVK.AppCore.Console
{
    internal class ConsoleApplicationEntryPoint : IConsoleApplicationEntryPoint
    {
        [NotNull]
        private readonly IApplicationEntryPoint _ApplicationEntryPoint;

        [NotNull]
        private readonly ILogger _Logger;

        public ConsoleApplicationEntryPoint([NotNull] IApplicationEntryPoint applicationEntryPoint,
                                            [NotNull] ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
                throw new ArgumentNullException(nameof(loggerFactory));

            _ApplicationEntryPoint =
                applicationEntryPoint ?? throw new ArgumentNullException(nameof(applicationEntryPoint));

            _Logger = loggerFactory.CreateLogger("Application");
        }

        public async Task<int> RunAsync()
        {
            var cts = new CancellationTokenSource();

            bool wasCancelledByUser = false;
            System.Console.CancelKeyPress += (s, e) =>
            {
                _Logger.Log(LogLevel.Debug, "User cancelled application with Ctrl+C");
                wasCancelledByUser = true;
                if (e != null)
                    e.Cancel = true;
                cts.Cancel();
            };

            using (_Logger.LogScope(LogLevel.Trace, $"{nameof(ConsoleApplicationEntryPoint)}.{nameof(RunAsync)}"))
            {
                try
                {
                    var exitcode = await _ApplicationEntryPoint.Execute(cts.Token);
                    return exitcode;
                }
                catch (TaskCanceledException)
                {
                    if (!wasCancelledByUser)
                        System.Console.Error.WriteLine("program terminated early, unknown reason");
                    return 1;
                }
                catch (Exception ex) when (!Debugger.IsAttached)
                {
                    _Logger.LogException(ex);
                    throw;
                }
            }
        }
    }
}