using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.AppCore.Console
{
    [PublicAPI]
    public class ServicesRegistrant : IServicesRegistrant
    {
        public void Register(IContainerBuilder containerBuilder)
        {
            if (containerBuilder is null)
                throw new ArgumentNullException(nameof(containerBuilder));
            
            containerBuilder.Register<LVK.AppCore.ServicesRegistrant>();
            containerBuilder.Register<LVK.Logging.ServicesRegistrant>();
        }

        public void Register(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.Register<IConsoleApplicationEntryPoint, ConsoleApplicationEntryPoint>();
            container.Register<IOptionsHelpTextProvider, ConsoleApplicationEntryPointOptionsHelpTextProvider>();
        }
    }
}