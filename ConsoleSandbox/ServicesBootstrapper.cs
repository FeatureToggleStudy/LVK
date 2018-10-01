using ConsoleSandbox.Migrations;

using DryIoc;

using LVK.AppCore;
using LVK.Data;
using LVK.DryIoc;

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

            container.Register<IApplicationEntryPoint, ApplicationEntryPoint>();

            container.Register<IDatabaseMigration, Version0To1>();
        }
    }
}