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
            container.Register<IValueConverter, ValueConverter>(Reuse.Singleton);
            container.Register<IValueConversionProvider, BasicTypesValueConversionProvider>();
        }
    }
}