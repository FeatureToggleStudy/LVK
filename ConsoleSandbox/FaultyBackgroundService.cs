using System;
using System.Threading;
using System.Threading.Tasks;

using LVK.Core.Services;

namespace ConsoleSandbox
{
    public class FaultyBackgroundService : IBackgroundService
    {
        public async Task Execute(CancellationToken cancellationToken)
        {
            await Task.Delay(1000, cancellationToken);
            throw new InvalidOperationException("test of retry");
        }
    }
}