using System.Threading;
using System.Threading.Tasks;

using LVK.Core;

namespace LVK.AppCore.Console
{
    internal class DaemonApplicationEntryPoint : IApplicationEntryPoint
    {
        public async Task<int> Execute(CancellationToken cancellationToken)
        {
            System.Console.WriteLine("daemon started, use Ctrl+C to halt");
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s => ((TaskCompletionSource<bool>)s)?.SetResult(true), tcs);
            await tcs.Task.NotNull();
            return 0;
        }
    }
}