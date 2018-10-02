using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Logging;

namespace LVK.WorkQueues
{
    internal class WorkQueueRepositoryManager : IWorkQueueRepositoryManager
    {
        [NotNull]
        private readonly IWorkQueueRetryPolicy _RetryPolicy;

        [NotNull]
        private readonly ILogger _Logger;

        [NotNull, ItemNotNull]
        private readonly List<IWorkQueueRepository> _Repositories;

        public WorkQueueRepositoryManager(
            [NotNull, ItemNotNull] IEnumerable<IWorkQueueRepository> repositories, [NotNull] IWorkQueueRetryPolicy retryPolicy, [NotNull] ILogger logger)
        {
            if (repositories == null)
                throw new ArgumentNullException(nameof(repositories));

            _RetryPolicy = retryPolicy ?? throw new ArgumentNullException(nameof(retryPolicy));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _Repositories = repositories.ToList();
        }

        [CanBeNull]
        private IWorkQueueRepository GetEnabledRepository()
        {
            List<IWorkQueueRepository> enabledRepositories = _Repositories.Where(repo => repo.IsEnabled).ToList();

            if (enabledRepositories.Count > 1)
                throw new InvalidOperationException("Multiple work queue repositories configured, this is not supported");

            return enabledRepositories.FirstOrDefault();
        }

        public Task EnqueueManyAsync(IEnumerable<WorkQueueItem> items) => GetEnabledRepository()?.EnqueueManyAsync(items) ?? Task.CompletedTask;
        public Task FaultedAsync(WorkQueueItem item) => GetEnabledRepository()?.FaultedAsync(item) ?? Task.CompletedTask;
        public Task<WorkQueueItem?> DequeueAsync() => GetEnabledRepository()?.DequeueAsync() ?? Task.FromResult<WorkQueueItem?>(null);

        public bool IsEnabled => GetEnabledRepository() != null;

        public async Task RetryAsync(WorkQueueItem item)
        {
            int retryCount = item.RetryCount + 1;
            DateTime? whenToRetry = _RetryPolicy.WhenToRetry(retryCount);
            if (whenToRetry == null)
            {
                _Logger.LogError($"work queue item '{item.Type}' retried too many times, item now faulted");
                await FaultedAsync(item);

                return;
            }

            await EnqueueManyAsync(new[] { new WorkQueueItem(item.Type, item.Payload, whenToRetry.Value, retryCount) });
        }
    }
}