using System;
using System.Threading;
using System.Threading.Tasks;

using LVK.AppCore;

namespace ConsoleSandbox
{
    internal class ApplicationEntryPoint : IApplicationEntryPoint
    {
        public Task<int> Execute(CancellationToken cancellationToken)
        {
            Console.WriteLine("Test");
            return Task.FromResult(1);
        }
    }
}