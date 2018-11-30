using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.Data;
using LVK.DryIoc;

namespace LVK.WorkQueues.Sqlite
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container is null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<LVK.WorkQueues.ServicesBootstrapper>()
               .Bootstrap<LVK.Logging.ServicesBootstrapper>()
               .Bootstrap<LVK.Configuration.ServicesBootstrapper>()
               .Bootstrap<LVK.Json.ServicesBootstrapper>()
               .Bootstrap<LVK.Core.Services.ServicesBootstrapper>()
               .Bootstrap<LVK.Data.Sqlite.ServicesBootstrapper>()
               .Bootstrap<LVK.Data.ServicesBootstrapper>()
               .Bootstrap<LVK.Security.Cryptography.ServicesBootstrapper>();

            container.RegisterAll<IDatabaseMigration>(GetType().Assembly);
            container.Register<IWorkQueueRepository, SqliteWorkQueueRepository>();
        }
    }
}