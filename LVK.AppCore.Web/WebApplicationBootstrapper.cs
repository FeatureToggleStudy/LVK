using DryIoc;

using LVK.DryIoc;

namespace LVK.AppCore.Web
{
    internal class WebApiApplicationBootstrapper<T> : IServicesBootstrapper
        where T: class, IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<ServicesBootstrapper>();
            container.Bootstrap<T>();
        }
    }
}