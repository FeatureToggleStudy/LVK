using JetBrains.Annotations;

using Newtonsoft.Json;

namespace LVK.Configuration
{
    internal interface IJsonSerializerFactory
    {
        [NotNull]
        JsonSerializer Create();
    }
}