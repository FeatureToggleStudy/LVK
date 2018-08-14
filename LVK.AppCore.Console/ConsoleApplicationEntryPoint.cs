using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Microsoft.Extensions.Logging;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.AppCore.Console
{
    [UsedImplicitly]
    internal class ConsoleApplicationEntryPoint : IConsoleApplicationEntryPoint
    {
        [NotNull]
        private readonly IApplicationEntryPoint _ApplicationEntryPoint;

        [NotNull]
        private readonly ILogger<ConsoleApplicationEntryPoint> _Logger;

        [NotNull, ItemNotNull]
        private readonly List<IApplicationInitialization> _Initializers;

        [NotNull, ItemNotNull]
        private readonly List<IApplicationCleanup> _Cleanups;

        public ConsoleApplicationEntryPoint([NotNull] IApplicationEntryPoint applicationEntryPoint,
                                            [NotNull] ILogger<ConsoleApplicationEntryPoint> logger,
                                            [NotNull, ItemNotNull] IEnumerable<IApplicationInitialization> initializers,
                                            [NotNull, ItemNotNull] IEnumerable<IApplicationCleanup> cleanups)
        {
            _ApplicationEntryPoint =
                applicationEntryPoint ?? throw new ArgumentNullException(nameof(applicationEntryPoint));

            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _Initializers = (initializers ?? throw new ArgumentNullException(nameof(initializers))).ToList();
            _Cleanups = (cleanups ?? throw new ArgumentNullException(nameof(cleanups))).ToList();
        }

        public async Task<int> Execute()
        {
            using (var cts = new CancellationTokenSource())
            {
                try
                {
                    return await HookUserAbortAndRunApplicationEntryPoint(cts);
                }
                catch (Exception ex)
                {
                    _Logger.LogError(ex, $"{ex.GetType().NotNull().Name}");
                    return 1;
                }
            }
        }

        [NotNull]
        private async Task<int> HookUserAbortAndRunApplicationEntryPoint(CancellationTokenSource cts)
        {
            var wasCancelledByUser = false;

            void OnConsoleOnCancelKeyPress(object s, ConsoleCancelEventArgs e)
            {
                wasCancelledByUser = true;
                e.Cancel = true;
                cts.Cancel();
                _Logger.LogError("application aborted by user");
            }

            global::System.Console.CancelKeyPress += OnConsoleOnCancelKeyPress;

            try
            {
                await Initialize(cts.Token);
                return await RunApplicationEntryPoint(cts);
            }
            catch (TaskCanceledException) when (wasCancelledByUser)
            {
                _Logger.LogDebug($"application was aborted by user");
                return 256;
            }
            finally
            {
                global::System.Console.CancelKeyPress -= OnConsoleOnCancelKeyPress;
                await Cleanup(wasCancelledByUser);
            }
        }

        [NotNull]
        private async Task<int> RunApplicationEntryPoint(CancellationTokenSource cts)
        {
            var result = await _ApplicationEntryPoint.Execute(cts.Token);
            _Logger.LogDebug($"application exited successfully with exit code {result}");
            return result;
        }

        [NotNull]
        private async Task Initialize(CancellationToken token)
        {
            foreach (IApplicationInitialization init in _Initializers)
                await init.Initialize(token);
        }

        [NotNull]
        private async Task Cleanup(bool wasCancelledByUser)
        {
            using (var timeout = new CancellationTokenSource(40000))
            {
                try
                {
                    foreach (var cleanup in _Cleanups)
                        await cleanup.Cleanup(wasCancelledByUser, timeout.Token);
                }
                catch (TaskCanceledException)
                {
                    _Logger.LogError("cleanup was aborted due to timeout (40sec)");
                }
            }

        }
    }
}