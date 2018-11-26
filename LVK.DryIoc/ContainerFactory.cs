using System;

using DryIoc;

using JetBrains.Annotations;

namespace LVK.DryIoc
{
    [PublicAPI]
    public static class ContainerFactory
    {
        public static IContainer Bootstrap<T1, T2>()
            where T1: class, IServicesBootstrapper
            where T2: class, IServicesBootstrapper
        {
            return Bootstrap(typeof(T1), typeof(T2));
        }

        public static IContainer Bootstrap<T1, T2, T3>()
            where T1: class, IServicesBootstrapper
            where T2: class, IServicesBootstrapper
            where T3: class, IServicesBootstrapper
        {
            return Bootstrap(typeof(T1), typeof(T2), typeof(T3));
        }
        
        [NotNull]
        public static IContainer Bootstrap<T>()
            where T: class, IServicesBootstrapper
            => Bootstrap(typeof(T));

        [NotNull]
        private static IContainer Bootstrap([NotNull, ItemNotNull] params Type[] servicesBootstrapperTypes)
        {
            IContainer container = new Container(rules => rules?.WithTrackingDisposableTransients());
            foreach (var servicesBootstrapperType in servicesBootstrapperTypes)
                container = container.Bootstrap(servicesBootstrapperType);

            return container.Finalize();
        }
    }
}