using DryIoc;

using LVK.DryIoc;

namespace LVK.AppCore.Console.CommandBased
{
    public class CommandBasedServicesBootstrapper<T> : IServicesBootstrapper
        where T: class, IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<T>();
            container.Register<IApplicationEntryPoint, CommandBasedApplicationEntryPoint>();
            container.Register<IOptionsHelpTextProvider, CommandsHelpTextProvider>();
        }
    }
}