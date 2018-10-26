using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

using Microsoft.Data.Sqlite;

namespace LVK.Data.Sqlite
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<LVK.Data.ServicesBootstrapper>();
            container.Bootstrap<LVK.Logging.ServicesBootstrapper>();
            container.Bootstrap<LVK.Configuration.ServicesBootstrapper>();
            container.Bootstrap<LVK.Data.Protection.ServicesBootstrapper>();

            container.Register<IDatabaseConnectionProvider<SqliteConnection>, SqliteDatabaseConnectionProvider>();
            container.Register<IDatabaseVersionHandler<SqliteConnection>, SqliteDatabaseVersionHandler>();
        }
    }
}