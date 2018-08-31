using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore;

// ReSharper disable PossibleNullReferenceException

namespace ConsoleSandbox
{
    [UsedImplicitly]
    internal class MyApplication : IApplicationEntryPoint
    {
        public async Task<int> Execute(CancellationToken cancellationToken)
        {
            Console.WriteLine("before");
            await Task.Delay(2500, cancellationToken);
            Console.WriteLine("after");
            return 0;
        }
    }
}