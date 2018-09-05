using DryIoc;

using JetBrains.Annotations;

namespace LVK.DryIoc
{
    [PublicAPI]
    public interface IContainerBuilder
    {
        [NotNull]
        IContainerBuilder Register<[MeansImplicitUse] T>()
            where T: class, IServicesRegistrant, new();

        [NotNull]
        IContainer Build();
    }
}