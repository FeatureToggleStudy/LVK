using DryIoc;

using LVK.DryIoc;

namespace LVK.AppCore.Windows.Wpf
{
    internal class ServicesBootstrapper<T> : IServicesBootstrapper
        where T: class, IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<T>();
            container.Bootstrap<ServicesBootstrapper>();
            container.Bootstrap<LVK.Core.Services.ServicesBootstrapper>();
            container.Bootstrap<LVK.Logging.ServicesBootstrapper>();

            container.Register<IWpfApplicationLifetimeManager, WpfApplicationLifetimeManager>();
        }
    }
}