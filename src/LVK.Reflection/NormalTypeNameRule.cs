using System;

namespace LVK.Reflection
{
    internal class NormalTypeNameRule : ITypeNameRule
    {
        public int Priority => int.MaxValue;

        public string TryGetNameOfType(Type type, ITypeHelper typeHelper, NameOfTypeOptions options)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            if (typeHelper is null)
                throw new ArgumentNullException(nameof(typeHelper));

            if (type.IsGenericType)
                return null;

            return (options & NameOfTypeOptions.IncludeNamespaces) != 0 ? type.FullName : type.Name;
        }
    }
}