using DryIoc;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.DryIoc;

namespace ConsoleSandbox
{
    [UsedImplicitly]
    internal class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<LVK.Net.Http.ServicesBootstrapper>();
            
            container.Register<IApplicationEntryPoint, MyApplication>();
            container.Register<IApplicationInitialization, MyInit>();
            container.Register<IApplicationCleanup, MyCleanup>();

            container.Register<ITestWebApi, TestWebApi>();
        }
    }
}