using System;

using JetBrains.Annotations;

namespace LVK.Resources
{
    [PublicAPI]
    public static class ResourcesFactoryExtensions
    {
        [NotNull]
        public static IResources GetResources([NotNull] IResourcesFactory resourcesFactory, [NotNull] Type type)
        {
            if (resourcesFactory == null)
                throw new ArgumentNullException(nameof(resourcesFactory));

            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return resourcesFactory.GetResources(type.Assembly, type.FullName + ".");
        }

        [NotNull]
        public static IResources GetResources<T>([NotNull] this IResourcesFactory resourcesFactory)
            => GetResources(resourcesFactory, typeof(T));

        [NotNull]
        public static IResources GetResourcesForInstance([NotNull] this IResourcesFactory resourcesFactory, [NotNull] object instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            return GetResources(resourcesFactory, instance.GetType());
        }
    }
}