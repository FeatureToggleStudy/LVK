using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.Net.Http.Server
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<LVK.Configuration.ServicesBootstrapper>();
            container.Bootstrap<Core.Services.ServicesBootstrapper>();
            container.Bootstrap<Logging.ServicesBootstrapper>();

            container.Register<IWebServer, WebServer>(Reuse.Singleton);
        }
    }
}