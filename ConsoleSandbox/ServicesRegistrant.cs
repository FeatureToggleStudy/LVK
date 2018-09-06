using DryIoc;

using LVK.AppCore;
using LVK.Core.Services;
using LVK.DryIoc;

namespace ConsoleSandbox
{
    internal class ServicesRegistrant : IServicesRegistrant
    {
        public void Register(IContainerBuilder containerBuilder)
        {
            containerBuilder.Register<LVK.AppCore.Console.ServicesRegistrant>();
            containerBuilder.Register<LVK.Conversion.ServicesRegistrant>();
            containerBuilder.Register<LVK.Reflection.ServicesRegistrant>();
        }

        public void Register(IContainer container)
        {
            container.Register<IBackgroundService, BackgroundService>();
            container.Register<IApplicationEntryPoint, ApplicationEntryPoint>();
        }
    }
}