using JetBrains.Annotations;

using Newtonsoft.Json;

namespace LVK.Json
{
    [PublicAPI]
    public interface IJsonSerializerSettingsFactory
    {
        [NotNull]
        JsonSerializerSettings Create();
    }
}