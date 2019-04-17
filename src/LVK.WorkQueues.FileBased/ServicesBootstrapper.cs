using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.WorkQueues.FileBased
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container is null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<WorkQueues.ServicesBootstrapper>()
               .Bootstrap<Logging.ServicesBootstrapper>()
               .Bootstrap<Configuration.ServicesBootstrapper>()
               .Bootstrap<Json.ServicesBootstrapper>()
               .Bootstrap<Core.Services.ServicesBootstrapper>()
               .Bootstrap<Security.Cryptography.ServicesBootstrapper>();

            container.Register<IWorkQueueRepository, FileWorkQueueRepository>();
        }
    }
}