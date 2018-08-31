using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Logging;

namespace LVK.AppCore.Console
{
    internal class DaemonApplicationEntryPoint : IApplicationEntryPoint
    {
        [NotNull]
        private readonly ILogger _Logger;

        public DaemonApplicationEntryPoint([NotNull] ILogger<DaemonApplicationEntryPoint> logger)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> Execute(CancellationToken cancellationToken)
        {
            _Logger.WriteLine("daemon started, use Ctrl+C to halt");
            
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s => ((TaskCompletionSource<bool>)s)?.SetResult(true), tcs);
            await tcs.Task.NotNull();
            return 0;
        }
    }
}