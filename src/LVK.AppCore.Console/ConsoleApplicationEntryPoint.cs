using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.Core.Services;
using LVK.Logging;

namespace LVK.AppCore.Console
{
    internal class ConsoleApplicationEntryPoint : ConsoleApplicationEntryPointBase, IConsoleApplicationEntryPoint
    {
        [NotNull]
        private readonly IApplicationEntryPoint _ApplicationEntryPoint;

        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        [NotNull]
        private readonly IConfiguration _Configuration;

        [NotNull]
        private readonly IConsoleApplicationHelpTextPresenter _ConsoleApplicationHelpTextPresenter;

        [NotNull]
        private readonly IBackgroundServicesManager _BackgroundServicesManager;

        public ConsoleApplicationEntryPoint(
            [NotNull] IApplicationEntryPoint applicationEntryPoint, [NotNull] ILogger logger,
            [NotNull] IBackgroundServicesManager backgroundServicesManager,
            [NotNull] IApplicationLifetimeManager applicationLifetimeManager, [NotNull] IConfiguration configuration,
            [NotNull] IConsoleApplicationHelpTextPresenter consoleApplicationHelpTextPresenter, [NotNull] IBus bus)
            : base(logger, bus, applicationLifetimeManager)
        {
            _ApplicationEntryPoint = applicationEntryPoint ?? throw new ArgumentNullException(nameof(applicationEntryPoint));

            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));

            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _ConsoleApplicationHelpTextPresenter = consoleApplicationHelpTextPresenter
                                                ?? throw new ArgumentNullException(nameof(consoleApplicationHelpTextPresenter));

            _BackgroundServicesManager = backgroundServicesManager ?? throw new ArgumentNullException(nameof(backgroundServicesManager));
        }

        public async Task<int> RunAsync()
        {
            CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(
                CtrlCCancellationToken, _ApplicationLifetimeManager.GracefulTerminationCancellationToken);

            HookCtrlC();

            using (Logger.LogScope(LogLevel.Trace, $"{nameof(ConsoleApplicationEntryPoint)}.{nameof(RunAsync)}"))
            {
                if (ShowHelp())
                    return 0;

                return await RunApplicationEntryPoint(cts);
            }
        }

        private bool ShowHelp()
        {
            if (!_Configuration["help"].Element<bool>().ValueOrDefault())
                return false;

            _ConsoleApplicationHelpTextPresenter.Present();
            return true;
        }

        [NotNull]
        private async Task<int> RunApplicationEntryPoint([NotNull] CancellationTokenSource cts)
        {
            try
            {
                int exitcode;
                try
                {
                    exitcode = await _ApplicationEntryPoint.Execute(cts.Token);
                }
                finally
                {
                    _ApplicationLifetimeManager.SignalGracefulTermination();
                    await _BackgroundServicesManager.WaitForBackgroundServicesToStop();
                }

                return exitcode;
            }
            catch (TaskCanceledException)
            {
                if (!WasCancelledByUser && !_ApplicationLifetimeManager.GracefulTerminationCancellationToken.IsCancellationRequested)
                    System.Console.Error.WriteLine("program terminated early, unknown reason");

                return 1;
            }
            catch (Exception ex) when (!Debugger.IsAttached)
            {
                Logger.LogException(ex);
                throw;
            }
        }
    }
}