using DryIoc;

using JetBrains.Annotations;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.DryIoc
{
    public static class ContainerExtensions
    {
        [NotNull]
        public static IContainer Bootstrap<T>([NotNull] this IContainer container)
            where T: class, IServicesBootstrapper
        {
            var register = container.Resolve<IContainerBootstrapperRegister>(IfUnresolved.ReturnDefaultIfNotRegistered);
            if (register == null)
            {
                register = container.New<ContainerBootstrapperRegister>();
                assume(register != null);
                
                container.UseInstance(register);
            }
            
            if (register.TryAddBootstrapper<T>())
                container.New<T>().NotNull().Bootstrap(container);                
            return container;
        }
    }
}