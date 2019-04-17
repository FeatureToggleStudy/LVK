using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.Data
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<Configuration.ServicesBootstrapper>();
            container.Bootstrap<Logging.ServicesBootstrapper>();
            container.Bootstrap<Protection.ServicesBootstrapper>();

            container.Register<IDatabaseConnectionFactory, DatabaseConnectionFactory>();
            container.Register<IDatabaseMigrator, DatabaseMigrator>();
        }
    }
}