using DryIoc;

using LVK.Core.Services;
using LVK.DryIoc;
using LVK.Net.Http.Server;

namespace WindowsServiceSandbox
{
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<LVK.Logging.ServicesBootstrapper>();
            container.Bootstrap<LVK.Net.Http.Server.ServicesBootstrapper>();

            container.Register<IBackgroundService, TestService>();
            container.Register<IBackgroundService, WebServerBackgroundService>();
        }
    }
}