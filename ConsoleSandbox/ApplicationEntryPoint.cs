using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.Configuration;
using LVK.Core;
using LVK.Core.Services;
using LVK.Logging;

namespace ConsoleSandbox
{
    internal class ApplicationEntryPoint : IApplicationEntryPoint
    {
        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        [NotNull]
        private readonly IConfiguration _Configuration;

        public ApplicationEntryPoint(
            [NotNull] ILogger logger,
            [NotNull] IApplicationLifetimeManager applicationLifetimeManager,
            [NotNull] IConfiguration configuration)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));
            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<int> Execute(CancellationToken cancellationToken)
        {
            var conf = _Configuration["Test"].Element<string>().WithDefault("<dummy>");
            while (!_ApplicationLifetimeManager.GracefulTerminationCancellationToken.IsCancellationRequested)
            {
                _Logger.LogTrace("TRACE");
                _Logger.LogVerbose("VERBOSE");
                _Logger.LogDebug("DEBUG");
                await Task.Delay(500, _ApplicationLifetimeManager.GracefulTerminationCancellationToken).NotNull();
                Console.WriteLine(conf.Value());
            }

            return 0;
        }
    }
}