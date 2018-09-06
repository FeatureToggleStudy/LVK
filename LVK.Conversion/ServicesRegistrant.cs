using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.Conversion.ValueConversionProviders;
using LVK.DryIoc;

namespace LVK.Conversion
{
    [PublicAPI]
    public class ServicesRegistrant : IServicesRegistrant
    {
        public void Register(IContainerBuilder containerBuilder)
        {
            if (containerBuilder is null)
                throw new ArgumentNullException(nameof(containerBuilder));

            containerBuilder.Register<LVK.Core.Services.ServicesRegistrant>();
        }

        public void Register(IContainer container)
        {
            if (container is null)
                throw new ArgumentNullException(nameof(container));

            container.Register<IValueConverter, ValueConverter>(Reuse.Singleton);
            container.Register<IValueConversionProvider, BasicTypesValueConversionProvider>();
        }
    }
}