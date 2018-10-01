using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

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

            container.Register<IDatabaseConnectionProvider, SqliteDatabaseConnectionProvider>();
        }
    }
}