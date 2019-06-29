using System.Reflection;

using JetBrains.Annotations;

namespace LVK.Resources
{
    [PublicAPI]
    public interface IResourcesFactory
    {
        [NotNull]
        IResources GetResources([NotNull] Assembly assembly, [NotNull] string namePrefix);
    }
}