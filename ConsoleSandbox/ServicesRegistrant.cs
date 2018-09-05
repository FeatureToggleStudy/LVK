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
        }

        public void Register(IContainer container)
        {
            container.Register<IApplicationRuntimeContext, BackgroundService>();
            container.Register<IApplicationEntryPoint, ApplicationEntryPoint>();
        }
    }
}