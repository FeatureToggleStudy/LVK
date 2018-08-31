using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core.Services;
using LVK.Logging;

namespace LVK.AppCore.Console
{
    internal class ConsoleApplicationEntryPoint : IConsoleApplicationEntryPoint
    {
        [NotNull]
        private readonly IApplicationEntryPoint _ApplicationEntryPoint;

        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        [NotNull]
        private readonly ILogger _Logger;

        [NotNull, ItemNotNull]
        private readonly List<IApplicationRuntimeContext> _ApplicationRuntimeContexts;

        public ConsoleApplicationEntryPoint([NotNull] IApplicationEntryPoint applicationEntryPoint,
                                            [NotNull] ILoggerFactory loggerFactory,
                                            [NotNull]
                                            IEnumerable<IApplicationRuntimeContext> applicationRuntimeContexts,
                                            [NotNull] IApplicationLifetimeManager applicationLifetimeManager)
        {
            if (loggerFactory == null)
                throw new ArgumentNullException(nameof(loggerFactory));

            _ApplicationEntryPoint =
                applicationEntryPoint ?? throw new ArgumentNullException(nameof(applicationEntryPoint));

            _ApplicationLifetimeManager = applicationLifetimeManager
                                       ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));

            _Logger = loggerFactory.CreateLogger("Application");
            _ApplicationRuntimeContexts = (applicationRuntimeContexts
                                        ?? throw new ArgumentNullException(nameof(applicationRuntimeContexts)))
               .ToList();
        }

        public async Task<int> RunAsync()
        {
            var userCancelKeyPressCTS = new CancellationTokenSource();
            var cts = CancellationTokenSource.CreateLinkedTokenSource(userCancelKeyPressCTS.Token,
                _ApplicationLifetimeManager.GracefulTerminationCancellationToken);

            var wasCancelledByUser = false;
            System.Console.CancelKeyPress += (s, e) =>
            {
                _Logger.Log(LogLevel.Debug, "User cancelled application with Ctrl+C");
                wasCancelledByUser = true;
                if (e != null)
                    e.Cancel = true;

                userCancelKeyPressCTS.Cancel();
            };

            using (_Logger.LogScope(LogLevel.Trace, $"{nameof(ConsoleApplicationEntryPoint)}.{nameof(RunAsync)}"))
            {
                try
                {
                    if (!await StartApplicationRuntimeContexts())
                        return 1;

                    int exitcode = 0;
                    try
                    {
                        exitcode = await _ApplicationEntryPoint.Execute(cts.Token);
                        _ApplicationLifetimeManager.SignalGracefulTermination();
                    }
                    finally
                    {
                        if (!await StopApplicationRuntimeContexts())
                            exitcode = 1;
                    }
                    return exitcode;
                }
                catch (TaskCanceledException)
                {
                    if (!wasCancelledByUser
                     && !_ApplicationLifetimeManager.GracefulTerminationCancellationToken.IsCancellationRequested)
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

        private async Task<bool> StartApplicationRuntimeContexts()
        {
            using (var startupCts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            {
                try
                {
                    foreach (IApplicationRuntimeContext arc in _ApplicationRuntimeContexts)
                        using (_Logger.LogScope(LogLevel.Debug, $"starting runtime context {arc.GetType().Name}"))
                            await arc.Start(startupCts.Token);

                    return true;
                }
                catch (TaskCanceledException)
                {
                    System.Console.Error.WriteLine("application took to long to start, terminating");
                    return false;
                }
            }
        }

        private async Task<bool> StopApplicationRuntimeContexts()
        {
            using (var stopCts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            {
                try
                {
                    List<IApplicationRuntimeContext> arcs = _ApplicationRuntimeContexts.ToList();
                    arcs.Reverse();

                    foreach (IApplicationRuntimeContext arc in arcs)
                        using (_Logger.LogScope(LogLevel.Debug, $"stopping runtime context {arc.GetType().Name}"))
                            try
                            {
                                await arc.Stop(stopCts.Token);
                            }
                            catch (TaskCanceledException)
                            {
                            }

                    return true;
                }
                catch (TaskCanceledException)
                {
                    System.Console.Error.WriteLine("application took to long to terminate gracefully, terminating");
                    return false;
                }
            }
        }
    }
}