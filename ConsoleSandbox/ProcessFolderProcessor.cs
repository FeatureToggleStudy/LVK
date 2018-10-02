using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.WorkQueues;

namespace ConsoleSandbox
{
    internal class ProcessFolderProcessor : IWorkQueueProcessor<ProcessFolderQueueItem>
    {
        [NotNull]
        private readonly IWorkQueue _WorkQueue;

        public ProcessFolderProcessor([NotNull] IWorkQueue workQueue)
        {
            _WorkQueue = workQueue ?? throw new ArgumentNullException(nameof(workQueue));
        }

        public async Task Process(ProcessFolderQueueItem item, CancellationToken cancellationToken)
        {
            Console.WriteLine(item.Path);

            var newItems = Directory.GetDirectories(item.Path)
               .Select(subFolder => new ProcessFolderQueueItem { Path = subFolder });

            await _WorkQueue.EnqueueManyAsync(newItems);
        }
    }
}