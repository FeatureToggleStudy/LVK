﻿using System;
using System.Net.Http;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;
using LVK.Logging;

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

            container.Register(Made.Of(()
                => HttpClientFactory.Create(Arg.Of<HttpMessageHandler>(IfUnresolved.ReturnDefaultIfNotRegistered),
                    Arg.Of<ILogger>())));
        }
    }
}