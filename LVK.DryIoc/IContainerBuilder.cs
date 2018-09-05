using DryIoc;

using JetBrains.Annotations;

namespace LVK.DryIoc
{
    [PublicAPI]
    public interface IContainerBuilder
    {
        void Register<[MeansImplicitUse] T>()
            where T: class, IServicesRegistrant, new();

        [NotNull]
        IContainer Build();
    }
}