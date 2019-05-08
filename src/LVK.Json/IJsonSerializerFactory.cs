using JetBrains.Annotations;

using Newtonsoft.Json;

namespace LVK.Json
{
    [PublicAPI]
    public interface IJsonSerializerFactory
    {
        [NotNull]
        JsonSerializer Create();
    }
}