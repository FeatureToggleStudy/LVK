using System;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

using NodaTime;

using static LVK.Core.JetBrainsHelpers;

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
            assume(_LastRefreshedConfiguration != null);
            return _LastRefreshedConfiguration;
        }

        private void RefreshIfNeeded()
        {
            var now = _Clock.GetCurrentInstant();
            if (now < _WhenToRefreshNext && _LastRefreshedConfiguration != null)
                return;

            _LastRefreshedConfiguration = _ConfigurationProvider.GetConfiguration();
            _WhenToRefreshNext = now.Plus(_Duration);
        }
    }
}