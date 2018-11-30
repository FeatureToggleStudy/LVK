using System;
using System.Threading;

using JetBrains.Annotations;

using LVK.AppCore.Console.BusMessages;
using LVK.Core.Services;
using LVK.Logging;

namespace LVK.AppCore.Console
{
    internal abstract class ConsoleApplicationEntryPointBase
    {
        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        [NotNull]
        private readonly CancellationTokenSource _CtrlCCancellationTokenSource;

        protected ConsoleApplicationEntryPointBase([NotNull] ILogger logger, [NotNull] IBus bus, [NotNull] IApplicationLifetimeManager applicationLifetimeManager)
        {
            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _CtrlCCancellationTokenSource = new CancellationTokenSource();
        }

        protected bool WasCancelledByUser { get; private set; }

        [NotNull]
        protected IBus Bus { get; }

        [NotNull]
        protected ILogger Logger { get; }

        protected CancellationToken CtrlCCancellationToken => _CtrlCCancellationTokenSource.Token;

        protected void HookCtrlC()
        {
            System.Console.CancelKeyPress += OnConsoleOnCancelKeyPress;
        }

        private void OnConsoleOnCancelKeyPress(object s, ConsoleCancelEventArgs e)
        {
            Logger.Log(LogLevel.Debug, "User cancelled application with Ctrl+C");
            WasCancelledByUser = true;
            if (e != null)
                e.Cancel = true;

            _CtrlCCancellationTokenSource.Cancel();
            _ApplicationLifetimeManager.SignalGracefulTermination();
            Bus.PublishAsync(new UserCancellationKeypressMessage());
        }
    }
}