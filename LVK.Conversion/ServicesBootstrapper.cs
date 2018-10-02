using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.Conversion.ValueConversionProviders;
using LVK.DryIoc;

namespace LVK.Conversion
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container is null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<LVK.Core.Services.ServicesBootstrapper>();
            container.Bootstrap<LVK.Reflection.ServicesBootstrapper>();

            container.Register<IValueConverter, ValueConverter>(Reuse.Singleton);
            container.Register<IValueConversionProvider, BasicTypesValueConversionProvider>();
        }
    }
}