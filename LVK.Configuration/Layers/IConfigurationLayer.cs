using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

using NodaTime;

namespace LVK.Configuration.Layers
{
    internal interface IConfigurationLayer
    {
        Instant LastChangedAt { get; }

        [NotNull]
        JObject GetConfiguration();
    }
}