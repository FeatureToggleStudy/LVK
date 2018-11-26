using DryIoc;

using JetBrains.Annotations;

using LVK.Core.Services;
using LVK.DryIoc;

namespace LVK.AppCore.Windows.Tray
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<LVK.AppCore.ServicesBootstrapper>();
            container.Bootstrap<LVK.Core.Services.ServicesBootstrapper>();
            container.Bootstrap<LVK.Logging.ServicesBootstrapper>();

            container.Register<ITrayIconMenuItem, ExitTrayIconAppMenuItem>();
            container.Register<IBackgroundService, TrayIconBackgroundService>();
        }
    }
}