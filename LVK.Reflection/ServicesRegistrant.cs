using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;
using LVK.Reflection.NameRules;

namespace LVK.Reflection
{
    [PublicAPI]
    public class ServicesRegistrant : IServicesRegistrant
    {
        public void Register(IContainerBuilder containerBuilder)
        {
            if (containerBuilder is null)
                throw new ArgumentNullException(nameof(containerBuilder));
        }

        public void Register(IContainer container)
        {
            if (container is null)
                throw new ArgumentNullException(nameof(container));

            container.Register<ITypeHelper, TypeHelper>(Reuse.Singleton);
            container.Register<ITypeNameRule, CSharpKeywordTypeNameRule>();
            container.Register<ITypeNameRule, NullableTypeNameRule>();
            container.Register<ITypeNameRule, GenericTypeNameRule>();
            container.Register<ITypeNameRule, NormalTypeNameRule>();

            container.Register<IContainerInitializer, ReflectionContainerInitializer>();
        }
    }
}