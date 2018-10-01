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

                        IEnumerable<Task> workerThreads = Enumerable.Range(1, _Configuration.Value().WorkerThreads).Select(_ => ProcessMessages(cancellationToken));

                        await Task.WhenAll(workerThreads).NotNull();
                    }
                }
            }
        }

        [NotNull]
        private Task ProcessMessages(CancellationToken cancellationToken)
        {
            using (_Logger.LogScope(LogLevel.Trace, "WorkQueueProcessorBackgroundService.ProcessMessages"))
            {
                IWorkQueueModel model;
                while ((model = _WorkQueueRepositoryManager.Dequeue()) != null)
                {
                    _Logger.Log(LogLevel.Debug, $"work queue model '{model.Type}'");

                    Type itemType = ResolveItemType(model.Type);
                    MethodInfo processMethod = GetType().GetMethod("Process", BindingFlags.Instance | BindingFlags.NonPublic).NotNull().MakeGenericMethod(itemType);
                    processMethod.Invoke(this, new object[] { model, cancellationToken });
                }

                return Task.CompletedTask;
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

        private async Task Process<T>([NotNull] IWorkQueueModel model, CancellationToken cancellationToken)
            where T: class
        {
            T item = model.Payload.ToObject<T>().NotNull();
            var processor = _Container.Resolve<IWorkQueueProcessor<T>>(IfUnresolved.ReturnDefaultIfNotRegistered);
            if (processor == null)
            {
                _Logger.LogError($"work queue item '{model.Type}' has no processor, item faulted");
                _WorkQueueRepositoryManager.Faulted(model.Type, model.Payload);
                return;
            }

            using (_Logger.LogScope(LogLevel.Verbose, $"processing work queue item '{model.Type}'"))
            {
                try
                {
                    await processor.Process(item, cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    _WorkQueueRepositoryManager.Enqueue(model.Type, model.Payload, model.WhenToProcess, model.RetryCount);
                    throw;
                }
                catch (Exception ex)
                {
                    _Logger.LogException(ex);
                    _WorkQueueRepositoryManager.Retry(model);
                }
            }
        }
    }
}