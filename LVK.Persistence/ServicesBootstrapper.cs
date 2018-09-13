using DryIoc;

using LVK.DryIoc;

namespace LVK.Persistence
{
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<LVK.Logging.ServicesBootstrapper>();
            container.Bootstrap<LVK.Reflection.ServicesBootstrapper>();
            
            container.Register(typeof(IPersistentData<>), typeof(PersistentData<>));
        }
    }
}