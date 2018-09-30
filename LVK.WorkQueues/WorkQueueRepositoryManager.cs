using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using LVK.Logging;

using Newtonsoft.Json.Linq;

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
        
        public WorkQueueRepositoryManager([NotNull, ItemNotNull] IEnumerable<IWorkQueueRepository> repositories, [NotNull] IWorkQueueRetryPolicy retryPolicy, [NotNull] ILogger logger)
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

        public void Enqueue(string type, JObject payload, DateTime whenToProcess, int retryCount) => GetEnabledRepository()?.Enqueue(type, payload, whenToProcess, retryCount);
        public void Faulted(string type, JObject payload) => GetEnabledRepository()?.Faulted(type, payload);
        public IWorkQueueModel Dequeue() => GetEnabledRepository()?.Dequeue();
        public bool IsEnabled => GetEnabledRepository() != null;

        public void Retry(IWorkQueueModel model)
        {
            int retryCount = model.RetryCount + 1;
            DateTime? whenToRetry = _RetryPolicy.WhenToRetry(retryCount);
            if (whenToRetry == null)
            {
                _Logger.LogError($"work queue item '{model.Type}' retried too many times, item now faulted");
                GetEnabledRepository()?.Faulted(model.Type, model.Payload);
                return;
            }
            
            GetEnabledRepository()?.Enqueue(model.Type, model.Payload, whenToRetry.Value, retryCount);
        }
    }
}