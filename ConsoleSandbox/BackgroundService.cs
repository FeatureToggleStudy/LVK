using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.Core.Services;

namespace ConsoleSandbox
{
    internal class BackgroundService : BasicBackgroundService
    {
        public BackgroundService([NotNull] IApplicationLifetimeManager applicationLifetimeManager)
            : base(applicationLifetimeManager)
        {
        }

        protected override async Task BackgroundTask(CancellationToken cancellationToken)
        {
            for (int index = 0; index < 3; index++)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                Console.WriteLine("here");
                await Task.Delay(1000, cancellationToken);
            }

            SignalGracefulTermination();
        }
    }
}