using System;
using System.Threading;
using System.Threading.Tasks;

using LVK.AppCore;
using LVK.Core;

namespace ConsoleSandbox
{
    internal class ApplicationEntryPoint : IApplicationEntryPoint
    {
        public Task<int> Execute(CancellationToken cancellationToken)
        {
            Console.WriteLine("Test");
            return Task.FromResult(1).NotNull();
        }
    }
}