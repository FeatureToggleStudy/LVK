using DryIoc;

using LVK.DryIoc;

namespace LVK.AppCore.Windows.Forms
{
    internal class ServicesBootstrapper<T> : IServicesBootstrapper
        where T: class, IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<T>();
            container.Bootstrap<ServicesBootstrapper>();
            container.Bootstrap<Core.Services.ServicesBootstrapper>();
        }
    }
}