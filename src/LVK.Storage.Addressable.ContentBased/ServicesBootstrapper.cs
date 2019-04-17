using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.Storage.Addressable.ContentBased
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container is null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<Json.ServicesBootstrapper>();
            container.Bootstrap<ServicesBootstrapper>();
            container.Bootstrap<Security.Cryptography.ServicesBootstrapper>();
            container.Bootstrap<Core.Services.ServicesBootstrapper>();
            container.Bootstrap<NodaTime.ServicesBootstrapper>();
            container.Bootstrap<Configuration.ServicesBootstrapper>();
            container.Bootstrap<Logging.ServicesBootstrapper>();

            container.Register<IContentAddressableRepositoryFactory, ContentAddressableRepositoryFactory>();
            container.Register<IContentAddressableStoreFactory, ContentAddressableStoreFactory>();
        }
    }
}