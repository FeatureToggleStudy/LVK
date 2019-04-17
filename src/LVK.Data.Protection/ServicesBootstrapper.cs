using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.DryIoc;

namespace LVK.Data.Protection
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<Core.Services.ServicesBootstrapper>();
            container.Bootstrap<Caching.ServicesBootstrapper>();
            container.Bootstrap<Configuration.ServicesBootstrapper>();

            container.Register<IDataProtection, DataProtection>();
            container.Register<IDataEncryption, DataEncryption>();
            container.Register<IDataProtectionPasswordProvider, EnvironmentVariableDataProtectionPasswordProvider>();
            container.Register<IDataProtectionPasswordProvider, ConfigurationDataProtectionPasswordProvider>();
            container.Register<IConfigurationDecoder, EncryptedStringConfigurationDecoder>();
        }
    }
}