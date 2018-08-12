using System;
using System.Collections.Generic;
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

        public async Task<int> Execute()
        {
            _Logger.LogInformation("before");
            await Task.Delay(1000);

            _Logger.LogInformation("after");

            return 0;
        }
    }
}