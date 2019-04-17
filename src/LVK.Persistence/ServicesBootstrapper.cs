using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.Persistence
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<Logging.ServicesBootstrapper>();
            container.Bootstrap<Reflection.ServicesBootstrapper>();
            
            container.Register(typeof(IPersistentData<>), typeof(PersistentData<>));
        }
    }
}