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

            container.Bootstrap<Data.ServicesBootstrapper>();
            container.Bootstrap<Logging.ServicesBootstrapper>();
            container.Bootstrap<Configuration.ServicesBootstrapper>();
            container.Bootstrap<Protection.ServicesBootstrapper>();

            container.Register<IDatabaseConnectionProvider<SqliteConnection>, SqliteDatabaseConnectionProvider>();
            container.Register<IDatabaseVersionHandler<SqliteConnection>, SqliteDatabaseVersionHandler>();

            container.Register<IContainerFinalizer, SqliteContainerFinalizer>();
            container.Register<ISQLitePCLInitializer, SQLitePclInitializer>(Reuse.Singleton);
        }
    }
}