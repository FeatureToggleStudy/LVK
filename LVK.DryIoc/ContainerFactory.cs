using DryIoc;

using JetBrains.Annotations;

namespace LVK.DryIoc
{
    [PublicAPI]
    public static class ContainerFactory
    {
        [NotNull]
        public static IContainer Create()
        {
            return new Container(rules => rules.WithTrackingDisposableTransients());
        }
    }
}