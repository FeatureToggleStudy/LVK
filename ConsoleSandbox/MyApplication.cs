using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.Logging;

namespace ConsoleSandbox
{
    [UsedImplicitly]
    internal class MyApplication : IApplicationEntryPoint
    {
        [NotNull]
        private readonly ILogger _Logger;

        public MyApplication([NotNull] ILogger logger)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> Execute()
        {
            _Logger.Information("before");
            await Task.Delay(1000);
            _Logger.Information("after");

            return 0;
        }
    }
}