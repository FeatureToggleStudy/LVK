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

            container.Register<IHttpClientDefaultOptions, HttpClientOptions>(Reuse.Singleton,
                Made.Of(() => new HttpClientOptions()));
            
            container.Register<IHttpClientOptions, HttpClientOptions>(Made.Of(()
                => new HttpClientOptions(Arg.Of<IHttpClientDefaultOptions>())));
            
            container.Register<IHttpClientFactory, HttpClientFactory>();
        }
    }
}