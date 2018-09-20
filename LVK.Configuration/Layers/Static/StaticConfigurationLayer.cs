using System;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

using NodaTime;

namespace LVK.Configuration.Layers.Static
{
    internal class StaticConfigurationLayer : IConfigurationLayer
    {
        [NotNull]
        private readonly JObject _Configuration;

        public StaticConfigurationLayer(Instant whenCreated, [NotNull] JObject configuration)
        {
            LastChangedAt = whenCreated;
            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public Instant LastChangedAt { get; }

        public JObject GetConfiguration() => _Configuration;
    }
}