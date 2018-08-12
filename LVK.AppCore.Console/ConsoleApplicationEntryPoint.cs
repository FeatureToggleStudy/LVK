using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Logging;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.AppCore.Console
{
    [UsedImplicitly]
    internal class ConsoleApplicationEntryPoint : IConsoleApplicationEntryPoint
    {
        [NotNull]
        private readonly IApplicationEntryPoint _ApplicationEntryPoint;

        [NotNull]
        private readonly ILogger _Logger;

        public ConsoleApplicationEntryPoint([NotNull] IApplicationEntryPoint applicationEntryPoint, [NotNull] ILogger logger)
        {
            _ApplicationEntryPoint = applicationEntryPoint ?? throw new ArgumentNullException(nameof(applicationEntryPoint));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> Execute()
        {
            using (_Logger.Scope(LogLevel.Debug, "application"))
            {
                try
                {
                    var result = await _ApplicationEntryPoint.Execute();
                    _Logger.Debug($"application exited successfully with exit code {result}");
                    return result;
                }
                catch (Exception ex)
                {
                    _Logger.Error($"{ex.GetType().NotNull().Name}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                    return 1;
                }
            }
        }
    }
}