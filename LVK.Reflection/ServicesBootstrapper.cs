using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;
using LVK.Reflection.NameRules;

namespace LVK.Reflection
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Register<ITypeHelper, TypeHelper>(Reuse.Singleton);
            container.Register<ITypeNameRule, CSharpKeywordTypeNameRule>();
            container.Register<ITypeNameRule, NullableTypeNameRule>();
            container.Register<ITypeNameRule, GenericTypeNameRule>();
            container.Register<ITypeNameRule, NormalTypeNameRule>();
        }
    }
}