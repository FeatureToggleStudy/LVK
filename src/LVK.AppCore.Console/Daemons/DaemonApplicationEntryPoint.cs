using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Core.Services;
using LVK.Logging;

namespace LVK.AppCore.Console.Daemons
{
    internal class DaemonApplicationEntryPoint : ConsoleApplicationEntryPointBase, IApplicationEntryPoint
    {
        public DaemonApplicationEntryPoint([NotNull] ILogger logger, [NotNull] IBus bus, [NotNull] IApplicationLifetimeManager applicationLifetimeManager)
            : base(logger, bus, applicationLifetimeManager)
        {
        }

        public async Task<int> Execute(CancellationToken cancellationToken)
        {
            Logger.WriteLine("daemon started, use Ctrl+C to halt");
            HookCtrlC();
            
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s => ((TaskCompletionSource<bool>)s)?.SetResult(true), tcs);
            await tcs.Task.NotNull();
            return 0;
        }
    }
}