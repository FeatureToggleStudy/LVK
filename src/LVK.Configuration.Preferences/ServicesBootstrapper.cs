using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.Configuration.Preferences
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container is null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<AppCore.ServicesBootstrapper>();
            container.Bootstrap<Json.ServicesBootstrapper>();
            container.Bootstrap<Core.Services.ServicesBootstrapper>();
            container.Bootstrap<NodaTime.ServicesBootstrapper>();
            container.Bootstrap<Configuration.ServicesBootstrapper>();
            container.Bootstrap<Data.Caching.ServicesBootstrapper>();

            container.Register<IPreferencesManager, PreferencesManager>(Reuse.Singleton);
            container.Register<IPreferencesFile, PreferencesFile>(Reuse.Singleton);
            container.Register<IPreferencesStore, PreferencesStore>(Reuse.Singleton);
        }
    }
}