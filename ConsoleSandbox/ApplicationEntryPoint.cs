using System;
using System.Threading;
using System.Threading.Tasks;

using LVK.AppCore;

namespace ConsoleSandbox
{
    public class ApplicationEntryPoint : IApplicationEntryPoint
    {
        public Task<int> Execute(CancellationToken cancellationToken)
        {
            Console.WriteLine("Test");
            return Task.FromResult(0);
        }
    }
}