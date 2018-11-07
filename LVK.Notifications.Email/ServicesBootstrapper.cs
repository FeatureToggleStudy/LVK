using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.Notifications.Email
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<LVK.Logging.ServicesBootstrapper>();
            container.Bootstrap<LVK.Configuration.ServicesBootstrapper>();
            container.Bootstrap<LVK.Core.Services.ServicesBootstrapper>();
            container.Bootstrap<LVK.Notifications.ServicesBootstrapper>();
            container.Bootstrap<LVK.Net.Smtp.ServicesBootstrapper>();

            container.Register<INotificationChannel, EmailNotificationChannel>();
        }
    }
}