using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.Notifications.PushBullet
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<Logging.ServicesBootstrapper>();
            container.Bootstrap<Configuration.ServicesBootstrapper>();
            container.Bootstrap<Core.Services.ServicesBootstrapper>();
            container.Bootstrap<Notifications.ServicesBootstrapper>();

            container.Register<INotificationChannel, PushbulletNotificationChannel>();
        }
    }
}