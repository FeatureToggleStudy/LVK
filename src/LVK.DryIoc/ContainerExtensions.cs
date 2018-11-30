using System;
using System.Collections.Generic;
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
        public static IContainer Finalize([NotNull] this IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            var finalizedTypes = new HashSet<Type>();
            while (true)
            {
                bool anyNewTypesInvolved = false;
                
                var containerFinalizers = container.Resolve<IEnumerable<IContainerFinalizer>>().NotNull();
                foreach (var finalizer in containerFinalizers)
                {
                    var type = finalizer.GetType();
                    if (!finalizedTypes.Add(type))
                        continue;

                    anyNewTypesInvolved = true;
                    finalizer.Finalize(container);
                }

                if (!anyNewTypesInvolved)
                    break;
            }

            return container;
        }

        [NotNull]
        public static IContainer Bootstrap([NotNull] this IContainer container, [NotNull] Type servicesBootstrapperType)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            if (servicesBootstrapperType == null)
                throw new ArgumentNullException(nameof(servicesBootstrapperType));
            
            var register = container.Resolve<IServicesBootstrapperRegister>(IfUnresolved.ReturnDefault);
            if (register == null)
            {
                register = new ServicesBootstrapperRegister();
                container.UseInstance(register);
            }

            if (register.TryAdd(servicesBootstrapperType))
            {
                var instance = (IServicesBootstrapper)container.New(servicesBootstrapperType);
                assume(instance != null);
                
                instance.Bootstrap(container);
            }

            return container;
        }

        [NotNull]
        public static IContainer Bootstrap<T>([NotNull] this IContainer container)
            where T: class, IServicesBootstrapper
            => Bootstrap(container, typeof(T));

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