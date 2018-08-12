using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Microsoft.Extensions.Logging;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.AppCore.Console
{
    [UsedImplicitly]
    internal class ConsoleApplicationEntryPoint : IConsoleApplicationEntryPoint
    {
        [NotNull]
        private readonly IApplicationEntryPoint _ApplicationEntryPoint;

        [NotNull]
        private readonly ILogger<ConsoleApplicationEntryPoint> _Logger;

        public ConsoleApplicationEntryPoint([NotNull] IApplicationEntryPoint applicationEntryPoint, [NotNull] ILogger<ConsoleApplicationEntryPoint> logger)
        {
            _ApplicationEntryPoint = applicationEntryPoint ?? throw new ArgumentNullException(nameof(applicationEntryPoint));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> Execute()
        {
            
            using (_Logger.BeginScope("application"))
            {
                try
                {
                    var result = await _ApplicationEntryPoint.Execute();
                    _Logger.LogDebug($"application exited successfully with exit code {result}");
                    return result;
                }
                catch (Exception ex)
                {
                    _Logger.LogError(ex, $"{ex.GetType().NotNull().Name}");
                    return 1;
                }
            }
        }
    }
}