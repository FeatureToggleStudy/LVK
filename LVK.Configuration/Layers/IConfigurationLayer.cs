using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

namespace LVK.Configuration.Layers
{
    internal interface IConfigurationLayer
    {
        [CanBeNull]
        JObject Configuration { get; }
    }
}