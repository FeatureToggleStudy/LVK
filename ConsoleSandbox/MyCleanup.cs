using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore;

namespace ConsoleSandbox
{
    [UsedImplicitly]
    internal class MyCleanup : IApplicationCleanup
    {
        public Task Cleanup(bool wasCancelledByUser, CancellationToken cancellationToken)
        {
            Console.WriteLine($"cleaning up {(wasCancelledByUser ? "due to user abort" : "after normal execution")}");
            return Task.CompletedTask;
        }
    }
}