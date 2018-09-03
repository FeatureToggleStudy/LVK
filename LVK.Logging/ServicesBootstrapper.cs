using DryIoc;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.Core.Services;
using LVK.DryIoc;

namespace LVK.Logging
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<LVK.Configuration.ServicesBootstrapper>();

            container.Register<ITextLogFormatter, TextLogFormatter>();
            container.Register<IApplicationInitialization, LoggingInitialization>();

            container.Register<IOptionsHelpTextProvider, LoggingOptionsHelpTextProvider>();
        }
    }
}