using System;
using System.Threading;
using System.Threading.Tasks;

using LVK.AppCore;

namespace PerformanceSandbox
{
    public class ApplicationEntryPoint : IApplicationEntryPoint
    {
        public Task<int> Execute(CancellationToken cancellationToken)
        {
            Console.WriteLine("executed");
            return Task.FromResult(0);
        }
    }
}
