using DryIoc;

using LVK.Core.Services;
using LVK.DryIoc;

namespace ConsoleSandbox
{
    internal class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<LVK.AppCore.Console.ServicesBootstrapper>();
            container.Bootstrap<LVK.Data.Protection.ServicesBootstrapper>();
            container.Bootstrap<LVK.Notifications.Email.ServicesBootstrapper>();
            container.Bootstrap<LVK.Notifications.PushBullet.ServicesBootstrapper>();
            container.Bootstrap<LVK.Net.Http.Server.ServicesBootstrapper>();

            container.Register<IBackgroundService, FirstBackgroundService>();
            container.Register<IBackgroundService, SecondBackgroundService>();
        }
    }
}