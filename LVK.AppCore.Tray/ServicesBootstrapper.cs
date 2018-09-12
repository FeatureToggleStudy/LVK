using DryIoc;

using JetBrains.Annotations;

using LVK.Core.Services;
using LVK.DryIoc;

namespace LVK.AppCore.Tray
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<LVK.AppCore.ServicesBootstrapper>();

            container.Register<ITrayIconMenuItem, ExitTrayIconAppMenuItem>();
            container.Register<IBackgroundService, TrayIconBackgroundService>();
        }
    }
}