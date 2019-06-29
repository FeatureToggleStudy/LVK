using System;
using System.Reflection;

using JetBrains.Annotations;

using LVK.Data.Caching;
using LVK.Json;
using LVK.Logging;

namespace LVK.Resources
{
    internal class ResourcesFactory : IResourcesFactory
    {
        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly IWeakCache<(Assembly assembly, string namePrefix), IResources> _Cache;

        [NotNull]
        private readonly IJsonSerializerFactory _JsonSerializerFactory;

        public ResourcesFactory(
            [NotNull] ILogger logger, [NotNull] IWeakCache<(Assembly assembly, string namePrefix), IResources> cache,
            [NotNull] IJsonSerializerFactory jsonSerializerFactory)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _Cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _JsonSerializerFactory = jsonSerializerFactory ?? throw new ArgumentNullException(nameof(jsonSerializerFactory));
        }

        public IResources GetResources(Assembly assembly, string namePrefix)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            if (namePrefix == null)
                throw new ArgumentNullException(nameof(namePrefix));

            return _Cache.GetOrAddValue((assembly, namePrefix), _ => new Resources(_Logger, _JsonSerializerFactory, assembly, namePrefix));
        }
    }
}