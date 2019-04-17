using System;
using System.Data.SqlClient;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.Data.SqlServer
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

            container.Register<IDatabaseConnectionProvider<SqlConnection>, SqlServerDatabaseConnectionProvider>();
            container.Register<IDatabaseVersionHandler<SqlConnection>, SqlServerDatabaseVersionHandler>();
        }
    }
}
