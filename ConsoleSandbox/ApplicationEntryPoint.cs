using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.Core;
using LVK.Core.Services;

namespace ConsoleSandbox
{
    internal class ApplicationEntryPoint : IApplicationEntryPoint
    {
        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        public ApplicationEntryPoint([NotNull] IApplicationLifetimeManager applicationLifetimeManager)
        {
            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));
        }

        public async Task<int> Execute(CancellationToken cancellationToken)
        {
            await _ApplicationLifetimeManager.GracefulTerminationCancellationToken.AsTask();
            return 0;
        }
    }
}