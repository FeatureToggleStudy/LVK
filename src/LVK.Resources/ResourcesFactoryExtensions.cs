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

            return resourcesFactory.GetResources(type.Assembly, type.Namespace + ".");
        }

        [NotNull]
        public static IResources GetResources<T>([NotNull] this IResourcesFactory resourcesFactory)
            => GetResources(resourcesFactory, typeof(T));
    }
}