using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using DryIoc;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Configuration;
using LVK.Core.Services;
using LVK.Logging;
using LVK.WorkQueues.Messages;

namespace LVK.WorkQueues
{
    internal class WorkQueueProcessorBackgroundService : IBackgroundService
    {
        [NotNull]
        private readonly IBus _Bus;

        [NotNull]
        private readonly IWorkQueueRepositoryManager _WorkQueueRepositoryManager;

        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly IContainer _Container;

        [NotNull]
        private readonly IConfigurationElementWithDefault<WorkQueueProcessorConfiguration> _Configuration;

        public WorkQueueProcessorBackgroundService(
            [NotNull] IBus bus, [NotNull] IWorkQueueRepositoryManager workQueueRepositoryManager, [NotNull] ILogger logger, [NotNull] IContainer container,
            [NotNull] IConfiguration configuration)
        {
            _Bus = bus;
            _WorkQueueRepositoryManager = workQueueRepositoryManager ?? throw new ArgumentNullException(nameof(workQueueRepositoryManager));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _Container = container ?? throw new ArgumentNullException(nameof(container));
            _Configuration = configuration.Element<WorkQueueProcessorConfiguration>("WorkQueues").WithDefault(() => new WorkQueueProcessorConfiguration());
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            using (var evt = new ManualResetEvent(false))
            {
                void notify(WorkQueueItemAddedMessage message)
                {
                    // ReSharper disable once AccessToDisposedClosure
                    evt.Set();
                }

                using (_Bus.Subscribe<WorkQueueItemAddedMessage>(notify))
                {
                    bool workerQueueIsEmpty = false;
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        evt.Reset();

                        var cts = new CancellationTokenSource();
                        Task delayTask = Task.Delay(5000, cts.Token);
                        Task evtTask = evt.AsTask();
                            
                        await Task.WhenAny(delayTask, evtTask).NotNull();
                        cts.Cancel();

                        if (cancellationToken.IsCancellationRequested)
                            break;

                        IEnumerable<Task<bool>> workerThreads = Enumerable.Range(1, _Configuration.Value().WorkerThreads).Select(_ => ProcessMessages(cancellationToken));

                        bool[] workerThreadProcessedMessages = await Task.WhenAll(workerThreads).NotNull();
                        if (workerThreadProcessedMessages.Any(b => b))
                            workerQueueIsEmpty = false;
                        else
                        {
                            if (!workerQueueIsEmpty)
                            {
                                workerQueueIsEmpty = true;
                                _Bus.Publish(WorkQueueEmptyMessage.Instance);
                            }
                        }
                    }
                }
            }
        }

        [NotNull]
        private async Task<bool> ProcessMessages(CancellationToken cancellationToken)
        {
            using (_Logger.LogScope(LogLevel.Trace, "WorkQueueProcessorBackgroundService.ProcessMessages"))
            {
                WorkQueueItem? item;
                bool any = false;
                while ((item = await _WorkQueueRepositoryManager.DequeueAsync()) != null)
                {
                    any = true;
                    _Logger.Log(LogLevel.Debug, $"work queue model '{item.Value.Type}'");

                    Type itemType = ResolveItemType(item.Value.Type);
                    MethodInfo processMethod = GetType().GetMethod("Process", BindingFlags.Instance | BindingFlags.NonPublic).NotNull().MakeGenericMethod(itemType);
                    processMethod.Invoke(this, new object[] { item, cancellationToken });
                }

                return any;
            }
        }

        private Type ResolveItemType([NotNull] string modelType)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            IEnumerable<Type> types =
                from assembly in assemblies
                let type = assembly.GetType(modelType)
                where type != null
                select type;

            return types.FirstOrDefault();
        }

        private async Task Process<T>(WorkQueueItem item, CancellationToken cancellationToken)
            where T: class
        {
            T payload = item.Payload.ToObject<T>().NotNull();
            var processor = _Container.Resolve<IWorkQueueProcessor<T>>(IfUnresolved.ReturnDefaultIfNotRegistered);
            if (processor == null)
            {
                _Logger.LogError($"work queue item '{item.Type}' has no processor, item faulted");
                await _WorkQueueRepositoryManager.FaultedAsync(item);
                return;
            }

            using (_Logger.LogScope(LogLevel.Verbose, $"processing work queue item '{item.Type}'"))
            {
                try
                {
                    await processor.Process(payload, cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    await _WorkQueueRepositoryManager.EnqueueManyAsync(new[] { item });
                    throw;
                }
                catch (Exception ex)
                {
                    _Logger.LogException(ex);
                    await _WorkQueueRepositoryManager.RetryAsync(item);
                }
            }
        }
    }
}