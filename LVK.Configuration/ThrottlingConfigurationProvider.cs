using System;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

using NodaTime;

namespace LVK.Configuration
{
    internal class ThrottlingConfigurationProvider : IConfigurationProvider
    {
        [NotNull]
        private readonly IClock _Clock;
        private readonly Duration _Duration;

        [NotNull]
        private readonly ConfigurationProvider _ConfigurationProvider;

        private JObject _LastRefreshedConfiguration;
        private Instant _LastRefreshedLastUpdatedAt;
        private Instant _WhenToRefreshNext;

        public ThrottlingConfigurationProvider([NotNull] IClock clock, Duration duration, [NotNull] ConfigurationProvider configurationProvider)
        {
            _Clock = clock ?? throw new ArgumentNullException(nameof(clock));
            _Duration = duration;
            _ConfigurationProvider = configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
            _WhenToRefreshNext = Instant.MinValue;
        }

        public JObject GetConfiguration()
        {
            RefreshIfNeeded();
            return _LastRefreshedConfiguration;
        }

        public Instant LastUpdatedAt
        {
            get
            {
                RefreshIfNeeded();
                return _LastRefreshedLastUpdatedAt;
            }
        }

        private void RefreshIfNeeded()
        {
            var now = _Clock.GetCurrentInstant();
            if (now < _WhenToRefreshNext)
                return;

            _LastRefreshedLastUpdatedAt = _ConfigurationProvider.LastUpdatedAt;
            _LastRefreshedConfiguration = _ConfigurationProvider.GetConfiguration();
            _WhenToRefreshNext = now.Plus(_Duration);
        }
    }
}