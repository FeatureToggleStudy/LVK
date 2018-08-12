using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore;

namespace ConsoleSandbox
{
    [UsedImplicitly]
    internal class MyInit : IApplicationInitialization
    {
        public Task Initialize(CancellationToken cancellationToken)
        {
            Console.WriteLine("initializing");
            return Task.CompletedTask;
        }
    }
}