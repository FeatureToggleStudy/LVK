using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.Core.Services;
using LVK.DryIoc;

namespace LVK.WorkQueues
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container is null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<Configuration.ServicesBootstrapper>()
               .Bootstrap<Logging.ServicesBootstrapper>()
               .Bootstrap<Json.ServicesBootstrapper>()
               .Bootstrap<Core.Services.ServicesBootstrapper>();

            container.Register<IWorkQueueRepositoryManager, WorkQueueRepositoryManager>(Reuse.Singleton);
            container.Register<IWorkQueue, WorkQueue>(Reuse.Singleton);
            container.Register<IBackgroundService, WorkQueueProcessorBackgroundService>();
            container.Register<IWorkQueueRetryPolicy, WorkQueueRetryPolicy>();
        }
    }
}