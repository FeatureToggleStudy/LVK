using System;
using System.Net.Http;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;
using LVK.Logging;

namespace LVK.Net.Http
{
    [PublicAPI]
    public class ServicesRegistrant : IServicesRegistrant
    {
        public void Register(IContainerBuilder containerBuilder)
        {
            if (containerBuilder is null)
                throw new ArgumentNullException(nameof(containerBuilder));
        }

        public void Register(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.Register(Made.Of(()
                => HttpClientFactory.Create(Arg.Of<HttpMessageHandler>(IfUnresolved.ReturnDefaultIfNotRegistered),
                    Arg.Of<ILogger>())));
        }
    }
}