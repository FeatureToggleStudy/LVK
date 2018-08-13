﻿using System;

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

            container.Register<IHttpClientOptions, HttpClientOptions>(Reuse.Singleton);
            container.Register<IHttpClientFactory, HttpClientFactory>();
        }
    }
}