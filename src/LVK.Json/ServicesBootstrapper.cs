using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.Json
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<LVK.Core.Services.ServicesBootstrapper>();
            
            container.Register<IJsonSerializerSettingsFactory, JsonSerializerSettingsFactory>();
            container.Register(Made.Of(r => ServiceInfo.Of<IJsonSerializerSettingsFactory>(), f => f.Create()));

            container.Register<IJsonSerializerFactory, JsonSerializerFactory>();

            container.Register<IJsonSerializerSettingsConfigurator, JsonConvertersSerializerSettingsConfigurator>();
            container.Register<IJsonConvertersProvider, JsonStringDecoderConverterProvider>();
            container.Register<IJsonStringDecoder, ConfigurationVariablesJsonStringDecoder>();
        }
    }
}