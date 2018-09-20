using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

using NodaTime;

namespace LVK.Configuration
{
    internal interface IConfigurationProvider
    {
        [NotNull]
        JObject GetConfiguration();

        Instant LastUpdatedAt { get; }
    }
}