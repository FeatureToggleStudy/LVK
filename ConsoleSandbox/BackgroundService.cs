using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.Core;
using LVK.Core.Services;
using LVK.Logging;

namespace ConsoleSandbox
{
    internal class BackgroundService : BasicBackgroundService
    {
        [NotNull]
        private readonly ILogger _Logger;

        public BackgroundService([NotNull] IApplicationLifetimeManager applicationLifetimeManager, [NotNull] ILogger logger)
            : base(applicationLifetimeManager)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task BackgroundTask(CancellationToken cancellationToken)
        {
            for (int index = 0; index < 3; index++)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                _Logger.WriteLine("here");
                await Task.Delay(1000, cancellationToken).NotNull();
            }
        }
    }
}