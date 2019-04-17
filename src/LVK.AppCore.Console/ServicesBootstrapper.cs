using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.Core.Services;
using LVK.DryIoc;

namespace LVK.AppCore.Console
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<AppCore.ServicesBootstrapper>();
            container.Bootstrap<Logging.ServicesBootstrapper>();

            container.Register<IConsoleApplicationEntryPoint, ConsoleApplicationEntryPoint>();
            container.Register<IOptionsHelpTextProvider, ConsoleApplicationEntryPointOptionsHelpTextProvider>();

            container.Register<IConsoleApplicationHelpTextPresenter, ConsoleApplicationHelpTextPresenter>();
        }
    }
}