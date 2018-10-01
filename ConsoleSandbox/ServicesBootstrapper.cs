using ConsoleSandbox.Controllers;

using DryIoc;

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

            container.Register<ITestService, TestService>();
        }
    }
}