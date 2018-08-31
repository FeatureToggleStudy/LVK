using DryIoc;

using JetBrains.Annotations;

using LVK.Core.Services;
using LVK.DryIoc;

namespace LVK.Logging
{
    [UsedImplicitly]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<LVK.Configuration.ServicesBootstrapper>();

            container.Register<ITextLogFormatter, TextLogFormatter>();
            container.Register<IApplicationInitialization, LoggingInitialization>();
        }
    }
}