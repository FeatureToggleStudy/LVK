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

            using (_Logger.BeginScope("application"))
            {
                try
                {
                    var cts = new CancellationTokenSource();

                    var wasCancelledByUser = false;
                    global::System.Console.CancelKeyPress += (s, e) =>
                    {
                        wasCancelledByUser = true;
                        cts.Cancel();
                    };

                    try
                    {
                        foreach (IApplicationInitialization init in _Initializers)
                            await init.Initialize(cts.Token);

                        var result = await _ApplicationEntryPoint.Execute(cts.Token);
                        _Logger.LogDebug($"application exited successfully with exit code {result}");
                        return result;
                    }
                    catch (TaskCanceledException) when (wasCancelledByUser)
                    {
                        _Logger.LogDebug($"application was aborted by user");
                        return 1;
                    }
                    finally
                    {
                        foreach (IApplicationCleanup cleanup in _Cleanups)
                            await cleanup.Cleanup(wasCancelledByUser, cts.Token);
                    }
                }
                catch (Exception ex)
                {
                    _Logger.LogError(ex, $"{ex.GetType().NotNull().Name}");
                    return 1;
                }
            }
        }
    }
}