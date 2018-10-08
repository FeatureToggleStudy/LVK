using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.Storage.Addressable.Content
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container is null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<LVK.Json.ServicesBootstrapper>();
            container.Bootstrap<ServicesBootstrapper>();
            container.Bootstrap<LVK.Security.Cryptography.ServicesBootstrapper>();
            container.Bootstrap<LVK.Core.Services.ServicesBootstrapper>();
            container.Bootstrap<LVK.NodaTime.ServicesBootstrapper>();
            container.Bootstrap<LVK.Configuration.ServicesBootstrapper>();
            container.Bootstrap<LVK.Logging.ServicesBootstrapper>();

            container.Register<IContentAddressableRepositoryFactory, ContentAddressableRepositoryFactory>();
            container.Register<IContentAddressableStoreFactory, ContentAddressableStoreFactory>();
        }
    }
}