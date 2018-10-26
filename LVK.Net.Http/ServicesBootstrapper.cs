using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.Net.Http
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<LVK.Logging.ServicesBootstrapper>();

            container.Register<IHttpClientProvider, HttpClientProvider>(Reuse.Singleton);
            container.Register(made: Made.Of(r => ServiceInfo.Of<IHttpClientProvider>(), hcp => hcp.Provide("Default")), reuse: Reuse.Singleton);
        }
    }
}