using DryIoc;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.DryIoc;

using Microsoft.Extensions.Logging;

namespace ConsoleSandbox
{
    [UsedImplicitly]
    internal class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Register<IApplicationEntryPoint, MyApplication>();
            container.Register<IApplicationInitialization, MyInit>();
            container.Register<IApplicationCleanup, MyCleanup>();
        }
    }
}