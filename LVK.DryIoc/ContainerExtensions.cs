using System;

using DryIoc;

using JetBrains.Annotations;

namespace LVK.DryIoc
{
    [PublicAPI]
    public static class ContainerExtensions
    {
        public static IContainer Bootstrap<T>([NotNull] this IContainer container)
            where T: class, IServicesBootstrapper
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            IServicesBootstrapperRegister register = container.Resolve<IServicesBootstrapperRegister>(IfUnresolved.ReturnDefault);
            if (register == null)
            {
                register = new ServicesBootstrapperRegister();
                container.UseInstance(register);
            }
            
            if (register.TryAdd<T>())
                container.New<T>().Bootstrap(container);
            
            return container;
        }
    }
}