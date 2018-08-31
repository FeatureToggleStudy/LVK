using DryIoc;

using LVK.DryIoc;

namespace LVK.AppCore.Console
{
    internal class DaemonServicesBootstrapper<T> : IServicesBootstrapper
        where T: class, IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Register<IApplicationEntryPoint, DaemonApplicationEntryPoint>();
            container.Bootstrap<T>();
        }
    }
}