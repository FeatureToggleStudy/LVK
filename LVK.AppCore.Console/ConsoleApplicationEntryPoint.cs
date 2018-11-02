using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore.Console.BusMessages;
using LVK.Configuration;
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
        private readonly IConfiguration _Configuration;

        [NotNull]
        private readonly IConsoleApplicationHelpTextPresenter _ConsoleApplicationHelpTextPresenter;

        [NotNull]
        private readonly IBus _Bus;

        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly IBackgroundServicesManager _BackgroundServicesManager;

        private bool _WasCancelledByUser;

        public ConsoleApplicationEntryPoint(
            [NotNull] IApplicationEntryPoint applicationEntryPoint, [NotNull] ILogger logger,
            [NotNull] IBackgroundServicesManager backgroundServicesManager,
            [NotNull] IApplicationLifetimeManager applicationLifetimeManager, [NotNull] IConfiguration configuration,
            [NotNull] IConsoleApplicationHelpTextPresenter consoleApplicationHelpTextPresenter, [NotNull] IBus bus)
        {
            _ApplicationEntryPoint =
                applicationEntryPoint ?? throw new ArgumentNullException(nameof(applicationEntryPoint));

            _ApplicationLifetimeManager = applicationLifetimeManager
                                       ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));

            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _ConsoleApplicationHelpTextPresenter = consoleApplicationHelpTextPresenter ?? throw new ArgumentNullException(nameof(consoleApplicationHelpTextPresenter));
            _Bus = bus ?? throw new ArgumentNullException(nameof(bus));

            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _BackgroundServicesManager = backgroundServicesManager ?? throw new ArgumentNullException(nameof(backgroundServicesManager));
        }

        public async Task<int> RunAsync()
        {
            var userCancelKeyPressCancellationTokenSource = new CancellationTokenSource();
            var cts = CancellationTokenSource.CreateLinkedTokenSource(
                userCancelKeyPressCancellationTokenSource.Token,
                _ApplicationLifetimeManager.GracefulTerminationCancellationToken);

            System.Console.CancelKeyPress += (s, e) =>
            {
                _Logger.Log(LogLevel.Debug, "User cancelled application with Ctrl+C");
                _WasCancelledByUser = true;
                if (e != null)
                    e.Cancel = true;

                userCancelKeyPressCancellationTokenSource.Cancel();
                _Bus.PublishAsync(new UserCancellationKeypressMessage());
            };

            using (_Logger.LogScope(LogLevel.Trace, $"{nameof(ConsoleApplicationEntryPoint)}.{nameof(RunAsync)}"))
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
                _BackgroundServicesManager.StartBackgroundServices();

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
                if (!_WasCancelledByUser
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
}