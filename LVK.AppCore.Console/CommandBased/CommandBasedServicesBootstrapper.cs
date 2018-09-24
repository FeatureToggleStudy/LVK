using DryIoc;

using LVK.Core.Services;
using LVK.DryIoc;

namespace LVK.AppCore.Console.CommandBased
{
    internal class CommandBasedServicesBootstrapper<T> : IServicesBootstrapper
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