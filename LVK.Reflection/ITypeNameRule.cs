using System;

using JetBrains.Annotations;

namespace LVK.Reflection
{
    [PublicAPI]
    public interface ITypeNameRule
    {
        int Priority { get; }

        [CanBeNull]
        string TryGetNameOfType([NotNull] Type type, [NotNull] ITypeHelper typeHelper, NameOfTypeOptions options);
    }
}