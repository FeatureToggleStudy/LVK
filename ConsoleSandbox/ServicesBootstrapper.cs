using System;
using System.Threading.Tasks;

using DryIoc;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.Core.Services;
using LVK.DryIoc;
using LVK.WorkQueues;
using LVK.WorkQueues.Messages;

namespace ConsoleSandbox
{
    internal class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<LVK.AppCore.Console.ServicesBootstrapper>();
            container.Bootstrap<LVK.Conversion.ServicesBootstrapper>();
            container.Bootstrap<LVK.Reflection.ServicesBootstrapper>();
            container.Bootstrap<LVK.Persistence.ServicesBootstrapper>();
            container.Bootstrap<LVK.Data.Sqlite.ServicesBootstrapper>();
            container.Bootstrap<LVK.Data.ServicesBootstrapper>();
            container.Bootstrap<LVK.WorkQueues.ServicesBootstrapper>();
            container.Bootstrap<LVK.WorkQueues.Sqlite.ServicesBootstrapper>();

            container.Register<IApplicationEntryPoint, ApplicationEntryPoint>();

            container.Register<IWorkQueueProcessor<ProcessFolderQueueItem>, ProcessFolderProcessor>();
            container.Register<ISubscriber<WorkQueueEmptyMessage>, RestartScanSubscriber>();
        }
    }

    internal class RestartScanSubscriber : ISubscriber<WorkQueueEmptyMessage>
    {
        [NotNull]
        private readonly IWorkQueue _WorkQueue;

        public RestartScanSubscriber([NotNull] IWorkQueue workQueue)
        {
            _WorkQueue = workQueue ?? throw new ArgumentNullException(nameof(workQueue));
        }

        public Task Notify(WorkQueueEmptyMessage message)
        {
            Console.WriteLine("restarting scanning");
            return _WorkQueue.EnqueueAsync(new ProcessFolderQueueItem { Path = @"C:\Dev\LVK\develop\LVK.Core" }, DateTime.Now.AddMinutes(1));
        }
    }
}