using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.Core.Services
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.Register<IApplicationLifetimeManager, ApplicationLifetimeManager>(Reuse.Singleton);
            container.Register<IBus, Bus>(Reuse.Singleton);
            container.Register<IDataEncoder, DataEncoder>();
            container.Register<IConfigurationVariables, EnvironmentConfigurationVariables>();
            container.Register<IConfigurationVariables, SystemConfigurationVariables>();
            
            container.Register<ISystemConfigurationVariablesProvider, ProcessSystemConfigurationVariablesProvider>();
            container.Register<IApplicationContext, ApplicationContext>(Reuse.Singleton);
        }
    }
}