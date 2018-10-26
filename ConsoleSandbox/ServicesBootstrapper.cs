using DryIoc;

using LVK.AppCore;
using LVK.DryIoc;

namespace ConsoleSandbox
{
    internal class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<LVK.AppCore.Console.ServicesBootstrapper>();
            container.Bootstrap<LVK.Data.Protection.ServicesBootstrapper>();
            container.Register<IApplicationEntryPoint, ApplicationEntryPoint>();
        }
    }
}