using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore;

using Microsoft.Extensions.Logging;

namespace ConsoleSandbox
{
    [UsedImplicitly]
    internal class MyApplication : IApplicationEntryPoint
    {
        [NotNull]
        private readonly ILogger<MyApplication> _Logger;

        public MyApplication([NotNull] ILogger<MyApplication> logger)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> Execute(CancellationToken cancellationToken)
        {
            _Logger.LogInformation("before");

            Console.WriteLine("waiting 5 seconds");
            await Task.Delay(5000, cancellationToken);
            Console.WriteLine("the wait is over");
            
            _Logger.LogInformation("after");

            return 0;
        }
    }
}