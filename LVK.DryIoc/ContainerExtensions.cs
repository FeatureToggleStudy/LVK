using System;
using System.Reflection;

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

            if (register.TryAdd(typeof(T)))
            {
                var instance = container.New<T>();
                assume(instance != null);
                
                instance.Bootstrap(container);
            }

            return container;
        }

        public static void RegisterAll<T>([NotNull] this IContainer container, [NotNull] Assembly assembly, [CanBeNull] IReuse reuse = null)
        {
            if (container is null)
                throw new ArgumentNullException(nameof(container));

            if (assembly is null)
                throw new ArgumentNullException(nameof(assembly));

            foreach (Type type in assembly.GetTypes())
            {
                if (!typeof(T).IsAssignableFrom(type))
                    continue;

                if (type.IsAbstract)
                    continue;

                container.Register(typeof(T), type, reuse);
            }
        }
    }
}