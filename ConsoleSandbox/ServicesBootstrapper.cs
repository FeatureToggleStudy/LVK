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
            container.Bootstrap<LVK.Persistence.ServicesBootstrapper>();

            container.Register<IApplicationEntryPoint, ApplicationEntryPoint>();
            container.Register<IBackgroundService, FaultyBackgroundService>(serviceKey: "faulty");
            container.Register<IBackgroundService, AutomaticRetryBackgroundServiceDecorator>(setup: Setup.DecoratorWith(req => req.ServiceKey == "faulty"));
        }
    }
}