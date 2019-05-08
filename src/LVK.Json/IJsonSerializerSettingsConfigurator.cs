using JetBrains.Annotations;

using Newtonsoft.Json;

namespace LVK.Json
{
    [PublicAPI]
    public interface IJsonSerializerSettingsConfigurator
    {
        void Configure([NotNull] JsonSerializerSettings settings);
    }
}