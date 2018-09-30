using System;

using DryIoc;

using JetBrains.Annotations;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.DryIoc
{
    [PublicAPI]
    public static class ContainerExtensions
    {
        [NotNull]
        public static IContainer Bootstrap<T>([NotNull] this IContainer container)
            where T: class, IServicesBootstrapper
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            var register = container.Resolve<IServicesBootstrapperRegister>(IfUnresolved.ReturnDefault);
            if (register == null)
            {
                register = new ServicesBootstrapperRegister();
                container.UseInstance(register);
            }

            if (register.TryAdd<T>())
            {
                var instance = container.New<T>();
                assume(instance != null);
                
                instance.Bootstrap(container);
            }

            return container;
        }
    }
}