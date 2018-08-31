using DryIoc;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.Core.Services;
using LVK.DryIoc;

namespace ConsoleSandbox
{
    [UsedImplicitly]
    internal class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<LVK.AppCore.Console.ServicesBootstrapper>();

            container.Register<IApplicationRuntimeContext, BackgroundService>();
            container.Register<IApplicationEntryPoint, ApplicationEntryPoint>();
        }
    }
}