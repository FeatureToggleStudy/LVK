using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.Core.Services;
using LVK.DryIoc;

namespace LVK.Logging
{
    [PublicAPI]
    public class ServicesRegistrant : IServicesRegistrant
    {
        public void Register(IContainerBuilder containerBuilder)
        {
            if (containerBuilder is null)
                throw new ArgumentNullException(nameof(containerBuilder));

            containerBuilder.Register<LVK.Configuration.ServicesRegistrant>();
            containerBuilder.Register<LVK.NodaTime.ServicesRegistrant>();
        }

        public void Register(IContainer container)
        {
            if (container is null)
                throw new ArgumentNullException(nameof(container));

            container.Register<ITextLogFormatter, TextLogFormatter>();
            container.Register<IContainerInitializer, LoggingContainerInitializer>();

            container.Register<IOptionsHelpTextProvider, LoggingOptionsHelpTextProvider>();
        }
    }
}