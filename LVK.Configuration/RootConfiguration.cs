using System;
using System.Diagnostics;

using JetBrains.Annotations;

using LVK.Configuration.Helpers;

using Newtonsoft.Json.Linq;

using NodaTime;

namespace LVK.Configuration
{
    [DebuggerDisplay("Configuration: {" + nameof(_Path) + "}")]
    internal class RootConfiguration : IConfiguration
    {
        [NotNull]
        private readonly IConfigurationProvider _ConfigurationProvider;

        [NotNull]
        private readonly string _Path;

        [CanBeNull]
        private JToken _Element;

        private Instant _ElementLastUpdatedAt;

        public RootConfiguration(
            [NotNull] IConfigurationProvider configurationProvider, [NotNull] string path)
        {
            _ConfigurationProvider =
                configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));

            _Path = path ?? throw new ArgumentNullException(nameof(path));
        }

        public IConfiguration this[string relativePath]
        {
            get
            {
                if (relativePath == null)
                    throw new ArgumentNullException(nameof(relativePath));

                return new RootConfiguration(_ConfigurationProvider, ConfigurationPath.Combine(_Path, relativePath));
            }
        }

        public IConfiguration this[string[] relativePaths]
        {
            get
            {
                if (relativePaths == null)
                    throw new ArgumentNullException(nameof(relativePaths));

                return new RootConfiguration(_ConfigurationProvider, ConfigurationPath.Combine(_Path, relativePaths));
            }
        }

        [NotNull]
        private JToken GetElement()
        {
            var providedConfigurationLastUpdatedAt = _ConfigurationProvider.LastUpdatedAt;
            if (_Element == null || _ElementLastUpdatedAt < providedConfigurationLastUpdatedAt)
            {
                _Element = GetSubElement(_ConfigurationProvider.GetConfiguration(), _Path.Split('/'));
                _ElementLastUpdatedAt = providedConfigurationLastUpdatedAt;
            }

            return _Element;
        }

        [NotNull]
        private JToken GetSubElement([NotNull] JObject root, string[] path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            JToken current = root;
            foreach (var element in path)
            {
                var child = current[element];
                if (child == null)
                    return new JObject();

                current = child;
            }

            return current;
        }

        public IConfigurationElement<T> Element<T>()
            => new ConfigurationElement<T>(GetElement);
    }
}