using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.Core;
using LVK.WorkQueues;

namespace ConsoleSandbox
{
    public class ApplicationEntryPoint : IApplicationEntryPoint
    {
        [NotNull]
        private readonly IWorkQueue _WorkQueue;

        public ApplicationEntryPoint([NotNull] IWorkQueue workQueue)
        {
            _WorkQueue = workQueue ?? throw new ArgumentNullException(nameof(workQueue));
        }

        public async Task<int> Execute(CancellationToken cancellationToken)
        {
            // await _WorkQueue.EnqueueAsync(new ProcessFolderQueueItem { Path = @"C:\Dev\LVK\develop\LVK.Core" });
            await cancellationToken.AsTask();
            return 0;
        }
    }
}