using DryIoc;

using LVK.AppCore;
using LVK.Core.Services;
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

            container.Register<IBackgroundService, BackgroundService>();
            container.Register<IApplicationEntryPoint, ApplicationEntryPoint>();
        }
    }
}