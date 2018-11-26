using DryIoc;

using LVK.Core.Services;
using LVK.DryIoc;

namespace WindowsServiceSandbox
{
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<LVK.Logging.ServicesBootstrapper>();

            container.Register<IBackgroundService, TestService>();
        }
    }
}